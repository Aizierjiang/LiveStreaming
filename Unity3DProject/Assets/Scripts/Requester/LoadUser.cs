using UnityEngine.UI;
using UnityEngine;

/*by Aizierjiang*/

public class LoadUser : MonoBehaviour
{
    public Text userName = null;
    private string ip = null;

    public void GetUUIDAndIPForPagemanager()
    {
        if (userName.text == null || ip == null)
        {
            Debug.LogError("User name and IP has not been got yet!!!!");
        }
        PageManager.calleeIP = ip;
        PageManager.calleeUUID = userName.text;

        //Debug.Log("ip got ---> " + PageManager.calleeIP + ";   " + ip);
        //Debug.Log("ip got ---> " + PageManager.calleeUUID + ";   " + userName.text);

        PageManager.gotIPToCall = true;
    }



    public void LoadUserInfo(string _userName, string _ip)
    {
        this.userName.text = _userName.Trim('"');
        // Remove the IPv4 or IPv6 header, like using if (_ip.ToLower().StartsWith("::ffff:")){}
        string[] spiltedIP = _ip.Trim('"').Split(':');
        this.ip = spiltedIP[spiltedIP.Length - 1];

        //Debug.Log("IP Got : " + this.ip + ";   original once : " + _ip.Trim('"'));
        //Debug.Log("UserName Got : " + _userName);
    }
}
