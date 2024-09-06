using UnityEngine;

/*by Aizierjiang*/

public class GetCallerStream : MonoBehaviour
{
    [SerializeField]
    private MediaPlayerCtrl m_mediaPlayerCtrl;
    public MediaPlayerCtrl mediaPlayerCtrl
    {
        set { m_mediaPlayerCtrl = value; }
    }

    public GameObject[] playerRawImages = null;
    public static string callerStreamAddress = null;
    //public static string callerStreamAddress = "rtsp://alexander:soft@10.26.6.73:8554";

    //// For test
    //private void OnEnable()
    //{
    //    callerStreamAddress = "https://cdn.letv-cdn.com/2018/12/05/JOCeEEUuoteFrjCg/playlist.m3u8";
    //}

    private void OnEnable()
    {
        GetStream(callerStreamAddress);
    }

    public void GetStream(string streamAddress)
    {
        m_mediaPlayerCtrl.m_strFileName = streamAddress;
        m_mediaPlayerCtrl.m_TargetMaterial = playerRawImages;
        this.GetComponent<MediaPlayerCtrl>().enabled = true;
        foreach (var playerRawImage in playerRawImages)
        {
            playerRawImage.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        //m_mediaPlayerCtrl.UnLoad();
        //m_mediaPlayerCtrl.DeleteVideoTexture();
    }
}
