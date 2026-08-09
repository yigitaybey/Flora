using UnityEngine;

public class FlamethrowerWeapon : MonoBehaviour
{
    [Header("Alev Silahı Ayarları")]
    public int currentLevel = 1;

    public float damagePerTick = 10f; 
    public float tickRate = 0.5f;    // Daha dengeli bekleme süresi
    public float range = 8f;          
    public float coneAngle = 60f;     

    [Header("Görsel Ayarlar")]
    public GameObject flameVisual;

    private float nextTickTime;

    void Start()
    {
        // Visual boyutları vb. Setup
    }

    void Update()
    {
        if (!enabled)
        {
            if (flameVisual != null && flameVisual.activeSelf) flameVisual.SetActive(false);
            return;
        }

        if (Time.time >= nextTickTime)
        {
            nextTickTime = Time.time + tickRate;
            FireFlamethrower();
        }
    }

    void FireFlamethrower()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minSqrDistance = range * range;

        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeInHierarchy) continue;

            float sqrDist = (transform.position - enemyObj.transform.position).sqrMagnitude;
            if (sqrDist < minSqrDistance)
            {
                minSqrDistance = sqrDist;
                nearestEnemy = enemyObj;
            }
        }

        if (nearestEnemy == null)
        {
            if (flameVisual != null && flameVisual.activeSelf)
                flameVisual.SetActive(false);
            return;
        }

        if (flameVisual != null && !flameVisual.activeSelf)
            flameVisual.SetActive(true);

        Vector3 fireDirection = (nearestEnemy.transform.position - transform.position).normalized;
        fireDirection.y = 0; 
        
        if (flameVisual != null && fireDirection != Vector3.zero)
        {
            flameVisual.transform.forward = fireDirection;
        }

        float sqrRange = range * range;
        
        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeInHierarchy) continue;

            Vector3 directionToEnemy = (enemyObj.transform.position - transform.position);
            directionToEnemy.y = 0; 

            float sqrDistanceToEnemy = directionToEnemy.sqrMagnitude;

            if (sqrDistanceToEnemy <= sqrRange)
            {
                float angle = Vector3.Angle(fireDirection, directionToEnemy.normalized);

                if (angle <= coneAngle / 2f)
                {
                    Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        // Global hasar çarpanını dahil et
                        float finalDamage = damagePerTick * UpgradeManager.Instance.globalDamageMultiplier;
                        enemyScript.TakeDamage(finalDamage);
                        
                        // MAX Level (8) ise "Yanık Toprak" (Burning) etkisi uygula
                        if (currentLevel >= 8)
                        {
                            enemyScript.ApplyBurn(finalDamage / 2f, 3f);
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // --- UPGRADE SİSTEMİ ÇAĞRILARI ---

    public void UnlockWeapon()
    {
        enabled = true;
        currentLevel = 1;
        Debug.Log("Alev Makinesi Açıldı!");
    }

    public void LevelUpWeapon()
    {
        if (currentLevel >= 8) return;

        currentLevel++;
        UpgradeManager upgradeManager = UpgradeManager.Instance;

        switch (currentLevel)
        {
            case 2:
                tickRate *= 0.80f; // Bekleme süresi -%20
                break;
            case 3:
                damagePerTick *= 1.25f; // Hasar +%25
                if (upgradeManager != null) upgradeManager.globalDamageMultiplier += 0.10f; // SİNERJİ: Global Hasar +%10
                break;
            case 4:
                range *= 1.5f; // Menzil x 1.5
                damagePerTick *= 1.25f; // Biraz da hasar ekleyelim (Uzar demiş)
                
                if (flameVisual != null && flameVisual.transform.childCount > 0)
                {
                    Transform cube = flameVisual.transform.GetChild(0);
                    Vector3 newScale = cube.localScale;
                    newScale.z *= 1.5f;
                    cube.localScale = newScale;
                }
                break;
            case 5:
                coneAngle *= 1.30f; // Alev genişliği +%30
                break;
            case 6:
                tickRate *= 0.70f; // Bekleme süresi -%30
                if (upgradeManager != null) upgradeManager.globalDamageMultiplier += 0.10f; // SİNERJİ: Global Hasar +%10
                break;
            case 7:
                damagePerTick *= 1.35f; // Hasar +%35
                break;
            case 8:
                // Yanma efekti Update içinde (currentLevel >= 8) olarak kontrol ediliyor
                if (upgradeManager != null) upgradeManager.globalDamageMultiplier += 0.15f; // SİNERJİ: Global Hasar +%15
                break;
        }

        Debug.Log("Alev Makinesi Seviye Atladı! Yeni Level: " + currentLevel);
    }
}
