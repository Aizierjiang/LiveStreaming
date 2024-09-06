using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*by Aizierjiang*/

public class Tests : MonoBehaviour
{
    //public static readonly string BASE_URL = "http://172.26.5.56:8080/";
    //public static readonly string WSS_URL = "ws://172.26.5.56:8383";

    private void OnEnable()
    {
       string httpURL =  Toolip.GetHTTPBaseUrl();
       string wsURL =  Toolip.GetWebSocketBaseUrl();
       Debug.Log($"httpUrl is {httpURL}, wsUrl is {wsURL}");
    }
}
