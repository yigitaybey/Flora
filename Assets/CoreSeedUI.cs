using UnityEngine;
using TMPro;

public class CoreSeedUI : MonoBehaviour
{
    [Header("UI Referansları")]
    public TextMeshProUGUI coreSeedText;

    void Start()
    {
        // Editor'de test ederken GameManager yoksa eski değer kalmasın diye sıfırlıyoruz
        CoreSeed.fallbackRunCount = 0;

        // Başlangıçta UI'ı güncelle
        UpdateUI();
        
        // Birisi Core Seed topladığında tetiklenecek eventi dinle
        CoreSeed.OnCoreSeedCollected += UpdateUI;
    }

    void OnDestroy()
    {
        // Script yok olduğunda (veya sahne değiştiğinde) dinlemeyi bırak
        CoreSeed.OnCoreSeedCollected -= UpdateUI;
    }

    void UpdateUI()
    {
        if (coreSeedText != null)
        {
            // GameManager varsa ondan oku (Savaşta sadece bu run'da toplananı göster)
            int count = 0;
            if (GameManager.Instance != null)
            {
                count = GameManager.Instance.currentRunCoreSeedCount;
            }
            else
            {
                // Fallback (Direkt CombatScene'den test ediliyorsa)
                count = CoreSeed.fallbackRunCount;
            }
            
            coreSeedText.text = "Tohum: " + count.ToString();
        }
    }
}
