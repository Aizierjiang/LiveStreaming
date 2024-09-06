using System.Text;
using System.Collections.Generic;
using UnityEngine;
using HybridWebSocket;
using LitJson;
using UnityEngine.UI;
using System.Collections;

/*by Aizierjiang*/

public class PageManager : MonoBehaviour
{
    public GameObject networkErrorToast = null;
    public GameObject callerToast = null; // get callers info and show calling
    public GameObject playerInCallerPage = null;
    public GameObject onlineList = null;
    public GameObject globalCanvas = null;
    public List<GameObject> pagesList = new List<GameObject>(); // last one is global canvas

    [SerializeField]
    private MediaPlayerCtrl m_mediaPlayerCtrl;
    public MediaPlayerCtrl mediaPlayerCtrl
    {
        set { m_mediaPlayerCtrl = value; }
    }
    public GameObject[] playerRawImages = null;

    private Toaster welcomePageToast = null;
    public static string uuidFromHTTPServer = null;
    //private bool isNetworkAvailable = false; // de facto http network check but related only to pushing streaming
    //temp
    public static bool isNetworkAvailable = false; // de facto http network check but related only to pushing streaming

    public static string calleeIP = "";
    public static string calleeUUID = "";
    public static bool gotIPToCall = false;

    JsonData jsonResult = new JsonData();

    private bool isCallerInfoToasted = false;
    bool shouldToastCalledMsg = false; string callerInfo = "";
    private const int timeToWaitFor = 10; // 60 seconds in life but 10 seconds in experiment

    bool shouldCallingCoroutineStop = false;
    bool shouldBackToMainPage = false;
    bool shouldInActivateGlobalCanvas = false;
    bool shouldActivatePlayerInCallerCanvasPage = false;

    #region 为了迎合推流前上传StreamInfo的需求，特地将本类进行单例化，本单例只与StreamInfo面板有关。
    // Unity中的单例模式用法
    public PageManager() { } // 公开，否则外界接用不了static方法
    private static PageManager m_instance = null; // 不要实例化! 因为继承自MonoBehaviour.
    public static PageManager Get()
    {
        // 只再第一次的时候查找并获取其身上的组件并绑定
        if (PageManager.m_instance == null) m_instance = GameObject.Find("PageManager").GetComponent<PageManager>();
        // 返回已经绑定过的组件;注意销毁时候置空
        return m_instance;
    }
    #endregion

    void Awake()
    {
        CheckNetWorkState(ConnectToWebSocketServer);
        LoadWelcomePage();
    }

    void Start()
    {
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldBeat);
    }

    void Update()
    {
        DetectUserInteration();

        // besides are for Non-Main-Thread to be Main-Thread 
        NonMainThreadToBeMainThread();

        CallControlledInOtherContext();
    }

    private void NonMainThreadToBeMainThread()
    {
        if (shouldToastCalledMsg)
        {
            ToastCallerInfo(callerInfo);
        }

        if (shouldCallingCoroutineStop)
        {
            StopCoroutine("Calling");
            shouldCallingCoroutineStop = false;
        }

        if (shouldBackToMainPage)
        {
            BackToMainPage();
            shouldBackToMainPage = false;
        }

        if (shouldInActivateGlobalCanvas)
        {
            globalCanvas.SetActive(false);
            shouldInActivateGlobalCanvas = false;
        }

        if (shouldActivatePlayerInCallerCanvasPage)
        {
            playerInCallerPage.SetActive(true);
            shouldActivatePlayerInCallerCanvasPage = false;
        }
    }


    private void StartWebsocketMsgListening()
    {
        WhenWebsocketGetMessage();
        WhenWebsocketClosed();
    }

    public void InactivateAllPages()
    {
        //playerRawImages[0].SetActive(false);

        if (onlineList.activeSelf)
            onlineList.SetActive(false);
        if (globalCanvas.activeSelf)
            globalCanvas.SetActive(false);
        playerInCallerPage.SetActive(false);
        foreach (var page in pagesList)
        {
            if (page.activeInHierarchy)
                page.SetActive(false);
        }
    }

    public void LoadWelcomePage()
    {
        //Always initiate the pages state before loading
        InactivateAllPages();

        //Always activate the first page
        pagesList[0].SetActive(true);
        pagesList[0].AddComponent<Toaster>();
        welcomePageToast = pagesList[0].gameObject.GetComponent<Toaster>();
        welcomePageToast.toastMsg = pagesList[0];
        //welcomePageToast.Toast(1); // old welcome canvas page duration
        welcomePageToast.Toast(2);
        welcomePageToast.callback = () =>
       {
           //Then activate the second page
           pagesList[1].SetActive(true);

           if (!isNetworkAvailable)
               ToastNetworkError(60, true); // Toast and Exit when time is up
       };
    }


