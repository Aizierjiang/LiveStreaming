using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*by Aizierjiang*/



/*


    using System.Collections;
using System.Collections.Generic;


public class ServerURL
{
    //public static readonly string BASE_URL = "http://172.26.5.56:8080/";
    //public static readonly string WSS_URL = "ws://172.26.5.56:8383";
    public static string BASE_URL = GetHTTPBaseUrl();
    //{
    //    get
    //    {
    //        return GetHTTPBaseUrl();
    //    }
    //}
    public static string WSS_URL
    {
        get
        {
            return GetWebSocketBaseUrl();
        }
    }

    public static readonly string NET_CHECK = BASE_URL + "net_check";
    public static readonly string REGISTER = BASE_URL + "register";
    public static readonly string LOGIN = BASE_URL + "log_in";
    public static readonly string CLIENT_LOG = BASE_URL + "client_log";
    public static readonly string IMG_BASE = BASE_URL + "img_base";
    public static readonly string NEW_SHOW = BASE_URL + "new_show";
    public static readonly string UPDATE_SHOW_STATE = BASE_URL + "update_show_state";
    public static readonly string GET_SHOWLIST = BASE_URL + "get_showlist"; // GET all the show list(back-end returns the online shows de facto) 
    public static readonly string GET_ONLINE_LIST = BASE_URL + "get_online"; // GET online user

    public static readonly string RANDOM_NAME = BASE_URL + "random_name";
    public static readonly string LOOK_UP = BASE_URL + "look_up";



    private static string GetServerIPFromConfig()
    {
        return "";
    }

    private static string GetHTTPServerPortFromConfig()
    {
        return "";
    }

    private static string GetWebSocketServerPortFromConfig()
    {
        return "";
    }

    private static string GetHTTPBaseUrl()
    {
        string result = "http://" + GetServerIPFromConfig() + GetHTTPServerPortFromConfig();
        return result;
    }

    private static string GetWebSocketBaseUrl()
    {
        string result = "ws://" + GetServerIPFromConfig() + GetWebSocketServerPortFromConfig();
        return "";
    }
}







Debugables:
    //Debug.Log(jsonResult["OnlineList"][0]["time"].GetString());
        //Debug.Log(jsonResult["OnlineList"][0]["img_url"]);




    
      private List<GameObject> m_videoObjects = new List<GameObject>();
    private int m_currentPage = 0;
    private int m_videosEachPage = 0;
    private int m_totalPages = 0;

    private List<ShowInfo> m_showList = new List<ShowInfo>();

    private void OnEnable()
    {
        m_videoObjects.Clear();
        for (int i = 0; i < transform.childCount; i++)
            m_videoObjects.Add(transform.GetChild(i).gameObject);

        SendShowListRequestAsync(xxx);

    }

    async void SendShowListRequestAsync(string url)
    {
        m_showList.Clear();

        JsonData bodyData = new JsonData();
        bodyData["pageNum"] = 1;
        bodyData["pageSize"] = 100;

        string response = await HttpRequest.Instance.PostJsonAsync(url, bodyData.ToJson());
        if (response == "")
            return;
        JsonData jsonResult = JsonMapper.ToObject(response);
        Debug.Log("result is : " + response);

        if (jsonResult["success"].Equals(null) || !jsonResult["success"].GetBoolean())
        {
            Debug.LogWarning("An error occurred at sever side!");
        }
        else
        {

            foreach (JsonData showJson in jsonResult["data"]["list"])
            {
                ShowInfo showInfo = new ShowInfo();
                showInfo.id = showJson["id"].GetString();
                showInfo.title = showJson["title"].GetString();
                showInfo.actorID = showJson["actorId"].GetString();
                if (showJson["cover"] != null) //封面可能为空
                    showInfo.coverUrl = showJson["cover"].GetString();
                showInfo.description = showJson["description"].GetString();
                showInfo.startTime = System.DateTime.Parse(showJson["startTime"].GetString());
                showInfo.endTime = System.DateTime.Parse(showJson["endTime"].GetString());
                showInfo.status = showJson["status"].GetString();
                if (showJson["background"] != null)
                    showInfo.background = showJson["background"].GetString();
                if (showJson["optStatus"] != null)
                    showInfo.optStatus = showJson["optStatus"].GetString();
                foreach (JsonData actorPosition in showJson["showSceneRes"]["actorPositionResList"])
                {
                    ActorPosInfo actorPos = new ActorPosInfo();
                    actorPos.id = actorPosition["positionId"].GetString();
                    actorPos.position = new Vector3(actorPosition["actorPositionX"].GetNatural() / 1000.0f,
                        actorPosition["actorPositionY"].GetNatural() / 1000.0f,
                        -1 * actorPosition["actorPositionZ"].GetNatural() / 1000.0f);//threejs 跟Unity Z轴相反
                    showInfo.actorPosList.Add(actorPos);
                }

                m_showList.Add(showInfo);
            }
        }

        m_currentPage = 0;
        m_videosEachPage = m_videoObjects.Count;
        m_totalPages = m_showList.Count % m_videosEachPage == 0 ? m_showList.Count / m_videosEachPage : m_showList.Count / m_videosEachPage + 1;
        UpdatePage(m_currentPage);
    }



    private void UpdatePage(int currentPage)
    {
        DisableAllVideos();
        Debug.Log("UpdatePage " + currentPage);
        int startIndex = currentPage * m_videosEachPage;
        int endIndex = m_showList.Count - startIndex > m_videosEachPage ? startIndex + m_videosEachPage : m_showList.Count;
        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject video = m_videoObjects[i % m_videosEachPage];
            video.GetComponent<ShowInfoLoader>().Load(m_showList[i]);
            video.SetActive(true);
        }
    }

    private void DisableAllVideos()
    {
        for (int i = 0; i < m_videoObjects.Count; i++)
            m_videoObjects[i].SetActive(false);
    }











    
    枚举类型判断需要使用hasFlag判断而非Equals判断,如下!
    
     switch (heartBeatState)
        {
            case HeartBeatState.ShouldBeat:
                Debug.Log("thread state:    "+ heartBeatThread.ThreadState);
                if (heartBeatThread.ThreadState.HasFlag(ThreadState.Unstarted))
                {
                        Debug.Log("ustarted and now started!!!!");
                        heartBeatThread.Start();
                }
                if (heartBeatThread.ThreadState.HasFlag(ThreadState.Suspended))
                {
                        Debug.Log("######################Suspended and now resumed!!!!");
                        heartBeatThread.Resume();
                }
                //switch (heartBeatThread.ThreadState)
                //{
                //    case ThreadState.Unstarted:
                //        Debug.Log("ustarted and now started!!!!");
                //        heartBeatThread.Start();
                //        break;
                //    case ThreadState.Suspended:
                //        Debug.Log("Suspended and now resumed!!!!");
                //        heartBeatThread.Resume();
                //        break;
                //    default:
                //        Debug.Log("default type !!!!");
                //        break;
                //}
                if (!heartBeatThread.ThreadState.HasFlag(ThreadState.Background))
                {
                    Debug.Log("backgrounded !!!!");
                    heartBeatThread.IsBackground = true;
                }
                break;
            case HeartBeatState.ShouldPause:
                if (heartBeatThread.IsAlive && !heartBeatThread.ThreadState.Equals(ThreadState.Suspended))
                {
                    Debug.Log("suspended / paused !!!!");
                    heartBeatThread.Suspend();
                }
                break;
            case HeartBeatState.ShouldDie:
                if (heartBeatThread.IsAlive)
                {
                    Debug.Log("dead !!!!");
                    heartBeatThread.Abort();
                }
                break;
            default:
                // never happens
                break;
        }
    }
    
    
    
    
    
    
    
          Debug.Log($"non-trimed : {jsonResult["Calling"].ToJson().ToString()}");
                Debug.Log($"trimed boolean : {jsonResult["Calling"].ToJson().ToString().Trim('"')}");
                Debug.Log($"data to send: {data2Send.ToJson()}");

 * 

                Debug.Log($"non-trimed : {jsonResult["Calling"].ToJson().ToString()}");
                Debug.Log($"trimed boolean : {jsonResult["Calling"].ToJson().ToString().Trim('"')}");
                Debug.Log($"data to send: {data2Send.ToJson()}");



 * 
 * *                 StateController.Instance.State = jsonResult["UserState"].ToJson().ToString().Trim('"');
                data2Send["UserState"] = StateController.Instance.State; // Chatting
                data2Send["Calling"] = jsonResult["Calling"].ToJson().ToString().Trim('"'); // false
                data2Send["Called"] = jsonResult["Called"].ToJson().ToString().Trim('"'); // true
                data2Send["CallResult"] = CallResult.StandStill;
                data2Send["CallerUUID"] = jsonResult["CallerUUID"].ToJson().ToString().Trim('"');
                data2Send["CalleeUUID"] = jsonResult["CalleeUUID"].ToJson().ToString().Trim('"'); // or directly got from this ctx
                data2Send["CalleeStreamAddress"] = jsonResult["CalleeStreamAddress"].ToJson().ToString().Trim('"');
                data2Send["CallerStreamAddress"] = jsonResult["CallerStreamAddress"].ToJson().ToString().Trim('"');
                data2Send["TestHead!!!!!!!"] = "**************this is a test head*****************";
    
    
    
    
    
    
    
    
    
    
    
    
    Unity中的单例模式与协程冲突


using UnityEngine;
using LitJson;
using System.Text;
using System.Collections;


//public class HeartBeatController : MonoBehaviour
//{
//    private static object m_mutex = new object();
//    private static bool m_initialized = false;
//    //private static HeartBeatController heartBeatController = null;
//    private static HeartBeatController heartBeatController;
//    private bool shouldHeartBeat = false;

//    //test
//    //private void OnEnable()
//    //{
//    //    ControllHeartBeat(true);
//    //}

//    //public static HeartBeatController Instance
//    //{
//    //    get
//    //    {
//    //        if (!m_initialized)
//    //        {
//    //            lock (m_mutex)
//    //            {
//    //                if (heartBeatController == null)
//    //                {
//    //                    heartBeatController = new HeartBeatController();
//    //                    m_initialized = true;
//    //                }
//    //            }
//    //        }
//    //        return heartBeatController;
//    //    }
//    //}


//    public static HeartBeatController Instance
//    {
//        get
//        {
//            heartBeatController = new HeartBeatController();
//            return heartBeatController;
//        }
//    }


//    public void Test()
//    {
//        new Logger.Log("****************");
//    }


//    public void ControllHeartBeat(bool shouldBeat)
//    {
//        shouldHeartBeat = shouldBeat;

//        if (shouldBeat)
//            StartCoroutine(HeartBeatCoroutine());
//        else
//            StopCoroutine(HeartBeatCoroutine());
//    }

//    IEnumerator HeartBeatCoroutine()
//    {
//        JsonData heartBeatReq = new JsonData();
//        heartBeatReq["HeartBeat"] = true;
//        heartBeatReq["ResponseCode"] = "NULL";
//        heartBeatReq["UserState"] = UserState.Online;
//        //heartBeatReq["Calling"] = false;
//        //heartBeatReq["Called"] = false;
//        //heartBeatReq["CallResult"] = CallResult.StandStill;
//        //heartBeatReq["CallerUUID"] = JsonMapper.ToObject(PageManager.nickNameFromServer)["fullName"].ToJson().ToString();
//        //heartBeatReq["CalleeUUID"] = "*********";
//        //heartBeatReq["StreamAddress"] = "rtsp://alexander:soft@" + Toolip.GetMyIPAddress() + ":8554";

//        while (shouldHeartBeat)
//        {
//            yield return new WaitForSeconds(1f);
//            if (WebSocketRequestSingleton.getInstance().ws.GetState().ToString().Equals("Open"))
//            {
//                WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(heartBeatReq.ToJson().ToString()));
//            }
//        }
//    }
//}




     */



