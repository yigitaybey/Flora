using UnityEngine;

public class PollenWeapon : MonoBehaviour
{
    [Header("Polen Enjektörü Statları")]
    public int currentLevel = 1;
    public float damage = 15f;
    public float baseFireRate = 0.8f;
    public float range = 10f;
    public float projectileSpeed = 15f;
    public float spreadAngle = 15f; // Çoklu mermi yayılım açısı

    [Header("Yüzen Silah (Orbital) Ayarları")]
    [Tooltip("Silahın 3D Model objesi (Sylva'nın çocuğu veya ayrı obje)")]
    public Transform gunModel;
    [Tooltip("Mermilerin çıkacağı namlu ucu")]
    public Transform firePoint;
    [Tooltip("Silahın üzerindeki Animator (Ateş animasyonu)")]
    public Animator gunAnimator;

    [Tooltip("Silahın Sylva'dan ne kadar uzakta süzüleceği (Yarıçap)")]
    public float orbitRadius = 1.3f;
    [Tooltip("Silahın yerden/Sylva'dan yüksekliği")]
    public float floatHeight = 0.8f;
    [Tooltip("Silahın düşmana doğru yörüngede kayma hızı")]
    public float orbitSpeed = 720f; // Derece / Saniye

    // Silahın yörüngedeki anlık açısı (Düşman yokken en son kaldığı açıyı korur!)
    private float currentOrbitAngle = 45f;
    private float nextFireTime;
    private GameObject currentTarget;

    void Start()
    {
        // Eğer gunModel atanmadıysa ve alt objelerde varsa otomatik bul
        if (gunModel == null && transform.childCount > 0)
        {
            gunModel = transform.GetChild(0);
        }

        if (gunModel != null && gunAnimator == null)
        {
            gunAnimator = gunModel.GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (!enabled) return;

        // 1. En yakın düşmanı tespit et
        currentTarget = FindNearestEnemy();

        // 2. Silahın yörünge pozisyonunu ve açısını güncelle
        UpdateGunPositionAndRotation();

        // 3. Ateşleme zamanı geldi mi?
        if (Time.time >= nextFireTime && currentTarget != null)
        {
            nextFireTime = Time.time + baseFireRate;
            FireAtTarget(currentTarget);
        }
    }

    void UpdateGunPositionAndRotation()
    {
        if (gunModel == null) return;

        // Düşman varsa, silah o düşmanın olduğu açıya doğru yörüngede kaysın
        if (currentTarget != null)
        {
            Vector3 toEnemy = (currentTarget.transform.position - transform.position);
            toEnemy.y = 0;
            if (toEnemy.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(toEnemy.x, toEnemy.z) * Mathf.Rad2Deg;
                // En kısa yoldan pürüzsüzce hedef açıya dön
                currentOrbitAngle = Mathf.MoveTowardsAngle(currentOrbitAngle, targetAngle, orbitSpeed * Time.deltaTime);
            }
        }
        // Düşman yokken: currentOrbitAngle OLDUĞU GİBİ KALIR! (Öne sıfırlanmaz)

        // Yörünge konumu hesapla (Sylva merkezli)
        Quaternion orbitRot = Quaternion.Euler(0, currentOrbitAngle, 0);
        Vector3 offset = orbitRot * Vector3.forward * orbitRadius;
        
        // Hafif tatlı süzülme salınımı (Bobbing)
        float bobbing = Mathf.Sin(Time.time * 3f) * 0.05f;
        Vector3 targetWorldPos = transform.position + offset + Vector3.up * (floatHeight + bobbing);

        // Silah modelinin pozisyonunu uygula
        gunModel.position = Vector3.Lerp(gunModel.position, targetWorldPos, Time.deltaTime * 15f);

        // Silahın bakış açısı: Hedef varsa direkt hedefe, yoksa yörünge açısına doğru baksın
        if (currentTarget != null)
        {
            Vector3 lookTarget = currentTarget.transform.position;
            lookTarget.y = gunModel.position.y; // Yatayda baksın
            Vector3 lookDir = (lookTarget - gunModel.position).normalized;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                gunModel.rotation = Quaternion.Slerp(gunModel.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 20f);
            }
        }
        else
        {
            // Düşman yokken en son baktığı yörünge yönüne baksın
            gunModel.rotation = Quaternion.Slerp(gunModel.rotation, Quaternion.Euler(0, currentOrbitAngle, 0), Time.deltaTime * 10f);
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minSqrDistance = range * range;

        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeInHierarchy) continue;

            float sqrDist = (transform.position - enemyObj.transform.position).sqrMagnitude;
            if (sqrDist < minSqrDistance)
            {
                minSqrDistance = sqrDist;
                nearest = enemyObj;
            }
        }

