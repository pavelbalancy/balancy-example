using System;
using Balancy.Models.Store;
using TMPro;
using UnityEngine;

public class UIStoreTabButton : MonoBehaviour
{
    [SerializeField]
    private MyButton button;
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private GameObject visual;
    
    [SerializeField]
    private Color colorSelected;
    [SerializeField]
    private Color colorNotSelected;

    private StorePage _storePage;
    
    public void Init(StorePage storePage, Action<StorePage> onSelected)
    {
        _storePage = storePage;
        button.onClick.RemoveAllListeners();

        title.SetText(storePage.DisplayName);
        button.onClick.AddListener(() => onSelected?.Invoke(_storePage));
    }

    public bool Equals(StorePage storePage)
    {
        return _storePage == storePage;
    }

    public void SetLocked(bool locked)
    {
        button.interactable = !locked;
        visual.SetActive(locked);

        title.color = locked ? colorSelected : colorNotSelected;
    }
}