//data2Send["HeartBeat"] = true;
//data2Send["ResponseCode"] = "NULL";
//data2Send["UserState"] = UserState.Online;
//data2Send["Calling"] = false;
//data2Send["Called"] = true;
//data2Send["CallResult"] = CallResult.StandStill;
//data2Send["CallerUUID"] = JsonMapper.ToObject(uuidFromHTTPServer)["fullName"].ToJson().ToString();
//data2Send["CalleeUUID"] = "*********";
//data2Send["StreamAddress"] = "rtsp://alexander:soft@" + Toolip.GetMyIPAddress() + ":8554";


/*

1. 当我连着手机充电的时候，启动模拟器调试，执行ADB指令时，报错。
```bash
C:\Users\Aizierjiang>adb shell
error: more than one device and emulator
C:\Users\Aizierjiang>adb install e:\good.apk
error: more than one device and emulator
```

2. 碰到这种情况，首先要查一下，是不是真的有多个设备或模拟器。
```bash
C:\Users\Aizierjiang>adb devices
List of devices attached
emulator-5554       device
4dfadcb86b00cf05    device
```

3. 发现还真是多个设备，那就需要为ADB命令指定设备的序列号了。
```bash
C:\Users\Aizierjiang>adb -s emulator-5554 shell
```

4. 也就是如上所示，给命令加上-s的参数就可以了！

5. 如果实际上只有一个设备或模拟器，并且查到有offline的状态；那就说明是ADB本身的BUG所导致的，就需要用如下的方法处理下了：
```bash
C:\Users\Aizierjiang>adb kill-server
C:\Users\Aizierjiang>taskkill /f /im adb.exe
```

5. 第一条命令是杀ADB的服务，第二条命令是杀ADB的进程！
6. *如果第一条没有用，才考虑用第二条命令再试试看的！* 
*/



