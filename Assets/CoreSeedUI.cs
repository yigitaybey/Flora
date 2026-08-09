using UnityEngine;
using TMPro;

public class CoreSeedUI : MonoBehaviour
{
    [Header("UI Referansları")]
    public TextMeshProUGUI coreSeedText;

    void Start()
    {
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
            // PlayerPrefs'ten oku ve Text'e yaz
            int count = PlayerPrefs.GetInt("CoreSeedCount", 0);
            coreSeedText.text = count.ToString();
        }
    }
}
