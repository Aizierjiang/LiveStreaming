using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*by Aizierjiang*/

public class WindowSizeController : MonoBehaviour
{
    public RectTransform thisRect = null;
    public GameObject parentGameObj = null;
    public List<Button> btnList; // to be optimized

    Vector2 ownSize;
    Vector3 ownPosition;

    bool isMaximized = false;

    private void Start()
    {
        StoreOwnInfo();
    }

    private void StoreOwnInfo()
    {
        ownPosition = thisRect.anchoredPosition;
        ownSize = thisRect.sizeDelta;
    }

    private void Maximize()
    {
        thisRect.anchoredPosition = new Vector3(0f, 0f, 0f);
        thisRect.sizeDelta = new Vector2(1920f, 1080f);
        isMaximized = true;
    }

    private void ResizeBack()
    {
        thisRect.anchoredPosition = ownPosition;
        thisRect.sizeDelta = ownSize;
        isMaximized = false;
    }

    public void ResizeWindow()
    {
        if (isMaximized)
        {
            ShowAllActiveWindow();
            ActivateAllButtons();
            ResizeBack();
        }
        else
        {
            LeaveOnlyOneActiveWindow();
            LeaveOnlyOneActivButton();
            Maximize();
        }
    }

    private void LeaveOnlyOneActiveWindow()
    {
        RawImage[] rawImages = parentGameObj.GetComponentsInChildren<RawImage>();
        foreach (RawImage rawImage in rawImages)
            rawImage.enabled = false;
        thisRect.gameObject.GetComponent<RawImage>().enabled = true;
    }

    private void ShowAllActiveWindow()
    {
        RawImage[] rawImages = parentGameObj.GetComponentsInChildren<RawImage>();
        foreach (RawImage rawImage in rawImages)
            rawImage.enabled = true;
    }

    private void LeaveOnlyOneActivButton()
    {
        foreach (Button btn in btnList)
            btn.enabled = false;
        this.gameObject.GetComponent<Button>().enabled = true;
    }

    private void ActivateAllButtons()
    {
        foreach (Button btn in btnList)
            btn.enabled = true;
    }

    //// anchorPosition only scales the anchor and left other components of the object!
    ///  which means that the button's influential area of the object remains as befors.
    ///  If this problem is well tackled, btnList is then disposable.
}
