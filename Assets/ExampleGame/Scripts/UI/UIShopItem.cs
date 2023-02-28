using System;
using Balancy.Models.LiveOps.Store;
using UnityEngine;

public class UIShopItem : UIItemBase
{
    [SerializeField]
    private UIStoreItemBuyButton button;
    
    private Action<UIShopItem, Slot> _onPurchase;
    private Slot _storeSlot;
    
    public void Init(Slot storeSlot, Action<UIShopItem, Slot> onPurchase)
    {
        base.Init(storeSlot.GetStoreItem());
        var slotView = GetComponent<UIStoreSlotType>();
        slotView?.SetType(storeSlot.Type);

        button.Init(storeSlot, OnTryToBuy);
        _onPurchase = onPurchase;
        _storeSlot = storeSlot;
    }

    private void OnTryToBuy()
    {
        _onPurchase?.Invoke(this, _storeSlot);
    }
}
