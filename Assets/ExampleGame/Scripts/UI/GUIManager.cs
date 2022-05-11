using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private UIWindowBase[] _allWindows;
    private UIWindowBase _currentWindow;

    private void Awake()
    {
        _allWindows = GetComponentsInChildren<UIWindowBase>(true);
        CloseAllWindows();

        GlobalEvents.UI.OpenWindowEvent += OnOpenWindow;
        GlobalEvents.UI.CloseCurrentWindowEvent += CloseCurrentWindowWindow;
    }

    private void OnDestroy()
    {
        GlobalEvents.UI.OpenWindowEvent -= OnOpenWindow;
        GlobalEvents.UI.CloseCurrentWindowEvent -= CloseCurrentWindowWindow;
    }

    private void OnOpenWindow(WinType winType, object data)
    {
        var window = OpenWindow(winType);
        window.Init(data);
    }

    private void CloseAllWindows()
    {
        foreach (var window in _allWindows)
            window.Hide();

        OpenWindow(WinType.Hud);
    }

    private UIWindowBase OpenWindow(WinType winType)
    {
        _currentWindow?.Hide();
        _currentWindow = GetWindow(winType);
        _currentWindow?.Show();
        return _currentWindow;
    }
    
    public void CloseCurrentWindowWindow()
    {
        _currentWindow?.Hide();
        OpenWindow(WinType.Hud);
    }

    private void CloseWindow(WinType winType)
    {
        if (_currentWindow?.WinType == winType)
        {
            _currentWindow?.Hide();
            OpenWindow(WinType.Hud);
        }
    }

    private void OpenHUD()
    {
        OpenWindow(WinType.Hud);
    }

    private UIWindowBase GetWindow(WinType winType)
    {
        foreach (var window in _allWindows)
            if (window.WinType == winType)
            {
                return window;
            }

        return null;
    }
}
