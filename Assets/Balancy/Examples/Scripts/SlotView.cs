using Balancy.API.Payments;
using Balancy.Models.LiveOps.Store;
using Balancy.Models.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Balancy.Example
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text slotName;
        [SerializeField] private TMP_Text count;
        [SerializeField] private TMP_Text countCrossed;
        [SerializeField] private Image icon;
        
        [SerializeField] private GameObject ribbon;
        [SerializeField] private Image ribbonIcon;
        [SerializeField] private TMP_Text ribbonText;
        
        [SerializeField] private Button buyButton;
        [SerializeField] private Image buyIcon;
        [SerializeField] private TMP_Text buyText;

        [SerializeField] private QualityRibbonConfig ribbonConfig;
        [SerializeField] private ForceTransformUpdater transformUpdater;

        private Slot _slot;

        private void Start()
        {
            buyButton?.onClick.AddListener(TryToBuy);
        }

        private void TryToBuy()
        {
            var storeItem = _slot.GetStoreItem();
            if (storeItem.IsInApp())
            {
                Balancy.LiveOps.Store.ItemWasPurchased(storeItem, new PaymentInfo
                {
                    Currency = "USD",
                    Price = storeItem.Price.Product.Price,
                    ProductId = storeItem.Price.Product.ProductId
                }, response =>
                {
                    Debug.Log("Purchase " + response.Success + " ? " + response.Error?.Message);
                    //TODO give resources from Reward
                });
            }
            else
            {
                //TODO Try to take soft resources from Price
                Balancy.LiveOps.Store.ItemWasPurchased(storeItem, storeItem.Price);
                //TODO give resources from Reward
            }
        }

        public void Init(Slot slot)
        {
            _slot = slot;
            ApplyMainInfo(slot.GetStoreItem());
            ApplyRibbon(slot);
            ApplyBuyButton(slot.GetStoreItem());
        }

        private void ApplyMainInfo(StoreItem storeItem)
        {
            slotName?.SetText(storeItem.Name.Value);
            if (icon != null)
                storeItem.Sprite?.LoadSprite(sprite =>
                {
                    icon.sprite = sprite;
                });

            ApplyResourcesCount(storeItem);
        }

        private void ApplyResourcesCount(StoreItem storeItem)
        {
            var firstItem = storeItem.Reward.Items.Length > 0 ? storeItem.Reward.Items[0] : null;
            if (firstItem != null)
            {
                count?.SetText(firstItem.Count.ToString());
                if (storeItem.GetMultiplier() <= 1.001f)
                {
                    countCrossed?.gameObject.SetActive(false);
                }
                else
                {
                    var originalCount = (int)(firstItem.Count / storeItem.GetMultiplier() + 0.5f);
                    countCrossed?.SetText(originalCount.ToString());
                    countCrossed?.gameObject.SetActive(true);
                }
                
                transformUpdater?.ForceUpdate();
            }
            else
            {
                count?.gameObject.SetActive(false);
                countCrossed?.gameObject.SetActive(false);
            }
        }

        private void ApplyRibbon(Slot slot)
        {
            var config = ribbonConfig.GetQualityConfig(slot.Type);
            if (config == null)
                ribbon.SetActive(false);
            else
            {
                ribbonText?.SetText(config.DisplayText);
                if (ribbonIcon != null)
                    ribbonIcon.sprite = config.RibbonImage;
                ribbon.SetActive(true);
            }
        }

        private void ApplyBuyButton(StoreItem storeItem)
        {
            buyIcon?.gameObject.SetActive(false);
            if (storeItem.IsInApp())
            {
                buyText?.SetText("$ " + storeItem.Price.Product.Price);
            }
            else
            {
                var firstItem = storeItem.Price.Items.Length > 0 ? storeItem.Price.Items[0] : null;
                if (firstItem == null)
                {
                    buyText?.SetText("No price");
                }
                else
                {
                    buyText?.SetText(firstItem.Count.ToString());
                    if (buyIcon != null)
                    {
                        //TODO buyIcon.sprite = 
                    }
                }
            }
        }
    }
}