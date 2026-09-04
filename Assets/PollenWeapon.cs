using UnityEngine;
using System.Collections.Generic;

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

    [Tooltip("Model ters veya yan bakıyorsa buradan Y eksenine 90, 180 veya -90 girerek düzeltebilirsiniz.")]
    public Vector3 modelRotationOffset;

    [Tooltip("Silahın Sylva'dan ne kadar uzakta süzüleceği (Yarıçap)")]
    public float orbitRadius = 1.3f;
    [Tooltip("Silah yanlış tarafta süzülüyorsa buraya 180 yazarak diğer tarafa atabilirsiniz.")]
    public float orbitAngleOffset = 0f;
    [Tooltip("Silah ateş ederken düşmanın diğer tarafına uçuyorsa (Pivot bozuksa) buraya 1, 2 veya -1, -2 gibi değerler girip düzeltin!")]
    public float pivotFixZ = 0f;
    [Tooltip("Silahın yerden/Sylva'dan yüksekliği")]
    public float floatHeight = 0.8f;
    [Tooltip("Silahın düşmana doğru yörüngede kayma hızı")]
    public float orbitSpeed = 720f; // Derece / Saniye

    [Header("Görsel Efektler")]
    [Tooltip("Namlu ucu ateş efekti (Sprite veya Particle). Mermi sıkarken 0.05 sn görünüp kaybolur.")]
    public GameObject muzzleFlash;
    [Tooltip("Geri tepme şiddeti")]
    public float recoilForce = 0.8f;

    // Silahın yörüngedeki anlık açısı (Düşman yokken en son kaldığı açıyı korur!)
    private float currentOrbitAngle = 45f;
    private float nextFireTime;
    private GameObject currentTarget;
    private Vector3 currentRecoilOffset = Vector3.zero; // Geri tepme vektörü

    void Start()
    {
        if (gunModel == null && transform.childCount > 0)
        {
            gunModel = transform.GetChild(0);
        }

        if (gunModel != null)
        {
            // --- ULTIMATE AUTO-FIX ---
            Camera[] cams = gunModel.GetComponentsInChildren<Camera>(true);
            foreach (Camera c in cams) Destroy(c.gameObject);
            
            Light[] lights = gunModel.GetComponentsInChildren<Light>(true);
            foreach (Light l in lights) Destroy(l.gameObject);

            GameObject container = new GameObject(gunModel.name + "_AutoFix");
            container.transform.position = gunModel.position;
            container.transform.rotation = gunModel.rotation;
            container.transform.parent = transform;

            Transform originalGun = gunModel;
            originalGun.SetParent(container.transform, true);

            originalGun.localRotation = Quaternion.Euler(0, 180, 0);

            Renderer[] renderers = originalGun.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
                Vector3 worldOffset = bounds.center - container.transform.position;
                originalGun.position -= worldOffset;
            }

            gunModel = container.transform;
        }

        if (gunModel != null && gunAnimator == null)
        {
            gunAnimator = gunModel.GetComponentInChildren<Animator>();
        }

        if (muzzleFlash != null) muzzleFlash.SetActive(false);
    }

    void Update()
    {
        if (!enabled) return;

        currentTarget = FindNearestEnemy();
        UpdateGunPositionAndRotation();

        if (Time.time >= nextFireTime && currentTarget != null)
        {
            nextFireTime = Time.time + baseFireRate;
            FireAtTarget(currentTarget);
        }
    }

    void UpdateGunPositionAndRotation()
    {
        if (gunModel == null) return;

        if (currentTarget != null)
        {
            Vector3 toEnemy = (currentTarget.transform.position - transform.position);
            toEnemy.y = 0;
            if (toEnemy.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(toEnemy.x, toEnemy.z) * Mathf.Rad2Deg;
                currentOrbitAngle = Mathf.MoveTowardsAngle(currentOrbitAngle, targetAngle, orbitSpeed * Time.deltaTime);
            }
        }

        Quaternion orbitRot = Quaternion.Euler(0, currentOrbitAngle + orbitAngleOffset, 0);
        Vector3 offset = orbitRot * Vector3.forward * orbitRadius;
        
        float bobbing = Mathf.Sin(Time.time * 3f) * 0.05f;
        
        // 1. Silahın pürüzsüz takip edeceği ana pozisyon (Lerp ile yavaşça gider)
        Vector3 baseTargetPos = transform.position + offset + Vector3.up * (floatHeight + bobbing);
        gunModel.position = Vector3.Lerp(gunModel.position, baseTargetPos, Time.deltaTime * 15f);

        // 2. KESKİN Geri Tepme (Lerp'in yavaşlığını by-pass edip direkt üstüne ekliyoruz ki net görünsün)
        currentRecoilOffset = Vector3.Lerp(currentRecoilOffset, Vector3.zero, Time.deltaTime * 15f);
        gunModel.position += currentRecoilOffset;

        // Silahın bakış açısı: Hedef varsa direkt hedefe, yoksa yörünge açısına doğru baksın
        if (currentTarget != null)
        {
            Vector3 lookTarget = currentTarget.transform.position;
            lookTarget.y = gunModel.position.y; // Yatayda baksın
            Vector3 lookDir = (lookTarget - gunModel.position).normalized;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir) * Quaternion.Euler(modelRotationOffset);
                gunModel.rotation = Quaternion.Slerp(gunModel.rotation, targetRot, Time.deltaTime * 20f);
            }
        }
        else
        {
            // Düşman yokken en son baktığı yörünge yönüne baksın
            Quaternion targetRot = Quaternion.Euler(0, currentOrbitAngle, 0) * Quaternion.Euler(modelRotationOffset);
            gunModel.rotation = Quaternion.Slerp(gunModel.rotation, targetRot, Time.deltaTime * 10f);
        }
    }

    GameObject FindNearestEnemy()
    {
        if (EnemyPool.Instance == null) return null;

        List<GameObject> enemies = EnemyPool.Instance.GetAllActiveEnemies();
        GameObject nearest = null;
        float minSqrDistance = range * range;

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null || !enemyObj.activeInHierarchy) continue;

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
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot");
        }

        // Ateş ederken silahı geriye doğru it (Keskin Recoil efekti)
        if (gunModel != null)
        {
            currentRecoilOffset = -gunModel.forward * recoilForce; 
        }

        // Namlu ateşini göster
        if (muzzleFlash != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }

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

    private System.Collections.IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        
        ParticleSystem ps = muzzleFlash.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        else
        {
            // Eğer Sprite ise yönünü ayarla
            muzzleFlash.transform.localRotation = Quaternion.Euler(-90f, 0, UnityEngine.Random.Range(0f, 360f)); 
        }

        yield return new WaitForSeconds(0.1f);
        
        if (ps == null) 
        {
            muzzleFlash.SetActive(false); // Sadece sprite ise kapat, particle kendi kaybolur
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

