using Balancy.Interfaces;
using Balancy.Models.LiveOps.Store;
using UnityEngine;

namespace Balancy
{
    //TOTO make your own version of this file, because the original file will be overwritten after Balancy update
    public class LiveOpsStoreEventsExample : IStoreEvents
    {
        public void OnStoreResourcesMultiplierChanged(float multiplier)
        {
            Debug.Log("=> OnStoreResourcesMultiplierChanged: " + multiplier);
        }

        public void OnStoreUpdated(Config storeConfig)
        {
            Debug.Log("=> OnStoreUpdated: " + storeConfig.UnnyId);
        }

        public void OnStorePageUpdated(Config storeConfig, Page page)
        {
            Debug.Log("=> OnStorePageUpdated: " + storeConfig.UnnyId + " page = " + page.Name.Value);
        }
    }
}
