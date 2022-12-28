using Noah;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnLoadPopup : UI_Popup
{
    public TextMeshProUGUI ProjectName;
    public RawImage ProjectImage;
    public int id;
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
        ProjectUnLoadBtn,
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

        GetButton((int)Buttons.ProjectUnLoadBtn).gameObject.BindEvent(DeleteProjectFromServer);

        return base.Init();
    }

    void RefreshUI()
    {

    }

    void DeleteProjectFromServer()
    {
     
        
        CoroutineManager.Start(Utils.Co_Delete("http://20.196.219.147:3030/api/project/", id.ToString(), (result) =>
        {
            // 처리로직
            Debug.Log($"result -> {result}");

        }));
        FindObjectOfType<UI_ProjectsScene>().RefreshUI();
    }
}
