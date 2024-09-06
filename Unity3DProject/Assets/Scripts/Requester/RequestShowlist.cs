using System;
using LitJson;
using UnityEngine;
using System.Collections.Generic;

/*by Aizierjiang*/

/**
 * Request all the shows from back-end.
 **/
public class RequestShowlist : MonoBehaviour
{
    public Transform videoContainersParent = null;
    public GameObject videoContainer = null;
    List<ShowInfo> showInfoList = new List<ShowInfo>();


    private void OnEnable()
    {
        ClearShowPannel();
        SendShowlistRequest();
    }

    private void ClearShowPannel()
    {
        for (int i = 0; i < videoContainersParent.childCount; i++)
            Destroy(videoContainersParent.GetChild(i).gameObject);
    }

    private async void SendShowlistRequest()
    {
        string response = await HttpRequest.Instance.GetAsync(ServerURL.GET_SHOWLIST);
        JsonData jsonResult = JsonMapper.ToObject(response);
        //new Logger.Log($"written in file?  {OperateFile.WriteFile(@Application.dataPath + "/showlist.json", response)}");
        ClearShowList();
        
        Queue<string> showCoverUrl = new Queue<string>();
        for (int showInfoIndex = 0; showInfoIndex < jsonResult["OnlineList"].Count; showInfoIndex++)
        {
            ShowInfo showInfo = new ShowInfo();//must be placed inside the loop!!!
            showInfo.id = jsonResult["OnlineList"][showInfoIndex]["id"].ToJson().ToString().Trim('"');
            showInfo.startTime = DateTime.Parse(jsonResult["OnlineList"][showInfoIndex]["time"].GetString());
            showInfo.actorName = jsonResult["OnlineList"][showInfoIndex]["name"].ToJson().ToString().Trim('"');
            showInfo.title = jsonResult["OnlineList"][showInfoIndex]["title"].ToJson().ToString().Trim('"');
            showInfo.imgURL = MakeupStaticFolderULR(jsonResult["OnlineList"][showInfoIndex]["img_url"].GetString());
            showInfo.ip = jsonResult["OnlineList"][showInfoIndex]["ip"].ToJson().ToString().Trim('"');
            showInfo.port = jsonResult["OnlineList"][showInfoIndex]["port"].ToJson().ToString().Trim('"');
            showInfo.isOnline = Boolean.Parse(jsonResult["OnlineList"][showInfoIndex]["is_online"].ToJson().ToString().Trim('"')); // non-safe, left to be ckecked!
            showInfoList.Add(showInfo);
        }

        LoadShowListOnUI();
    }

    private void ClearShowList()
    {
        if (!showInfoList.Count.Equals(0))
            showInfoList.Clear();
    }

    private string MakeupStaticFolderULR(string originalURL)
    {
        string usefulURL = originalURL.Substring(7); // remove 'piblic/', which includes 7 chars
        return ServerURL.BASE_URL + usefulURL;
    }

    private void LoadShowListOnUI()
    {
        for (int showIndex = 0; showIndex < showInfoList.Count; showIndex++)
        {
            GameObject instantiatedVideoContainer = GameObject.Instantiate(videoContainer, videoContainersParent.transform);
            instantiatedVideoContainer.GetComponent<ShowInfoLoader>().LoadShowInfo(showInfoList[showIndex]);
        }
    }
}
