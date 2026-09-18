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
    public float rotationSpeed = 8f; // Silahın düşmana doğru ne kadar yumuşak (slide) döneceği

    private float nextTickTime;
    private Vector3 currentTargetDirection = Vector3.forward;

    private ParticleSystem[] flameParticles;
    private bool isFiring = false;

    void Start()
    {
        if (flameVisual != null)
        {
            flameParticles = flameVisual.GetComponentsInChildren<ParticleSystem>(true);
            
            // Oyun başlar başlamaz (silah alındığında) parçacıkların otomatik ateşlenmesini engelle
            foreach (var ps in flameParticles)
            {
                if (ps != null) ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    void Update()
    {
        if (!enabled)
        {
            if (flameVisual != null && flameVisual.activeSelf) flameVisual.SetActive(false);
            return;
        }
        else
        {
            // Silah açıksa modeli her zaman görünür olsun
            if (flameVisual != null && !flameVisual.activeSelf) flameVisual.SetActive(true);
        }

        // Hedef bulma ve Namlu Dönüşünü (Görseli) HER KARE anında yap
        HandleTargetingAndVisuals();

        // Hasar verme işlemini (Tick) sadece düşman varken ve süre dolduğunda yap
        if (isFiring && Time.time >= nextTickTime)
        {
            nextTickTime = Time.time + tickRate;
            DealDamageInCone();
        }
    }

    void SetFiringState(bool fire)
    {
        if (isFiring == fire) return;
        isFiring = fire;

        if (flameParticles == null) return;

        foreach (var ps in flameParticles)
        {
            if (ps == null) continue;
            
            if (fire)
                ps.Play(true);
            else
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void HandleTargetingAndVisuals()
    {
        Enemy nearestEnemy = null;
        float minSqrDistance = range * range;

        for (int i = 0; i < Enemy.ActiveEnemies.Count; i++)
        {
            Enemy enemy = Enemy.ActiveEnemies[i];
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float sqrDist = (transform.position - enemy.transform.position).sqrMagnitude;
            if (sqrDist < minSqrDistance)
            {
                minSqrDistance = sqrDist;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy == null)
        {
            SetFiringState(false); // Düşman yoksa anında ateşi kes
            return;
        }

        SetFiringState(true); // Düşman menzildeyse ANINDA ateşe başla

        // Hedef yönünü sürekli güncelle
        Vector3 fireDirection = (nearestEnemy.transform.position - transform.position).normalized;
        fireDirection.y = 0; 
        
        if (fireDirection != Vector3.zero)
        {
            currentTargetDirection = fireDirection;
        }

        // --- YUMUŞAK (SLIDE) DÖNÜŞ (RADYAL HAREKET) ---
        if (flameVisual != null && currentTargetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentTargetDirection);
            flameVisual.transform.rotation = Quaternion.Slerp(flameVisual.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void DealDamageInCone()
    {
        float sqrRange = range * range;
        float finalDamage = damagePerTick * (UpgradeManager.Instance != null ? UpgradeManager.Instance.globalDamageMultiplier : 1f);
        
        for (int i = 0; i < Enemy.ActiveEnemies.Count; i++)
        {
            Enemy enemyScript = Enemy.ActiveEnemies[i];
            if (enemyScript == null || !enemyScript.gameObject.activeInHierarchy) continue;

            Vector3 directionToEnemy = (enemyScript.transform.position - transform.position);
            directionToEnemy.y = 0; 

            float sqrDistanceToEnemy = directionToEnemy.sqrMagnitude;

            if (sqrDistanceToEnemy <= sqrRange)
            {
                float angle = Vector3.Angle(currentTargetDirection, directionToEnemy.normalized);

                if (angle <= coneAngle / 2f)
                {
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
