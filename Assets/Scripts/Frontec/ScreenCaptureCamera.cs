using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class ScreenCaptureCamera : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    RawImage RawImage;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        RawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame

    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }

    public void  OnCaptureScreen()
    {
        RawImage.texture = RTImage(camera);
    }

    private void LateUpdate()
    {
        if(Input.GetKey(KeyCode.Escape))
            OnCaptureScreen();
    }
}
