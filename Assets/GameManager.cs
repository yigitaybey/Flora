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

    [Header("Atölye (Kalıcı Yetenekler - 0-3 Seviye)")]
    public int flameMultishotLevel = 0; // 1. Cehennem Çoğaltıcı (2x, 3x, 4x Silah/Mermi)
    public int sunshineCurseLevel = 0;  // 2. Güneş Gazabı (Lanet & Dev Kazanç)
    public int appleHealthLevel = 0;    // 3. Kök Sağlığı (Maksimum Can)
    public int magnetRadiusLevel = 0;   // 4. Polen Mıknatısı (Çekim Alanı)
    public int vitalSeedGainLevel = 0;  // 5. Bereket (Core Seed Çarpanı)
    public int pollenDamageLevel = 0;   // 6. Keskin Tohum (Temel Hasar)
    public int axeArmorLevel = 0;       // 7. Ağaç Kabuğu (Gelen Hasarı Silme)
    public int uvSpeedLevel = 0;        // 8. Işık Hızı (Koşu Hızı)

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

    // --- SAHNE GEÇİŞLERİ (ASENKRON LOADING SCREEN) ---
    public void LoadTransitionScene()
    {
        LoadingScreen.LoadScene("TransitionScene", "Exploring World Map...");
    }

    public void LoadCombatScene()
    {
        currentRunCoreSeedCount = 0; // Savaş başlarken tohumları sıfırla
        LoadingScreen.LoadScene("CombatScene", "Entering Corrupted Forest...");
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
        LoadingScreen.LoadScene("BaseScene", "Returning to Radio Tower...");
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
        
        // Atölye yeteneklerini de sıfırla
        flameMultishotLevel = 0;
        sunshineCurseLevel = 0;
        appleHealthLevel = 0;
        magnetRadiusLevel = 0;
        vitalSeedGainLevel = 0;
        pollenDamageLevel = 0;
        axeArmorLevel = 0;
        uvSpeedLevel = 0;

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

    // --- ATÖLYE YETENEK YÜKSELTME VE MALİYET SİSTEMİ ---
    public int GetSkillLevel(PermanentSkillType type)
    {
        switch (type)
        {
            case PermanentSkillType.FlameMultishot: return flameMultishotLevel;
            case PermanentSkillType.SunshineCurse:  return sunshineCurseLevel;
            case PermanentSkillType.AppleHealth:    return appleHealthLevel;
            case PermanentSkillType.MagnetRadius:   return magnetRadiusLevel;
            case PermanentSkillType.VitalSeedGain:  return vitalSeedGainLevel;
            case PermanentSkillType.PollenDamage:   return pollenDamageLevel;
            case PermanentSkillType.AxeArmor:       return axeArmorLevel;
            case PermanentSkillType.UVSpeed:        return uvSpeedLevel;
            default: return 0;
        }
    }

    public int GetSkillCost(PermanentSkillType type, int targetLevel)
    {
        // Hedef level: 1, 2 veya 3
        if (targetLevel < 1 || targetLevel > 3) return -1; // Maksimum seviye

        switch (type)
        {
            case PermanentSkillType.FlameMultishot:
                return (targetLevel == 1) ? 200 : (targetLevel == 2 ? 500 : 1000);
            case PermanentSkillType.SunshineCurse:
                return (targetLevel == 1) ? 100 : (targetLevel == 2 ? 250 : 500);
            case PermanentSkillType.AppleHealth:
                return (targetLevel == 1) ? 50 : (targetLevel == 2 ? 100 : 200);
            case PermanentSkillType.MagnetRadius:
                return (targetLevel == 1) ? 40 : (targetLevel == 2 ? 80 : 160);
            case PermanentSkillType.VitalSeedGain:
                return (targetLevel == 1) ? 75 : (targetLevel == 2 ? 150 : 300);
            case PermanentSkillType.PollenDamage:
                return (targetLevel == 1) ? 60 : (targetLevel == 2 ? 120 : 250);
            case PermanentSkillType.AxeArmor:
                return (targetLevel == 1) ? 50 : (targetLevel == 2 ? 100 : 200);
            case PermanentSkillType.UVSpeed:
                return (targetLevel == 1) ? 50 : (targetLevel == 2 ? 100 : 200);
            default: return 9999;
        }
    }

    public bool PurchasePermanentSkill(PermanentSkillType type)
    {
        int currentLvl = GetSkillLevel(type);
        if (currentLvl >= 3) return false; // Zaten Max

        int cost = GetSkillCost(type, currentLvl + 1);
        if (coreSeedCount < cost) return false; // Yetersiz tohum

        coreSeedCount -= cost;

        switch (type)
        {
            case PermanentSkillType.FlameMultishot: flameMultishotLevel++; break;
            case PermanentSkillType.SunshineCurse:  sunshineCurseLevel++; break;
            case PermanentSkillType.AppleHealth:    appleHealthLevel++; break;
            case PermanentSkillType.MagnetRadius:   magnetRadiusLevel++; break;
            case PermanentSkillType.VitalSeedGain:  vitalSeedGainLevel++; break;
            case PermanentSkillType.PollenDamage:   pollenDamageLevel++; break;
            case PermanentSkillType.AxeArmor:       axeArmorLevel++; break;
            case PermanentSkillType.UVSpeed:        uvSpeedLevel++; break;
        }

        SaveData();
        return true;
    }

    // --- ATÖLYE STAT VE ÇARPAN HESAPLAMALARI ---
    public int FlameMultishotMultiplier => 1 + flameMultishotLevel; // 1x, 2x, 3x, 4x Silah Adedi!
    public float SunshineCurseSpeedMultiplier => 1f + (sunshineCurseLevel * 0.10f); // Düşman hızı
    public float SunshineCurseRewardMultiplier => 1f + (sunshineCurseLevel * 0.25f); // XP & Tohum
    public float AppleBonusHealth => appleHealthLevel == 1 ? 25f : (appleHealthLevel == 2 ? 50f : (appleHealthLevel == 3 ? 100f : 0f));
    public float MagnetBonusRadiusMultiplier => 1f + (magnetRadiusLevel * 0.40f);
    public float VitalSeedBonusMultiplier => 1f + (vitalSeedGainLevel * 0.20f);
    public float PollenBonusDamageMultiplier => 1f + (pollenDamageLevel == 1 ? 0.15f : (pollenDamageLevel == 2 ? 0.30f : (pollenDamageLevel == 3 ? 0.50f : 0f)));
    public float AxeDamageReduction => axeArmorLevel * 1f; // -1, -2, -3 Darbe engelleme
    public float UVBonusSpeedMultiplier => 1f + (uvSpeedLevel * 0.10f); // +%10, +%20, +%30

    public void SaveData()
    {
        PlayerPrefs.SetInt("CoreSeedCount", coreSeedCount);
        PlayerPrefs.SetInt("TowerCoreCount", towerCoreCount);
        PlayerPrefs.SetInt("RadioTowerLevel", radioTowerLevel);
        PlayerPrefs.SetInt("DonationTreesCount", donationTreesCount);

        // Atölye Kayıtları
        PlayerPrefs.SetInt("Perm_Flame", flameMultishotLevel);
        PlayerPrefs.SetInt("Perm_Sunshine", sunshineCurseLevel);
        PlayerPrefs.SetInt("Perm_Apple", appleHealthLevel);
        PlayerPrefs.SetInt("Perm_Magnet", magnetRadiusLevel);
        PlayerPrefs.SetInt("Perm_VitalSeed", vitalSeedGainLevel);
        PlayerPrefs.SetInt("Perm_Pollen", pollenDamageLevel);
        PlayerPrefs.SetInt("Perm_Axe", axeArmorLevel);
        PlayerPrefs.SetInt("Perm_UV", uvSpeedLevel);

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

        // Atölye Yükleme
        flameMultishotLevel = PlayerPrefs.GetInt("Perm_Flame", 0);
        sunshineCurseLevel  = PlayerPrefs.GetInt("Perm_Sunshine", 0);
        appleHealthLevel    = PlayerPrefs.GetInt("Perm_Apple", 0);
        magnetRadiusLevel   = PlayerPrefs.GetInt("Perm_Magnet", 0);
        vitalSeedGainLevel  = PlayerPrefs.GetInt("Perm_VitalSeed", 0);
        pollenDamageLevel   = PlayerPrefs.GetInt("Perm_Pollen", 0);
        axeArmorLevel       = PlayerPrefs.GetInt("Perm_Axe", 0);
        uvSpeedLevel        = PlayerPrefs.GetInt("Perm_UV", 0);
        
        // Ayarları yükle
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        isScreenShakeEnabled = PlayerPrefs.GetInt("ScreenShake", 1) == 1;
        isDamageNumEnabled = PlayerPrefs.GetInt("DamageNum", 1) == 1;
        isHapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
    }
}

public enum PermanentSkillType
{
    FlameMultishot,
    SunshineCurse,
    AppleHealth,
    MagnetRadius,
    VitalSeedGain,
    PollenDamage,
    AxeArmor,
    UVSpeed
}
