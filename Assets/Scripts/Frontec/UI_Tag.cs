using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;

namespace Noah
{
    public class UI_Tag : UI_Base
    {
        enum Texts
        {

        }



        public override bool Init()
        {
            if (base.Init() == false)
                return false;

           // BindButton(typeof(Buttons));
            // GetButton((int)Buttons.BuyButton).gameObject.BindEvent(OnClickButton);
            // GetButton((int)Buttons.AdsButton).gameObject.BindEvent(OnClickButton);
            BindText(typeof(Texts));

            RefreshUI();

            return true;
        }


        void RefreshUI()
        {
            if (_init == false)
                return;

            //Todo 초기화 설정     

        }

        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}