        return nearest;
    }

    void FireAtTarget(GameObject target)
    {
        // Ateş animasyonunu tetikle
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot");
        }

        // Namlu ucu varsa oradan, yoksa gunModel'in önünden çıkar
        Vector3 spawnPos = firePoint != null ? firePoint.position : (gunModel != null ? gunModel.position : transform.position);
        
        Vector3 baseDirection = (target.transform.position - spawnPos).normalized;
        baseDirection.y = 0;

        int bulletCount = 1;
        int pierceAmount = 1;

        if (currentLevel >= 8)
        {
            bulletCount = 5;
            pierceAmount = 3;
        }
        else if (currentLevel >= 4)
        {
            bulletCount = 3;
        }

        SpawnBullets(spawnPos, baseDirection, bulletCount, pierceAmount);
    }

    void SpawnBullets(Vector3 spawnPos, Vector3 baseDir, int count, int pierceAmount)
    {
        if (count == 1)
        {
            FireBullet(spawnPos, baseDir, pierceAmount);
            return;
        }

        float startAngle = -spreadAngle * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = startAngle + (spreadAngle * i);
            Vector3 shootDir = Quaternion.Euler(0, currentAngle, 0) * baseDir;
            FireBullet(spawnPos, shootDir, pierceAmount);
        }
    }

    void FireBullet(Vector3 spawnPos, Vector3 direction, int pierceAmount)
    {
        if (ProjectilePool.Instance == null) return;

        GameObject bulletObj = ProjectilePool.Instance.GetProjectile();
        if (bulletObj == null) return;

        bulletObj.transform.position = spawnPos;
        bulletObj.transform.forward = direction;

        Projectile projectile = bulletObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            float finalDamage = damage * UpgradeManager.Instance.globalDamageMultiplier;
            projectile.Fire(direction, projectileSpeed, finalDamage, pierceAmount);
        }
    }

    // --- UPGRADE SİSTEMİ ---
    public void UnlockWeapon()
    {
        enabled = true;
        currentLevel = 1;
        if (gunModel != null) gunModel.gameObject.SetActive(true);
        Debug.Log("Polen Silahı Açıldı!");
    }

    public void LevelUpWeapon()
    {
        if (currentLevel >= 8) return;

        currentLevel++;
        PlayerHealth playerHealth = UpgradeManager.Instance.playerHealth;

        switch (currentLevel)
        {
            case 2:
                baseFireRate *= 0.85f;
                break;
            case 3:
                damage *= 1.20f;
                if (playerHealth != null) playerHealth.IncreaseMaxHealth(10f);
                break;
            case 4:
                // 3 Mermi
                break;
            case 5:
                projectileSpeed *= 1.20f;
                range *= 1.20f;
                break;
            case 6:
                damage *= 1.25f;
                if (playerHealth != null) playerHealth.IncreaseMaxHealth(20f);
                break;
            case 7:
                baseFireRate *= 0.75f;
                break;
            case 8:
                // 5 Mermi ve Delip Geçme (Pierce)
                if (playerHealth != null) playerHealth.IncreaseMaxHealth(20f);
                break;
        }

        Debug.Log("Polen Silahı Seviye Atladı! Yeni Level: " + currentLevel);
    }

    // Unity Scene Ekranında Menzili Göster (Gizmos)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}

