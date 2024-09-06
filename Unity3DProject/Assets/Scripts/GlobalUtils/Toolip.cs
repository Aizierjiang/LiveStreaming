using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Net.Sockets;

/*by Aizierjiang*/

public class Toolip: MonoBehaviour
{
    private static string configFilePath = Application.streamingAssetsPath + "/settings.config";



    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        return ipAddress.ToString();
    }

    public static string GetHTTPBaseUrl()
    {
        string result = "http://" + GetServerIPFromConfig() + ":" + GetHTTPServerPortFromConfig() + "/";
        return result;
    }

    public static string GetWebSocketBaseUrl()
    {
        string result = "ws://" + GetServerIPFromConfig() + ":" + GetWebSocketServerPortFromConfig() + "/";
        return result;
    }





    private static string GetServerIPFromConfig()
    {
        StringBuilder result = new StringBuilder();
        using (StreamReader sr = new StreamReader(configFilePath))
        {
            string line;
            // 从文件读取并显示行，直到文件的末尾 
            while ((line = sr.ReadLine()) != null)
            {
                if (line.ToLower().StartsWith("ip"))
                {
                    string[] splitedContent = line.Split(':');
                    string ip = splitedContent[splitedContent.Length - 1];
                    result.Append(ip);
                }
            }
        }
        return result.ToString();
    }

    private static string GetHTTPServerPortFromConfig()
    {
        StringBuilder result = new StringBuilder();
        using (StreamReader sr = new StreamReader(configFilePath))
        {
            string line;
            // 从文件读取并显示行，直到文件的末尾 
            while ((line = sr.ReadLine()) != null)
            {
                if (line.ToLower().StartsWith("http-port"))
                {
                    string[] splitedContent = line.Split(':');
                    string httpPort = splitedContent[splitedContent.Length - 1];
                    result.Append(httpPort);
                }
            }
        }
        return result.ToString();
    }

    private static string GetWebSocketServerPortFromConfig()
    {
        StringBuilder result = new StringBuilder();
        using (StreamReader sr = new StreamReader(configFilePath))
        {
            string line;
            // 从文件读取并显示行，直到文件的末尾 
            while ((line = sr.ReadLine()) != null)
            {
                if (line.ToLower().StartsWith("ws-port")|| line.ToLower().StartsWith("wss-port") || line.ToLower().StartsWith("websocket-port"))
                {
                    string[] splitedContent = line.Split(':');
                    string wsPort = splitedContent[splitedContent.Length - 1];
                    result.Append(wsPort);
                }
            }
        }
        return result.ToString();
    }

}
