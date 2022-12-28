using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Define;

public class Utils
{
    public static byte[] TextureToPng(Texture texture)
    {
        int width = texture.width;
        int height = texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);
        Graphics.Blit(texture, copiedRenderTexture);
        RenderTexture.active = copiedRenderTexture;

        Texture2D tex2D = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex2D.Apply();

        RenderTexture.active = currentRenderTexture;

        return tex2D.EncodeToPNG();
    }



    public static Texture2D toTexture2D(Texture texture)
    {
        int width = texture.width;
        int height = texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);
        Graphics.Blit(texture, copiedRenderTexture);
        RenderTexture.active = copiedRenderTexture;

        Texture2D tex2D = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex2D.Apply();

        return tex2D;
    }
    public static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(900, 600, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public static Texture2D ChangeFormat(Texture2D oldTexture, TextureFormat newFormat)
    {
        //Create new empty Texture
        Texture2D newTex = new Texture2D(450, 300, newFormat, false);
        //Copy old texture pixels into new one
        newTex.SetPixels(oldTexture.GetPixels());
        //Apply
        newTex.Apply();
        return newTex;
    }


    public static IEnumerator Co_GetText(string ip, System.Action<string> OnCompleteDownload)
    {
        UnityWebRequest www = UnityWebRequest.Get(ip);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            var result = www.downloadHandler.text;
            OnCompleteDownload(result);

        }
    }

    public static IEnumerator Co_UploadImage(Texture2D tex, string ip, string imageName, string description, System.Action<string> OnCompleteUpload)
    {
        
        byte[] imageBytes = tex.EncodeToPNG();
        byte[] stringbytes = Encoding.Default.GetBytes($"{imageName}.png");
        //multipartForm
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", imageBytes, $"{imageName}.png", "image/png"));
        formData.Add(new MultipartFormDataSection("imageName", stringbytes, "text/plain"));
        formData.Add(new MultipartFormDataSection("name", imageName, "text/plain"));
        formData.Add(new MultipartFormDataSection("description", description, "text/plain"));

        using (var unityWebRequest = UnityWebRequest.Post(ip, formData))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Failed to upload {tex.name}: {unityWebRequest.result} - {unityWebRequest.error}");
            }
            else
            {
                Debug.Log($"Finished Uploading {tex.name}");
                var result = unityWebRequest.downloadHandler.text;
                OnCompleteUpload(result);
            }
        }
    }

    public static IEnumerator Co_UploadImage(byte[] bytes, string ip, string imageName, System.Action<string> OnCompleteUpload)
    {
        var form = new WWWForm();
        form.AddBinaryData("file", bytes, "test", "image/png");
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(ip, form))
        {
            yield return unityWebRequest.SendWebRequest();
        
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Failed to upload {imageName}: {unityWebRequest.result} - {unityWebRequest.error}");
            }
            else
            {
                Debug.Log($"Finished Uploading {imageName}");
                var result = unityWebRequest.downloadHandler.text;
                OnCompleteUpload(result);
            }
        
        }
    }


    public static IEnumerator Co_GetTexture(string ip, string id, System.Action<Texture> OnCompleteDownload)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture($"{ip}/{id}");
        //  UnityWebRequest www = UnityWebRequestTexture.GetTexture($"http://20.196.219.147:3030/api/project/{projectNameWithExtension}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //receivedImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            OnCompleteDownload(((DownloadHandlerTexture)www.downloadHandler).texture);
        }
    }

    public static IEnumerator Co_Delete(string ip, string id, System.Action<Texture> OnCompleteDownload)
    {
        UnityWebRequest www = UnityWebRequest.Delete($"{ip}/{id}");
        //  UnityWebRequest www = UnityWebRequestTexture.GetTexture($"http://20.196.219.147:3030/api/project/{projectNameWithExtension}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //receivedImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            OnCompleteDownload(((DownloadHandlerTexture)www.downloadHandler).texture);
        }
    }


    public static IEnumerator TextureLoad(string _url, string goName)
	{
		string url = _url;
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.result);
		}
		else
		{
			//matching
			RawImage rawimg = GameObject.Find
				 (goName).GetOrAddComponent<RawImage>();
			rawimg.texture = (www.downloadHandler as DownloadHandlerTexture).texture;
		}
	}


	public static T ParseEnum<T>(string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            Transform transform = go.transform.Find(name);
            if (transform != null)
                return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }




    public static string GetRewardValueString(int value)
	{
		string valueText = "";

		if (value > 0)
			valueText = $"+{value}";
		else
			valueText = $"{value}";

		return valueText;
	}

    public static Color GetRewardColor(RewardType type, int value)
	{
		// 스트레스는 줄어드는게 좋은거다
		if (type == RewardType.Stress)
		{
			if (value > 0)
				return new Color(0.08971164f, 0.5462896f, 0.9056604f);
			else
				return new Color(1.0f, 0, 0);
		}
		else
		{
			if (value < 0)
				return new Color(0.08971164f, 0.5462896f, 0.9056604f);
			else
				return new Color(1.0f, 0, 0);
		}
	}


    public static JobTitleType GetRandomNpc()
	{
        // 인턴, 신입 제외한 나머지에서 추출
        int randomCount = UnityEngine.Random.Range(2, JOB_TITLE_TYPE_COUNT);
        return (JobTitleType)randomCount;
	}


	public static string GetMoneyString(int value)
	{
		int money = value / 10000;
		return $"{money}만";
		//return string.Format("{0:0.0}만", value / 10000.0f);
	}
}
