using System;
using Balancy.Data;
using TMPro;
using UnityEngine;

public class UIItemStatus : MonoBehaviour
{
    [SerializeField]
    private ModelExtensions.ItemType type;
    [SerializeField]
    private TMP_Text text;

    private DefaultProfile _profile;

    private void Awake()
    {
        Launcher.WaitForBalancyToInit(Initialized);
    }

    private void Initialized()
    {
        Launcher.LoadProfile(profile =>
        {
            _profile = profile;
            _profile.Info.SubscribeForChanges(Refresh);
            Refresh();
        });
    }

    private void Refresh()
    {
        int value = 0;
        switch (type)
        {
            case ModelExtensions.ItemType.Other:
                break;
            case ModelExtensions.ItemType.Gems:
                value = _profile.Info.Gems;
                break;
            case ModelExtensions.ItemType.Gold:
                value = _profile.Info.Gold;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        text.SetText(value.ToString());
    }
}
