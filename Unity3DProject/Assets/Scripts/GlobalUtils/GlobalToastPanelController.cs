using UnityEngine;

/*by Aizierjiang*/

/**
 * This class controlls the global toasing page.
 * This script must be linked on GlobalToast gameObject!
 * */
public class GlobalToastPanelController : MonoBehaviour
{
    public static System.Action acceptionCallback;
    public static System.Action rejectionCallback;

    public static bool isLastChoiceAcception = false;

    private void InActivateToast()
    {
        this.gameObject.SetActive(false);
    }

    public void Reject()
    {
        StateController.Instance.State = UserState.Online;
        InActivateToast();
        acceptionCallback();
        isLastChoiceAcception = true;
    }

    public void Accept()
    {
        StateController.Instance.State = UserState.Chatting;
        InActivateToast();
        rejectionCallback();
        isLastChoiceAcception = false;
    }
}
