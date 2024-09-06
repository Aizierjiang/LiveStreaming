using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

/*by Aizierjiang*/

public class Register : MonoBehaviour
{
    public GameObject registerPannel = null;
    public InputField emailField = null;
    public InputField nameField = null;
    public InputField pwdField = null;
    public InputField confirmedPWDField = null;
    public Button comfirmButton = null;
    public GameObject infoToast = null;
    public List<GameObject> btns2Block = new List<GameObject>();

    private void Awake()
    {
        InactivateRegisterPannel();
    }

    private void OnDestroy()
    {
        InactivateRegisterPannel();
    }

    public void InactivateRegisterPannel()
    {
        if (registerPannel.activeSelf)
            registerPannel.SetActive(false);
    }

    public void ActivateRegisterPannel()
    {
        Refresh();
        if (!registerPannel.activeSelf)
            registerPannel.SetActive(true);
    }

    public async void SendRegisterRequest()
    {
        JsonData data2Send = new JsonData();
        data2Send["email"] = emailField.text;
        data2Send["name"] = nameField.text;
        data2Send["password"] = pwdField.text;
        //data2Send["confirmpwd"] = confirmedPWDField.text; //前端验证两次输入是否一致，更加高效
        string response = await HttpRequest.Instance.PostJsonAsync(ServerURL.REGISTER, data2Send.ToJson().ToString());
        JsonData jsonResult = JsonMapper.ToObject(response);

        infoToast.GetComponentInChildren<Text>().text = jsonResult["msg"].GetString();
        RegisterResultToast();

        if (jsonResult["code"].GetString().Equals("200"))
        {
            registerPannel.SetActive(false);
        }

        Refresh();
    }

    private void RegisterResultToast()
    {
        infoToast.SetActive(true);
        infoToast.AddComponent<Toaster>();
        infoToast.gameObject.GetComponent<Toaster>().toastMsg = infoToast;
        infoToast.gameObject.GetComponent<Toaster>().uiInteractionList = btns2Block;
        infoToast.gameObject.GetComponent<Toaster>().Toast(2);
        infoToast.gameObject.GetComponent<Toaster>().callback = () =>
        {
            //Do something
        };
    }

    private void Refresh()
    {
        if (emailField != null
            && nameField != null
            && pwdField != null
            && confirmedPWDField != null)
        {
            emailField.text = "";
            nameField.text = "";
            pwdField.text = "";
            confirmedPWDField.text = "";
        }
        else
        {
            Debug.LogError("game object is null!");
        }
    }
}
