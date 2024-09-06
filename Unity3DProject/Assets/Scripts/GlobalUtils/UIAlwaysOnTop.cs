using UnityEngine;

public class UIAlwaysOnTop : MonoBehaviour
{
    void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}