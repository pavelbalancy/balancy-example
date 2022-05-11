using Balancy;
using Balancy.Data.SmartObjects;
using UnityEngine;

public class OffersManager : MonoBehaviour
{
    private void Awake()
    {
        ExternalEvents.SmartObjects.NewOfferActivatedEvent += OnNewOfferActivated;
    }

    private void OnDestroy()
    {
        ExternalEvents.SmartObjects.NewOfferActivatedEvent -= OnNewOfferActivated;
    }

    private void OnNewOfferActivated(OfferInfo offerInfo)
    {
        GlobalEvents.UI.InvokeOpenWindow(WinType.SpecialOffer, offerInfo);
    }
}
