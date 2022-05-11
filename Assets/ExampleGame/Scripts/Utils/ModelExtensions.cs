using System;
using Balancy;
using Balancy.Data;
using Balancy.Models.SmartObjects;
using UnityEngine;

public static class ModelExtensions
{
    public enum ItemType
    {
        Other,
        Gems,
        Gold
    }

    public static ItemType GetRewardType(this StoreItem storeItem)
    {
        var firstItem = storeItem.Reward.Items.Length > 0 ? storeItem.Reward.Items[0] : null;
        return GetItemType(firstItem?.Item);
    }
    
    public static ItemType GetItemType(this Item item)
    {
        if (item == DataEditor.Game.GameConfig.GoldItem)
            return ItemType.Gold;
        if (item == DataEditor.Game.GameConfig.GemsItem)
            return ItemType.Gems;

        return ItemType.Other;
    }

    public static bool TryToPurchase(this StoreItem storeItem, DefaultProfile profile)
    {
        if (storeItem.TryToTakePrice(profile))
        {
            storeItem.GiveReward(profile);
            return true;
        }

        return false;
    }

    private static bool TryToTakePrice(this StoreItem storeItem, DefaultProfile profile)
    {
        if (storeItem.IsInApp())
        {
            //TODO Implement payment system
            return true;
        }

        //TODO refactor this with Inventory
        var priceItem = storeItem.Price.Items.Length > 0 ? storeItem.Price.Items[0] : null;
        var type = GetItemType(priceItem?.Item);
        switch (type)
        {
            case ItemType.Other:
                return false;
            case ItemType.Gems:
                if (profile.Info.Gems < priceItem.count)
                    return false;
                profile.Info.Gems -= priceItem.count;
                return true;
            case ItemType.Gold:
                if (profile.Info.Gold < priceItem.count)
                    return false;
                profile.Info.Gold -= priceItem.count;
                return true;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void GiveReward(this StoreItem storeItem, DefaultProfile profile)
    {
        var rewardItem = storeItem.Reward.Items.Length > 0 ? storeItem.Reward.Items[0] : null;
        var type = GetItemType(rewardItem?.Item);
        switch (type)
        {
            case ItemType.Other:
                break;
            case ItemType.Gems:
                profile.Info.Gems += rewardItem.count;
                break;
            case ItemType.Gold:
                profile.Info.Gold += rewardItem.count;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
