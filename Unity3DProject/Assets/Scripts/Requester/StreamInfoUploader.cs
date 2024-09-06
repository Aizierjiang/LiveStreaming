using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.InteropServices;

/*by Aizierjiang*/

/// <summary>
/// 文件控制脚本 && 加上了自定义发送请求的功能使本脚本适用于本项目
/// </summary>
public class StreamInfoUploader : MonoBehaviour
{
    public GameObject streamInfoPannel = null;
    public InputField titleInputField = null;
    public Text coverImgPathText = null;

    private byte[] coverData = null;
    private string imgExtension = "";


    private void Awake()
    {
        InActivateStreamInfoPannel();
    }

    private void InitializeImgPathPlacholder()
    {
        coverImgPathText.text = "Cover path...";
        coverImgPathText.fontStyle = FontStyle.Italic;
        coverImgPathText.color = new Color(0f,0f,0f,0.5f);

        titleInputField.text = ""; // refresh title text BTW
    }

    private void SetPathPlacholderToInfo(string fileURL)
    {
        coverImgPathText.text = fileURL;
        coverImgPathText.fontStyle = FontStyle.Normal;
        coverImgPathText.color = new Color(0f, 0f, 0f, 1f);
    }


    public void ActivateStreamInfoPannel()
    {
        if (!streamInfoPannel.activeSelf)
            streamInfoPannel.SetActive(true);
    }

    public void InActivateStreamInfoPannel()
    {
        InitializeImgPathPlacholder();
        if (streamInfoPannel.activeSelf)
            streamInfoPannel.SetActive(false);
    }

    public void SendInfoToStartPushing()
    {
        if (coverData == null||titleInputField.text == "" || titleInputField.text == null)
        {
            //TODO: Toast title should not be empty
        }
        else
        {
            StartCoroutine(HttpPostImage(ServerURL.NEW_SHOW, coverData, imgExtension));
            InActivateStreamInfoPannel();
            PageManager.Get().StartPushing();
        }

    }

    /// <summary>
    /// 打开文件
    /// </summary>
    public void OpenFile()
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        pth.filter = "All files (*.*)|*.*";
        //pth.filter = "Image Files(*.JPG;*.BMP;*.PNG)|*.JPG;*.BMP;*.PNG"; // useless!!!!! to be fixed!
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath.Replace("/", "\\") + "\\Resources"; //默认路径
        pth.title = "Select cover image";
        pth.defExt = "dat";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filePath = pth.file; //选择的文件路径;  
            Debug.Log(filePath);
            //byte[] file = ReadFile(filePath); // both API is available
            string[] spiltedFilePath = filePath.Split('.');
            string imgFileExtension = spiltedFilePath[spiltedFilePath.Length - 1];
            byte[] fileData = LoadFile(filePath);

            // data to be used at other fields
            SetPathPlacholderToInfo(filePath);
            coverData = fileData;
            imgExtension = imgFileExtension;
        }
    }


    /// <summary>
    /// 读取外部文件
    /// </summary>
    /// <param name="fileURL">文件路径</param>
    /// <returns></returns>
    byte[] ReadFile(string fileURL)
    {
        byte[] data = null;

        using (FileStream fs = File.OpenRead(fileURL))
        {
            int index = 0;
            long len = fs.Length;
            data = new byte[len];
            int offset = data.Length > 1024 ? 1024 : data.Length;
            while (index < len)
            {
                int readByteCnt = fs.Read(data, index, offset);
                index += readByteCnt;
                long leftByteCnt = len - index;
                offset = leftByteCnt > offset ? offset : (int)leftByteCnt;
            }
            Debug.Log("Read Done");
        }
        return data;
    }

    // 将日志字节流上传到web服务器
    IEnumerator HttpPostImage(string url, byte[] data, string imgExtension)
    {
        WWWForm form = new WWWForm();
        form.AddField("desc", "upload image file for pushing list");

        #region customized fields to make req body tie with cover img data
        form.AddField("name",PageManager.uuidFromHTTPServer) ;
        form.AddField("title",titleInputField.text) ;
        form.AddField("time", DateTime.Now.ToUniversalTime().ToString());
        form.AddField("ip", Toolip.GetLocalIPAddress());
        form.AddField("port", "8554"); // get from global port manager in usal case but here we temporarlly used static once
        #endregion

        form.AddBinaryData("image", data, "img."+ imgExtension, "application/x-gzip");
        // 使用UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Post(url, form);
//#pragma warning disable CS0618 // Type or member is obsolete
        AsyncOperation result = request.Send();
        //bool isError = request.isError;
//#pragma warning restore CS0618 // Type or member is obsolete
        //if (isError)
        //{
        //    Debug.LogError(request.error);
        //}
        while (!result.isDone)
        {
            yield return null;
            //Debug.Log("result.progress: " + request.uploadProgress);
        }

        Debug.Log("Finished upload, http returned msg: \n" + request.downloadHandler.text);
    }


    /// <summary>
    /// 读取外部文件
    /// </summary>
    /// <param name="url">文件路径</param>
    /// <returns></returns>
    public byte[] LoadFile(string url)
    {
        try
        {
            using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e}");
            return null;
        }
    }

    /// <summary>
    /// 保存文件项目
    /// </summary>
    public void SaveProject()
    {
        SaveFileDlg pth = new SaveFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        pth.filter = "All files (*.*)|*.*";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath; //默认路径
        pth.title = "保存项目";
        pth.defExt = "dat";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (SaveFileDialog.GetSaveFileName(pth))
        {
            string filepath = pth.file; //选择的文件路径;  
            Debug.Log(filepath);
        }
    }
}