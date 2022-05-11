using System;
using Balancy.Models.SmartObjects;
using Balancy.Models.Store;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    [SerializeField]
    private RemoteImage remoteImage;
    
    [SerializeField]
    private TMP_Text itemName;
    
    [SerializeField]
    private TMP_Text itemsAmount;
    
    [SerializeField]
    private Image imageGlow;

    [SerializeField]
    private UIStoreItemBuyButton button;

    [SerializeField]
    private PurchaseTypeConfig config;

    private Action<UIShopItem, StoreSlot> _onPurchase;
    private StoreSlot _storeSlot;
    
    public void Init(StoreSlot storeSlot, Action<UIShopItem, StoreSlot> onPurchase)
    {
        var slotView = GetComponent<UIStoreSlotType>();
        slotView?.SetType(storeSlot.Type);
        
        itemName.SetText(storeSlot.StoreItem.Name.ToString());
        remoteImage.LoadObject(storeSlot.StoreItem.Sprite);
        button.Init(storeSlot.StoreItem.Price, OnTryToBuy);

        ApplyPurchaseConfig(storeSlot);

        _onPurchase = onPurchase;
        _storeSlot = storeSlot;
    }

    private void ApplyPurchaseConfig(StoreSlot storeSlot)
    {
        var purchaseConfig = config.GetPurchaseConfig(storeSlot.StoreItem.GetRewardType());
        if (purchaseConfig != null)
        {
            itemsAmount.gameObject.SetActive(purchaseConfig.ShowText);
            itemsAmount.color = purchaseConfig.TextColor;
            imageGlow.color = purchaseConfig.GlowColor;

            if (purchaseConfig.ShowText)
                SetPurchaseItemCount(storeSlot.StoreItem);
        }
    }

    private void SetPurchaseItemCount(StoreItem storeItem)
    {
        var firstItem = storeItem.Reward.Items.Length > 0 ? storeItem.Reward.Items[0] : null;
        itemsAmount.SetText(firstItem != null ? firstItem.count.ToString() : string.Empty);
    }

    private void OnTryToBuy()
    {
        _onPurchase?.Invoke(this, _storeSlot);
    }
}
