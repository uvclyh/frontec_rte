using Noah;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_RemoteProjectItem : UI_Base
{

    public TextMeshProUGUI ProjectName;
    public int id;

    enum Texts
    {
        ProjectName,

    }

    enum Buttons
    {
        Button
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        ProjectName = GetText((int)Texts.ProjectName);
        GetButton((int)Buttons.Button).gameObject.BindEvent(OnClickedButton);
        RefreshUI();

        return base.Init();
    }



    void RefreshUI()
    {
    }

    void OnClickedButton()
    {
        // UI UnLoadPopup으로 대체 
        Debug.Log($"{ProjectName.text} : OnClickedButton() -> ToDo UI_UnLoadPopup 로딩해야함!");

        
        UI_UnLoadPopup temp = Managers.UI.ShowPopupUI<UI_UnLoadPopup>();
        temp.ProjectName.text = ProjectName.text;
        temp.id = id;


        CoroutineManager.Start(Utils.Co_GetTexture($"http://20.196.219.147:3030/api/project", id.ToString(), (texture) =>
        {
            Debug.Log(texture);
            temp.ProjectImage.texture = texture;
        }));
        // TODO 이미지 넘겨주기 
        // Texture2D tex = new Texture2D(450, 300);
        // tex.LoadImage(PngByte);
        // rawImageTest.texture = tex;
        // temp.ProjectImage.texture = tex;
        
    }
}
