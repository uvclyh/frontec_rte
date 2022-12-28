using Noah;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreateProjectPopup : UI_Popup
{

    TextMeshProUGUI ProjectName;
    TextMeshProUGUI typeLabel;
    enum Texts
    {
        ProjectName,
        typeLabel,
    }

    enum Buttons
    {
        ProjectCreateBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ProjectCreateBtn).gameObject.BindEvent(OnProjectCreatedBtnClicked);
        

        return base.Init();
    }


    public void OnProjectCreatedBtnClicked()
    {
        Managers.CurrentProjectName = GetText((int)Texts.ProjectName).text;
        Managers.ProjectType = GetText((int)Texts.typeLabel).text;
        Debug.Log($"Proejct Name {Managers.CurrentProjectName}");
        Debug.Log($"Project Type {Managers.ProjectType}");
        Managers.Scene.ChangeScene(Define.Scene.frontec_rte);

    }
}


