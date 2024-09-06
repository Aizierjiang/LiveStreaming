using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

/*by Aizierjiang*/

public class ShowInfoLoader : MonoBehaviour
{
    public Image cover = null;
    public Sprite defaultCover = null;
    public Text actorName = null;
    public Text startTime = null;
    public Text title = null;

    private ShowInfo m_showInfo = null;
    private object m_mutex = new object();


    public ShowInfo GetInfo()
    {
        return m_showInfo;
    }

    public void LoadShowInfo(ShowInfo info)
    {
        m_showInfo = info;
        LoadCover(m_showInfo.imgURL);
        actorName.text = m_showInfo.actorName;
        startTime.text = info.startTime.ToString(); // Datetime to string
        title.text = info.title;
    }

    private async void LoadCover(string coverURL)
    {
        if (coverURL == "")
        {
            cover.sprite = defaultCover;
            return;
        }

        byte[] coverData = null;
        string fileName = Path.GetFileName(coverURL);

        coverData = FileDataBase.Get().LoadFile(fileName);
        if (coverData == null)
        {
            coverData = await HttpRequest.Instance.DownloadFile(coverURL);
            if (coverData == null)
            {
                cover.sprite = defaultCover;
                return;
            }
            FileDataBase.Get().SaveFile(fileName, coverData);
        }
        CreateCovers(coverData);
    }

    private bool CreateCovers(byte[] imageData)
    {
        try
        {
            Texture2D imageTexture = new Texture2D((int)cover.rectTransform.rect.width, (int)cover.rectTransform.rect.height);
            imageTexture.LoadImage(imageData);
            imageTexture.Apply();
            Material mat = new Material(Shader.Find("UI/RoundMask"));
            mat.SetFloat("_RoundRadius", 0.036f);
            mat.mainTexture = imageTexture;
            cover.sprite = null;
            cover.material = mat;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception:{e}");
            return false;
        }
    }

    private async Task DownloadModel(string url)
    {
        string fileName = Path.GetFileName(url);
        string localFileURL = FileDataBase.Get().GetFileUrl(fileName);
        if (!FileDataBase.Get().CheckFile(localFileURL))
        {
            byte[] modelData = await HttpRequest.Instance.DownloadFile(url);
            FileDataBase.Get().SaveFile(fileName, modelData);
        }
    }

    public void OnClick()
    {
        string streamURL = MakeupStreamURL(m_showInfo.ip);
        PageManager.Get().StartPlaying(streamURL);
        Debug.Log("original url is: "+ m_showInfo.ip + ":::after packed is: " + streamURL);
    }

    private string MakeupStreamURL(string url)
    {
        if (url.StartsWith("1"))
        {
            string result = "rtsp://alexander:soft@" + url + ":" + m_showInfo.port;
            return result;
        }
        else
        {
            return url;
        }
    }

}
