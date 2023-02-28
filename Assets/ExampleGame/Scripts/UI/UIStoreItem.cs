using System;
using Balancy.Models.SmartObjects;
using UnityEngine;

public class UIStoreItem : UIItemBase
{
    [SerializeField]
    private UIBuyButton button;
    
    private Action<UIStoreItem, StoreItem> _onPurchase;
    private StoreItem _storeItem;
    
    public void Init(StoreItem storeItem, bool canPurchase, Action<UIStoreItem, StoreItem> onPurchase)
    {
        base.Init(storeItem);

        _storeItem = storeItem;
        
        button.Init(storeItem, canPurchase, OnTryToBuy);

        _onPurchase = onPurchase;
    }

    private void OnTryToBuy()
    {
        _onPurchase?.Invoke(this, _storeItem);
    }
}
