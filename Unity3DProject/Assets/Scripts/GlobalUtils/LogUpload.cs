using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Text;

/*by Aizierjiang*/

public class LogUpload : MonoBehaviour
{
    private string UPLOAD_URL = ServerURL.CLIENT_LOG;
    private string m_logFileSavePath = null;
    private StringBuilder m_logStr = null;


    private void Awake()
    {
        m_logStr = new StringBuilder("This is a test for uploading log files and mostly for utility of uploading imgs before pushing.");
        m_logFileSavePath = string.Format("{0}/output.log", Application.persistentDataPath);
        ///Debug.Log("log file saved path: " + m_logFileSavePath);
    }


    private void OnEnable()
    {
        WriteLogToFile();
    }

    private void Start()
    {
        StartCoroutine(SendLogCoroutine());
    }

    IEnumerator SendLogCoroutine()
    {
        int timeCounter = 0;
        while (true)
        {
            timeCounter++;
            if (PageManager.isNetworkAvailable)
            {
                SendLogToServer();
                break;
            }
            else
            {
                if (timeCounter.Equals(60)) break;
                else yield return new WaitForSeconds(1);
            }
        }
    }

    private void OnDestroy()
    {
        StopCoroutine("SendLogCoroutine");
    }

    void SendLogToServer()
    {
        byte[] data = ReadLogFile();
        //Debug.Log(data.Length);
        StartCoroutine(HttpPost(UPLOAD_URL, data));
    }

    // 将日志写入本地文件中
    void WriteLogToFile()
    {
        if (m_logStr.Length <= 0) return;
        if (!File.Exists(m_logFileSavePath))
        {
            var fs = File.Create(m_logFileSavePath);
            fs.Close();
        }
        using (var sw = File.AppendText(m_logFileSavePath))
        {
            sw.WriteLine(m_logStr.ToString());
            m_logStr.Remove(0, m_logStr.Length);
        }
    }

    // 读取日志文件的字节流
    byte[] ReadLogFile()
    {
        byte[] data = null;

        using (FileStream fs = File.OpenRead(m_logFileSavePath))
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
            //Debug.Log("Read Done");
        }
        return data;
    }

    // 将日志字节流上传到web服务器
    IEnumerator HttpPost(string url, byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddField("desc", "test upload log file");
        form.AddBinaryData("log", data, "test.log", "application/x-gzip");
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
            Debug.Log("log file upload result.progress: " + request.uploadProgress);
        }

        Debug.Log("Finished upload, http returned msg: \n" + request.downloadHandler.text);
    }
}



/*
 * Using multer in Nodejs back-end to get files uploaded 
 * then it got these info below:
 * 
[Object: null prototype] { desc: 'test upload log file' } [
  {
    fieldname: 'errlog',
    originalname: 'test_log.txt',
    encoding: '7bit',
    mimetype: 'application/x-gzip',
    destination: './UploadedClientLogs',
    filename: '564bbaf37ee621bd3c7a3a3e5cba6f92',
    path: 'UploadedClientLogs\\564bbaf37ee621bd3c7a3a3e5cba6f92',
    size: 194
  }
]
 */
