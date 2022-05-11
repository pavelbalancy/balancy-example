using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseTypeConfig", menuName = "ScriptableObjects/PurchaseTypeConfig", order = 1)]
public class PurchaseTypeConfig : ScriptableObject
{
    [Serializable]
    public class PurchaseConfig
    {
        public ModelExtensions.ItemType Type;
        public bool ShowText; 
        public Color TextColor;
        public Color GlowColor;
    }

    [SerializeField]
    private PurchaseConfig[] configs;

    public PurchaseConfig GetPurchaseConfig(ModelExtensions.ItemType type)
    {
        foreach (var config in configs)
        {
            if (config.Type == type)
                return config;
        }

        return null;
    }
}
