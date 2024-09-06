using LitJson;
using System.Text;
using System.Threading;


/*by Aizierjiang*/

public enum HeartBeatState
{
    ShouldBeat,
    ShouldPause,
    ShouldDie
}

public class HeartBeatController
{
    private static object m_mutex = new object();
    private static bool m_initialized = false;
    private static HeartBeatController heartBeatController = null;
    private static Thread heartBeatThread = null;

    public static HeartBeatController Instance
    {
        get
        {
            if (!m_initialized)
            {
                lock (m_mutex)
                {
                    if (heartBeatController == null)
                    {
                        heartBeatController = new HeartBeatController();
                        heartBeatThread = new Thread(HeartBeatThread);
                        m_initialized = true;
                    }
                }
            }
            return heartBeatController;
        }
    }



    public void ControllHeartBeat(HeartBeatState heartBeatState)
    {
        if (!heartBeatThread.IsAlive)
        {
            heartBeatThread = new Thread(HeartBeatThread);
        }
        switch (heartBeatState)
        {
            case HeartBeatState.ShouldBeat:
                if (heartBeatThread.ThreadState.HasFlag(ThreadState.Unstarted))
                    heartBeatThread.Start();
                if (heartBeatThread.ThreadState.HasFlag(ThreadState.Suspended))
                    heartBeatThread.Resume();
                if (!heartBeatThread.ThreadState.HasFlag(ThreadState.Background) && heartBeatThread.IsAlive)
                    heartBeatThread.IsBackground = true;
                break;
            case HeartBeatState.ShouldPause:
                if (heartBeatThread.IsAlive && !heartBeatThread.ThreadState.HasFlag(ThreadState.Suspended))
                    heartBeatThread.Suspend();
                break;
            case HeartBeatState.ShouldDie:
                if (heartBeatThread.IsAlive)
                    heartBeatThread.Abort();
                break;
            default:
                // never happens
                break;
        }
    }


    public bool Reconnect()
    {
        ControllHeartBeat(HeartBeatState.ShouldPause);

        int tryCounts = 0;
        while (!ConnectToWebSocketServer())
        {
            tryCounts++;

            if (!ConnectToWebSocketServer())
            {
                if (tryCounts < 7)
                {
                    ConnectToWebSocketServer();
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }
            else
            {
                ControllHeartBeat(HeartBeatState.ShouldBeat);
                return true;
            }
        }

        return false;
    }


    private bool ConnectToWebSocketServer()
    {
        try
        {
            WebSocketRequestSingleton.getInstance().ws.Connect();
            return true;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }


    static void HeartBeatThread()
    {
        JsonData heartBeatReq = new JsonData();
        heartBeatReq["HeartBeat"] = true;
        heartBeatReq["ResponseCode"] = 200;
        heartBeatReq["UserState"] = UserState.Online;
        heartBeatReq["CallResult"] = CallResult.StandStill;

        while (true)
        {
            Thread.Sleep(1000);
            if (WebSocketRequestSingleton.getInstance().ws.GetState().ToString().Equals("Open"))
            {
                WebSocketRequestSingleton.getInstance().ws.Send(Encoding.UTF8.GetBytes(heartBeatReq.ToJson().ToString()));
            }
        }
    }
}
