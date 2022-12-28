using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;
using RTG;
using UnityEditor;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

public class FrontecScene : BaseScene
{

    public int NumberOfSavedImage = 0 ;

    Vector3 rayOrigin;
    Vector3 rayDirection;
    public SelectedObject currentSelectObject;
    public TextMeshProUGUI currentSelectedObjectName;
    public TextMeshProUGUI educationContents;
    public TextMeshProUGUI tagName;
    public string projectNameWithExtension = "XR 설계.png";
    public RawImage receivedImage;
    public string willBeProjectName;
    public RenderTexture screenRenderTexture;



    protected override bool Init()
    {
        if (base.Init())
            return false;        
        return true;
    }

    private void Start()
    {
        SceneType = Define.Scene.frontec_rte;
    }

    public void Update()
    { 
        if(Input.GetMouseButtonUp(0))
        {
            if (currentSelectObject == null) return;
            
            rayOrigin = Camera.main.transform.position;
            rayDirection = MouseWorldPosition() - Camera.main.transform.position;
           // rayDirection *= -1f;
            //rayDirection *= -100f;
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
            {
                if (hitInfo.transform.gameObject.name.StartsWith("T_")) return;
                
                if (hitInfo.collider.tag == SelectedObject.selectableTag)
                {
                    currentSelectObject.isSelected = false;

                    currentSelectObject = hitInfo.collider.GetComponent<SelectedObject>();
                    currentSelectObject.isSelected = true;
                    currentSelectedObjectName.text = currentSelectObject.transform.name;
                    Debug.Log("Selectable Object Clicked");
                }
            }
            else
            {
                currentSelectObject.isSelected = false;
                Debug.Log("Lock off ");
            }
        }
    }

    public void OnGetProjectListButton()
    {
        CoroutineManager.Start(Utils.Co_GetText("http://20.196.219.147:3030/api/project", (result) =>
        {
            ProjectsData projectData = JsonUtility.FromJson<ProjectsData>(result);

            foreach (var project in projectData.record)
            {
                Debug.Log(project);
            }

        }));
    }

    public void OnGetProjectButton()
    {
     //   CoroutineManager.Start(Utils.Co_GetTexture("http://20.196.219.147:3030/api/prjoect", projectNameWithExtension, (texture) =>
       // {
         //   Debug.Log(texture);
           // receivedImage.texture = texture;
//
  //      }));
    }

    public void OnUploadProject()
    {
        Texture2D tex = Utils.toTexture2D(screenRenderTexture);
        if (tex == null) return;
        StartCoroutine(Utils.Co_UploadImage(tex, "http://20.196.219.147:3030/api/prjoect", "t1t1", "description", (result) =>
        {
        // 처리로직
        Debug.Log($"result -> {result}");

        }));

    }



    private void OnDrawGizmos()
    {
        Debug.DrawLine(rayOrigin, rayDirection, Color.white);
    }
    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    public void OnProjectExecute()
    {
        string folderPath = Directory.GetCurrentDirectory() + "/Screenshots/";

        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        var screenshotName =
                                "Screenshot_" +
                                System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName));
        Debug.Log(folderPath + screenshotName);
    }

}

// ProjectsData myDeserializedClass = JsonConvert.DeserializeObject<ProjectsData>(myJsonResponse);
public class ProjectsData
{
    public int status;
    public string code;
    public string message;
    public List<string> record;
    public object remark;
}




