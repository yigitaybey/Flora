using UnityEngine;
using TMPro; // UI Text işlemleri için
using UnityEngine.UI; // UI Image işlemleri için

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    [Header("Deneyim (XP) Ayarları")]
    public float currentXP = 0f;
    public float requiredXP = 100f; // Level 2 için gereken ilk XP
    public int currentLevel = 1;

    [Header("Arayüz (UI) Bağlantıları")]
    [Tooltip("Level yazısı (Örn: Level 1)")]
    public TextMeshProUGUI levelText;
    
    [Tooltip("XP Barı (Doldurulabilir Image - Image Type: Filled olmalı)")]
    public Image xpBarFill;
    
    [Tooltip("Level atlayınca açılacak olan Panel (İçinde 3 seçenek butonu olacak)")]
    public GameObject levelUpPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false); // Önce kapalı yapıyoruz
        }
        
        // OYUN BAŞI: İlk silahı seçmek için direkt seçim ekranını aç (Level artırmadan)
        TriggerUpgradeScreen();
    }

    // Level atlandığında tetiklenecek Event
    public event System.Action OnLevelUpEvent;

    // Tohumlar (Seed) oyuncuya çarptığında bu fonksiyonu çağırır
    public void AddExperience(float amount)
    {
        currentXP += amount;
        
        // Sınırı geçtik mi? (Level atlama kontrolü)
        while (currentXP >= requiredXP)
        {
            LevelUp();
        }
        
        UpdateUI();
    }

    // Geliştirici Hilesi: Anında Level Atlat
    public void ForceLevelUp()
    {
        LevelUp();
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= requiredXP; // Kalan (artan) XP'yi silme, bir sonraki levele aktar
        requiredXP = 100f + (currentLevel * 30f); // Düşman sayısı çok arttığı için XP eğrisi dengelendi
        
        Debug.Log("LEVEL ATLADIN! Yeni Level: " + currentLevel);
        
        // Eventi dinleyen diğer scriptlere (Örn: EnemySpawner) haber ver
        OnLevelUpEvent?.Invoke();

        TriggerUpgradeScreen();
    }

    // Hem level atlayınca hem de oyun başında çağrılacak Seçim Ekranı tetikleyicisi
    void TriggerUpgradeScreen()
    {
        // Oyunu Durdur (Zamanı dondur)
        Time.timeScale = 0f;

        // Yetenek Seçim Ekranını (UI) aç ve rastgele yetenekler üret
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
            if (UpgradeManager.Instance != null)
            {
                UpgradeManager.Instance.GenerateUpgrades();
            }
        }
    }

    // Seçenek butonlarından birine tıkladığında oyunun devam etmesi için (Unity içinden Button OnClick'e bağlanacak)
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Zamanı tekrar akıt
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false); // Paneli kapat
        }
    }

    // Ekrandaki yazıyı ve XP barını günceller
    void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
        
        if (xpBarFill != null)
        {
            xpBarFill.fillAmount = currentXP / requiredXP;
        }
    }
}
