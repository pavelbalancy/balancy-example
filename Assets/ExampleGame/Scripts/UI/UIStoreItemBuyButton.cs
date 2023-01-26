using System;
using Balancy;
using Balancy.Models.Game;
using Balancy.Models.LiveOps.Store;
using Balancy.Models.SmartObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreItemBuyButton : MonoBehaviour
{
    private const int REFRESH_RATE = 1;
    
    [SerializeField]
    private Button buttonHard;
    [SerializeField]
    private Button buttonSoft;
    
    [SerializeField]
    private TMP_Text labelHard;
    [SerializeField]
    private TMP_Text labelSoft;
    [SerializeField]
    private RemoteImage iconSoft;
    
    [SerializeField]
    private TMP_Text timerText;

    private Action _callback;
    private bool _subscribed;
    private Slot _slot;

    public void Init(Slot slot, Action callback)
    {
        _slot = slot;
        _callback = callback;
        FillTheData(slot);
        SubscribeForTimers();
    }

    private void FillTheData(Slot slot)
    {
        string limitString = GetLimitString(slot);

        Price price = slot.GetStoreItem().Price;
        
        if (price.IsFree())
        {
            buttonHard.gameObject.SetActive(true);
            buttonSoft.gameObject.SetActive(false);
            labelHard.SetText("Free" + limitString);
            return;
        }
        
        var isInApp = price.IsInApp();
        buttonHard.gameObject.SetActive(isInApp);
        buttonSoft.gameObject.SetActive(!isInApp);
        if (isInApp)
        {
            labelHard.SetText($"US ${price.Product?.Price}" + limitString);
        }
        else
        {
            var firstItem = price.Items.Length > 0 ? price.Items[0] : null;
            if (firstItem != null)
            {
                labelSoft.SetText(firstItem.Count + limitString);
                iconSoft.LoadObject((firstItem.Item as GameItem)?.Icon);
            } else
                buttonSoft.gameObject.SetActive(false);
        }
        
        UpdateButtonState();
    }

    private string GetLimitString(Slot slot)
    {
        string limitString = string.Empty;

        switch (slot)
        {
            case SlotPeriod slotPeriod:
                limitString = $" ({slotPeriod.GetPurchasesDoneCount()}/{slotPeriod.Limit})";
                break;
        }
        
        return limitString;
    }
    
    private void SubscribeForTimers()
    {
        if (_subscribed || _slot == null)
            return;
            
        switch (_slot)
        {
            case SlotPeriod _:
            case SlotCooldown _:
            {
                _subscribed = true;
                BalancyTimer.SubscribeForTimer(REFRESH_RATE, Refresh);
                break;
            } 
        }
    }

    private void UnsubscribeFromTimers()
    {
        if (!_subscribed)
            return;

        _subscribed = false;
        BalancyTimer.UnsubscribeFromTimer(REFRESH_RATE, Refresh);
    }

    private void Refresh()
    {
        UpdateButtonState();
    }

    private void OnEnable()
    {
        SubscribeForTimers();
    }

    private void OnDisable()
    {
        UnsubscribeFromTimers();
    }

    private void UpdateButtonState()
    {
        bool showHint = false;
        switch (_slot)
        {
            case SlotPeriod slotPeriod:
            {
                timerText.SetText($"Resets in {slotPeriod.GetSecondsUntilReset()}");//TODO fix
                showHint = true;
                break;
            }
            case SlotCooldown slotCooldown:
            {
                if (slotCooldown.IsAvailable())
                    timerText.SetText($"CD {slotCooldown.Cooldown}");
                else
                    timerText.SetText($"Resets in {slotCooldown.GetSecondsLeftUntilAvailable()}");
                showHint = true;
                break;
            }
        }
            
        timerText.gameObject.SetActive(showHint);
        bool canBuy = _slot.IsAvailable();
        buttonHard.interactable = canBuy;
        buttonSoft.interactable = canBuy;
    }

    private void Awake()
    {
        buttonHard.onClick.AddListener(OnClick);
        buttonSoft.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _callback?.Invoke();
        FillTheData(_slot);
    }
}