/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayVideoOnRawImg : MonoBehaviour
{
    public List<GameObject> rawWebCam = new List<GameObject>();

    WebCamTexture[] webCamTextures;
    WebCamDevice[] webCamDevice;

    Color32[] data;

    Texture2D texture2D;


    IEnumerator OpenCamera()
    {

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam); //等待用户允许访问

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            webCamDevice = WebCamTexture.devices; //先获取设备

            webCamTextures = new WebCamTexture[webCamDevice.Length];
            for (int i = 0; i < webCamDevice.Length; i++)
            {
                webCamTextures[i] = new WebCamTexture(webCamDevice[i].name);
                webCamTextures[i].requestedHeight = 1080;
                webCamTextures[i].requestedWidth = 1920;
            }

            rawWebCam[0].gameObject.GetComponent<RawImage>().texture = webCamTextures[0];

            webCamTextures[0].Play();
            data = new Color32[webCamTextures[0].width * webCamTextures[0].height];
            // texture2D = new Texture2D(webCamTextures[0].width, webCamTextures[0].height);
            texture2D = new Texture2D(webCamTextures[0].width, webCamTextures[0].height, TextureFormat.RGBA32, false, false);

        }
    }


    void Start()
    {
        StartCoroutine("OpenCamera");
    }

    void LateUpdate()
    {
        if (rawWebCam[0].GetComponent<RawImage>().texture != null)
        {
            // Texture2D texture2D = Utils.BytesToTex2D(
            //     Utils.ConvertColor32ArrayToByteArray(data));
            // rawWebCam[1].gameObject.GetComponent<RawImage>().texture = texture2D;
            // Debug.Log("0: Pixels count --- " + webCamTextures[0].GetPixels32().Length);
            // Debug.Log("0: Bytes count --- " + Utils.Color32ArrayToByteArray(data).Length);


            // Texture2D texture2D = new Texture2D(webCamTextures[0].width, webCamTextures[0].height, TextureFormat.RGBA32, true, true); //Utils.ConvertByteArrayToColor32Array(Utils.ConvertColor32ArrayToByteArray(data))
            // Texture2D texture2D = new Texture2D(webCamTextures[0].width, webCamTextures[0].height, TextureFormat.ARGB32, true, true); //Utils.ConvertByteArrayToColor32Array(Utils.ConvertColor32ArrayToByteArray(data))
            // texture2D.LoadRawTextureData(Utils.ConvertColor32ArrayToByteArray(webCamTextures[0].GetPixels32(data)));
            // texture2D.SetPixels32(webCamTextures[0].GetPixels32(data));
            // texture2D.SetPixels32(data);
            texture2D.SetPixels32(Utils.ConvertByteArrayToColor32Array(Utils.ConvertColor32ArrayToByteArray(webCamTextures[0].GetPixels32(data))));

            rawWebCam[1].gameObject.GetComponent<RawImage>().texture = texture2D as Texture;


            Debug.Log("WebCamTextures: " + data.Length);
            Debug.Log("WebCamTextures: " + Utils.ConvertByteArrayToColor32Array(Utils.ConvertColor32ArrayToByteArray(data)).Length);
            Debug.Log("WebCamTextures: " + Utils.ConvertColor32ArrayToByteArray(webCamTextures[0].GetPixels32(data)).Length);
            // Debug.Log("WebCamTextures: " + data);
            // Debug.Log("WebCamTextures: " + Utils.Color32ArrayToByteArray(data));
            // Debug.Log("WebCamTextures: " + Utils.ConvertColor32ArrayToByteArray(data));
        }
    }

    void OnDestroy()
    {
        StopCoroutine("OpenCamera");
    }
}


