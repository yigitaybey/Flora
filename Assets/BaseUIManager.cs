using UnityEngine;
using TMPro;

public class BaseUIManager : MonoBehaviour
{
    public static BaseUIManager Instance;

    [Header("Paneller")]
    public GameObject worldMapPanel;
    public GameObject workshopPanel;
    public GameObject settingsPanel;
    public GameObject storePanel; // RevenueCat Ağaç Dikme Mağazası

    [Header("Her Zaman Görünen Yazılar (Tepe)")]
    public TextMeshProUGUI txtCoreCount; // Kule Çekirdeği Sayısı
    public TextMeshProUGUI txtSeedCount; // Tohum Sayısı

    [Header("Ayarlar UI Elemanları")]
    public UnityEngine.UI.Slider soundSlider;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Toggle screenShakeToggle;
    public UnityEngine.UI.Toggle damageNumToggle;
    public UnityEngine.UI.Toggle hapticsToggle;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Başlangıçta paneller kapalı olsun
        CloseAllPanels();
        UpdateCurrencyUI();
        LoadSettingsUI();
    }

    public void LoadSettingsUI()
    {
        if (GameManager.Instance != null)
        {
            if (soundSlider != null) soundSlider.value = GameManager.Instance.soundVolume;
            if (musicSlider != null) musicSlider.value = GameManager.Instance.musicVolume;
            if (screenShakeToggle != null) screenShakeToggle.isOn = GameManager.Instance.isScreenShakeEnabled;
            if (damageNumToggle != null) damageNumToggle.isOn = GameManager.Instance.isDamageNumEnabled;
            if (hapticsToggle != null) hapticsToggle.isOn = GameManager.Instance.isHapticsEnabled;
        }
    }

    // --- PANEL AÇMA/KAPAMA ---
    public void OpenWorldMap()
    {
        CloseAllPanels();
        if (worldMapPanel != null) worldMapPanel.SetActive(true);
    }

    public void OpenWorkshop()
    {
        CloseAllPanels();
        if (workshopPanel != null) workshopPanel.SetActive(true);
    }

    public void CloseAllPanels()
    {
        if (worldMapPanel != null) worldMapPanel.SetActive(false);
        if (workshopPanel != null) workshopPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (storePanel != null) storePanel.SetActive(false);
    }

    // --- KAYNAKLARI GÜNCELLEME ---
    public void UpdateCurrencyUI()
    {
        if (GameManager.Instance != null)
        {
            if (txtCoreCount != null) txtCoreCount.text = "Kule Çekirdeği: " + GameManager.Instance.towerCoreCount;
            if (txtSeedCount != null) txtSeedCount.text = "Tohum: " + GameManager.Instance.coreSeedCount;
        }
    }

    // --- BUTON FONKSİYONLARI (Unity içinden bağlanacak) ---
    
    // Radyo Kulesindeki "Ormana Git" butonuna basınca çalışır
    public void GoToForestCombat()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadCombatScene();
        }
    }

    // Radyo Kulesindeki "Kuleyi Yükselt" butonuna basınca çalışır
    public void ClickUpgradeTower()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpgradeTower();
            UpdateCurrencyUI(); // Harcama yaptık, yazıları güncelle
        }
    }

    // Atölyedeki "Hasar Yükselt" (Örnek) butonuna basınca çalışır
    public void ClickUpgradeDamage()
    {
        if (GameManager.Instance != null && GameManager.Instance.coreSeedCount >= 10)
        {
            GameManager.Instance.coreSeedCount -= 10;
            UpdateCurrencyUI();
            Debug.Log("Hasar Yükseltildi!");
        }
        else
        {
            Debug.Log("Yeterli Tohum Yok!");
        }
    }

    // Atölyedeki "Max Can Yükselt" butonuna basınca çalışır
    public void ClickUpgradeHealth()
    {
        if (GameManager.Instance != null && GameManager.Instance.coreSeedCount >= 10)
        {
            GameManager.Instance.coreSeedCount -= 10;
            UpdateCurrencyUI();
            Debug.Log("Max Can Yükseltildi!");
        }
        else
        {
            Debug.Log("Yeterli Tohum Yok!");
        }
    }

    // Ayarlar butonuna basınca çalışır
    public void OpenSettings()
    {
        CloseAllPanels();
        if (settingsPanel != null) settingsPanel.SetActive(true);
        Debug.Log("Ayarlar Menüsü Açıldı!");
    }

    // Tohumları sıfırlama butonu
    public void ResetSeedsButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetAllSeeds();
            UpdateCurrencyUI();
        }
    }

    // Tüm save datasını fabrika ayarlarına döndürme butonu
    public void ResetAllSaveDataButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetAllSaveData();
            UpdateCurrencyUI();
        }
    }

    // Tohumun yanındaki '+' butonuna basınca çalışır (RevenueCat mağazası)
    public void OpenStore()
    {
        CloseAllPanels();
        if (storePanel != null) storePanel.SetActive(true);
        Debug.Log("Ağaç Dikme / Mağaza Menüsü Açıldı! (RevenueCat bağlanacak)");
    }

    // --- REVENUECAT MAĞAZA (TEST İÇİN SAHTE SATIN ALIMLAR) ---

    public void BuyPackage1()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(5000);
            UpdateCurrencyUI();
            Debug.Log("5000 Tohum Satın Alındı! (API Bağlanana Kadar Test)");
        }
    }

    public void BuyPackage2()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(7000);
            GameManager.Instance.donationTreesCount += 1;
            GameManager.Instance.SaveData();
            UpdateCurrencyUI();
            Debug.Log("7000 Tohum ve 1 Ağaç Dikildi! Toplam Ağaç: " + GameManager.Instance.donationTreesCount);
        }
    }

    public void BuyPackage3()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(10000);
            GameManager.Instance.donationTreesCount += 2;
            GameManager.Instance.SaveData();
            UpdateCurrencyUI();
            Debug.Log("10000 Tohum ve 2 Ağaç Dikildi! Toplam Ağaç: " + GameManager.Instance.donationTreesCount);
        }
    }

    // --- AYARLAR (UI SLIDER VE TOGGLE) BAĞLANTILARI ---
    
    public void OnSoundVolumeChanged(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.soundVolume = value;
            GameManager.Instance.SaveSettings();
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.musicVolume = value;
            GameManager.Instance.SaveSettings();
        }
    }

    public void OnScreenShakeToggled(bool value)
    {
        Debug.Log("Ekran Titremesi (ScreenShake) UI'dan gelen değer: " + value);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isScreenShakeEnabled = value;
            GameManager.Instance.SaveSettings();
        }
    }

    public void OnDamageNumToggled(bool value)
    {
        Debug.Log("Hasar Yazıları (DamageNum) UI'dan gelen değer: " + value);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isDamageNumEnabled = value;
            GameManager.Instance.SaveSettings();
        }
    }

    public void OnHapticsToggled(bool value)
    {
        Debug.Log("Titreşim (Haptics) UI'dan gelen değer: " + value);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isHapticsEnabled = value;
            GameManager.Instance.SaveSettings();
        }
        HapticFeedback.isEnabled = value;
    }
}
