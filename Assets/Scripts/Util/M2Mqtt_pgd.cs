using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class M2Mqtt_pgd : MonoBehaviour
{
    public static M2Mqtt_pgd instance;


    // MQTT setting
    protected MqttClient client;

    protected string brokerAddress = "rpa-server.flexing.ai"; // 원격 10.20.193.104 로컬 220.90.129.98
    protected int brokerPort = 1883; // 원격 1883 로컬Ʈ 62658
    public string doosanTopic = "hyundai-advantech-001/1";
    public string kukaTopic = "hyundai-advantech-002/2"; // 원격 hyundai-advantech-002/2 로컬 UDP_test/217
    public string froctecTopic = "Siwha_01/9";
    private string mqttUserName = null;
    private string mqttPassword = null;

    [Tooltip("Use encrypted connection")]
    public bool isEncrypted = false;
    [Tooltip("Connection timeout in milliseconds")]
    public int timeoutOnConnection = MqttSettings.MQTT_CONNECT_TIMEOUT;

    private List<MqttMsgPublishEventArgs> messageQueue1 = new List<MqttMsgPublishEventArgs>();
    private List<MqttMsgPublishEventArgs> messageQueue2 = new List<MqttMsgPublishEventArgs>();
    private List<MqttMsgPublishEventArgs> frontMessageQueue = null;
    private List<MqttMsgPublishEventArgs> backMessageQueue = null;

    private bool mqttClientConnectionClosed = false;
    private bool mqttClientConnected = false;

    public Text log;

    public float doosanInputTime;
    public float kukaInputTime;

    //private List<string> eventMessages = new List<string>();


    void Awake() {
        instance = this;
        
        frontMessageQueue = messageQueue1;
        backMessageQueue = messageQueue2;
    }

    void Start()
    {
        Connect();
    }

    void Update()
    {
        ProcessMqttEvents();

        doosanInputTime += Time.deltaTime;
        kukaInputTime += Time.deltaTime;


    }

    protected virtual void ProcessMqttEvents()
    {
        SwapMqttMessageQueues();
        ProcessMqttMessageBackgroundQueue();
    }
    
    private void ProcessMqttMessageBackgroundQueue()
    {
        foreach (MqttMsgPublishEventArgs msg in backMessageQueue)
        {
            DecodeMessage(msg.Topic, msg.Message);
        }
        backMessageQueue.Clear();
    }

    private void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        print("topic " + doosanTopic);
      

    }

    private void SwapMqttMessageQueues()
    {
        frontMessageQueue = frontMessageQueue == messageQueue1 ? messageQueue2 : messageQueue1;
        backMessageQueue = backMessageQueue == messageQueue1 ? messageQueue2 : messageQueue1;
    }

    public void Connect()
    {

        if (client == null || !client.IsConnected)
        {
            StartCoroutine(DoConnect());
        }
    }

    private IEnumerator DoConnect()
    {
        if (client == null)
        {
            try
            {
                client = new MqttClient(brokerAddress, brokerPort, isEncrypted, null, null, isEncrypted ? MqttSslProtocols.SSLv3 : MqttSslProtocols.None);
            }
            catch (Exception e)
            {
                client = null;
                OnConnectionFailed(e.Message);
                StartCoroutine(DoConnect());
                yield break;
            }
        }
        else if (client.IsConnected)
        {
            yield break;
        }

       // client.Settings.TimeoutOnConnection = timeoutOnConnection;

        string clientId = Guid.NewGuid().ToString();

        try
        {
            client.Connect(clientId, mqttUserName, mqttPassword);
        }
        catch (Exception e)
        {
            client = null;
            OnConnectionFailed(e.Message);
            StartCoroutine(DoConnect());
            yield break;
        }

        if (client.IsConnected)
        {
            client.ConnectionClosed += OnMqttConnectionClosed;
            client.MqttMsgPublishReceived += OnMqttMessageReceived;

            mqttClientConnected = true;
            SubscribeTopics();
            StartCoroutine(OnConnectionSuccess());
        }
    }

    private void OnConnectionFailed(string errorMessage)
    {
        Debug.LogWarning("Connection failed. " + errorMessage + " " + brokerAddress + " " + brokerPort);
    }

    private IEnumerator OnConnectionSuccess()
    {
        //log.text = "Connected to " + brokerAddress + " " + brokerPort.ToString();
        yield return new WaitForSeconds(1.5f);
       // log.text = "";
    }

    private void OnMqttConnectionClosed(object sender, EventArgs e)
    {
        Debug.LogWarning("CONNECTION LOST!");
        StartCoroutine(DoDisconnect());
        mqttClientConnectionClosed = mqttClientConnected;
        mqttClientConnected = false;
    }

    public void OnMqttConnectionNoAuleClosed()
    {
        Debug.LogWarning("CONNECTION LOST!");
        StartCoroutine(DoDisconnect());
        mqttClientConnectionClosed = mqttClientConnected;
        mqttClientConnected = false;
    }

    private void OnMqttMessageReceived(object sender, MqttMsgPublishEventArgs msg)
    {
        frontMessageQueue.Add(msg);
    }

    private void SubscribeTopics()
    {
        client.Subscribe(new string[] { froctecTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    private void UnsubscribeTopics()
    {

    }

    private IEnumerator DoDisconnect()
    {
        yield return new WaitForEndOfFrame();
        CloseConnection();
        Debug.Log("Disconnected.");
    }

    public void CloseConnection()
    {
        mqttClientConnected = false;
        if (client != null)
        {
            if (client.IsConnected)
            {
                UnsubscribeTopics();
                client.Disconnect();
            }
            mqttClientConnectionClosed = false;
            client.MqttMsgPublishReceived -= OnMqttMessageReceived;
            client.ConnectionClosed -= OnMqttConnectionClosed;
            client = null;
            StartCoroutine(DoConnect());
        }
    }
}
