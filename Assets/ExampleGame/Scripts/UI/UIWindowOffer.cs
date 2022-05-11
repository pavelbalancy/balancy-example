using Balancy.API.Payments;
using Balancy.Data.SmartObjects;
using Balancy.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowOffer : UIWindowBase
{
    [SerializeField]
    private TMP_Text offerName;
    
    [SerializeField]
    private TMP_Text offerDescription;
    
    [SerializeField]
    private TMP_Text offerDiscount;
    
    [SerializeField]
    private TMP_Text offerPrice;
    
    [SerializeField]
    private Button buttonBuy;
    
    [SerializeField]
    private RemoteImage offerImage;

    private OfferInfo _offerInfo;

    private void Awake()
    {
        buttonBuy.onClick.AddListener(PurchaseOffer);
    }

    private void PurchaseOffer()
    {
        Manager.OfferWasPurchased(_offerInfo, new PaymentInfo
        {
            Currency = "USD",
            Price = _offerInfo.PriceUSD,
            ProductId = _offerInfo.ProductId,
        }, (data) => { GlobalEvents.UI.InvokeCloseCurrentWindow(); });
    }

    public override void Init(object data)
    {
        base.Init(data);
        if (data is OfferInfo offerInfo)
            InitOffer(offerInfo);
        else
            Debug.LogError("Wrong init data type");
    }

    private void InitOffer(OfferInfo offerInfo)
    {
        var gameOffer = offerInfo.GameOffer;
        offerName.SetText(gameOffer.Name.Value);
        offerDescription.SetText(gameOffer.Description.Value);
        offerImage.LoadObject(gameOffer.Sprite);
        
        offerDiscount.SetText($"-{offerInfo.Discount}%");
        offerPrice.SetText($"US ${offerInfo.PriceUSD}");

        _offerInfo = offerInfo;
    }
}
