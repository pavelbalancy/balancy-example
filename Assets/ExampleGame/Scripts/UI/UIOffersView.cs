using Balancy;
using Balancy.Data.SmartObjects;
using UnityEngine;

public class UIOffersView : MonoBehaviour
{
    [SerializeField]
    private Transform offersContainer;
    [SerializeField]
    private GameObject offerPrefab;
    [SerializeField]
    private GameObject offerGroupPrefab;
    
    private void Awake()
    {
        Launcher.WaitForBalancyToInit(RefreshOffers);
        MySmartObjectsEvents.OnNewOfferActivatedEvent += OnNewOfferActivated;
        MySmartObjectsEvents.OnOfferDeactivatedEvent += OnOfferDeactivated;
        
        MySmartObjectsEvents.OnNewOfferGroupActivatedEvent += OnNewOfferGroupActivated;
        MySmartObjectsEvents.OnOfferGroupDeactivatedEvent += OnOfferGroupDeactivated;
    }

    private void OnDestroy()
    {
        MySmartObjectsEvents.OnNewOfferActivatedEvent -= OnNewOfferActivated;
        MySmartObjectsEvents.OnOfferDeactivatedEvent -= OnOfferDeactivated;
        
        MySmartObjectsEvents.OnNewOfferGroupActivatedEvent -= OnNewOfferGroupActivated;
        MySmartObjectsEvents.OnOfferGroupDeactivatedEvent -= OnOfferGroupDeactivated;
    }

    private void OnNewOfferGroupActivated(OfferGroupInfo offerInfo)
    {
        RefreshOffers();
    }

    private void OnOfferGroupDeactivated(OfferGroupInfo offerInfo)
    {
        RefreshOffers();
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
        if (allOffers != null)
        {
            foreach (var offer in allOffers)
            {
                var newItem = Instantiate(offerPrefab, offersContainer);
                var storeItemView = newItem.GetComponent<UIOfferIconView>();
                storeItemView.Init(offer);
            }
        }
        
        var allOfferGroups = LiveOps.GameOffers.GetActiveOfferGroups();
        if (allOfferGroups != null)
        {
            foreach (var offer in allOfferGroups)
            {
                var newItem = Instantiate(offerGroupPrefab, offersContainer);
                var storeItemView = newItem.GetComponent<UIOfferGroupIconView>();
                storeItemView.Init(offer);
            }
        }
    }
}
