using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [Tooltip("Atılacak merminin Prefab'ı")]
    public GameObject projectilePrefab;
    
    [Tooltip("Oyun başında hafızada kaç mermi hazır beklesin?")]
    public int poolSize = 50;

    private List<GameObject> pool;

    void Awake()
    {
        Instance = this;
        pool = new List<GameObject>();

        // Oyun başlarken havuzu mermilerle dolduruyoruz
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetProjectile()
    {
        // Havuzda kullanılmayan mermi varsa onu ver
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Eğer çok hızlı ateş edilirse ve havuz yetmezse yenisini yaratıp havuza ekle
        GameObject newObj = Instantiate(projectilePrefab, transform);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}
