using System;
using Balancy.Data;
using Balancy.Data.SmartObjects;
using Balancy.Models.SmartObjects;
using Balancy.SmartObjects;
using UnityEngine;

namespace Balancy
{
    public class MySmartObjectsEvents : ISmartObjectsEvents
    {
        public static Action<OfferInfo> OnNewOfferActivatedEvent;
        public static Action<OfferInfo, bool> OnOfferDeactivatedEvent;
        public static Action OnSmartObjectsInitializedEvent;
        
        public void OnSystemProfileConflictAppeared()
        {
            Debug.Log("=> OnSystemProfileConflictAppeared");
            // UnnyProfileManager.SolveConflict(ConflictsManager.VersionType.Local);
            UnnyProfileManager.SolveConflict(ConflictsManager.VersionType.Cloud);
        }

        public void OnSmartObjectConflictAppeared(ParentBaseData parentBaseData)
        {
            Debug.Log("=> OnSmartObjectConflictAppeared: " + parentBaseData);
            //TODO Show window with conflict between 'parentBaseData' (ConflictsManager.VersionType.Local) and 'parentBaseData.ConflictsManager.ConflictData' (ConflictsManager.VersionType.Cloud)
            // parentBaseData.ConflictsManager.SolveConflict(ConflictsManager.VersionType.Local);
            parentBaseData.ConflictsManager.SolveConflict(ConflictsManager.VersionType.Cloud);
        }
        
        public void OnSmartObjectNewVersionAppeared(ParentBaseData parentBaseData)
        {
            Debug.Log("=> OnSmartObjectNewVersionAppeared: " + parentBaseData);
            //TODO Show window with conflict between 'parentBaseData' (ConflictsManager.VersionType.Local) and 'parentBaseData.ConflictsManager.ConflictData' (ConflictsManager.VersionType.Cloud)
            // parentBaseData.ConflictsManager.SolveConflict(ConflictsManager.VersionType.Local);
            parentBaseData.ConflictsManager.SolveConflict(ConflictsManager.VersionType.Cloud);
        }

        public void OnNewOfferActivated(OfferInfo offerInfo)
        {
            Debug.Log("=> OnNewOfferActivated: " + offerInfo?.GameOffer?.Name + " ; Price = " + offerInfo?.PriceUSD + " ; Discount = " + offerInfo?.Discount);
            GlobalEvents.UI.InvokeOpenWindow(WinType.SpecialOffer, offerInfo);
            OnNewOfferActivatedEvent?.Invoke(offerInfo);
        }

        public void OnOfferDeactivated(OfferInfo offerInfo, bool wasPurchased)
        {
            Debug.Log("=> OnOfferDeactivated: " + offerInfo?.GameOffer?.Name + " ; wasPurchased = " + wasPurchased);
            OnOfferDeactivatedEvent?.Invoke(offerInfo, wasPurchased);
        }

        public void OnNewEventActivated(EventInfo eventInfo)
        {
            Debug.Log("=> OnNewEventActivated: " + eventInfo?.GameEvent?.Name);
        }

        public void OnEventDeactivated(EventInfo eventInfo)
        {
            Debug.Log("=> OnEventDeactivated: " + eventInfo?.GameEvent?.Name);
        }

        public void OnOfferPurchased(OfferInfo offerInfo)
        {
            Debug.Log("=> OnOfferPurchased: " + offerInfo?.GameOffer?.Name);
        }

        public void OnOfferFailedToPurchase(OfferInfo offerInfo, string error)
        {
            Debug.Log("=> OnOfferFailedToPurchase: " + offerInfo?.GameOffer?.Name + " ; Error = " + error);
        }

        public void OnStoreItemFailedToPurchase(StoreItem storeItem, string error)
        {
            Debug.Log("=> OnOfferFailedToPurchase: " + storeItem?.Name + " ; Error = " + error);
        }

        public void OnSegmentUpdated(Segment segment)
        {
            Debug.Log("=> OnSegmentUpdated: " + segment?.Name + " ; Option = " + SegmentsManager.GetSegmentSystemOption(segment));
        }

        public void OnSmartObjectsInitialized()
        {
            Debug.Log("=> OnSmartObjectsInitialized:  You can now make purchase, request all GameEvents, GameOffers, A/B Tests, etc...");
            OnSmartObjectsInitializedEvent?.Invoke();
        }
    }
}
