using System.Collections.Generic;
using Balancy.API.Payments;
using Balancy.Data;
using Balancy.Models.LiveOps.Store;
using Balancy.Models.SmartObjects;
using Balancy.Models.Store;
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
    
    private List<UIStoreTabButton> _tabButtons;
    private DefaultProfile _profile;
    private Page _selectedStorePage;

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
        tabsContent.RemoveChildren();
        _tabButtons = new List<UIStoreTabButton>();
        var storeConfig = Balancy.LiveOps.Store.DefaultStore;
        var pages = storeConfig.ActivePages;
        
        foreach (var page in pages)
        {
            var newButton = Instantiate(storeTabPrefab, tabsContent);
            var storeTabButton = newButton.GetComponent<UIStoreTabButton>();
            storeTabButton.Init(page, TabWasSelected);
            _tabButtons.Add(storeTabButton);
        }

        if (pages.Count > 0)
            TabWasSelected(pages[0]);
    }

    private void ProfileLoaded(DefaultProfile profile)
    {
        _profile = profile;
    }

    private void TabWasSelected(Page storePage)
    {
        foreach (var button in _tabButtons)
            button.SetLocked(button.EqualsToPage(storePage));

        RefreshContent(storePage);
    }

    private void RefreshPageAfterUpdate(SmartConfig storeConfig, Page page)
    {
        RefreshContent(_selectedStorePage);
    }

    private void RefreshContent(Page storePage)
    {
        if (_selectedStorePage != storePage)
        {
            if (_selectedStorePage != null)
                _selectedStorePage.OnStorePageUpdatedEvent -= RefreshPageAfterUpdate;
            _selectedStorePage = storePage;
            _selectedStorePage.OnStorePageUpdatedEvent += RefreshPageAfterUpdate;
        }

        content.RemoveChildren();
        
        foreach (var storeSlot in storePage.ActiveSlots)
        {
            var newItem = Instantiate(storeItemPrefab, content);
            var storeItemView = newItem.GetComponent<UIShopItem>();
            storeItemView.Init(storeSlot, OnTryToPurchase);
        }
    }

    private void OnTryToPurchase(UIShopItem item, Slot storeSlot)
    {
        if (_profile == null)
            return;

        var storeItem = storeSlot.GetStoreItem();
        if (storeItem.IsFree())
        {
            Balancy.LiveOps.Store.ItemWasPurchased(storeItem);
        }
        else
        {
            if (storeItem.TryToPurchase(_profile))
            {
                if (storeItem.IsInApp())
                    Balancy.LiveOps.Store.ItemWasPurchased(storeItem, new PaymentInfo
                    {
                        Currency = "USD",
                        Price = storeItem.Price.Product.Price,
                        ProductId = storeItem.Price.Product.ProductId,
                    });
                else
                {
                    Balancy.LiveOps.Store.ItemWasPurchased(storeItem, storeItem.Price);
                }
                //TODO play some animation
            }
        }
    }
}
