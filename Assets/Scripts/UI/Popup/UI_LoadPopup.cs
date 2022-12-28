using Noah;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Noah;


public class UI_LoadPopup : UI_Popup
{
    public TextMeshProUGUI ProjectName;
    public RawImage ProjectImage;
    public byte[] PngData;
    const string ip = "http://20.196.219.147:3030/api/project";
    public readonly string imgNale;
    public List<Noah.EditedObject> edtiedObjects;
    private string editedObejctsJson;
    private class Test_ReadOnly
    {
        public readonly string _value;
        public Test_ReadOnly(string value)
        {
            _value = value;
        }
    
    }

    enum GameObjects
    {
        ProjectImage,
    }

    enum Texts
    {
        ProjectName,
    }

    enum Buttons
    {
        ProjectLoadBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));


        ProjectName = GetText((int)Texts.ProjectName);
        ProjectImage = GetObject((int)GameObjects.ProjectImage).GetComponent<RawImage>();

        GetButton((int)Buttons.ProjectLoadBtn).gameObject.BindEvent(SendProjectToServer);


        return base.Init();
    }

    void RefreshUI()
    {

    }

    void SendProjectToServer()
    {
  
        if (ProjectImage.texture == null) return;
        byte[] oldbyte = PngData;
        byte[] newbyte = Utils.TextureToPng(ProjectImage.texture);
        Test_ReadOnly ro = new Test_ReadOnly(ProjectName.text);
        editedObejctsJson = JsonHelper.ToJson(edtiedObjects.ToArray());
       if ( ByteArrayCompare(oldbyte, newbyte))
        {
            Debug.Log("Same byte[]");
        }

        StartCoroutine(Utils.Co_UploadImage(Utils.toTexture2D(ProjectImage.texture), ip, ProjectName.text, editedObejctsJson, (result) =>
        {
            // 처리로직
            Debug.Log($"result -> {result}");
        }));


        // RefreshUI
        FindObjectOfType<UI_ProjectsScene>().RefreshUI();
    }

    bool ByteArrayCompare(byte[] a1, byte[] a2)
    {
        if (a1.Length != a2.Length)
            return false;

        for (int i = 0; i < a1.Length; i++)
            if (a1[i] != a2[i])
                return false;

        return true;
    }


}
