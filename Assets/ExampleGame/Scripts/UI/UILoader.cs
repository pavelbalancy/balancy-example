using UnityEngine;

public class UILoader : MonoBehaviour
{
    void Start()
    {
        Launcher.WaitForBalancyToInit(Hide);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
