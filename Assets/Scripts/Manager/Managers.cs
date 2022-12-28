using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;
using Noah;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace Noah
{
    public class Managers : MonoBehaviour
    {
        //
        public static string CurrentProjectName;
        public static string ProjectType;
        public static ProjectData ProjectData;
        //

        public static Managers s_instance = null;
        public static Managers Instance { get { return s_instance; } }
        private static DataManager s_dataManager = new DataManager();
        private static UIManager s_uiManager = new UIManager();
        private static ResourceManager s_resourceManager = new ResourceManager();
        private static SceneManagerEx s_sceneManager = new SceneManagerEx();
        private static SoundManager s_soundManager = new SoundManager();
        private static PoolManager s_poolManager = new PoolManager();
 

        public static DataManager Data { get { Init(); return s_dataManager; } }
        public static PoolManager Pool { get { Init(); return s_poolManager; } }
        public static UIManager UI { get { Init(); return s_uiManager; } }
        public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
        public static SceneManagerEx Scene { get { Init(); return s_sceneManager; } }
        public static SoundManager Sound {  get { Init(); return s_soundManager; } }
    


        
        public static string GetText(int id)
	    {
            return "null Please set";
	    }
        

        private void Start()
        {
            Init();
        }

        private static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                    go = new GameObject { name = "@Managers" };

                s_instance = Utils.GetOrAddComponent<Managers>(go);
                DontDestroyOnLoad(go);
                s_poolManager.Init();
                s_dataManager.Init();
                s_resourceManager.Init();
                s_sceneManager.Init();
                s_soundManager.Init();
                ProjectDataInit();


                Application.targetFrameRate = 60;
            }
        }



        static void ProjectDataInit()
        {
            List<Project> projects = new List<Project>();
            byte[] buffer = new byte[1];
            List<EditedObject> editedObjects = new List<EditedObject>();
            VectorInfo position = new VectorInfo(0, 0, 0);
            VectorInfo rotation = new VectorInfo(0, 0, 0);
            VectorInfo scale = new VectorInfo(0, 0, 0);

            EditedObject editedObject = new EditedObject(0, position,rotation,scale, "sampleDescription");
            editedObjects.Add(editedObject);
            Project project = new Project("name", "광주 공장", "2022.11.30", buffer, editedObjects);
            //projects.Add(project);
            ProjectData = new ProjectData(projects);
        }
     }

}