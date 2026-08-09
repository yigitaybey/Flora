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
}
