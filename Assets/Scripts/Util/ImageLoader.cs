using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Noah;



public class ImageLoader : MonoBehaviour
{
    RawImage rawImage;


    private void OnEnable()
    {
        //GameObject.Utils.GetOrAddComponent<RawImage>()
        CoroutineManager.Start(Utils.TextureLoad("https://file.mk.co.kr/meet/neds/2020/01/image_readtop_2020_17627_15783372624040620.jpg", gameObject.name), "test");
    }




    private void OnDisable()
    {
        
    }





}
