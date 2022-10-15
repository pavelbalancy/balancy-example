using System;
using Balancy.Models.LiveOps.Store;
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

    private Page _storePage;
    
    public void Init(Page storePage, Action<Page> onSelected)
    {
        _storePage = storePage;
        button.onClick.RemoveAllListeners();

        title.SetText(storePage.Name.Value);
        button.onClick.AddListener(() => onSelected?.Invoke(_storePage));
    }

    public bool EqualsToPage(Page storePage)
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
