using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/*by Aizierjiang*/

public class Loggin : MonoBehaviour
{
    public GameObject logginPannel = null;
    public InputField nameField = null; 
    public InputField pwdField = null; 
    public Text logedInfo = null;
    public GameObject errorToast = null;
    public List<GameObject> btns2Block = new List<GameObject>();

    private void Awake()
    {
        InactivateLogginPannel();
    }

    private void OnDestroy()
    {
        InactivateLogginPannel();
    }

    public void InactivateLogginPannel()
    {
        if (logginPannel.activeSelf)
            logginPannel.SetActive(false);
    }

    public void ActivateLogginPannel()
    {
        Refresh();
        if (!logginPannel.activeSelf)
            logginPannel.SetActive(true);
    }

    public async void SendLogginRequest()
    {
        JsonData data2Send = new JsonData();
        data2Send["name"] = nameField.text;
        data2Send["password"] = pwdField.text;
        string response = await HttpRequest.Instance.PostJsonAsync(ServerURL.LOGIN, data2Send.ToJson().ToString());
        JsonData jsonResult = JsonMapper.ToObject(response);
        if (!jsonResult["code"].GetString().Equals("200"))
        {
            GlobalLoginState.isLogedIn = false; 
            errorToast.GetComponentInChildren<Text>().text = jsonResult["msg"].GetString();
            LogginErrorToast();
        }
        else
        {
            GlobalLoginState.isLogedIn = true; 
            logedInfo.text = nameField.text;
            logginPannel.SetActive(false);
        }

        Refresh();
    }

    private void LogginErrorToast()
    {
        errorToast.SetActive(true);
        errorToast.AddComponent<Toaster>();
        errorToast.gameObject.GetComponent<Toaster>().toastMsg = errorToast;
        errorToast.gameObject.GetComponent<Toaster>().uiInteractionList = btns2Block;
        errorToast.gameObject.GetComponent<Toaster>().Toast(2);
        errorToast.gameObject.GetComponent<Toaster>().callback = () =>
        {
            //Do something
        };
    }

    private void Refresh()
    {
        nameField.text = "";
        pwdField.text = "";
    }

}
