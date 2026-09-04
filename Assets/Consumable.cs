using UnityEngine;
using System.Collections.Generic;

public enum ConsumableType
{
    Potion,      // Şifa
    ScreenWipe,  // Güneş Işığı (Tüm düşmanları yok eder)
    Vacuum       // Vakum (Tüm XP tohumlarını çeker)
}

// Bu sınıf sadece menajer/spawner görevi görecek
public class Consumable : MonoBehaviour
{
    public static Consumable Instance;
    
    [Header("Prefab Referansları (Unity'den Atanacak)")]
    public GameObject potionPrefab;
    public GameObject screenWipePrefab;
    public GameObject vacuumPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnRandom(Vector3 position)
    {
        // Ağırlıklı Şans Sistemi (0 ile 99 arası sayı)
        int rnd = Random.Range(0, 100);
        GameObject prefabToSpawn = null;
        
        if (rnd < 60) // %60 İhtimalle İksir
        {
            prefabToSpawn = potionPrefab;
        }
        else if (rnd < 95) // %35 İhtimalle Vakum
        {
            prefabToSpawn = vacuumPrefab;
        }
        else // %5 İhtimalle Güneş Işığı
        {
            prefabToSpawn = screenWipePrefab;
        }

        if (prefabToSpawn != null)
        {
            // Eşyayı yere düşür (Biraz yukarıdan düşebilir veya doğrudan yerde çıkabilir)
            Instantiate(prefabToSpawn, position + Vector3.up * 0.5f, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("DİKKAT: Consumable (Harita Eşyası) Prefabları atanmamış!");
        }
    }

    public void PlayScreenWipeEffect()
    {
        StartCoroutine(ScreenWipeRoutine());
    }

    private System.Collections.IEnumerator ScreenWipeRoutine()
    {
        // 1. Oyunu dondur (Zamanı aşırı yavaşlat - Hit Stop)
        Time.timeScale = 0.01f;

        // 2. Ekranı kaplayan geçici bembeyaz bir UI (Parlama) oluştur
        GameObject flashObj = new GameObject("ScreenFlash");
        Canvas canvas = flashObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Her şeyin üstünde olsun
        
        UnityEngine.UI.Image flashImg = flashObj.AddComponent<UnityEngine.UI.Image>();
        flashImg.color = new Color(1f, 1f, 1f, 0.65f); // Göz almaması için 0.9'dan 0.65'e düşürdük
        
        RectTransform rt = flashImg.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        // 3. 0.15 saniye GÜÇLÜ BEKLEME (Gerçek zamanlı bekleme, çünkü oyun dondu)
        yield return new WaitForSecondsRealtime(0.15f);

        // 4. Zamanı normal hızına (1) geri getir
        Time.timeScale = 1f;

        // 5. Ekrandaki beyazlığı yavaşça (0.3 saniyede) şeffaflaştırarak yok et
        float t = 0;
        while(t < 0.3f)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0.65f, 0f, t / 0.3f); // Burayı da 0.65 ile eşitledik
            flashImg.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // Parlama objesini sahneden tamamen sil
        Destroy(flashObj);
    }
}
