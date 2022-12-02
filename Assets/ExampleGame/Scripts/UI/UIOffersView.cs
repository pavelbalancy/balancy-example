using Balancy;
using Balancy.Data.SmartObjects;
using Balancy.SmartObjects;
using UnityEngine;

public class UIOffersView : MonoBehaviour
{
    [SerializeField]
    private Transform offersContainer;
    [SerializeField]
    private GameObject offerPrefab;
    
    private void Awake()
    {
        Launcher.WaitForBalancyToInit(RefreshOffers);
        MySmartObjectsEvents.OnNewOfferActivatedEvent += OnNewOfferActivated;
        MySmartObjectsEvents.OnOfferDeactivatedEvent += OnOfferDeactivated;
    }

    private void OnDestroy()
    {
        MySmartObjectsEvents.OnNewOfferActivatedEvent -= OnNewOfferActivated;
        MySmartObjectsEvents.OnOfferDeactivatedEvent -= OnOfferDeactivated;
    }
    
    private void OnNewOfferActivated(OfferInfo offerInfo)
    {
        RefreshOffers();
    }
    
    private void OnOfferDeactivated(OfferInfo offerInfo, bool wasPurchased)
    {
        RefreshOffers();
    }

    private void RefreshOffers()
    {
        offersContainer.RemoveChildren();

        var allOffers = LiveOps.GameOffers.GetActiveOffers();
        foreach (var offer in allOffers)
        {
            var newItem = Instantiate(offerPrefab, offersContainer);
            var storeItemView = newItem.GetComponent<UIOfferIconView>();
            storeItemView.Init(offer);
        }
    }
}
