using Balancy.Models.LiveOps.Store;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreSlotType : MonoBehaviour
{
    [SerializeField]
    private GameObject qualityRibbon;
    [SerializeField]
    private Image qualityImage;
    [SerializeField]
    private TMP_Text qualityText;
    
    [SerializeField]
    private Image backImage;
    [SerializeField]
    private Image glowImage;
    
    [SerializeField]
    private QualityRibbonConfig config;

    public void SetType(SlotType type)
    {
        var qConfig = config.GetQualityConfig(type);
        if (qConfig == null)
        {
            qualityRibbon.SetActive(false);
        }
        else
        {
            qualityRibbon.SetActive(true);
            qualityImage.sprite = qConfig.RibbonImage;
            qualityText.SetText(qConfig.DisplayText);
            backImage.sprite = qConfig.BackImage;
            glowImage.color = qConfig.BackColor;
        }
    }
}
