using UnityEngine;

/*by Aizierjiang*/

public class PlayerUpsideDown : MonoBehaviour
{

    public GameObject playerImage = null;
    private float xAngles = 0f;

    public void MakeImageUpsideDown()
    {
        if (xAngles > 360f || xAngles < -360f) xAngles = 0f;
        else xAngles += 180f;

        playerImage.GetComponent<RectTransform>().transform.localEulerAngles = new Vector3(xAngles, 0f, 0f);
    }
}
