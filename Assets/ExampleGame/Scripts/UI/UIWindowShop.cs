using System.Collections.Generic;
using Balancy.API.Payments;
using Balancy.Data;
using Balancy.Models.LiveOps.Store;
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

    private void RefreshContent(Page storePage)
    {
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

        if (storeSlot.StoreItem.TryToPurchase(_profile))
        {
            if (storeSlot.StoreItem.IsInApp())
                Balancy.LiveOps.Store.ItemWasPurchased(storeSlot.StoreItem, new PaymentInfo
                {
                    Currency = "USD",
                    Price = storeSlot.StoreItem.Price.Product.Price,
                    ProductId = storeSlot.StoreItem.Price.Product.ProductId,
                });
            else
            {
                Balancy.LiveOps.Store.ItemWasPurchased(storeSlot.StoreItem, storeSlot.StoreItem.Price);
            }
            //TODO play some animation
        }
    }
}
