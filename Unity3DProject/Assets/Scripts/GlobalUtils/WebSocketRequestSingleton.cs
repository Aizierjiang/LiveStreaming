using UnityEngine;
using System;
using HybridWebSocket;

/*by Aizierjiang*/

public class WebSocketRequestSingleton : MonoBehaviour
{
    public WebSocket ws = WebSocketFactory.CreateInstance(ServerURL.WSS_URL);
    
    // Hungery mode singleton
    private static WebSocketRequestSingleton m_instance = new WebSocketRequestSingleton();
    public static WebSocketRequestSingleton getInstance()
    {
        return m_instance;
    }

    public void OnOpen(Action action)
    {
        ws.OnOpen += () =>
        {
            action();
        };
    }
}