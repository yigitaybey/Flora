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

        Debug.Log("✅ RevenueCat SDK Başarıyla Başlatıldı! Key: " + apiKey.Substring(0, 8) + "...");
#else
        Debug.Log("🧪 RevenueCat Unity Editöründe Çalışmaz. SİMÜLASYON MODU AKTİF (Telefonda gerçek çalışacak)!");
#endif
        isSDKReady = true;
    }

    // --- SATIN ALMA FONKSİYONLARI ---

    /// <summary>
    /// Paket 1: Tohum Paketi - 5000 Tohum ($0.99)
    /// </summary>
    public void PurchaseSeedPack()
    {
        Debug.Log("🛒 Tohum Paketi (Simülasyon) başlatılıyor...");
        GrantSeedPack();
    }

    /// <summary>
    /// Paket 2: Ağaç Dik - 7000 Tohum + 1 Ağaç ($2.99) 🌳
    /// Peace Prize kategorisi için!
    /// </summary>
    public void PurchasePlantTree()
    {
        Debug.Log("🌳 Ağaç Dikme Paketi (Simülasyon) başlatılıyor...");
        GrantPlantTree();
    }

    /// <summary>
    /// Paket 3: Mega Paket - 10000 Tohum + 2 Ağaç ($4.99) 🌳🌳
    /// </summary>
    public void PurchaseMegaPack()
    {
        Debug.Log("🌲🌲 Mega Paket (Simülasyon) başlatılıyor...");
        GrantMegaPack();
    }

    // --- ÖDÜL DAĞITIMI (Satın Alma Başarılı Olunca) ---

    void GrantSeedPack()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(5000);
            GameManager.Instance.SaveData();
            Debug.Log("✅ 5000 Tohum hesaba eklendi!");
            
            // UI güncelle (BaseScene'deyse)
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
            Debug.Log("✅ 7000 Tohum + 1 Ağaç Dikildi! 🌳 Toplam: " + GameManager.Instance.donationTreesCount);

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
            Debug.Log("✅ 10000 Tohum + 2 Ağaç Dikildi! 🌳🌳 Toplam: " + GameManager.Instance.donationTreesCount);

            if (BaseUIManager.Instance != null) BaseUIManager.Instance.UpdateCurrencyUI();
        }
    }

    // --- RESTORE PURCHASES (Zorunlu - App Store Red Sebebi Olmaması İçin!) ---
    public void RestorePurchases()
    {
        Debug.Log("🔄 Satın almalar geri yükleniyor...");
        // Purchases.SharedInstance.RestorePurchases((info, error) => { ... });
        Debug.Log("✅ Geri yükleme tamamlandı (Simülasyon)");
    }
}
