using System;
using Balancy.Models.Store;
using UnityEngine;

[CreateAssetMenu(fileName = "QualityRibbonConfig", menuName = "ScriptableObjects/QualityRibbonConfig", order = 1)]
public class QualityRibbonConfig : ScriptableObject
{
    [Serializable]
    public class QualityConfig
    {
        public StoreSlotType Type;
        public string DisplayText;
        public Color BackColor;
        public Sprite RibbonImage;
        public Sprite BackImage;
    }

    [SerializeField]
    private QualityConfig[] configs;

    public QualityConfig GetQualityConfig(StoreSlotType type)
    {
        foreach (var config in configs)
        {
            if (config.Type == type)
                return config;
        }

        return null;
    }
}
