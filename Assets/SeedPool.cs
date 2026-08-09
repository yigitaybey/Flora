using System.Collections.Generic;
using UnityEngine;

public class SeedPool : MonoBehaviour
{
    public static SeedPool Instance;

    [Tooltip("Düşen Tohum (Yeşil Top vb.) Prefabını buraya sürükleyin")]
    public GameObject seedPrefab;
    
    [Tooltip("Oyun başında kaç adet tohum hazırlansın? (Optimizasyon için)")]
    public int initialPoolSize = 100;

    private List<GameObject> pool;

    void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Havuzu doldur
        pool = new List<GameObject>();
        
        if (seedPrefab != null)
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(seedPrefab, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
        else
        {
            Debug.LogWarning("SeedPool: Lütfen Seed Prefab ataması yapın!");
        }
    }

    // Düşman ölünce havuzdan 1 tane boşta olan tohum verir
    public GameObject GetSeed()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Eğer havuz yetmezse (100 tohum aynı anda ekrandaysa) mecburen yenisini yarat
        if (seedPrefab != null)
        {
            GameObject newObj = Instantiate(seedPrefab, transform);
            newObj.SetActive(false);
            pool.Add(newObj);
            return newObj;
        }

        return null;
    }
}
