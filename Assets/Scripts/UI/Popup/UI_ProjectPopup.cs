using Noah;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using File = System.IO.File;
using System;
using RTG;
using RuntimeInspectorNamespace;

namespace Noah
{
    public class UI_ProjectPopup : UI_Popup
    {
        public RawImage rawImage2;

        public RawImage rawImage;
        public TextMeshProUGUI ProjectName;
        public FrontecScene frontecScene;

        Texture2D tex;

        string path;
        string fileName = "ProjectData.json";

        enum Texts
        {
            ProjectName
        }
        enum Buttons
        {
            CancelBtn,
            ConfirmBtn,
        }

        enum GameObjects
        {
            RawImage,
        }



        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindButton(typeof(Buttons));
            BindText(typeof(Texts));
            BindObject(typeof(GameObjects));
            path = Application.persistentDataPath + "/";
            frontecScene = Managers.Scene.CurrentScene as FrontecScene;

            GetText((int)Texts.ProjectName).text = $"{Managers.CurrentProjectName}"; // _{frontecScene.NumberOfSavedImage}
            GetButton((int)Buttons.ConfirmBtn).gameObject.BindEvent(OnClickedConfirmBtn);
            GetButton((int)Buttons.CancelBtn).gameObject.BindEvent(OnClikcedCancleBtn);
            RefreshUI();

            return base.Init();
        }



        void RefreshUI()
        {
           RenderTexture screenRenderTexture =
                FindObjectOfType<ScreenRenderer>().gameObject.GetComponent<RawImage>().texture as RenderTexture;

            tex = Utils.toTexture2D(screenRenderTexture);
            GetObject((int)GameObjects.RawImage).GetComponent<RawImage>().texture = tex;
        }

        public void OnClikcedCancleBtn()
        {
            Destroy(gameObject);
        }

        public void OnClickedConfirmBtn()
        {
            Debug.Log("OnClickedConfirimBtn");
            if (tex == null) return;
            byte[] PngTex = tex.EncodeToPNG();
            List<EditedObject> editedObjects = new List<EditedObject>();
            //Find Obejct WIth "Selectable" Tag
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Selectable");
            foreach (var go in gos)
            {


                int id = new int();
                string content = "";


                switch (go.name)
                {
                    case "M_HNF_514":
                        id = 0;
                        content = "Description";
                        break;
                    case "M_HNF_615":
                        id = 1;
                        content = "Description";
                        break;
                    case "P_GreenSignalLamp":
                        id = 2;
                        content = go.GetComponentInChildren<Text>().text;
                        break;
                    case "P_RedSignalLamp":
                        id = 3;
                        content = go.GetComponentInChildren<Text>().text;
                        break;
                    case "P_Status":
                        id = 4;
                        content = go.GetComponentInChildren<Text>().text;
                        break;
                    case "P_BlueCircularBar":
                        id = 5;
                        content = go.GetComponentInChildren<Text>().text;
                        break;
                    case "P_GreenCircularBar":
                        id = 6;
                        content = go.GetComponentInChildren<Text>().text;
                        break;
                    case "P_OrangeCircularBar":
                        id = 7;
                        content = go.GetComponentInChildren<Text>().text;
                        break;
                    case "T_education":
                        id = 8;
                        content = go.GetComponentInChildren<TextMeshProUGUI>().text;
                        break;
                    case "M_Edukit":
                        id = 9;
                        content = "Description";
                        break;
                    case "M_GreenArrow":
                        id = 10;
                        content = "Description";
                        break;
                    default:
                        break;
                }

                VectorInfo position = new VectorInfo(
                    go.transform.position.x,
                    go.transform.position.y,
                    go.transform.position.z
                    );

                float rotateAngleX = go.transform.localEulerAngles.x;
                rotateAngleX = (rotateAngleX > 180) ? rotateAngleX - 360 : rotateAngleX;
                float rotateAngleY = go.transform.localEulerAngles.y;
                rotateAngleY = (rotateAngleX > 180) ? rotateAngleY - 360 : rotateAngleY;
                float rotateAngleZ = go.transform.localEulerAngles.z;
                rotateAngleZ = (rotateAngleZ > 180) ? rotateAngleZ - 360 : rotateAngleZ;


                VectorInfo rotation = new VectorInfo(
                    rotateAngleX,
                    rotateAngleY,
                    rotateAngleZ
                    );


                VectorInfo scale = new VectorInfo(
                     go.transform.localScale.x,
                     go.transform.localScale.y,
                     go.transform.localScale.z
                     );


                EditedObject obj = new EditedObject(id, position, rotation, scale, content);

                editedObjects.Add(obj);
            }

            Project project = new Project(GetText((int)Texts.ProjectName).text, Managers.ProjectType, DateTime.Now.ToString("yyyyMMdd"), PngTex, editedObjects);
            Managers.ProjectData.projects.Add(project);

            
            // ProejctData 
            string data = File.ReadAllText(Application.persistentDataPath + "/" + "ProjectData.json");
            Debug.Log(Application.persistentDataPath + "/" + "ProjectData.json");

            string textAsset = JsonUtility.ToJson(Managers.ProjectData);
            File.WriteAllText(path + fileName, textAsset);

            Managers.Scene.ChangeScene(Define.Scene.frontec_projects);
            /*

            if (data.IsNull()) // 비어있으면
            {
                string createdProejctDataJson = JsonUtility.ToJson<ProjectData>(Managers.ProjectData);

            }
            else // 비어있지 않다면 
            {
                ProjectData localProjectData = JsonUtility.FromJson<ProjectData>(File.ReadAllText(path + fileName));
                foreach (Project proj in localProjectData.projects)
                {
                    Debug.Log($"project Name : {proj.Name}");
                    Debug.Log($"project Date : {proj.Date}");
                }
                Debug.Log(localProjectData);

                ProjectData projectData = JsonUtility.FromJson<ProjectData>(data);
                Managers.ProjectData.projects.Add(project);
                string textAsset = JsonUtility.ToJson(Managers.ProjectData);
                File.WriteAllText(path + fileName, textAsset);
            }
        }
         
            //projectData.projects.add(project);

            */
        }
    }

}
