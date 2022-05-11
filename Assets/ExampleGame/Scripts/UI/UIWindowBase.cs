using UnityEngine;

public enum WinType
{
    Hud,
    Inventory,
    Store,
    SpecialOffer,
}

public class UIWindowBase : MonoBehaviour
{
    [SerializeField]
    private WinType winType;

    public WinType WinType => winType;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void Init(object data) {}
}
