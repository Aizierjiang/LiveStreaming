using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*by Aizierjiang*/

public class MultiCamPageBTNController : MonoBehaviour
{
    // Now here in the list is only two pages

    public List<GameObject> multiCamPageList;
    public List<GameObject> multiCamPageListForInteraction;
    public GameObject previousPageBTNUI = null;
    public GameObject nextPageBTNUI = null;

    public void OnNextBTNClicked()
    {
        InActivateAllPages();
        multiCamPageList[multiCamPageList.Count - 1].SetActive(true);
        multiCamPageListForInteraction[multiCamPageList.Count - 1].SetActive(true);

        previousPageBTNUI.SetActive(true);
        nextPageBTNUI.SetActive(false);
    }

    public void OnPreviousBTNClicked()
    {
        InActivateAllPages();
        multiCamPageList[0].SetActive(true);
        multiCamPageListForInteraction[0].SetActive(true);

        previousPageBTNUI.SetActive(false);
        nextPageBTNUI.SetActive(true);
    }

    private void InActivateAllPages()
    {
        foreach (GameObject page in multiCamPageList)
            page.SetActive(false);

        foreach (GameObject page in multiCamPageListForInteraction)
            page.SetActive(false);
    }
}
