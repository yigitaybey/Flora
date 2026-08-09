using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [Header("Havuz Ayarları")]
    [Tooltip("Farklı düşman türlerinin Prefab'larını buraya ekleyin.")]
    public GameObject[] enemyPrefabs;
    
    [Tooltip("Her düşman tipinden başlangıçta kaçar tane hazırda beklesin?")]
    public int poolSizePerType = 30;

    // Düşman tipine göre listeleri (havuzları) tutan sözlük (Dictionary)
    private Dictionary<EnemyType, List<GameObject>> pools;

    void Awake()
    {
        // 1. Singleton Kurulumu
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // 2. Havuzları Kesin Olarak İlk Saniyede Doldur (Start yerine Awake)
        pools = new Dictionary<EnemyType, List<GameObject>>();

        // Bütün prefab'lar için ayrı bir havuz oluştur
        foreach (GameObject prefab in enemyPrefabs)
        {
            Enemy enemyComponent = prefab.GetComponent<Enemy>();
            if (enemyComponent == null)
            {
                Debug.LogWarning(prefab.name + " objesinde Enemy scripti yok!");
                continue;
            }

            EnemyType type = enemyComponent.enemyType;
            pools[type] = new List<GameObject>();

            // Havuzu doldur
            for (int i = 0; i < poolSizePerType; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                pools[type].Add(obj);
            }
        }
    }

    // İstenilen tipe göre düşman çağırır
    public GameObject GetEnemy(EnemyType type)
    {
        if (!pools.ContainsKey(type)) 
        {
            Debug.LogWarning("Havuzda bu düşman tipinden yok: " + type);
            return null;
        }

        foreach (GameObject obj in pools[type])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Eğer havuz yetmezse mecburen yenisini yarat
        foreach (GameObject prefab in enemyPrefabs)
        {
            if (prefab.GetComponent<Enemy>().enemyType == type)
            {
                GameObject newObj = Instantiate(prefab, transform);
                newObj.SetActive(false);
                pools[type].Add(newObj);
                return newObj;
            }
        }
        
        return null;
    }

    // Ekrandaki aktif (canlı) düşman sayısını döndürür (FPS Hard Cap kontrolü için)
    public int GetActiveEnemyCount()
    {
        int count = 0;
        if (pools == null) return count;

        foreach (var list in pools.Values)
        {
            foreach (GameObject obj in list)
            {
                if (obj.activeInHierarchy)
                {
                    count++;
                }
            }
        }
        return count;
    }

    // Sahnedeki tüm aktif düşmanların listesini döndürür (FPS dostu işlem için)
    public List<GameObject> GetAllActiveEnemies()
    {
        List<GameObject> activeEnemies = new List<GameObject>();
        if (pools == null) return activeEnemies;

        foreach (var list in pools.Values)
        {
            foreach (GameObject obj in list)
            {
                if (obj.activeInHierarchy)
                {
                    activeEnemies.Add(obj);
                }
            }
        }
        return activeEnemies;
    }
}
