using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Events;
using Noah;

namespace RTG
{
    public class SelectedObject : MonoBehaviour
    {
        private ObjectTransformGizmo _transformGizmo;

        public TextMeshProUGUI positionX;
        public TextMeshProUGUI positionY;
        public TextMeshProUGUI positionZ;

        public TextMeshProUGUI rotationX;
        public TextMeshProUGUI rotationY;
        public TextMeshProUGUI rotationZ;

        
        public TextMeshProUGUI scaleX;
        public TextMeshProUGUI scaleY;
        public TextMeshProUGUI scaleZ;

        public bool _isSelected;
        public UI_SelectedObjectStatusPopup _selectedObjectStatusPopup;


        private enum state
        {
            none,
            move,
            rotation,
            scale,
            dragging,
        }

        private state _state = state.none;
        private state State
        {
            get { return _state; }
            set { _state = value; }
        }


        Vector3 offset;


        public static string destinationTag = "DropArea";
        public static string selectableTag = "Selectable";
        public bool Dropped = false;

        public SelectedObject[] selectedObjects;
  
        public void OnSellected()
        {
            transform.position = Vector3.zero;
        }


        void OnMouseDrag()
        {

            if (Dropped) return;
            if (transform.gameObject.name.StartsWith("T_")) 
             transform.position = MouseWorldPosition() + offset;
        }

        void OnMouseUp()
        {
            if (transform.gameObject.name.StartsWith("T_")) return;

            var rayOrigin = Camera.main.transform.position;
            var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
            RaycastHit hitInfo;

            if (!Dropped) // 드랍되지 않은 상태에서만 
            {

                if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
                {
                    if (hitInfo.transform.tag == destinationTag)
                    {
                        transform.position = hitInfo.transform.position;
                        Dropped = true;
                    }
                }
                transform.GetComponent<Collider>().enabled = true;
            }

        }


        Vector3 MouseWorldPosition()
        {
            var mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mouseScreenPos);
        }


       public bool isSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;

