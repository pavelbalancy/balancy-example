using System;
using Balancy.Example;
using Balancy.Models.LiveOps.Store;
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
    private TMP_Text itemsAmountCrossed;
    
    [SerializeField]
    private TMP_Text itemsAmount;
    
    [SerializeField]
    private Image imageGlow;

    [SerializeField]
    private UIStoreItemBuyButton button;

    [SerializeField]
    private PurchaseTypeConfig config;
    
    [SerializeField]
    private ForceTransformUpdater transformUpdater;

    private Action<UIShopItem, Slot> _onPurchase;
    private Slot _storeSlot;
    
    public void Init(Slot storeSlot, Action<UIShopItem, Slot> onPurchase)
    {
        var slotView = GetComponent<UIStoreSlotType>();
        slotView?.SetType(storeSlot.Type);

        var storeItem = storeSlot.GetStoreItem();
        itemName.SetText(storeItem.Name.ToString());
        remoteImage.LoadObject(storeItem.Sprite);
        button.Init(storeSlot, OnTryToBuy);

        ApplyPurchaseConfig(storeItem);

        _onPurchase = onPurchase;
        _storeSlot = storeSlot;
    }

    private void ApplyPurchaseConfig(StoreItem storeItem)
    {
        var purchaseConfig = config.GetPurchaseConfig(storeItem.GetRewardType());
        if (purchaseConfig != null)
        {
            itemsAmount.gameObject.SetActive(purchaseConfig.ShowText);
            itemsAmount.color = purchaseConfig.TextColor;
            itemsAmountCrossed.gameObject.SetActive(purchaseConfig.ShowText);
            itemsAmountCrossed.color = purchaseConfig.TextColor;
            imageGlow.color = purchaseConfig.GlowColor;

            if (purchaseConfig.ShowText)
                SetPurchaseItemCount(storeItem);
        }
    }

    private void SetPurchaseItemCount(StoreItem storeItem)
    {
        var firstItem = storeItem.Reward.Items.Length > 0 ? storeItem.Reward.Items[0] : null;
        if (firstItem != null)
        {
            itemsAmount.SetText(firstItem.count.ToString());
            if (storeItem.GetMultiplier() <= 1.001f)
            {
                itemsAmountCrossed?.gameObject.SetActive(false);
            }
            else
            {
                var originalCount = (int)(firstItem.Count / storeItem.GetMultiplier() + 0.5f);
                itemsAmountCrossed?.SetText(originalCount.ToString());
                itemsAmountCrossed?.gameObject.SetActive(true);
            }

            transformUpdater?.ForceUpdate();
        }
    }

    private void OnTryToBuy()
    {
        _onPurchase?.Invoke(this, _storeSlot);
    }
}
