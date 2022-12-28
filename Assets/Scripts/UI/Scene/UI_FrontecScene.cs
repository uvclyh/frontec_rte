using DG.Tweening.Core.Easing;
using Noah;
using RTG;
using RuntimeInspectorNamespace;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Noah
{
    public class UI_FrontecScene : UI_Scene
    {

        GameObject navigationPanel;
        GameObject leftPanel;
        GameObject rightPanel;
        TMP_Dropdown addressSpaceDropDown;

        public SceneStates _state;

        Image on_selectImage;
        Image off_selectImage;
        Image on_dragImage;
        Image off_dragImage;
        Image on_rotaionImage;
        Image off_rotationImage;


        RTFocusCamera RTFocusCamera;
        public FrontecScene scene;
        public RawImage testingPNG;

        enum Images
        {
            On_Select,
            Off_Select,
            On_Drag,
            Off_Drag,
            On_Rotatoin,
            Off_Rotatoin,
        }


        public SceneStates State
        {
            get { return _state; }
            set
            {
                _state = value;

                switch (_state)
                {
                    case SceneStates.None:
                        // All setactive
                        on_dragImage.enabled = false;
                        on_rotaionImage.enabled = false;
                        on_selectImage.enabled = false;
                        off_selectImage.enabled = true;
                        off_rotationImage.enabled = true;
                        off_dragImage.enabled = true;
                        break;
                    case SceneStates.Selecting:
                        on_dragImage.enabled = false;
                        on_rotaionImage.enabled = false;
                        on_selectImage.enabled = true;
                        off_selectImage.enabled = false;
                        off_rotationImage.enabled = true;
                        off_dragImage.enabled = true;

                        break;
                    case SceneStates.Dragging:
                        on_dragImage.enabled = true;
                        on_rotaionImage.enabled = false;
                        on_selectImage.enabled = false;
                        off_selectImage.enabled = true;
                        off_rotationImage.enabled = true;
                        off_dragImage.enabled = false;
                        break;
                    case SceneStates.Rotating:
                        on_dragImage.enabled = false;
                        on_rotaionImage.enabled = true;
                        on_selectImage.enabled = false;
                        off_selectImage.enabled = true;
                        off_rotationImage.enabled = false;
                        off_dragImage.enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public enum SceneStates
        {
            None,
            Selecting,
            Dragging,
            Rotating,
        }

        enum Buttons
        {
            ExitButton,
            ProjectSaveBtn,
            SphereBtn,
            CapsuleBtn,
            CubeBtn,
            MachinePanelBtn,
            AddEduTagBtn,
            EnrollTagBtn,
            BlueCircularBar_Btn,
            OrangeCircularBar_Btn,
            GreenCirclularBar_Btn,
            GreenSignalLamp_Btn,
            RedSignalLamp_Btn,
            StatusPanel_Btn,
            GreenArrow_Btn,
        }
        enum GameObjects
        {
            NavigationPanel,
            LeftPanel,
            RightPanel,
            AddressSpaceDropDown,
        }

        enum Texts
        {
            SelectedPlcTagName,
        }


        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindObject(typeof(GameObjects));
            BindImage(typeof(Images));
            BindButton(typeof(Buttons));
            BindText(typeof(Texts));

            scene = Managers.Scene.CurrentScene as FrontecScene;
            // Paneel allocation
            navigationPanel = GetObject((int)GameObjects.NavigationPanel);
            leftPanel = GetObject((int)GameObjects.LeftPanel);
            rightPanel = GetObject((int)GameObjects.RightPanel);
            // Navigaation Bar Status Images Bindings
            on_dragImage = GetImage((int)Images.On_Drag);
            off_dragImage = GetImage((int)Images.Off_Drag);
            on_selectImage = GetImage((int)Images.On_Select);
            off_selectImage = GetImage((int)Images.Off_Select);
            on_rotaionImage = GetImage((int)Images.On_Rotatoin);
            off_rotationImage = GetImage((int)Images.Off_Rotatoin);

            addressSpaceDropDown = GetObject((int)GameObjects.AddressSpaceDropDown).GetComponent<TMP_Dropdown>();

            GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickedExitButton);
            GetButton((int)Buttons.ProjectSaveBtn).gameObject.BindEvent(OnClickedSaveProjectBtn);

            /** 좌측 상단 PLC 자산 버튼들 **/
            GetButton((int)Buttons.BlueCircularBar_Btn).gameObject.BindEvent(OnClickedBlueCircularBarBtn);
            GetButton((int)Buttons.OrangeCircularBar_Btn).gameObject.BindEvent(OnClickedOrangeCircularBarBtn);
            GetButton((int)Buttons.GreenCirclularBar_Btn).gameObject.BindEvent(OnClickedGreenCircularBarBtn);
            GetButton((int)Buttons.GreenSignalLamp_Btn).gameObject.BindEvent(OnClickedzGreenSignalLampBtn);
            GetButton((int)Buttons.RedSignalLamp_Btn).gameObject.BindEvent(OnClickedRedSignalLampBtn);
            GetButton((int)Buttons.StatusPanel_Btn).gameObject.BindEvent(OnClickedStatusBarBtn);
            GetButton((int)Buttons.GreenArrow_Btn).gameObject.BindEvent(OnClickedGreenArrowBtn);
            // GetButton((int)Buttons.MachinePanelBtn).gameObject.BindEvent(Pool_Panel);
            GetButton((int)Buttons.AddEduTagBtn).gameObject.BindEvent(Pool_EduTag);
            GetButton((int)Buttons.ProjectSaveBtn).gameObject.BindEvent(OnClickedProjectSaveBtn);
            GetButton((int)Buttons.EnrollTagBtn).gameObject.BindEvent(Pool_Tag);
            //GetButton((int)Buttons.MoveButton).gameObject.BindEvent(OnClickMoveButton);
            //GetButton((int)Buttons.RotationButton).gameObject.BindEvent(OnClickedRotationButton);
            //GetButton((int)Buttons.ScaleButton).gameObject.BindEvent(OnClickedScaleButton);
            //Debug.Log("UI_SelectedObjectStatusPopup Binding Finish");
            // GetButton((int)Buttons.AdsButton).gameObject.BindEvent(OnClickButton);


            RefreshUI();
            if(Managers.ProjectType.Equals("시화 공장"))
            {
                Pool_HNF514();
            }
            else if(Managers.ProjectType.Equals("광주 공장"))
            {//광주공장
                Pool_HNF615();
            }
            else
            {
                Pool_Edukit();
            }
            RTFocusCamera = FindObjectOfType<RTFocusCamera>();

            return true;
        }


        //Navigation Status Bar Realtime Changing 

        private void LateUpdate()
        {
            NavStatusOnserve();
        }


        void NavStatusOnserve()
        {
            if (RTFocusCamera._lookAroundSettings.IsLookAroundEnabled && RTFocusCamera.Hotkeys.LookAround.IsActive())
            {
                State = SceneStates.Rotating;
            }
            else if (RTFocusCamera.CanUseMouseScrollWheel())
            {
                float mouseScroll = RTInput.MouseScroll();
                if (mouseScroll != 0.0f && RTFocusCamera._zoomSettings.IsZoomEnabled)
                {
                    State = SceneStates.Dragging;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                State = SceneStates.Selecting;
            }
        }
        void RefreshUI()
        {
            if (_init == false)
                return;

            LeftPanel_AddressSpaceDropDownConfig();

            Debug.Log($"{this.name} :  refresh UI" +
                $"{navigationPanel.name}" +
                $"{leftPanel.name} ");
            //Todo 초기화 설정     

            State = SceneStates.None;
            State = SceneStates.Rotating;
            State = SceneStates.Selecting;

        }
        void OnClickedExitButton()
        {
            Managers.Scene.ChangeScene(Define.Scene.frontec_projects);
        }

        public void OnClickedSaveProjectBtn()
        {

        }

        public void DissapearAllPanel()
        {
            navigationPanel.SetActive(false);
            leftPanel.SetActive(false);
            rightPanel.SetActive(false);
        }


        public void OnClickedBlueCircularBarBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Blue Circular Bar";
        }

        public void OnClickedGreenArrowBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Green Arrow";
        }

        public void OnClickedOrangeCircularBarBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Orange Circular Bar";
        }

        public void OnClickedGreenCircularBarBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Green Circular Bar";
        }

        public void OnClickedStatusBarBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Status";
        }

        public void OnClickedzGreenSignalLampBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Green Signal Lamp";
        }

        public void OnClickedRedSignalLampBtn()
        {
            GetText((int)Texts.SelectedPlcTagName).text = "Red Signal Lamp";
        }







        public void Pool_BlueCirculareBar(string plcTag)
        {

            GameObject go = Managers.Resource.Instantiate("P_BlueCircularBar");

            go.transform.position = new Vector3(0, 1, 0);
            go.GetComponentInChildren<Text>().text = plcTag;
            scene.currentSelectObject = go.GetComponent<SelectedObject>();

            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }

        }

        public void Pool_OrangeCircularBar(string plcTag)
        {

            GameObject go = Managers.Resource.Instantiate("P_OrangeCircularBar");
            go.transform.position = new Vector3(0, 1, 0);
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            go.GetComponentInChildren<Text>().text = plcTag;
            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }

        }

        public void Pool_GreenCircularBar(string plcTag)
        {
            GameObject go = Managers.Resource.Instantiate("P_GreenCircularBar");
            go.transform.position = new Vector3(0, 1, 0);
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            go.GetComponentInChildren<Text>().text = plcTag;
            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }

        public void Pool_RedSignalLamp(string plcTag)
        {
            GameObject go = Managers.Resource.Instantiate("P_RedSignalLamp");
            go.transform.position = new Vector3(0, 1, 0);
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            go.GetComponentInChildren<Text>().text = plcTag;
            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }





        public void Pool_GreenSignalLamp(string plcTag)
        {
            GameObject go = Managers.Resource.Instantiate("P_GreenSignalLamp");
            go.transform.position = new Vector3(0, 1, 0);
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            go.GetComponentInChildren<Text>().text = plcTag;
            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }
        public void Pool_Status(string plcTag)
        {
            GameObject go = Managers.Resource.Instantiate("P_Status");
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            go.GetComponentInChildren<Text>().text = plcTag;
            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }


        public void Pool_HNF514()
        {
            if (GameObject.Find("M_HNF_514") != null)
            {
                Debug.Log("HNF-514가 이미 존재합니다.");
                return;
            }
                
            GameObject go = Managers.Resource.Instantiate("M_HNF_514");
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            if (go.GetComponent<SelectedObject>().Dropped == false)
                go.GetComponent<SelectedObject>().OnSellected();
        }




        public void Pool_GreenArrow()
        {

            GameObject go = Managers.Resource.Instantiate("M_GreenArrow");
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            if (go.GetComponent<SelectedObject>().Dropped == false)
                go.GetComponent<SelectedObject>().OnSellected();
        }

        public void Pool_Edukit()
        {

            if (GameObject.Find("M_Edukit") != null)
            {
                Debug.Log("M_Edukit가 이미 존재합니다.");
                return;
            }
            GameObject go = Managers.Resource.Instantiate("M_Edukit");
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            if (go.GetComponent<SelectedObject>().Dropped == false)
                scene.currentSelectObject.OnSellected();
        }

        public void Pool_HNF615()
        {
            
            if (GameObject.Find("M_HNF_615") != null)
            {
                Debug.Log("HNF-615가 이미 존재합니다.");
                return;
            }
            GameObject go = Managers.Resource.Instantiate("M_HNF_615");
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            if (go.GetComponent<SelectedObject>().Dropped == false)
                scene.currentSelectObject.OnSellected();
        }

        public void Pool_Capsule()
        {
            if (GameObject.Find("Capsule") != null) return;
            GameObject go = Managers.Resource.Instantiate("Capsule");

            scene.currentSelectObject = go.GetComponent<SelectedObject>();

            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }

        public void Pool_Cube()
        {
            if (GameObject.Find("Cube") != null) return;
            GameObject go = Managers.Resource.Instantiate("Cube");

            scene.currentSelectObject = go.GetComponent<SelectedObject>();

            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }

        public void Pool_Sphere()
        {
            if (GameObject.Find("Sphere") != null) return;
            GameObject go = Managers.Resource.Instantiate("Sphere");

            scene.currentSelectObject = go.GetComponent<SelectedObject>();

            if (go.GetComponent<SelectedObject>().Dropped == false)
            {
                scene.currentSelectObject.OnSellected();
            }
        }





        public void Pool_Panel()
        {
            if (GameObject.Find("Status_Panel") != null) return;
            GameObject go = Managers.Resource.Instantiate("Status_Panel");
            scene.currentSelectObject = go.GetComponent<SelectedObject>();
            if (go.GetComponent<SelectedObject>().Dropped == false)
                go.GetComponent<SelectedObject>().OnSellected();
        }


        public void Pool_Tag()
        {
            // Todo Selected Tag TJS
      
            switch (GetText((int)Texts.SelectedPlcTagName).text)
            {
                case "Blue Circular Bar":
                    Pool_BlueCirculareBar(scene.tagName.text);
                    break;
                case "Orange Circular Bar":
                    Pool_OrangeCircularBar(scene.tagName.text);
                    break;
                case "Green Circular Bar":
                    Pool_GreenCircularBar(scene.tagName.text);
                    break;
                case "Green Signal Lamp":
                    Pool_GreenSignalLamp(scene.tagName.text);
                    break;
                case "Red Signal Lamp":
                    Pool_RedSignalLamp(scene.tagName.text);
                    break;
                case "Status":
                    Pool_Status(scene.tagName.text);
                    break;
                case "Green Arrow":
                    Pool_GreenArrow();
                    break;
                default:
                    Debug.Log("생성할 태그가 없음");
                    break;
            }
        }

        public void Pool_EduTag()
        {
            GameObject go = Managers.Resource.Instantiate("T_education");
            go.GetComponentInChildren<TextMeshProUGUI>().text = scene.educationContents.text;
            go.transform.position = new Vector3(0, 1, 0);
        }

        public void OnClickedProjectSaveBtn()
        {
            var popup = Managers.UI.ShowPopupUI<UI_ProjectPopup>("UI_ProjectPopup");
        }

        void LeftPanel_AddressSpaceDropDownConfig()
        {
            Dictionary<string, Profile> testAddress = Managers.Data.AddressSpaceDatas;
            List<Request> reqs = testAddress["Frontec_Profile"].requests;
            foreach (var req in reqs)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = req.korName;
                addressSpaceDropDown.options.Add(option);
            }
        }

    }

}
