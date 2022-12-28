using Noah;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectsScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init())
            return false;

        SceneType = Define.Scene.frontec_projects;


        return true;
    }

    public void Load_Frontec_RteScene()
    {
        Managers.Scene.ChangeScene(Define.Scene.frontec_rte);

    }
}
