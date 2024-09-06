using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.InteropServices;

/*by Aizierjiang*/

/// <summary>
/// 文件控制脚本
/// </summary>
public class FileController : MonoBehaviour
{
    /// <summary>
    /// 打开项目
    /// </summary>
    public void OpenProject()
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        pth.filter = "All files (*.*)|*.*";
        //pth.filter = "Image Files(*.JPG;*.BMP;*.PNG)|*.JPG;*.BMP;*.PNG|All files (*.*)|*.*";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath.Replace("/", "\\") + "\\Resources"; //默认路径
        pth.title = "打开项目";
        pth.defExt = "dat";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filePath = pth.file; //选择的文件路径;  
            Debug.Log(filePath);
            //byte[] file = ReadFile(filePath);
            string[] spiltedFilePath = filePath.Split('.');
            string imgFileExtension = spiltedFilePath[spiltedFilePath.Length - 1];
            byte[] fileData = LoadFile(filePath);
            StartCoroutine(HttpPostImage(ServerURL.IMG_BASE, fileData, imgFileExtension));
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
        form.AddBinaryData("image", data, "img." + imgExtension, "application/x-gzip");
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