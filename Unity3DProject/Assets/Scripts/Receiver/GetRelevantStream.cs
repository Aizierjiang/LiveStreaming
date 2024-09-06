using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*by Aizierjiang*/

public class GetRelevantStream : MonoBehaviour
{

    public InputField streamAddress = null;

    [SerializeField]
    private MediaPlayerCtrl m_mediaPlayerCtrl;
    public MediaPlayerCtrl mediaPlayerCtrl
    {
        set { m_mediaPlayerCtrl = value; }
    }

    public GameObject inputPanel = null;
    public GameObject[] playerRawImages = null;


    void OnEnable()
    {
        ShowInputPanel(true);
        this.GetComponent<MediaPlayerCtrl>().enabled = false;
        foreach (var playerRawImage in playerRawImages)
        {
            playerRawImage.SetActive(false);
        }
    }

    void OnDestroy()
    {
        //m_mediaPlayerCtrl.UnLoad();
        //m_mediaPlayerCtrl.DeleteVideoTexture();
    }

    public void GetStream()
    {
        if (streamAddress.text != null || streamAddress.text != "")
        {
            m_mediaPlayerCtrl.m_strFileName = streamAddress.text;
            streamAddress.text = ""; //Clear text
            m_mediaPlayerCtrl.m_TargetMaterial = playerRawImages;
            this.GetComponent<MediaPlayerCtrl>().enabled = true;
            foreach (var playerRawImage in playerRawImages)
            {
                playerRawImage.SetActive(true);
            }
            ShowInputPanel(false);
        }
    }

    private bool ShowInputPanel(bool shouldShow)
    {
        return ExceptionHandler.Assert(() =>
        {
            if (shouldShow)
            {
                inputPanel.SetActive(true);
            }
            else
            {
                inputPanel.SetActive(false);
            }
        });
    }
}
