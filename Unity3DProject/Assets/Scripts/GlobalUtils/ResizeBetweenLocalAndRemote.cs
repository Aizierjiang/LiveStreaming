using UnityEngine;

/*by Aizierjiang*/

public class ResizeBetweenLocalAndRemote : MonoBehaviour
{
    public GameObject localView = null;
    public GameObject remoteView = null;
    public GameObject interactablesToBeBlocked = null;

    private Vector2 localSize, remoteSize;
    private Vector3 localPosition, remotePosition;
    private bool isRemoteViewMaximized = false;

    private void StorInfo()
    {
        localPosition = localView.gameObject.GetComponent<RectTransform>().anchoredPosition;
        localSize = localView.gameObject.GetComponent<RectTransform>().sizeDelta;
        remotePosition = remoteView.gameObject.GetComponent<RectTransform>().anchoredPosition;
        remoteSize = remoteView.gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    private void MaximizeRemoteView()
    {
        localView.gameObject.GetComponent<RectTransform>().anchoredPosition = remotePosition;
        localView.gameObject.GetComponent<RectTransform>().sizeDelta = remoteSize;
        remoteView.gameObject.GetComponent<RectTransform>().anchoredPosition = localPosition;
        remoteView.gameObject.GetComponent<RectTransform>().sizeDelta = localSize;
        localView.transform.SetAsLastSibling();
        interactablesToBeBlocked.SetActive(false);
        isRemoteViewMaximized = true;
    }

    private void MaximizeLocalView()
    {
        remoteView.gameObject.GetComponent<RectTransform>().anchoredPosition = remotePosition;
        remoteView.gameObject.GetComponent<RectTransform>().sizeDelta = remoteSize;
        localView.gameObject.GetComponent<RectTransform>().anchoredPosition = localPosition;
        localView.gameObject.GetComponent<RectTransform>().sizeDelta = localSize;
        remoteView.transform.SetAsLastSibling();
        interactablesToBeBlocked.SetActive(true);
        isRemoteViewMaximized = false;
    }

    private void OnEnable()
    {
        StorInfo();
    }

    public void Rezise()
    {
        if (isRemoteViewMaximized)
            MaximizeLocalView();
        else
            MaximizeRemoteView();
    }
}
