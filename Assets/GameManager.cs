using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Kalıcı Değerler")]
    public int coreSeedCount = 0; // Para
    public int towerCoreCount = 0; // Boss'tan düşen Kule Çekirdeği
    public int radioTowerLevel = 1; // Kule Seviyesi

    void Awake()
    {
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
    public void LoadCombatScene()
    {
        SceneManager.LoadScene("CombatScene");
    }

    public void LoadBaseScene()
    {
        SceneManager.LoadScene("BaseScene");
    }

    // --- KAYIT SİSTEMİ (SAVE/LOAD) ---
    public void AddCoreSeed(int amount)
    {
        coreSeedCount += amount;
        SaveData();
    }

    public void AddTowerCore(int amount)
    {
        towerCoreCount += amount;
        SaveData();
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

    private void SaveData()
    {
        PlayerPrefs.SetInt("CoreSeedCount", coreSeedCount);
        PlayerPrefs.SetInt("TowerCoreCount", towerCoreCount);
        PlayerPrefs.SetInt("RadioTowerLevel", radioTowerLevel);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        coreSeedCount = PlayerPrefs.GetInt("CoreSeedCount", 0);
        towerCoreCount = PlayerPrefs.GetInt("TowerCoreCount", 0);
        radioTowerLevel = PlayerPrefs.GetInt("RadioTowerLevel", 1);
    }
}
