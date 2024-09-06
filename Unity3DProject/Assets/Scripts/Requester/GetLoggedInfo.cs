using UnityEngine;
using UnityEngine.UI;
using System.Text;
using LitJson;

/*by Aizierjiang*/

public class GetLoggedInfo : MonoBehaviour
{
    private static bool textIsUpdated = false;
    private string uuid = null;


    private void Start()
    {
        if (PageManager.isNetworkAvailable)
            GetUserNameToShowOnUI();
    }


    private void Update()
    {
        if (textIsUpdated == false)
        {
            //Debug.Log(PageManager.uuidFromHTTPServer);
            this.gameObject.GetComponent<Text>().text = uuid;
            textIsUpdated = true;
        }
    }

    private void GetUserNameToShowOnUI()
    {
        JsonData jsonNickName = JsonMapper.ToObject(PageManager.uuidFromHTTPServer);
        uuid = jsonNickName["fullName"].GetString();
    }
}
