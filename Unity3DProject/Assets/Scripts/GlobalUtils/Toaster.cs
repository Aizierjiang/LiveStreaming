using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

/*by Aizierjiang*/

/**
 * Utilization Example:
        toastObject.SetActive(true);
        toastObject.AddComponent<Toaster>();
        toastObject.gameObject.GetComponent<Toaster>().toastMsg = toastObject;
        toastObject.gameObject.GetComponent<Toaster>().Toast(3);
        toastObject.gameObject.GetComponent<Toaster>().callback = () =>
        {
           //Do something
        };
 */

public class Toaster : MonoBehaviour
{
    public GameObject toastMsg = null;//需要toast的UI游戏对象
    public List<GameObject> uiInteractionList = new List<GameObject>();//在Toast过程中需要屏蔽的交互
    public Action callback = null;//Toast之后需要执行的操作
    private int toastTime = 0;

    public void Toast(int duration = 3)
    {
        toastTime = duration;
        if (toastMsg == null) throw new Exception("Toast message object should not be null!");
        toastMsg.SetActive(true);
        ActivateUIsToBlock(false);
        StartCoroutine("Timer");
    }

    private void ActivateUIsToBlock(bool shouldActivate = true)
    {
        if (!uiInteractionList.Count.Equals(0))
        {
            foreach (GameObject uiElements in uiInteractionList)
                uiElements.SetActive(shouldActivate);
        }
    }

    IEnumerator Timer()
    {
        while (toastTime >= 0)
        {
            yield return new WaitForSeconds(1);
            toastTime--;
            if (toastTime <= 0)
            {
                toastMsg.SetActive(false);
                ActivateUIsToBlock();
                if (this.callback != null) callback();
                StopCoroutine("Timer");
            }
        }
    }
}
