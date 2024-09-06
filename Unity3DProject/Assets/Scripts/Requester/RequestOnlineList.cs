using LitJson;
using UnityEngine;

/*by Aizierjiang*/

/**
 * Request online user from back-end.
 **/
public class RequestOnlineList : MonoBehaviour
{

    public Transform userContainerParent = null;
    public GameObject userContainer = null;


    void OnEnable()
    {
        //先清空指定游戏对象群，再发送请求
        for (int i = 0; i < userContainerParent.childCount; i++)
            Destroy(userContainerParent.GetChild(i).gameObject);

        SendAsyncOnlineListRequest(ServerURL.GET_ONLINE_LIST);
    }


    public async void SendAsyncOnlineListRequest(string url)
    {
        string response = await HttpRequest.Instance.GetAsync(url);
        JsonData jsonResult = JsonMapper.ToObject(response);

        for (int i = 0; i < jsonResult["OnlineList"].Count; i++)
        {
            JsonData eachUser = JsonMapper.ToObject(jsonResult["OnlineList"][i].ToJson().ToString());
            string name = eachUser["name"].ToJson().ToString();
            string ip = eachUser["ip"].ToJson().ToString();
            string globalUUID = JsonMapper.ToObject(PageManager.uuidFromHTTPServer)["fullName"].ToJson().ToString();
            if (!name.Equals(globalUUID))
            {
                GameObject userNameContainer = GameObject.Instantiate(userContainer, userContainerParent.transform);
                userNameContainer.GetComponent<LoadUser>().LoadUserInfo(name, ip);
            }
            else
            {
                //new Logger.Log($"{globalUUID} is user it self!", LogType.Log);
            }
        }
    }

}
