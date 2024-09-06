using System;
using UnityEngine;
using System.Collections.Generic;
using LitJson;
using Logger;


public class GetAddrAndPort
{
    public AddrAndPort GetAddrByService(string channel, string serviceName, string showCode)
    {
        AddrAndPort addrAndPort = new AddrAndPort();

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["channel"] = channel;
        data["serviceName"] = serviceName;
        data["showCode"] = showCode;

        string response = "";
        JsonData jsonResult;
        try
        {
            response = HttpRequest.Instance.HttpComplexRequest("POST", ServerURL.LOOK_UP, data);
            jsonResult = JsonMapper.ToObject(response);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception occurred: " + e);
            addrAndPort.isResponded = false;
            return addrAndPort;
        }


        if (jsonResult["success"] == null || !jsonResult["success"].GetBoolean() || jsonResult == null)
        //if (jsonResult["success"].GetBoolean())//For testing server side error occurrence 
        {
            if (jsonResult.Equals(null)) Debug.LogError("Server side fatal error!");
            new Log("An error occurred at sever side!");
            addrAndPort.isResponded = false;
            // addrAndPort.UDPSERVER = ServerURL.UDPSERVER;
            // addrAndPort.UDPPORT = ServerURL.UDPPORT;
        }
        else
        {
            addrAndPort.isResponded = true;
            addrAndPort.UDPSERVER = jsonResult["data"]["addr"].ToString();
            addrAndPort.UDPPORT = int.Parse(jsonResult["data"]["portInfo"][0]["port"].ToString());
            addrAndPort.PORT_TYPE = jsonResult["data"]["portInfo"][0]["type"].ToString();

            //Debug.Log("******port**********" + addrAndPort.UDPPORT);
            //Debug.Log("*******type*********" + addrAndPort.PORT_TYPE);
            //Debug.Log("********server********" + addrAndPort.UDPSERVER);
        }

        return addrAndPort;
    }
}

public class AddrAndPort
{
    public bool isResponded = false;
    public string UDPSERVER = "";
    public int UDPPORT = 0;
    public string PORT_TYPE = "";
}
