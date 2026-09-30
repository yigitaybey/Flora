using UnityEngine;
using TMPro; // TextMeshPro (Yazılar) için gerekli
using UnityEngine.SceneManagement;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance;

    [Header("UI Panelleri")]
    public GameObject deathPanel;
    public GameObject winPanel;

    [Header("Ölüm Ekranı Metinleri")]
    public TextMeshProUGUI deathLootText; // Kazanılan tohumu göstermek için
    [Tooltip("Tohum sayısının önüne eklenecek metin (Örn: 'x' veya boş bırakırsan direkt sayıyı yazar)")]
    public string lootTextPrefix = "";

    [Header("Kazanma Ekranı Metinleri")]
    public TextMeshProUGUI winLootText; // Kazanılan tohumu göstermek için
    
    [Header("Reklam Butonları")]
    public GameObject deathDoubleLootButtonObj; // Ölüm ekranındaki reklam butonu
    public GameObject winDoubleLootButtonObj;   // Kazanma ekranındaki reklam butonu

    [Header("Duraklatma ve Ayarlar (Pause)")]
    public GameObject settingsPanel;
    
    [Header("Ayarlar UI Elemanları")]
    public UnityEngine.UI.Slider soundSlider;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Toggle screenShakeToggle;
    public UnityEngine.UI.Toggle damageNumToggle;
    public UnityEngine.UI.Toggle hapticsToggle;

    // GameManager yokken (Savaş sahnesi direkt test edilirken) ayarları tutmak için statik değişkenler
    public static bool fallbackDamageNumEnabled = true;
    public static bool fallbackScreenShakeEnabled = true;
    public static bool fallbackHapticsEnabled = true;

    // Kazanılan tohum miktarını takip etmek için
    private int runLoot = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Başlangıçta panelleri kapalı tut
        if (deathPanel != null) deathPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        
        // Zamanı normal hızına al (önceden donmuşsa diye)
        Time.timeScale = 1f;
    }

    void Start()
    {
        LoadSettingsUI();
    }

    void Update()
    {
        // Escape tuşuna basıldığında Ayarlar / Pause menüsünü aç/kapat
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null)
        {
            if (kb.escapeKey.wasPressedThisFrame)
            {
                ToggleSettings();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleSettings();
            }
        }
    }

    // Savaştan Çekil (End Run) Butonu: O ana kadar toplanan tohumlarla Base'e döner
    public void EndRun()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadBaseScene();
        }
        else
        {
            SceneManager.LoadScene("BaseScene");
        }
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
        else
        {
            // Savaş sahnesi direkt test ediliyorsa
            if (screenShakeToggle != null) screenShakeToggle.isOn = fallbackScreenShakeEnabled;
            if (damageNumToggle != null) damageNumToggle.isOn = fallbackDamageNumEnabled;
            if (hapticsToggle != null) hapticsToggle.isOn = fallbackHapticsEnabled;
        }
    }

    public void ShowDeathScreen()
    {
        // GameManager'dan toplanan seed'i al (Eğer sahneyi direkt başlattıysan fallback kullan)
        if (GameManager.Instance != null)
        {
            runLoot = GameManager.Instance.currentRunCoreSeedCount;
        }
        else
        {
            runLoot = CoreSeed.fallbackRunCount;
        }

        if (deathLootText != null)
        {
            string prefix = string.IsNullOrEmpty(lootTextPrefix) ? "" : lootTextPrefix + " ";
            deathLootText.text = prefix + runLoot.ToString();
        }

        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }

        // Oyunu durdur
        Time.timeScale = 0f;
    }

    public void TriggerVictory(float delay = 1.5f)
    {
        Invoke(nameof(ShowWinScreen), delay);
    }

    public void ShowWinScreen()
    {
        // GameManager'dan toplanan seed'i al (Eğer sahneyi direkt başlattıysan fallback kullan)
        if (GameManager.Instance != null)
        {
            runLoot = GameManager.Instance.currentRunCoreSeedCount;
        }
        else
        {
            runLoot = CoreSeed.fallbackRunCount;
        }

        if (winLootText != null)
        {
            string prefix = string.IsNullOrEmpty(lootTextPrefix) ? "" : lootTextPrefix + " ";
            winLootText.text = prefix + runLoot.ToString();
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Oyunu durdur
        Time.timeScale = 0f;
    }

    // Butonlardan çağırılacak fonksiyon: Base'e dön
    public void ReturnToBase()
    {
        Time.timeScale = 1f; // Zamanı düzeltmeyi unutmayalım!
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadBaseScene();
        }
        else
        {
            SceneManager.LoadScene("BaseScene");
        }
    }

    public void WatchAdForDoubleLoot()
    {
        if (AdManager.Instance != null)
        {
            Debug.Log("Loading/showing rewarded video ad...");
            AdManager.Instance.ShowRewardedAd();
        }
        else
        {
            Debug.LogWarning("AdManager not found, doubling loot in simulation mode...");
            ApplyDoubleLoot();
        }
    }

    public void ApplyDoubleLoot()
    {

        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentRunCoreSeedCount *= 2;
            runLoot = GameManager.Instance.currentRunCoreSeedCount;
        }
        else
        {
            CoreSeed.fallbackRunCount *= 2;
            runLoot = CoreSeed.fallbackRunCount;
        }

        // Metinleri ekranda güncelle
        string prefix = string.IsNullOrEmpty(lootTextPrefix) ? "" : lootTextPrefix + " ";
        if (deathLootText != null) deathLootText.text = prefix + runLoot.ToString();
        if (winLootText != null) winLootText.text = prefix + runLoot.ToString();

        // Reklam butonunu gizle ki oyuncu 2. kez basıp hile yapamasın
        if (deathDoubleLootButtonObj != null) deathDoubleLootButtonObj.SetActive(false);
        if (winDoubleLootButtonObj != null) winDoubleLootButtonObj.SetActive(false);
    }

    // --- DURAKLATMA (PAUSE) VE AYARLAR ---

    public void ToggleSettings()
    {
        // Eğer oyun Ölüm/Kazanma ekranındaysa (Zaman durmuşsa ve o paneller açıksa) ayarları açma
        if (deathPanel != null && deathPanel.activeSelf) return;
        if (winPanel != null && winPanel.activeSelf) return;

        if (settingsPanel == null) return;

        bool isSettingsOpen = settingsPanel.activeSelf;

        if (isSettingsOpen)
        {
            // Ayarları kapat ve oyuna devam et
            settingsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            // Ayarları aç ve oyunu durdur (Pause mantığı)
            settingsPanel.SetActive(true);
            Time.timeScale = 0f;
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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isScreenShakeEnabled = value;
            GameManager.Instance.SaveSettings();
        }
        else
        {
            fallbackScreenShakeEnabled = value;
        }
    }

    public void OnDamageNumToggled(bool value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isDamageNumEnabled = value;
            GameManager.Instance.SaveSettings();
        }
        else
        {
            fallbackDamageNumEnabled = value;
        }
    }

    public void OnHapticsToggled(bool value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isHapticsEnabled = value;
            GameManager.Instance.SaveSettings();
        }
        else
        {
            fallbackHapticsEnabled = value;
        }
        HapticFeedback.isEnabled = value;
    }
}
