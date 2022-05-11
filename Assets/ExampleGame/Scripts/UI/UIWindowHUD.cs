using UnityEngine;
using UnityEngine.UI;

public class UIWindowHUD : UIWindowBase
{
    [SerializeField]
    private Button storeButton;
    [SerializeField]
    private Button assetLink;

    private void Awake()
    {
        storeButton.onClick.AddListener(OpenStore);
        assetLink.onClick.AddListener(OpenAssetLink);
    }

    private void OpenAssetLink()
    {
        Application.OpenURL("https://assetstore.unity.com/packages/2d/gui/gui-pro-kit-fantasy-rpg-170168");
    }

    private void OpenStore()
    {
        GlobalEvents.UI.InvokeOpenWindow(WinType.Store);
    }
}
