using System;
using Balancy;
using Balancy.API.Payments;
using Balancy.Data.SmartObjects;
using Balancy.Models.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowOfferGroup : UIWindowBase
{
    [SerializeField]
    private TMP_Text offerName;
    
    [SerializeField]
    private Transform content;
    
    [SerializeField]
    private Button closeButton;
    
    [SerializeField]
    private GameObject storeItemPrefab;

    private OfferGroupInfo _offerInfo;

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseWindow);
    }

    public override void Init(object data)
    {
        base.Init(data);
        if (data is OfferGroupInfo offerInfo)
            InitOffer(offerInfo);
        else
            Debug.LogError("Wrong init data type");
    }

    private void InitOffer(OfferGroupInfo offerInfo)
    {
        var gameOffer = offerInfo.GameOfferGroup;
        offerName.SetText(gameOffer.Name.Value);

        _offerInfo = offerInfo;
        RefreshContent();
    }

    private void RefreshContent()
    {
        content.RemoveChildren();
        
        foreach (var storeItem in _offerInfo.GameOfferGroup.StoreItems)
        {
            var canPurchase = _offerInfo.CanPurchase(storeItem);
            var newItem = Instantiate(storeItemPrefab, content);
            var storeItemView = newItem.GetComponent<UIStoreItem>();
            storeItemView.Init(storeItem, canPurchase, OnTryToPurchase);
        }
    }

    private void OnTryToPurchase(UIStoreItem uiStoreItem, StoreItem storeItem)
    {
        if (storeItem.IsFree())
        {
            LiveOps.GameOffers.OfferWasPurchased(_offerInfo, storeItem, null);
            RefreshContent();
        }
        else
        {
            switch (storeItem.Price.Type)
            {
                case PriceType.Hard:
                    LiveOps.GameOffers.OfferWasPurchased(_offerInfo, storeItem, new PaymentInfo
                    {
                        Currency = "USD",
                        Price = storeItem.Price.Product.Price,
                        ProductId = storeItem.Price.Product.ProductId,
                    }, response =>
                    {
                        Debug.LogWarning("Offer Purchase " + response.Success);
                        RefreshContent();
                    });
                    break;
                case PriceType.Soft:
                    LiveOps.GameOffers.OfferWasPurchased(_offerInfo, storeItem, storeItem.Price);
                    RefreshContent();
                    break;
                case PriceType.Ads:
                    LiveOps.GameOffers.PurchaseOffer(_offerInfo, storeItem, response =>
                    {
                        Debug.LogWarning("Offer Purchase for Ads " + response.Success);
                        RefreshContent();
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
