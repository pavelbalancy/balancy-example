using Balancy.Interfaces;
using Balancy.Models.LiveOps.Store;
using Balancy.Models.SmartObjects;
using UnityEngine;

namespace Balancy
{
    public class MyStoreEvents : IStoreEvents
    {
        public void OnStoreResourcesMultiplierChanged(float multiplier)
        {
            Debug.Log("=> OnStoreResourcesMultiplierChanged: " + multiplier);
        }

        public void OnStoreUpdated(GameStoreBase storeConfig)
        {
            Debug.Log("=> OnStoreUpdated: " + storeConfig.UnnyId);
        }

        public void OnStorePageUpdated(GameStoreBase storeConfig, Page page)
        {
            Debug.Log("=> OnStorePageUpdated: " + storeConfig.UnnyId + " page = " + page.Name.Value);
        }
    }
}
