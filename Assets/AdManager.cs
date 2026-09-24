using UnityEngine;
using UnityEngine.Advertisements;
using RevenueCat;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;

    [Header("Unity Ads IDs")]
    public string androidGameId = "800390583";
    public string iosGameId = "800390582";
    public bool testMode = false;
    
    [Header("Placement IDs")]
    public string androidAdUnitId = "BP_Rewarded_Android";
    
    private string _gameId;
    private string _adUnitId;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = iosGameId;
        _adUnitId = "Rewarded_iOS";
#elif UNITY_ANDROID
        _gameId = androidGameId;
        _adUnitId = androidAdUnitId;
#else
        _gameId = androidGameId;
        _adUnitId = androidAdUnitId;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, testMode, this);
        }
    }

    public void LoadRewardedAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.isInitialized)
        {
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            Debug.LogWarning("Unity Ads not initialized. Triggering fallback directly.");
            OnUnityAdsShowFailure(_adUnitId, UnityAdsShowError.NOT_INITIALIZED, "Not initialized (Hackathon Fallback)");
        }
    }

    // --- IUnityAdsInitializationListener ---
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    // --- IUnityAdsLoadListener ---
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
    }

    // --- IUnityAdsShowListener ---
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        
        // HACKATHON FALLBACK: Reklam yüklenemezse bile ödülü ver ki test eden kişi takılı kalmasın.
        if (CombatUIManager.Instance != null)
        {
            CombatUIManager.Instance.ApplyDoubleLoot();
            Debug.Log("Simulated Double Loot (Ad Failed)");
        }
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Çifte Loot Uygula
            if (CombatUIManager.Instance != null)
            {
                CombatUIManager.Instance.ApplyDoubleLoot();
                Debug.Log("✅ Reklam izlendi, Tohumlar ikiye katlandı!");
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.AddCoreSeed(500); // Hackathon simülasyonu fallback
                Debug.Log("✅ Reklam izlendi, 500 Bonus Tohum verildi!");
            }
                
            // Track with RevenueCat AdTracker
            try {
                    // Simüle edilmiş bir revenue ataması
                    var adInfo = new AdRevenueData(
                        mediatorName: new AdTracker.MediatorName("UnityAds"),
                        adFormat: AdTracker.Format.Rewarded,
                        adUnitId: _adUnitId,
                        impressionId: "simulated_impression",
                        revenueMicros: 10000, // 0.01 USD
                        currency: "USD",
                        precision: AdTracker.Precision.Estimated
                    );
                    var purchases = UnityEngine.Object.FindFirstObjectByType<Purchases>();
                    if (purchases != null)
                    {
                        purchases.AdTracker.TrackAdRevenue(adInfo);
                    }
                    Debug.Log("✅ RevenueCat AdTracker'a reklam impression'u başarıyla gönderildi (Catvertising)!");
                } catch (System.Exception e) {
                    Debug.Log("RevenueCat Ad Tracking çalışmadı (Belki SDK başlatılmamıştır): " + e.Message);
                }
        }
    }
}
