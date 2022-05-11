using System;
using Balancy.Models.Game;
using Balancy.Models.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreItemBuyButton : MonoBehaviour
{
    [SerializeField]
    private Button buttonHard;
    [SerializeField]
    private Button buttonSoft;
    
    [SerializeField]
    private TMP_Text labelHard;
    [SerializeField]
    private TMP_Text labelSoft;
    [SerializeField]
    private RemoteImage iconSoft;

    private Action _callback;

    public void Init(Price price, Action callback)
    {
        _callback = callback;
        FillTheData(price);
    }

    private void FillTheData(Price price)
    {
        var isInApp = price.IsInApp();
        buttonHard.gameObject.SetActive(isInApp);
        buttonSoft.gameObject.SetActive(!isInApp);
        if (isInApp)
        {
            labelHard.SetText($"US ${price.Product.Price}");
        }
        else
        {
            var firstItem = price.Items.Length > 0 ? price.Items[0] : null;
            if (firstItem != null)
            {
                labelSoft.SetText(firstItem.Count.ToString());
                iconSoft.LoadObject((firstItem.Item as GameItem)?.Icon);
            } else
                buttonSoft.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        buttonHard.onClick.AddListener(OnClick);
        buttonSoft.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _callback?.Invoke();
    }
}