*/
public class Trashes : MonoBehaviour
{

    public bool isNetworkAvailable()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                //网络断开
                Debug.Log("Network is unconnected!");
                return false;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                //WIFI
                Debug.Log("Network is now WIFI!");
                return true;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                //4G/3G
                Debug.Log("Network is now 3G/4G!");
                return true;
            default:
                //TODO:Send request to my own server to check the connection
                Debug.Log("Default network check!");
                return false;
        }
    }

    void Start() { }


    private byte[] GetBytesFromTexture(Texture source)
    {
        source = source as Texture2D;

        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        //这里可以转 JPG PNG EXR， Unity都封装了固定的Api
        byte[] bytes = readableText.EncodeToPNG();
        return bytes;
    }



    void Update()
    {
        // Texture2D texture2D = rawWebCam[0].gameObject.GetComponent<RawImage>().texture as Texture2D;
        // byte[] streamBytes = texture2D.EncodeToPNG();
        // Debug.Log("Pixels --- " + streamBytes[0]);


        //  if (Input.GetKeyDown(KeyCode.I))
        // {
        //     Debug.Log("Pixels --- " + Utils.Color32ArrayToByteArray(webCamTextures[0].GetPixels32()).Length);
        //     for (int i = 100; i < 1000; i++)
        //     {
        //         new Log("&&:  " + Utils.Color32ArrayToByteArray(webCamTextures[0].GetPixels32())[i]);
        //     }
        // }
    }
}
