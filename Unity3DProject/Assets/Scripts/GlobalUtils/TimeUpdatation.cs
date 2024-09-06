using UnityEngine;
using UnityEngine.UI;

public class TimeUpdatation : MonoBehaviour
{
    /// Way 1
    //public int frameRate = 30;

    //void Update()
    //{
    //    var t = Time.unscaledTime;
    //    var seconds = (int)t;
    //    var h = seconds / 3600;
    //    var hInSec = h * 3600;
    //    var m = (seconds - hInSec) / 60;
    //    var mInSec = m * 60;
    //    var s = (seconds - (hInSec + mInSec));
    //    var framesDouble = frameRate * (t - (double)seconds);
    //    var f = (int)framesDouble;
    //    //var frac = 100 * (framesDouble - f) / frameRate;
    //    GetComponent<Text>().text = $"{h:00}:{m:00}:{s:00}:{f:00}";
    //}


    /// Way 2
    int hour, minute, second, millisecond = 0;
    bool shouldCount = false;
    float timeSpent = 0.0f;


    void OnEnable()
    {
        shouldCount = true;
    }

    void Update()
    {
        if (shouldCount)
        {
            timeSpent += Time.deltaTime;

            hour = (int)timeSpent / 3600;
            minute = ((int)timeSpent - hour * 3600) / 60;
            second = (int)timeSpent - hour * 3600 - minute * 60;
            millisecond = (int)((timeSpent - (int)timeSpent) * 1000);

            //text_timeSpent.text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
            GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
        }
    }

    //void OnDestroy
    void OnDisable()
    {
        // reset all
        shouldCount = false;
        timeSpent = 0.0f;
        GetComponent<Text>().text = "";
    }
}