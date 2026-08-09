using System.Collections.Generic;
using UnityEngine;

public class DamagePopupPool : MonoBehaviour
{
    public static DamagePopupPool Instance;

    public GameObject damagePopupPrefab;
    public int poolSize = 30;

    private List<GameObject> pool;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        pool = new List<GameObject>();
    }

    void Start()
    {
        // Havuzu başlangıçta doldur
        if (damagePopupPrefab != null)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(damagePopupPrefab, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    public GameObject GetPopup()
    {
        if (damagePopupPrefab == null) return null;

        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Havuz dolduysa ve hepsi ekrandaysa yeni oluştur (Havuzu genişlet)
        GameObject newObj = Instantiate(damagePopupPrefab, transform);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }
}
