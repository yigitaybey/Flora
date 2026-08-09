using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    [Header("Balta (Yörünge) Ayarları")]
    public int currentLevel = 1;

    public float damage = 20f;
    public float orbitRadius = 2.5f; 
    public float orbitSpeed = 180f;  
    public float tickRate = 0.2f;    
    public float hitArea = 1.2f;     

    [Header("Görsel Ayarlar")]
    public GameObject axeVisual; 
    
    private GameObject axeModelPrefab;
    private List<Transform> activeAxes = new List<Transform>();
    
    private float currentAngle = 0f;
    private float nextTickTime;

    void Start()
    {
        if (axeVisual != null && axeVisual.transform.childCount > 0)
        {
            axeModelPrefab = axeVisual.transform.GetChild(0).gameObject;
            activeAxes.Add(axeModelPrefab.transform); 
        }
        else
        {
            Debug.LogWarning("AxeWeapon için Axe Visual atanmamış veya içi boş!");
        }
    }

    void Update()
    {
        if (!enabled || activeAxes.Count == 0)
        {
            if (axeVisual != null && axeVisual.activeSelf) axeVisual.SetActive(false);
            return;
        }
        
        if (axeVisual != null && !axeVisual.activeSelf) axeVisual.SetActive(true);

        // Baltaların oyuncu etrafındaki yörünge açısını hesapla
        currentAngle -= orbitSpeed * Time.deltaTime; 
        
        float angleStep = 360f / activeAxes.Count;

        for (int i = 0; i < activeAxes.Count; i++)
        {
            float angle = currentAngle + (i * angleStep);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * orbitRadius;
            float z = Mathf.Sin(rad) * orbitRadius;

            activeAxes[i].localPosition = new Vector3(x, 1f, z);
            
            // Balta fırıldak gibi kendi etrafında da dönsün
            activeAxes[i].Rotate(Vector3.up, 720f * Time.deltaTime); 
        }

        // HASAR HESAPLAMA
        if (Time.time >= nextTickTime)
        {
            nextTickTime = Time.time + tickRate;
            DealDamage();
        }
    }

    void DealDamage()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float hitRadiusSqr = hitArea * hitArea;

        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeInHierarchy) continue;

            foreach (Transform axe in activeAxes)
            {
                float sqrDistance = (axe.position - enemyObj.transform.position).sqrMagnitude;
                
                if (sqrDistance <= hitRadiusSqr)
                {
                    Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        // Global hasar çarpanını dahil et
                        float finalDamage = damage * UpgradeManager.Instance.globalDamageMultiplier;
                        enemyScript.TakeDamage(finalDamage);
                        break; // 1 tick'te 1 balta vursun yeter
                    }
                }
            }
        }
    }

    // --- UPGRADE SİSTEMİ ÇAĞRILARI ---

    public void UnlockWeapon()
    {
        enabled = true;
        currentLevel = 1;
        Debug.Log("Balta Silahı Açıldı!");
    }

    public void LevelUpWeapon()
    {
        if (currentLevel >= 8) return;

        currentLevel++;
        PlayerMovement playerMovement = UpgradeManager.Instance.playerMovement;

        switch (currentLevel)
        {
            case 2:
                damage *= 1.25f; // Hasar +%25
                break;
            case 3:
                orbitSpeed *= 1.20f; // Dönüş Hızı +%20
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.05f); // SİNERJİ: Hız +%5
                break;
            case 4:
                SetAxeCount(2); // 2 Balta
                orbitSpeed *= 1.10f; // Dönüş Hızı hafif artar
                break;
            case 5:
                // Tüm baltaların boyutunu (scale) %25 büyüt
                if (axeModelPrefab != null)
                {
                    Vector3 newScale = axeModelPrefab.transform.localScale * 1.25f;
                    axeModelPrefab.transform.localScale = newScale;
                    foreach (Transform axe in activeAxes)
                    {
                        axe.localScale = newScale;
                    }
                }
                break;
            case 6:
                damage *= 1.30f; // Hasar +%30
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.10f); // SİNERJİ: Hız +%10
                break;
            case 7:
                orbitSpeed *= 1.30f; // Dönüş Hızı +%30
                break;
            case 8:
                SetAxeCount(4); // 4 Balta
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.10f); // SİNERJİ: Hız +%10
                break;
        }

        Debug.Log("Balta Seviye Atladı! Yeni Level: " + currentLevel);
    }

    void SetAxeCount(int count)
    {
        if (axeModelPrefab == null) return;

        // Öncekileri temizle (0. index hariç çünkü o bizim ana modelimiz)
        for (int i = 1; i < activeAxes.Count; i++)
        {
            if (activeAxes[i] != null) Destroy(activeAxes[i].gameObject);
        }
        
        activeAxes.Clear();
        activeAxes.Add(axeModelPrefab.transform); // Ana modeli tekrar koy

        // Eksik olanları üret
        for (int i = 1; i < count; i++)
        {
            GameObject newAxe = Instantiate(axeModelPrefab, axeVisual.transform);
            // Referans aldığımız objenin scale değerini yeni üretilenlere de ata
            newAxe.transform.localScale = axeModelPrefab.transform.localScale;
            activeAxes.Add(newAxe.transform);
        }
    }
}
