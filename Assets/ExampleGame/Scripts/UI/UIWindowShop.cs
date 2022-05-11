using System.Collections.Generic;
using Balancy;
using Balancy.API.Payments;
using Balancy.Data;
using Balancy.Models.Store;
using Balancy.SmartObjects;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowShop : UIWindowBase
{
    [SerializeField]
    private GameObject storeTabPrefab;
    [SerializeField]
    private GameObject storeItemPrefab;
    
    [SerializeField]
    private Transform content;
    [SerializeField]
    private Transform tabsContent;
    
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button homeButton;
    
    private StorePage _currentPage;

    private List<UIStoreTabButton> _tabButtons;
    private DefaultProfile _profile;

    private void Awake()
    {
        Launcher.WaitForBalancyToInit(Init);
        backButton.onClick.AddListener(CloseWindow);
        homeButton.onClick.AddListener(CloseWindow);
    }

    private void CloseWindow()
    {
        GlobalEvents.UI.InvokeCloseCurrentWindow();
    }

    private void Init()
    {
        CreateTabs();
        Launcher.LoadProfile(ProfileLoaded);
    }

    private void OnEnable()
    {
        if (_profile != null)
            CreateTabs();
    }

    private void CreateTabs()
    {
        var configs = DataEditor.StoreConfigs;

        tabsContent.RemoveChildren();
        _tabButtons = new List<UIStoreTabButton>();
        foreach (var config in configs)
        {
            if (!config.Condition?.CanPass() ?? false)
                continue;

            var pages = config.Pages;
            foreach (var page in pages)
            {
                var newButton = Instantiate(storeTabPrefab, tabsContent);
                var storeTabButton = newButton.GetComponent<UIStoreTabButton>();
                storeTabButton.Init(page, TabWasSelected);
                _tabButtons.Add(storeTabButton);
            }

            TabWasSelected(pages[0]);
            break;
        }
    }

    private void ProfileLoaded(DefaultProfile profile)
    {
        _profile = profile;
    }

    private void TabWasSelected(StorePage storePage)
    {
        foreach (var button in _tabButtons)
            button.SetLocked(button.Equals(storePage));

        RefreshContent(storePage);
    }

    private void RefreshContent(StorePage storePage)
    {
        content.RemoveChildren();
        
        foreach (var storeItem in storePage.Slots)
        {
            if (!storeItem.Condition?.CanPass() ?? false)
                continue;
            
            var newItem = Instantiate(storeItemPrefab, content);
            var storeItemView = newItem.GetComponent<UIShopItem>();
            storeItemView.Init(storeItem, OnTryToPurchase);
        }
    }

    private void OnTryToPurchase(UIShopItem item, StoreSlot storeSlot)
    {
        if (_profile == null)
            return;

        if (storeSlot.StoreItem.TryToPurchase(_profile))
        {
            if (storeSlot.StoreItem.IsInApp())
                Manager.ItemWasPurchased(storeSlot.StoreItem, new PaymentInfo
                {
                    Currency = "USD",
                    Price = storeSlot.StoreItem.Price.Product.Price,
                    ProductId = storeSlot.StoreItem.Price.Product.ProductId,
                });
            else
            {
                Manager.ItemWasPurchasedSoft(storeSlot.StoreItem, storeSlot.StoreItem.Price);
            }
            //TODO play some animation
        }
    }
}
