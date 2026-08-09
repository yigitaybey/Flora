using UnityEngine;

public class PollenWeapon : MonoBehaviour
{
    [Header("Polen Enjektörü Ayarları")]
    public int currentLevel = 1; 
    
    public float damage = 15f;
    public float baseFireRate = 0.8f; 
    public float range = 10f; 
    public float projectileSpeed = 15f; 

    private float nextFireTime;

    void Update()
    {
        if (!enabled) return;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + baseFireRate;
            FireAtNearestEnemy();
        }
    }

    void FireAtNearestEnemy()
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

        if (nearestEnemy != null)
        {
            Vector3 baseDirection = (nearestEnemy.transform.position - transform.position).normalized;
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

            SpawnBullets(baseDirection, bulletCount, pierceAmount);
        }
    }

    void SpawnBullets(Vector3 baseDir, int count, int pierceAmount)
    {
        float spreadAngle = 15f; 

        if (count == 1)
        {
            FireBullet(baseDir, pierceAmount);
            return;
        }
        
        float startAngle = -spreadAngle * (count / 2);

        for (int i = 0; i < count; i++)
        {
            float currentAngle = startAngle + (spreadAngle * i);
            Vector3 shootDir = Quaternion.Euler(0, currentAngle, 0) * baseDir;
            FireBullet(shootDir, pierceAmount);
        }
    }

    void FireBullet(Vector3 direction, int pierceAmount)
    {
        GameObject bulletObj = ProjectilePool.Instance.GetProjectile();
        bulletObj.transform.position = transform.position;
        bulletObj.transform.forward = direction;
        
        Projectile projectile = bulletObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Global hasar çarpanını burada uyguluyoruz (Örn: Flamethrower sinerjisinden gelen bonus)
            float finalDamage = damage * UpgradeManager.Instance.globalDamageMultiplier;
            projectile.Fire(direction, projectileSpeed, finalDamage, pierceAmount);
        }
    }

    // --- UPGRADE SİSTEMİ ÇAĞRILARI ---

    public void UnlockWeapon()
    {
        enabled = true;
        currentLevel = 1;
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
                baseFireRate *= 0.85f; // %15 artış
                break;
            case 3:
                damage *= 1.20f; // %20 artış
                if (playerHealth != null) playerHealth.IncreaseMaxHealth(10f); // SİNERJİ
                break;
            case 4:
                // Mermi sayısı Update'te currentLevel >= 4 ile kontrol ediliyor.
                break;
            case 5:
                projectileSpeed *= 1.20f; // %20 artış
                range *= 1.20f;
                break;
            case 6:
                damage *= 1.25f; // %25 artış
                if (playerHealth != null) playerHealth.IncreaseMaxHealth(20f); // SİNERJİ
                break;
            case 7:
                baseFireRate *= 0.75f; // %25 artış
                break;
            case 8:
                // 5 Mermi ve Pierce Update'te currentLevel >= 8 ile kontrol ediliyor.
                if (playerHealth != null) playerHealth.IncreaseMaxHealth(20f); // SİNERJİ
                break;
        }

        Debug.Log("Polen Silahı Seviye Atladı! Yeni Level: " + currentLevel);
    }
}
