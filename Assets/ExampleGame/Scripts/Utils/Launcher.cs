using System;
using System.Collections.Generic;
using Balancy;
using Balancy.Data;
using Balancy.SmartObjects.Analytics;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private const string GAME_ID = "game_id";
    private const string PUBLIC_KEY = "public_key";
    
    [SerializeField]
    private string apiGameId;
    [SerializeField]
    private string publicKey;

    private Dictionary<string, string> _urlParams;

    private static Action _waitToInitCallbacks;
    private static bool _isBalancyFullyInitialized;

    private static Action<DefaultProfile> _profileLoadedCallback;
    private static DefaultProfile _loadedProfile;
    private static bool _loadingProfile;

    private void Start()
    {
        _urlParams = ParseUrl();
        ExternalEvents.RegisterSmartObjectsListener(new MySmartObjectsEvents());
        ExternalEvents.RegisterLiveOpsListener(new MyStoreEvents());
        
        MySmartObjectsEvents.OnSmartObjectsInitializedEvent += () =>
        {
            Debug.Log("Init completed");
            _waitToInitCallbacks?.Invoke();
            _isBalancyFullyInitialized = true;

            var tests = Balancy.LiveOps.ABTests.GetAllTests();
            Debug.Log("Tests Count = " + tests.Count);
            foreach (var test in tests)
            {
                Debug.LogWarning(">> " + test.AbTest.Name + " > " + test.Variant.Name);
            }
        };
        
        Main.Init(new AppConfig
        {
            ApiGameId = GetGameId(),
            PublicKey = GetPublicKey(),
            Environment = Constants.Environment.Development,
            Platform = Constants.Platform.AndroidGooglePlay,
            OnReadyCallback = response =>
            {
                Debug.Log("Balancy Init  " + response.Success);
                if (!response.Success)
                    Controller.PrintAllErrors();
                
                Debug.Log("USER " + Auth.GetUserId());
            }
        });
    }

    public static void WaitForBalancyToInit(Action callback)
    {
        _waitToInitCallbacks += callback;
        if (_isBalancyFullyInitialized)
            callback?.Invoke();
    }

    public static void LoadProfile(Action<DefaultProfile> callback)
    {
        _profileLoadedCallback += callback;
        if (_loadedProfile != null)
            callback?.Invoke(_loadedProfile);
        else
        {
            if (!_loadingProfile)
            {
                _loadingProfile = true;
                SmartStorage.LoadSmartObject<DefaultProfile>(null, responseData =>
                {
                    _loadingProfile = false;
                    _loadedProfile = responseData.Data;
                    _profileLoadedCallback?.Invoke(_loadedProfile);
                });
            }
        }
    }

    private string GetGameId()
    {
        if (HasBothGameIDAndPublicKey())
            return _urlParams[GAME_ID];

        return apiGameId;
    }
    
    private string GetPublicKey()
    {
        if (HasBothGameIDAndPublicKey())
            return _urlParams[PUBLIC_KEY];

        return publicKey;
    }

    private bool HasBothGameIDAndPublicKey()
    {
        return _urlParams.ContainsKey(GAME_ID) && _urlParams.ContainsKey(PUBLIC_KEY);
    }

    private Dictionary<string, string> ParseUrl()
    {
        var paramsDict = new Dictionary<string, string>();

        var url = Application.absoluteURL;
        var splitUrl = url.Split('?');
        if (splitUrl.Length == 2)
        {
            var prms = splitUrl[1];
            var splitParams = prms.Split('&');
            foreach (var kvp in splitParams)
            {
                var splitKvp = kvp.Split('=');
                if (splitKvp.Length == 2)
                {
                    if (!paramsDict.ContainsKey(splitKvp[0]))
                        paramsDict.Add(splitKvp[0], splitKvp[1]);
                }
            }
        }

        return paramsDict;
    }
}
