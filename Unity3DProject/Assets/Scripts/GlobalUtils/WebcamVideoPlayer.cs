using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*by Aizierjiang*/

public class WebcamVideoPlayer : MonoBehaviour
{
    public List<GameObject> rawWebCam = null;


    private WebCamTexture[] webCamTextures;
    private WebCamDevice[] webCamDevice;
    private Color32[] color32Data;

    private bool isAuthorized = false;

    private int webCamCount = 0;


    void Awake() { }

    void OnEnable()
    {
        if (!isAuthorized)
        {
            StartCoroutine("GetCameraDevice");
        }
        else
        {
            //StartWebCam(0);
            StartAllWebCams();
        }
    }

    void Start() { }

    void Update() { }

    void OnDisable()
    {
        StopAllWebCams(); // crucial to stop wihle going out
    }

    void OnDestroy()
    {
        CleanWebCams();
    }




    IEnumerator GetCameraDevice()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam); //等待用户允许访问

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            webCamDevice = WebCamTexture.devices; //先获取设备

            webCamTextures = new WebCamTexture[webCamDevice.Length];
            for (int i = 0; i < webCamDevice.Length; i++)
            {
                if (!webCamDevice[i].name.ToLower().Trim().Contains("virtual")) // ignore virtual cameras
                {
                    webCamTextures[i] = new WebCamTexture(webCamDevice[i].name);
                    webCamTextures[i].requestedHeight = 1080;
                    webCamTextures[i].requestedWidth = 1920;
                    rawWebCam[i].gameObject.GetComponent<RawImage>().texture = webCamTextures[i];
                    StartWebCam(i);
                    rawWebCam[i].GetComponent<RawImage>().color = new Color(255f, 255f, 255f, 255f); // whiten image for clearence 
                }
            }
            isAuthorized = true;
        }
    }

    void StartWebCam(int i)
    {
        if (i < 0 || i > webCamTextures.Length - 1)
        {
            Debug.LogError($"The camera with index {i} is outside the valid range, please check your index!");
        }
        else
        {
            if (webCamDevice[i].name.ToLower().Trim().Contains("virtual"))
            {
                Debug.Log($"The camera with index {i} is virtual, please check your index!");
            }
            else
            {
                webCamTextures[i].Play();
                color32Data = new Color32[webCamTextures[i].width * webCamTextures[i].height];
                webCamCount++;
            }
        }
    }

    void StartAllWebCams()
    {
        for (int i = 0; i < webCamTextures.Length; i++)
            if (!webCamDevice[i].name.ToLower().Trim().Contains("virtual")) // ignore virtual cameras
                webCamTextures[i].Play();
    }

    void StopAllWebCams()
    {
        for (int i = 0; i < webCamTextures.Length; i++)
            if (!webCamDevice[i].name.ToLower().Trim().Contains("virtual")) // ignore virtual cameras
                webCamTextures[i].Stop();
    }

    void CleanWebCams()
    {
        StopAllWebCams();
        rawWebCam = null;
        StopCoroutine("GetCameraDevice");
    }

    void DebugMsg()
    {
        Debug.Log("Length of color32[] data: " + color32Data.Length);
        Debug.Log("Length of converted color32[] data: " + Utils.ConvertByteArrayToColor32Array(Utils.ConvertColor32ArrayToByteArray(color32Data)).Length);
        Debug.Log("Length of byte[] data: " + Utils.ConvertColor32ArrayToByteArray(webCamTextures[0].GetPixels32(color32Data)).Length);
    }
}



