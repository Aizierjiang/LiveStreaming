using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using UnityEngine;
using System.Net;
using LitJson;
using System;

class HttpRequest
{
    private readonly HttpClient m_client = null;
    private static HttpRequest m_instance = null;
    private HttpRequest()
    {
        m_client = new HttpClient();
        m_client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
    }

    public static HttpRequest Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new HttpRequest();
            }
            return m_instance;
        }
    }

    private void SetHttpHeaders(Dictionary<string, string> headers)
    {
        m_client.DefaultRequestHeaders.Clear();
        if (headers.ContainsKey("user-agent") == false)
        {
            m_client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        foreach (var item in headers)
        {
            m_client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }
    }


    /// <summary>
    /// 使用get方法异步请求
    /// </summary>
    /// <param name="url">目标链接</param>
    /// <param name="headers">请求的头部信息</param>
    /// <returns>返回的字符串</returns>
    public async Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
    {
        try
        {
            if (headers != null)
            {
                SetHttpHeaders(headers);
            }
            HttpResponseMessage response = await m_client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Debug.Log(e.Message);
            return null;
            //return e.Message; // do not use this returner
        }
    }

    /// <summary>
    /// 使用post方法异步请求
    /// </summary>
    /// <param name="url">目标链接</param>
    /// <param name="json">发送的参数字符串，只能用json</param>
    /// <param name="headers">请求的头部信息</param>
    /// <returns>返回的字符串</returns>
    public async Task<string> PostJsonAsync(string url, string json, Dictionary<string, string> headers = null)
    {
        try
        {
            if (headers != null)
            {
                SetHttpHeaders(headers);
            }
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await m_client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError(e.Message);
            return ErrorCode.httpException.ToString();
        }

    }

    /// <summary>
    /// 指定Post地址使用Get 方式获取全部字符串
    /// </summary>
    /// <param name="url">请求后台地址</param>
    /// <param name="content">Post提交数据内容(utf-8编码的)</param>
    /// <returns></returns>
    public string Post(string url, string content)
    {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        //req.ContentType = "application/x-www-form-urlencoded";
        req.ContentType = "application/form-data";

        #region 添加Post 参数
        byte[] data = Encoding.UTF8.GetBytes(content);
        req.ContentLength = data.Length;
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }
        #endregion

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        // 获取响应内容
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }

    /// <summary>
    /// 使用put方法异步请求
    /// </summary>
    /// <param name="url">目标链接</param>
    /// <param name="json">发送的参数字符串，只能用json</param>
    /// <param name="headers">请求的头部信息</param>
    /// <returns>返回的字符串</returns>
    public async Task<string> PutJsonAsync(string url, string json, Dictionary<string, string> headers = null)
    {
        try
        {
            if (headers != null)
            {
                SetHttpHeaders(headers);
            }
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await m_client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError(e.Message);
            return "";
        }
    }


    /// <summary>
    /// 使用delete方法异步请求
    /// </summary>
    /// <param name="url">目标链接</param>
    /// <param name="headers">请求的头部信息</param>
    /// <returns>返回的字符串</returns>
    public async Task<string> DeleteAsync(string url, Dictionary<string, string> headers = null)
    {
        try
        {
            if (headers != null)
            {
                SetHttpHeaders(headers);
            }
            HttpResponseMessage response = await m_client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError(e.Message);
            return "";
        }
    }


    /// <summary>
    /// 下载文件，返回byte[]
    /// </summary>
    /// <param name="url">目标链接</param>
    /// <param name="headers">请求的头部信息</param>
    /// <returns>返回的字符串</returns>
    public async Task<byte[]> DownloadFile(string url, Dictionary<string, string> headers = null)
    {
        try
        {
            if (headers != null)
            {
                SetHttpHeaders(headers);
            }
            HttpResponseMessage response = await m_client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBody = await response.Content.ReadAsByteArrayAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }


    /// <summary>
    /// 下载文件，返回stream
    /// </summary>
    /// <param name="url">目标链接</param>
    /// <param name="headers">请求的头部信息</param>
    /// <returns>返回的字符串</returns>
    public async Task<Stream> DownloadFileStream(string url, Dictionary<string, string> headers = null)
    {
        try
        {
            if (headers != null)
            {
                SetHttpHeaders(headers);
            }
            HttpResponseMessage response = await m_client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Stream responseBody = await response.Content.ReadAsStreamAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    /// <summary>
    /// 同步方式发送请求
    /// </summary>
    /// <param name="requestType">发送请求的具体类型</param>
    /// <param name="url">要发送的地址</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">要携带的请求头</param>
    /// <returns></returns>
    public string HttpComplexRequest(string requestType, string url, Dictionary<string, string> data, Dictionary<string, string> headers = null)
    {
        JsonData body = new JsonData();

        foreach (KeyValuePair<string, string> datum in data)
        {
            body[datum.Key] = datum.Value;
        }

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(body.ToJson());

        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.Method = requestType;
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        //if (!headers.Count.Equals(0) || headers != null)
        if (headers != null)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        try
        {
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bytes, 0, bytes.Length);
            }

            //处理响应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStm = response.GetResponseStream())
            {
                StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                string result = redStm.ReadToEnd();
                return result;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception occurred: " + e);
            return "";
        }
    }
}
