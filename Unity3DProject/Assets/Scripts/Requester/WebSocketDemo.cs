using System.Text;
using UnityEngine;

// Use plugin namespace
//using HybridWebSocket;

public class WebSocketDemo : MonoBehaviour
{

    //private WebSocket ws;

    //// Use this for initialization
    //void Start()
    //{

    //    // Create WebSocket instance
    //    ws = WebSocketFactory.CreateInstance("ws://127.0.0.1:8383");

    //    // Add OnOpen event listener
    //    ws.OnOpen += () =>
    //    {
    //        Debug.Log("WS connected!");
    //        Debug.Log("WS state: " + ws.GetState().ToString());

    //        ws.Send(Encoding.UTF8.GetBytes("Hello from Unity 3D!"));
    //    };

    //    // Add OnMessage event listener
    //    ws.OnMessage += (byte[] msg) =>
    //    {
    //        Debug.Log("WS received message: " + Encoding.UTF8.GetString(msg));

    //        // // ws.Close();
    //    };

    //    // Add OnError event listener
    //    ws.OnError += (string errMsg) =>
    //    {
    //        Debug.Log("WS error: " + errMsg);
    //    };

    //    // Add OnClose event listener
    //    ws.OnClose += (WebSocketCloseCode code) =>
    //    {
    //        Debug.Log("WS closed with code: " + code.ToString());
    //    };

    //    // Connect to the server
    //    ws.Connect();
    //}

    //private void OnDisable()
    //{
    //    ws.Close();
    //}

    //private void OnDestroy() { }
}
