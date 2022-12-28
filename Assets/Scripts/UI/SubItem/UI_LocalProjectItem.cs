using Noah;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using Noah;

public class UI_LocalProjectItem : UI_Base
{

    public TextMeshProUGUI ProjectName;
    public TextMeshProUGUI ProjectType;
    public TextMeshProUGUI Date;
    public List<Noah.EditedObject> EditedObjects;
    public byte[] PngByte;
    enum Texts
    {
        ProjectName,
        ProjectType,
        Date,
    }

    enum Buttons
    {
        Button
    }

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        ProjectName = GetText((int)Texts.ProjectName);
        ProjectType = GetText((int)Texts.ProjectType);
        Date = GetText((int)Texts.Date);

        GetButton((int)Buttons.Button).gameObject.BindEvent(OnClickedButton);

        RefreshUI();
        return base.Init();
    }



    void RefreshUI()
    {
    }

    void OnClickedButton()
    {
        UI_LoadPopup temp = Managers.UI.ShowPopupUI<UI_LoadPopup>();
        temp.ProjectName.text = ProjectName.text;
        // TODO 이미지 넘겨주기 
        Texture2D tex = new Texture2D(450, 300);
        temp.edtiedObjects = EditedObjects;
        tex.LoadImage(PngByte);
        temp.PngData = PngByte;
        temp.ProjectImage.texture = tex;
    }
}
