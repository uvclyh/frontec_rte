using Noah;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http.Headers;
using UnityEngine.UI;
using RTG;
using System;
using RuntimeInspectorNamespace;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine.TextCore.Text;

public class UI_ProjectsScene : UI_Scene
{
    GameObject loadedContents;
    GameObject unloadedContents;
    RawImage rawImageTest;

    enum GameObjects
    {
        LoadedContents,
        UnLoadedContents,
        RawImageTest,
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
        BindObject(typeof(GameObjects));


        GetButton((int)Buttons.ProjectCreateBtn).gameObject.BindEvent(OnClickedPrjectCreateBtn);

        loadedContents = GetObject((int)GameObjects.LoadedContents);
        unloadedContents = GetObject((int)GameObjects.UnLoadedContents);
        rawImageTest = GetObject((int)GameObjects.RawImageTest).GetComponent<RawImage>();

        RefreshUI();
        return true;
    }




    public void RefreshUI() {
        ClearChildernObjects();
        ConfigureLoadedContents();
        ConfigureUnLoadedContents();
    }

    void OnClickedPrjectCreateBtn()
    {
        Managers.UI.ShowPopupUI<UI_CreateProjectPopup>("UI_CreateProjectPopup");
    }

    void ConfigureUnLoadedContents()
    {
        string data = File.ReadAllText(Application.persistentDataPath + "/" + "ProjectData.json");

        if (data == " ")
        {
            Debug.Log("로컬에 데이터가 존재 하지 않습니다.");
            return;
        }
 
        ProjectData projectData = JsonUtility.FromJson<ProjectData>(data);
        foreach (var proj in projectData.projects)
        {
          UI_LocalProjectItem temp = Managers.UI.MakeSubItem<UI_LocalProjectItem>(unloadedContents.transform);
          temp.ProjectName.text = proj.Name; 
          temp.ProjectType.text = proj.projectType;
          temp.Date.text = proj.Date;
          temp.EditedObjects = proj.EditedObjects;
          temp.PngByte = proj.pngImage;
        }
    }

    void ConfigureLoadedContents()
    {
        CoroutineManager.Start(Utils.Co_GetText("http://20.196.219.147:3030/api/project", (result) =>
        {
            LoadedProjectDataResponse loadedProjectDataResponse = JsonUtility.FromJson<LoadedProjectDataResponse>(result);
          
          foreach (LoadedProjectData data in loadedProjectDataResponse.record)
          {
              Debug.Log(data);
              UI_RemoteProjectItem temp = Managers.UI.MakeSubItem<UI_RemoteProjectItem>(loadedContents.transform);
              temp.ProjectName.text = data.name;
              temp.id = data.id;
          }
        }));
    }

    void ClearChildernObjects()
    {
        var loadedcontents = loadedContents.GetComponentsInChildren<Transform>();
        foreach (var content in loadedcontents)
        {
            if (content.gameObject.name.StartsWith("UI_"))
            {
                Destroy(content.gameObject);
            }
        }
        var unloadedcontents = unloadedContents.GetComponentsInChildren<Transform>();
        foreach (var content in unloadedcontents)
        {
            if (content.gameObject.name.StartsWith("UI_"))
            {
                Destroy(content.gameObject);
            }
        }
    }

    public void OpenUVCWebSite()
    {
        Application.OpenURL("https://uvc.co.kr/");
    }
}


