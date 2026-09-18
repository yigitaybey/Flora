using UnityEngine;

public class Turret : MonoBehaviour
{
    private float lifetime;
    private float damage;
    private float fireRate = 1f; // Saniyede 1 kere ateş eder
    private float nextFireTime;
    private float range = 10f; // 10 metre menzil
    private float projectileSpeed = 15f;

    public void Setup(float duration, float dmg)
    {
        lifetime = duration;
        damage = dmg;
        nextFireTime = Time.time + 0.5f; // Kurulduktan 0.5 saniye sonra ilk ateşi atar
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            // Ömrü bitince kendini yok et ve listeden çıkar
            if (TurretManager.Instance != null) TurretManager.Instance.RemoveTurret(this);
            gameObject.SetActive(false);
            Destroy(gameObject); // Veya Object Pool kullanılabilir
            return;
        }

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            FireAtNearestEnemy();
        }
    }

    void FireAtNearestEnemy()
    {
        // En yakındaki düşmanı bul (Sıfır çöp / Zero-Alloc)
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

        // Eğer menzilde düşman varsa ateş et
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            direction.y = 0; // Yerden paralel atsın

            if (ProjectilePool.Instance != null)
            {
                GameObject bulletObj = ProjectilePool.Instance.GetProjectile();
                if (bulletObj != null)
                {
                    bulletObj.transform.position = transform.position + Vector3.up * 0.5f; // Yerden az yukardan çıksın
                    bulletObj.transform.forward = direction;
                    
                    Projectile projectile = bulletObj.GetComponent<Projectile>();
                    if (projectile != null)
                    {
                        // Taret mermileri delici değil (1)
                        projectile.Fire(direction, projectileSpeed, damage, 1);
                    }
                }
            }
        }
    }
}
