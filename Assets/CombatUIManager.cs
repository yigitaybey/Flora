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

    // GameManager yokken (Savaş sahnesi direkt test edilirken) ayarları tutmak için statik değişkenler
    public static bool fallbackDamageNumEnabled = true;
    public static bool fallbackScreenShakeEnabled = true;

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

    public void LoadSettingsUI()
    {
        if (GameManager.Instance != null)
        {
            if (soundSlider != null) soundSlider.value = GameManager.Instance.soundVolume;
            if (musicSlider != null) musicSlider.value = GameManager.Instance.musicVolume;
            if (screenShakeToggle != null) screenShakeToggle.isOn = GameManager.Instance.isScreenShakeEnabled;
            if (damageNumToggle != null) damageNumToggle.isOn = GameManager.Instance.isDamageNumEnabled;
        }
        else
        {
            // Savaş sahnesi direkt test ediliyorsa
            if (screenShakeToggle != null) screenShakeToggle.isOn = fallbackScreenShakeEnabled;
            if (damageNumToggle != null) damageNumToggle.isOn = fallbackDamageNumEnabled;
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
            deathLootText.text = "Toplanan Tohum: " + runLoot.ToString();
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
            winLootText.text = "Toplanan Tohum: " + runLoot.ToString();
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

    // 2x Reklam Butonundan çağrılacak fonksiyon
    public void WatchAdForDoubleLoot()
    {
        // Şimdilik reklam izlenmiş gibi kabul edip tohumu ikiye katlıyoruz
        Debug.Log("Reklam İzlendi! Tohumlar 2'ye Katlanıyor...");

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
        if (deathLootText != null) deathLootText.text = "Toplanan Tohum: " + runLoot.ToString();
        if (winLootText != null) winLootText.text = "Toplanan Tohum: " + runLoot.ToString();

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
}
