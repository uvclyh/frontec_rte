using Noah;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogInScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init()) return false;

        SceneType = Define.Scene.frontec_login;
        //로컬데이터 초기화
        File.WriteAllText(Application.persistentDataPath + "/" + "ProjectData.json", " ");

        return true;
    }

    public void Load_Frontec_ProjectsScene()
    {
        Managers.Scene.ChangeScene(Define.Scene.frontec_projects);
    }
}
