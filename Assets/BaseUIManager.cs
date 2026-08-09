using UnityEngine;
using TMPro;

public class BaseUIManager : MonoBehaviour
{
    public static BaseUIManager Instance;

    [Header("Paneller")]
    public GameObject worldMapPanel;
    public GameObject workshopPanel;

    [Header("Her Zaman Görünen Yazılar (Tepe)")]
    public TextMeshProUGUI txtCoreCount; // Kule Çekirdeği Sayısı
    public TextMeshProUGUI txtSeedCount; // Tohum Sayısı

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
        // 10 Tohum harcama testi (Örnek)
        if (GameManager.Instance != null && GameManager.Instance.coreSeedCount >= 10)
        {
            GameManager.Instance.coreSeedCount -= 10;
            // TODO: MetaUpgradeManager.Instance.UpgradeDamage();
            UpdateCurrencyUI();
            Debug.Log("Hasar Yükseltildi!");
        }
        else
        {
            Debug.Log("Yeterli Tohum Yok!");
        }
    }
}
