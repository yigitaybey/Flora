using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Kalıcı Değerler")]
    public int coreSeedCount = 0; // Para
    public int towerCoreCount = 0; // Boss'tan düşen Kule Çekirdeği
    public int radioTowerLevel = 1; // Kule Seviyesi
    public int donationTreesCount = 0; // RevenueCat Bağış Ağaçları

    [Header("Savaş (Run) Değerleri")]
    public int currentRunCoreSeedCount = 0;

    [Header("Ayarlar")]
    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public bool isScreenShakeEnabled = true;
    public bool isDamageNumEnabled = true;
    public bool isHapticsEnabled = true;

    void Awake()
    {
        // Mobil 60 FPS Kilidi ve Ekran Kararmasını Engelleme (Optimizasyon)
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Singleton Pattern (Sahneler arası silinmez)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData(); // Oyuna girerken verileri yükle
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- SAHNE GEÇİŞLERİ ---
    public void LoadTransitionScene()
    {
        SceneManager.LoadScene("TransitionScene");
    }

    public void LoadCombatScene()
    {
        currentRunCoreSeedCount = 0; // Savaş başlarken tohumları sıfırla
        SceneManager.LoadScene("CombatScene");
    }

    public void LoadBaseScene()
    {
        // Base'e dönerken run'da kazanılanları asıl hesaba ekle
        if (currentRunCoreSeedCount > 0)
        {
            coreSeedCount += currentRunCoreSeedCount;
            currentRunCoreSeedCount = 0;
            SaveData();
        }
        SceneManager.LoadScene("BaseScene");
    }

    // --- KAYIT SİSTEMİ (SAVE/LOAD) ---
    public void AddCoreSeed(int amount)
    {
        // Eğer savaştaysak sadece o run'ın hanesine ekle, değilse asıl bakiyeye
        if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            currentRunCoreSeedCount += amount;
        }
        else
        {
            coreSeedCount += amount;
            SaveData();
        }
    }

    public void AddTowerCore(int amount)
    {
        towerCoreCount += amount;
        SaveData();
    }

    // --- SIFIRLAMA / RESET FONKSİYONLARI ---
    public void ResetAllSeeds()
    {
        coreSeedCount = 0;
        currentRunCoreSeedCount = 0;
        SaveData();
        Debug.Log("🧹 Tüm Tohumlar Sıfırlandı!");
    }

    public void ResetAllSaveData()
    {
        coreSeedCount = 0;
        currentRunCoreSeedCount = 0;
        towerCoreCount = 0;
        radioTowerLevel = 1;
        donationTreesCount = 0;
        PlayerPrefs.DeleteAll();
        SaveData();
        Debug.Log("🗑️ Tüm Oyun Kayıtları Sıfırlandı (Fabrika Ayarları)!");
    }

    public void UpgradeTower()
    {
        // Lvl 1'den Lvl 2'ye geçmek için 1 Çekirdek yetsin (Hackathon için)
        if (towerCoreCount >= 1) 
        {
            towerCoreCount -= 1;
            radioTowerLevel++;
            SaveData();
            Debug.Log("Kule Yükseltildi! Yeni Level: " + radioTowerLevel);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CoreSeedCount", coreSeedCount);
        PlayerPrefs.SetInt("TowerCoreCount", towerCoreCount);
        PlayerPrefs.SetInt("RadioTowerLevel", radioTowerLevel);
        PlayerPrefs.SetInt("DonationTreesCount", donationTreesCount);
        SaveSettings(); // Kayıt alırken ayarları da kaydet
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("ScreenShake", isScreenShakeEnabled ? 1 : 0);
        PlayerPrefs.SetInt("DamageNum", isDamageNumEnabled ? 1 : 0);
        PlayerPrefs.SetInt("HapticsEnabled", isHapticsEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        coreSeedCount = PlayerPrefs.GetInt("CoreSeedCount", 0);
        towerCoreCount = PlayerPrefs.GetInt("TowerCoreCount", 0);
        radioTowerLevel = PlayerPrefs.GetInt("RadioTowerLevel", 1);
        donationTreesCount = PlayerPrefs.GetInt("DonationTreesCount", 0);
        
        // Ayarları yükle
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        isScreenShakeEnabled = PlayerPrefs.GetInt("ScreenShake", 1) == 1;
        isDamageNumEnabled = PlayerPrefs.GetInt("DamageNum", 1) == 1;
        isHapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
    }
}