#if CLIENT_COPY
    public void CheckNetWorkState(System.Action callback)
    {
        uuidFromHTTPServer = "{ \"firstName\":\"Carleen\",\"middleName\":\"Jean\",  \"lastName\":\"Malti\", \"fullName\":\"Carleen.Jean.Malti\"    } ";
#else
    public async void CheckNetWorkState(System.Action callback)
    {
        uuidFromHTTPServer = await HttpRequest.Instance.GetAsync(ServerURL.RANDOM_NAME);
#endif
        Debug.Log("Default network  has been checked!");

        if (uuidFromHTTPServer != null)
        {
            isNetworkAvailable = true;
            callback();
        }
        else
        {
            isNetworkAvailable = false;
        }
    }

    public void StartPushing()
    {
        if (isNetworkAvailable)
        {
            ActivatePusherPage();
        }
        else
        {
            ToastNetworkError();
        }
    }

    private void InitiateMediaPlayerCtrl()
    {
        /// m_mediaPlayerCtrl.GetComponent<MediaPlayerCtrl>().Stop();  dangerous to use this for video stop initialization
        m_mediaPlayerCtrl.GetComponent<MediaPlayerCtrl>().enabled = false;
        m_mediaPlayerCtrl.m_strFileName = "";
    }

    public void StartPlaying(string streamAddress)
    {
        InitiateMediaPlayerCtrl();
        m_mediaPlayerCtrl.m_strFileName = streamAddress;
        m_mediaPlayerCtrl.m_TargetMaterial = playerRawImages;
        m_mediaPlayerCtrl.GetComponent<MediaPlayerCtrl>().enabled = true;
        playerRawImages[0].SetActive(true);
        ActivateClickedShowPlayerPage();
        Debug.Log("Started playing...");
    }

    public void StartCalling()
    {
        ActivateCallingPage();
        StartCoroutine("Calling");
    }


    public void ActivateOnlinListView()
    {
        if (!onlineList.activeSelf)
            onlineList.SetActive(true);
    }

    private void ONTHEWAYTOACTIVATEONLIETVIEW()
    {
        if (GlobalLoginState.isLogedIn)
        {
            if (!onlineList.activeSelf)
                onlineList.SetActive(true);
        }
        else
        {
            ToastNetworkError();
        }
    }

    public void InActivateOnlinListView()
    {
        if (onlineList.activeSelf)
            onlineList.SetActive(false);
    }

    private void CallControlledInOtherContext()
    {
        if (gotIPToCall)
        {
            StartCalling();
            gotIPToCall = false;
        }
    }

    private IEnumerator Calling()
    {
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldPause);

        JsonData data2Send = new JsonData();
        data2Send["HeartBeat"] = false;
        data2Send["ResponseCode"] = jsonResult["ResponseCode"].ToJson().ToString();
        data2Send["UserState"] = UserState.Chatting;
        data2Send["Calling"] = true;
        data2Send["Called"] = false;
        data2Send["CallResult"] = CallResult.StandStill;
        data2Send["CallerUUID"] = JsonMapper.ToObject(uuidFromHTTPServer)["fullName"].ToJson().ToString().Trim('"');
        data2Send["CalleeUUID"] = calleeUUID;
        data2Send["CallerStreamAddress"] = "rtsp://alexander:soft@" + Toolip.GetLocalIPAddress() + ":8554";
        data2Send["CalleeStreamAddress"] = "rtsp://alexander:soft@" + calleeIP + ":8554";

        //test
        //data2Send["CalleeUUID"] = "Carleen.Jean.Malti";
        //local test
        //data2Send["CalleeStreamAddress"] = "rtsp://alexander:soft@" + Toolip.GetMyIPAddress() + ":8555";
        //remote test
        //data2Send["CalleeStreamAddress"] = "rtsp://alexander:soft@10.26.6.73:8554";

        int timeCounter = 0;

        while (true)
        {
            if (WebSocketRequestSingleton.getInstance().ws.GetState().Equals(WebSocketState.Open) && !jsonResult.ToJson().Equals(""))
            {
                if (timeCounter > timeToWaitFor)
                {
                    data2Send["Calling"] = false;
                    WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));
                    BackToMainPage();
                    HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldBeat);
                    break;
                }
                else timeCounter++;


                // when CallResult is not null && CallerCanvas page is active
                if (jsonResult["CallResult"].ToJson().ToString().Trim('"').Equals(CallResult.StandStill) && pagesList[5].activeSelf.Equals(true))
                {
                    WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));
                    Debug.Log("Calling message is sent!!!");
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break;
                }
            }
            else
            {
                Debug.Log("Connection with websocket server is not not established!");
                yield return new WaitForSeconds(1f);
            }
        }
    }


    private void ReceptionTimeOut()
    {
        shouldToastCalledMsg = false;
        shouldInActivateGlobalCanvas = true;
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldBeat);
    }


    public void RejectCall()
    {
        shouldToastCalledMsg = false;
        globalCanvas.SetActive(false); // or inactivateAllPages when activating MainPage is needed

        JsonData data2Send = new JsonData();
        data2Send["HeartBeat"] = false;
        data2Send["ResponseCode"] = jsonResult["ResponseCode"].ToJson().ToString();
        data2Send["UserState"] = UserState.Online;
        data2Send["Calling"] = false;
        data2Send["Called"] = true;
        data2Send["CallResult"] = CallResult.Rejected;
        data2Send["CallerUUID"] = jsonResult["CallerUUID"].ToJson().ToString().Trim('"');
        data2Send["CalleeUUID"] = jsonResult["CalleeUUID"].ToJson().ToString().Trim('"'); // or directly got from this ctx
        //data2Send["CalleeStreamAddress"] = jsonResult["CalleeStreamAddress"].ToJson().ToString(); // no need to send
        //data2Send["CallerStreamAddress"] = jsonResult["CallerStreamAddress"].ToJson().ToString(); // no need to send

        WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldBeat);
    }

    public void AcceptCall()
    {
        shouldToastCalledMsg = false;

        JsonData data2Send = new JsonData();
        data2Send["HeartBeat"] = false;
        data2Send["ResponseCode"] = jsonResult["ResponseCode"].ToJson().ToString();
        data2Send["UserState"] = UserState.Chatting;
        data2Send["Calling"] = false;
        data2Send["Called"] = true;
        data2Send["CallResult"] = CallResult.Accepted;
        data2Send["CallerUUID"] = jsonResult["CallerUUID"].ToJson().ToString().Trim('"');
        data2Send["CalleeUUID"] = jsonResult["CalleeUUID"].ToJson().ToString().Trim('"'); // or directly got from this ctx
        data2Send["CalleeStreamAddress"] = jsonResult["CalleeStreamAddress"].ToJson().ToString().Trim('"'); // must send
        data2Send["CallerStreamAddress"] = jsonResult["CallerStreamAddress"].ToJson().ToString().Trim('"'); // must send

        WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));

        ActivateCallingPage(); // accept calling
        GetCallerStream.callerStreamAddress = jsonResult["CallerStreamAddress"].ToJson().ToString().Trim('"');
        shouldActivatePlayerInCallerCanvasPage = true;
    }

    public void ToastCallerInfo(string callerInfo)
    {
        globalCanvas.SetActive(true);

        callerToast.SetActive(true);
        callerToast.AddComponent<Toaster>();
        callerToast.GetComponentInChildren<Text>().text = callerInfo;
        callerToast.gameObject.GetComponent<Toaster>().toastMsg = callerToast;
        callerToast.gameObject.GetComponent<Toaster>().Toast(60); // a call duration is 60 seconds
        callerToast.gameObject.GetComponent<Toaster>().callback = () =>
        {
            callerToast.SetActive(false);
        };
    }

    public void ActivatePusherPage()
    {
        InactivateAllPages();
        pagesList[2].SetActive(true);
        pagesList[4].SetActive(true);
    }

    public void ActivateCallingPage(bool shouldActivate = true)
    {
        if (isNetworkAvailable)
        {
            InactivateAllPages();
            pagesList[5].SetActive(shouldActivate); // CallerCanvasPage
            pagesList[6].SetActive(shouldActivate); // InteractionForCallerCanvas
        }
        else
        {
            ToastNetworkError();
        }
    }

    public void ActivatePlayerPage()
    {
        InactivateAllPages();
        pagesList[3].SetActive(true);
        ///HeartBeatController.Instance.gUserState = UserState.Playing;
    }

    public void ActivateClickedShowPlayerPage()
    {
        InactivateAllPages();
        pagesList[7].SetActive(true);
        ///HeartBeatController.Instance.gUserState = UserState.Playing;
    }

    public void ToastNetworkError(int tostDuration = 3, bool shouldExit = false)
    {
        networkErrorToast.SetActive(true);
        networkErrorToast.AddComponent<Toaster>();
        networkErrorToast.gameObject.GetComponent<Toaster>().toastMsg = networkErrorToast;
        networkErrorToast.gameObject.GetComponent<Toaster>().Toast(tostDuration);
        networkErrorToast.gameObject.GetComponent<Toaster>().callback = () =>
        {
            networkErrorToast.SetActive(false);
            if (shouldExit) Exit();
        };
    }

    private void DetectUserInteration()
    {
        //用户按下退出键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //当且仅当在主页面时才关闭应用
            if (pagesList[1].activeSelf) Exit();
            //否则回退到上一个页面
            else BackToPreviousPage();
        }
    }

    private void BackToMainPage()
    {
        // 当退出Pushing页面时需要通知服务器更新推流状态为下线状态
        ///InformServerOfflineShow(); /// 不要将本代码放到MainPage被激活之后，否则会出现执行顺序问题。

        InactivateAllPages();
        pagesList[1].SetActive(true);
    }

    private void BackToPreviousPage()
    {
        // temp!!!暂时这样处置，以防出现MainPage中列表请求早于推流离线状态告知请求。
        // 当退出Pushing页面时需要通知服务器更新推流状态为下线状态
        InformServerOfflineShow();

        for (int i = 0; i < pagesList.Count; i++)
        {
            if (i.Equals(3))
            {
                // 当退出Pushing页面时需要通知服务器更新推流状态为下线状态
                InformServerOfflineShow();
            }
            if (i.Equals(5))
            {
                // 从Calling页面退出的时候告知服务器已下线
                SetUserStateOnline();
            }
            if (pagesList[i].activeSelf)
            {
                InactivateAllPages();
                // 当前为Play或Push或Calling页面时直接返回到主页面，并更新用户状态为Online
                // 3: pushing page
                // 5: playing page
                // 7: clicked show to play page
                if (i.Equals(3) || i.Equals(5) || i.Equals(7))
                {
                    pagesList[1].SetActive(true);
                }
                else pagesList[i - 1].SetActive(true);
            }
        }
    }

    private async void InformServerOfflineShow()
    {
        //JsonData firstName = JsonMapper.ToObject(uuidFromHTTPServer)["firstName"].GetString();
        //data2Send["name"] = firstName; // ! Must be in accordance with the back-end!!
        JsonData data2Send = new JsonData();
        data2Send["name"] = uuidFromHTTPServer; 
        data2Send["online"] = false;
        string result = await HttpRequest.Instance.PostJsonAsync(ServerURL.UPDATE_SHOW_STATE, data2Send.ToJson().ToString());
        Debug.Log("<color=orange>Informed!!!!!</color>");
    }

    private void ConnectToWebSocketServer()
    {
        WebSocketRequestSingleton.getInstance().ws.Connect();

        WebSocketRequestSingleton.getInstance().ws.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            JsonData jsonUUID = JsonMapper.ToObject(uuidFromHTTPServer);
            WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(jsonUUID["fullName"].GetString()));
        };
        StartWebsocketMsgListening();
    }


    private void WhenWebsocketGetMessage()
    {
        JsonData data2Send = new JsonData();
        int timeCounter = 0;
        WebSocketRequestSingleton.getInstance().ws.OnMessage += (byte[] data) =>
        {
            jsonResult = JsonMapper.ToObject(Encoding.UTF8.GetString(data));
            Debug.Log($"Msg from server: {jsonResult.ToJson()}");
            data2Send["ResponseCode"] = jsonResult["ResponseCode"].ToJson().ToString();

            if (System.Convert.ToInt32(jsonResult["ResponseCode"].ToJson().ToString()).Equals(200)) { /* continue heartbeatting */ }
            else if (!System.Convert.ToInt32(jsonResult["ResponseCode"].ToJson().ToString()).Equals(200)) { /* other logics */ }


            if (System.Convert.ToBoolean(jsonResult["Called"].ToJson().ToString()))
            {
                timeCounter++;

                HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldPause);

                callerInfo = jsonResult["CallerStreamAddress"].ToJson().ToString();
                shouldToastCalledMsg = true;

                data2Send["HeartBeat"] = false;
                StateController.Instance.State = jsonResult["UserState"].ToJson().ToString().Trim('"');
                data2Send["UserState"] = StateController.Instance.State; // Chatting
                data2Send["Calling"] = jsonResult["Calling"].ToJson().ToString().Trim('"'); // false
                data2Send["Called"] = jsonResult["Called"].ToJson().ToString().Trim('"'); // true
                data2Send["CallResult"] = CallResult.StandStill;
                data2Send["CallerUUID"] = jsonResult["CallerUUID"].ToJson().ToString().Trim('"');
                data2Send["CalleeUUID"] = jsonResult["CalleeUUID"].ToJson().ToString().Trim('"'); // or directly got from this ctx
                data2Send["CalleeStreamAddress"] = jsonResult["CalleeStreamAddress"].ToJson().ToString().Trim('"');
                data2Send["CallerStreamAddress"] = jsonResult["CallerStreamAddress"].ToJson().ToString().Trim('"');

                WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));

                // reception time out, calling has got an end! the last count would'nt be added.
                if (timeCounter > timeToWaitFor)
                {
                    ReceptionTimeOut();
                    timeCounter = 0;
                    //if (timeCounter.Equals(timeToWaitFor + 1)) Debug.Log("time counter initialized");
                }
            }
            else
            {
                if (System.Convert.ToBoolean(jsonResult["Calling"].ToJson().ToString()))
                {
                    string callResultGot = jsonResult["CallResult"].ToJson().ToString().Trim('"');

                    if (callResultGot.Equals(CallResult.Accepted))
                    {
                        shouldCallingCoroutineStop = true;
                        //StopCoroutine("Calling"); // non-workable
                        //playerInCallerPage.SetActive(true); // non-workable
                        GetCallerStream.callerStreamAddress = jsonResult["CalleeStreamAddress"].ToJson().ToString().Trim('"');
                        shouldActivatePlayerInCallerCanvasPage = true;
                    }
                    else if (callResultGot.Equals(CallResult.Rejected))
                    {
                        shouldCallingCoroutineStop = true;
                        //StopCoroutine("Calling"); // non-workable
                        //BackToMainPage(); // non-workable
                        shouldBackToMainPage = true;
                        Debug.Log("Main page to be backed!!");
                        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldBeat);
                    }
                    else if (callResultGot.Equals(CallResult.StandStill))
                    {
                        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldPause);
                    }
                }
                else
                {
                    // BOTH SIDE IS CURRENTLY NOT GOING TO CHATTING
                }
            }
        };
    }


    private void WhenWebsocketClosed()
    {
        WebSocketRequestSingleton.getInstance().ws.OnClose += (WebSocketCloseCode closeCode) =>
        {
            if (!closeCode.Equals(WebSocketCloseCode.Normal))
            {
                bool isReconnected = HeartBeatController.Instance.Reconnect();
                if (!isReconnected)
                {
                    ToastNetworkError();
                    return;
                }
            }
        };
    }


    private void OnDestroy()
    {
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldDie);
        if (WebSocketRequestSingleton.getInstance().ws.GetState() != WebSocketState.Closed)
            WebSocketRequestSingleton.getInstance().ws.Close();

        if (isNetworkAvailable)
            SetUserStateOffline(); // actually done in back-end, this is just for double check.
    }

    private void SetUserStateOffline()
    {
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldDie);

        JsonData data2Send = new JsonData();
        data2Send["HeartBeat"] = false;
        data2Send["ResponseCode"] = jsonResult["ResponseCode"].ToJson().ToString();
        data2Send["UserState"] = UserState.Offline;
        data2Send["CallResult"] = CallResult.StandStill;

        WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));
    }

    private void SetUserStateOnline()
    {
        HeartBeatController.Instance.ControllHeartBeat(HeartBeatState.ShouldDie);

        JsonData data2Send = new JsonData();
        data2Send["HeartBeat"] = false;
        data2Send["ResponseCode"] = jsonResult["ResponseCode"].ToJson().ToString();
        data2Send["UserState"] = UserState.Online;
        data2Send["CallResult"] = CallResult.StandStill;

        WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(data2Send.ToJson().ToString()));
    }

    private void Exit()
    {
        if (isNetworkAvailable)
            SetUserStateOffline(); // actually done in back-end, this is just for double check.

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
    }
}

