using UnityEngine;
using RevenueCat;

/// <summary>
/// RevenueCat SDK'yı başlatır ve In-App Purchase (Ağaç Dikme Paketleri) yönetir.
/// BaseScene'deki Canvas > GameManager objesine veya ayrı boş objeye bağlanır.
/// </summary>
public class RevenueCatManager : MonoBehaviour
{
    public static RevenueCatManager Instance;

    [Header("Config (API Keys)")]
    [Tooltip("Assets > Create > Flora > RevenueCat Config ile oluşturun, key'leri girin")]
    public RevenueCatConfig config;

    [Header("Durum")]
    public bool isSDKReady = false;

    // Ürün ID'leri (RevenueCat Dashboard'da oluşturulacak)
    public const string PRODUCT_SEED_PACK = "flora_seed_pack";           // 5000 Tohum ($0.99)
    public const string PRODUCT_PLANT_TREE = "flora_plant_tree";         // 7000 Tohum + 1 Ağaç ($2.99)
    public const string PRODUCT_MEGA_PACK = "flora_mega_pack";           // 10000 Tohum + 2 Ağaç ($4.99)

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
            return;
        }
    }

    void Start()
    {
        InitializeRevenueCat();
    }

    void InitializeRevenueCat()
    {
        if (config == null)
        {
            Debug.LogWarning("⚠️ RevenueCatConfig atanmamış! Assets > Create > Flora > RevenueCat Config ile oluşturun.");
            // Config yoksa bile simülasyon modunda çalışsın
            isSDKReady = true;
            return;
        }

        string apiKey = "";

        #if UNITY_ANDROID
            apiKey = config.googleAPIKey;
        #elif UNITY_IOS
            apiKey = config.appleAPIKey;
        #endif

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogWarning("⚠️ RevenueCat API Key boş! Dashboard'dan kopyalayıp Config'e yapıştırın.");
            isSDKReady = true; // Test modunda yine çalışsın
            return;
        }

        // ═══════════════════════════════════════════════════
        // RevenueCat SDK Başlatma:
        // ═══════════════════════════════════════════════════
        
#if !UNITY_EDITOR
        var purchases = gameObject.AddComponent<Purchases>();
        
        // Yeni API (PurchasesConfiguration) kullanıyoruz
        var builder = Purchases.PurchasesConfiguration.Builder.Init(apiKey);
        purchases.Configure(builder.Build());
        purchases.SetLogLevel(Purchases.LogLevel.Debug);

        Debug.Log("✅ RevenueCat SDK Initialized! Key: " + apiKey.Substring(0, 8) + "...");
#else
        Debug.Log("🧪 RevenueCat Editor Mode: SIMULATION ACTIVE (Real SDK runs on mobile build)!");
#endif
        isSDKReady = true;
    }

    // --- PURCHASE METHODS ---

    /// <summary>
    /// Pack 1: Seed Pack - 5000 Seeds ($0.99)
    /// </summary>
    public void PurchaseSeedPack()
    {
        Debug.Log("🛒 Seed Pack (Simulation) initiated...");
        GrantSeedPack();
    }

    /// <summary>
    /// Pack 2: Plant a Tree - 7000 Seeds + 1 Tree Donation ($2.99) 🌳
    /// Peace Prize Category Track!
    /// </summary>
    public void PurchasePlantTree()
    {
        Debug.Log("🌳 Plant a Tree Pack (Simulation) initiated...");
        GrantPlantTree();
    }

    /// <summary>
    /// Pack 3: Mega Pack - 10000 Seeds + 2 Trees Donation ($4.99) 🌳🌳
    /// </summary>
    public void PurchaseMegaPack()
    {
        Debug.Log("🌲🌲 Mega Reforestation Pack (Simulation) initiated...");
        GrantMegaPack();
    }

    // --- REWARD DISTRIBUTION ---

    void GrantSeedPack()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(5000);
            GameManager.Instance.SaveData();
            Debug.Log("✅ 5000 Seeds added to account!");
            
            // Update UI if in BaseScene
            if (BaseUIManager.Instance != null) BaseUIManager.Instance.UpdateCurrencyUI();
        }
    }

    void GrantPlantTree()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(7000);
            GameManager.Instance.donationTreesCount += 1;
            GameManager.Instance.SaveData();
            Debug.Log("✅ 7000 Seeds + 1 Tree Planted! 🌳 Total: " + GameManager.Instance.donationTreesCount);

            if (BaseUIManager.Instance != null) BaseUIManager.Instance.UpdateCurrencyUI();
        }
    }

    void GrantMegaPack()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(10000);
            GameManager.Instance.donationTreesCount += 2;
            GameManager.Instance.SaveData();
            Debug.Log("✅ 10000 Seeds + 2 Trees Planted! 🌳🌳 Total: " + GameManager.Instance.donationTreesCount);

            if (BaseUIManager.Instance != null) BaseUIManager.Instance.UpdateCurrencyUI();
        }
    }

    // --- RESTORE PURCHASES ---
    public void RestorePurchases()
    {
        Debug.Log("🔄 Restoring purchases...");
        // Purchases.SharedInstance.RestorePurchases((info, error) => { ... });
        Debug.Log("✅ Purchases restored (Simulation)");
    }
}
