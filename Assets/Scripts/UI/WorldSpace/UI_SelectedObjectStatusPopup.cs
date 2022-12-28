using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;
using RTG;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Noah
{
    public class UI_SelectedObjectStatusPopup : UI_Base
    {
        Image moveGizmoOn;
        Image moveGizmoOff;
        Image rotationGizmoOn;
        Image rotationGizmoOff;
        Image scaleGizmoOn;
        Image scaleGizmoOff;

        

        enum Images
        {
            MoveGizmoOn,
            MoveGizmoOff,
            RotationGizmoOn,
            RotationGizmoOff,
            ScaleGizmoOn,
            ScaleGizmoOff,
        }

        enum Buttons
        {
            MoveButton,
            RotationButton,
            ScaleButton,
        }



        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindButton(typeof(Buttons));
            BindImage(typeof(Images));

            GetButton((int)Buttons.MoveButton).gameObject.BindEvent(OnClickMoveButton);
            GetButton((int)Buttons.RotationButton).gameObject.BindEvent(OnClickedRotationButton);
            GetButton((int)Buttons.ScaleButton).gameObject.BindEvent(OnClickedScaleButton);

            moveGizmoOn = GetImage((int)Images.MoveGizmoOn);
            moveGizmoOff = GetImage((int)Images.MoveGizmoOff);
            rotationGizmoOn = GetImage((int)Images.RotationGizmoOn);
            rotationGizmoOff = GetImage((int)Images.RotationGizmoOff);
            scaleGizmoOn = GetImage((int)Images.ScaleGizmoOn);  
            scaleGizmoOff = GetImage((int)Images.ScaleGizmoOff);


            Debug.Log("UI_SelectedObjectStatusPopup Binding Finish");
            // GetButton((int)Buttons.AdsButton).gameObject.BindEvent(OnClickButton);

            RefreshUI();

            return true;
        }


        void RefreshUI()
        {
            if (_init == false)
                return;

            moveGizmoOn.enabled = false;
            moveGizmoOff.enabled = true;
            rotationGizmoOn.enabled = false;
            rotationGizmoOff.enabled = true;
            scaleGizmoOn.enabled = false;
            scaleGizmoOff.enabled = true;
            //Todo 초기화 설정     

        }

        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
        }

        private void OnClickMoveButton()
        {
           transform.root.GetComponent<SelectedObject>().MoveGizmo();
            moveGizmoOn.enabled = true;
            moveGizmoOff.enabled = false;
            rotationGizmoOn.enabled = false;
            rotationGizmoOff.enabled = true;
            scaleGizmoOn.enabled = false;
            scaleGizmoOff.enabled = true;

            Debug.Log($" OnClickMoveButton");
        }

        private void OnClickedRotationButton()
        {
            transform.root.GetComponent<SelectedObject>().RotationGizmo();
            moveGizmoOn.enabled = false;
            moveGizmoOff.enabled = true;
            rotationGizmoOn.enabled = true;
            rotationGizmoOff.enabled = false;
            scaleGizmoOn.enabled = false;
            scaleGizmoOff.enabled = true;

            Debug.Log("OnClickedRotationButton");
        }

        private void OnClickedScaleButton()
        {
            transform.root.GetComponent<SelectedObject>().ScaleGizmo();
            moveGizmoOn.enabled = false;
            moveGizmoOff.enabled = true;
            rotationGizmoOn.enabled = false;
            rotationGizmoOff.enabled = true;
            scaleGizmoOn.enabled = true;
            scaleGizmoOff.enabled = false;
            Debug.Log("OnClickedScaleButton");
        }

    }
}

