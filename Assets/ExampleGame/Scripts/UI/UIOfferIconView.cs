using Balancy;
using Balancy.Data.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOfferIconView : MonoBehaviour
{
    private const int TIMER_UPDATE = 1;
    
    [SerializeField]
    private RemoteImage image;
    [SerializeField]
    private TMP_Text discount;
    [SerializeField]
    private TMP_Text timeLeft;
    [SerializeField]
    private Button button;

    private OfferInfo _offerInfo;
    public void Init(OfferInfo offerInfo)
    {
        _offerInfo = offerInfo;
        image.LoadObject(_offerInfo.GameOffer.Sprite);
        discount.SetText($"{_offerInfo.Discount}%");
    }

    private void Awake()
    {
        button.onClick.AddListener(OnOpenOffer);
        BalancyTimer.SubscribeForTimer(TIMER_UPDATE, UpdateTimer);
    }

    private void OnOpenOffer()
    {
        GlobalEvents.UI.InvokeOpenWindow(WinType.SpecialOffer, _offerInfo);
    }

    private void OnDestroy()
    {
        BalancyTimer.UnsubscribeFromTimer(TIMER_UPDATE, UpdateTimer);
    }

    private void UpdateTimer()
    {
        var time = _offerInfo.GetSecondsLeftBeforeDeactivation();
        var timeLeftString = TimeFormatter.GetTimeString(time);
        timeLeft.SetText(timeLeftString);
    }
}