                if (_isSelected)
                {
                    MappingTransformInformation();
                    _selectedObjectStatusPopup.gameObject.SetActive(true);
                    transform.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    transform.GetComponent<Collider>().enabled = true;
                    if (transform.gameObject.name.StartsWith("T_")) return;

                        _selectedObjectStatusPopup.gameObject.SetActive(false);

                    if (_transformGizmo != null)
                        RTGizmosEngine.Get.RemoveGizmo(_transformGizmo.Gizmo);
                }
             
            } 
        }

        private void MappingTransformInformation()
        {
            positionX = GameObject.Find("@positionX").GetOrAddComponent<TextMeshProUGUI>();
            positionY = GameObject.Find("@positionY").GetOrAddComponent<TextMeshProUGUI>();
            positionZ = GameObject.Find("@positionZ").GetOrAddComponent<TextMeshProUGUI>();

            rotationX = GameObject.Find("@rotationX").GetOrAddComponent<TextMeshProUGUI>();
            rotationY = GameObject.Find("@rotationY").GetOrAddComponent<TextMeshProUGUI>();
            rotationZ = GameObject.Find("@rotationZ").GetOrAddComponent<TextMeshProUGUI>();

            scaleX = GameObject.Find("@scaleX").GetOrAddComponent<TextMeshProUGUI>();
            scaleY = GameObject.Find("@scaleY").GetOrAddComponent<TextMeshProUGUI>();
            scaleZ = GameObject.Find("@scaleZ").GetOrAddComponent<TextMeshProUGUI>();
        }

        #region             // 원클릭 여부 확인
        long Firsttime = 0;   // 첫번째 클릭시간
        private bool One_Click()
        {
            long CurrentTime = DateTime.Now.Ticks;
            if (CurrentTime - Firsttime < 4000000) // 0.4초 ( MS에서는 더블클릭 평균 시간을 0.4초로 보는거 같다.)
            {
                Firsttime = CurrentTime;   // 더블클릭 또는 2회(2회, 3회 4회...)클릭 시 실행되지 않도록 함
                return false;   // 더블클릭 됨
            }
            else
            {
                Firsttime = CurrentTime;   // 1번만 실행되도록 함
                return true;   // 더블클릭 아님
            }
            
        }
        #endregion

        private void Start()
        {
            transform.tag = selectableTag;
            _selectedObjectStatusPopup = transform.GetComponentInChildren<UI_SelectedObjectStatusPopup>();
            isSelected = false;
            print(gameObject.name);
        }

    

        public void MoveGizmo()
        {
            if (State != state.none && _transformGizmo != null)
            {
                RTGizmosEngine.Get.RemoveGizmo(_transformGizmo.Gizmo);
                State = state.none;
                MoveGizmo();
            }
            else
            {
                _transformGizmo = RTGizmosEngine.Get.CreateObjectMoveGizmo();
                _transformGizmo.SetTargetObject(transform.gameObject);
                _transformGizmo.Gizmo.MoveGizmo.SetVertexSnapTargetObjects(new List<GameObject> { transform.gameObject });
                _transformGizmo.SetTransformSpace(GizmoSpace.Local);
                State = state.move;
            }
        }
        public void RotationGizmo()
        {

            if (State != state.none && _transformGizmo != null)
            {
                RTGizmosEngine.Get.RemoveGizmo(_transformGizmo.Gizmo);
                State = state.none;
                RotationGizmo();
            }
            else
            {

                _transformGizmo = RTGizmosEngine.Get.CreateObjectRotationGizmo();
                _transformGizmo.SetTargetObject(transform.gameObject);
                // _transformGizmo.Gizmo.MoveGizmo.SetVertexSnapTargetObjects(new List<GameObject> {transform.gameObject}); 
                _transformGizmo.SetTransformSpace(GizmoSpace.Local);
                State = state.rotation;
            }
        }
        public void ScaleGizmo()
        {
            if (State != state.none && _transformGizmo != null)
            {
                RTGizmosEngine.Get.RemoveGizmo(_transformGizmo.Gizmo);
                State = state.none;
                ScaleGizmo();
            }
            else
            {

                _transformGizmo = RTGizmosEngine.Get.CreateObjectScaleGizmo();
                _transformGizmo.SetTargetObject(transform.gameObject);
                // _transformGizmo.Gizmo.MoveGizmo.SetVertexSnapTargetObjects(new List<GameObject> {transform.gameObject}); 
                _transformGizmo.SetTransformSpace(GizmoSpace.Local);

                State = state.scale;
            }
        }
        
        private void Update()
        {
            if(isSelected)
            {

                if (transform.hasChanged)
                {
                    // 소수점 둘째자리까지 표현
                    positionX.text = string.Format("{0:0.##}", transform.position.x);
                    positionY.text = string.Format("{0:0.##}", transform.position.y);
                    positionZ.text = string.Format("{0:0.##}", transform.position.z);


                    // RotatlocalEulerAngles 이용하여 계산 
                    float rotateAngleX = transform.localEulerAngles.x;
                    rotateAngleX = (rotateAngleX > 180) ? rotateAngleX - 360 : rotateAngleX;
                    float rotateAngleY = transform.localEulerAngles.y;
                    rotateAngleY = (rotateAngleX > 180) ? rotateAngleY - 360 : rotateAngleY;
                    float rotateAngleZ = transform.localEulerAngles.z;
                    rotateAngleZ = (rotateAngleZ > 180) ? rotateAngleZ - 360 : rotateAngleZ;

                    rotationX.text = string.Format("{0:0.##}", rotateAngleX);
                    rotationY.text = string.Format("{0:0.##}", rotateAngleY);
                    rotationZ.text = string.Format("{0:0.##}", rotateAngleZ);

                    scaleX.text = string.Format("{0:0.##}", transform.localScale.x);
                    scaleY.text = string.Format("{0:0.##}", transform.localScale.y);
                    scaleZ.text = string.Format("{0:0.##}", transform.localScale.z);
                }
         
            }

            if (Input.GetKey("c") && One_Click())
                isSelected = !isSelected;

            if (Input.GetKey(KeyCode.Delete) && One_Click())
            {

                if (_transformGizmo != null)
                    RTGizmosEngine.Get.RemoveGizmo(_transformGizmo.Gizmo);

                Managers.Resource.Destroy(transform.root.gameObject);
            }
        
        }

        private void LateUpdate()
        {
            if(transform.gameObject.name.StartsWith("P_"))
                transform.LookAt(Camera.main.transform);
        }
    }
}