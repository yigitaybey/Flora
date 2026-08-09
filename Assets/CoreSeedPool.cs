using System.Collections.Generic;
using UnityEngine;

public class CoreSeedPool : MonoBehaviour
{
    public static CoreSeedPool Instance;

    public GameObject coreSeedPrefab; // Core Seed Prefab'i
    public int poolSize = 20; // Sahnede aynı anda çok fazla olmayacağı için 20 yeterli

    private List<GameObject> pool;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializePool();
    }

    void InitializePool()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(coreSeedPrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetCoreSeed()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Eğer havuzda boşta obje kalmadıysa yeni yarat ve havuza ekle
        GameObject newObj = Instantiate(coreSeedPrefab, transform);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}
