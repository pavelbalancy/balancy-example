using UnityEngine;

public class CloseCurrentWindow : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<MyButton>();
        button.onClick.AddListener(Close);
    }

    void Close()
    {
        GlobalEvents.UI.InvokeCloseCurrentWindow();
    }
}
