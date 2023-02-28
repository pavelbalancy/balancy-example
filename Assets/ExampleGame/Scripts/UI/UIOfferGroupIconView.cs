using Balancy;
using Balancy.Data.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOfferGroupIconView : MonoBehaviour
{
    private const int TIMER_UPDATE = 1;
    
    [SerializeField]
    private RemoteImage image;
    [SerializeField]
    private TMP_Text timeLeft;
    [SerializeField]
    private Button button;

    private OfferGroupInfo _offerInfo;
    
    public void Init(OfferGroupInfo offerInfo)
    {
        _offerInfo = offerInfo;
        image.LoadObject(_offerInfo.GameOfferGroup.Icon);
    }

    private void Awake()
    {
        button.onClick.AddListener(OnOpenOffer);
        BalancyTimer.SubscribeForTimer(TIMER_UPDATE, UpdateTimer);
    }

    private void OnOpenOffer()
    {
        GlobalEvents.UI.InvokeOpenWindow(WinType.OffersGroup, _offerInfo);
    }

    private void OnDestroy()
    {
        BalancyTimer.UnsubscribeFromTimer(TIMER_UPDATE, UpdateTimer);
    }

    private void UpdateTimer()
    {
        if (_offerInfo.GameOfferGroup.Duration == 0)
        {
            timeLeft.gameObject.SetActive(false);
            return;
        }
        
        var time = _offerInfo.GetSecondsLeftBeforeDeactivation();
        var timeLeftString = TimeFormatter.GetTimeString(time);
        timeLeft.SetText(timeLeftString);
    }
}
