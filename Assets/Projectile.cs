using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    private float damage;
    private int pierceCount;
    
    // Optimizasyon: Çarpışma algılama mesafesinin karesi (1.5f * 1.5f) = 2.25f (Merminin isabet alanı büyütüldü)
    private float hitDistanceSq = 2.25f;

    // Aynı düşmana her frame'de tekrar vurmamak için vurduğumuz düşmanların listesi
    private List<GameObject> hitEnemies = new List<GameObject>();

    // Maksimum uçuş süresi (Sonsuza kadar gidip rami şişirmesin)
    private float lifetime = 3f;
    private float currentLifeTime = 0f;

    public void Fire(Vector3 direction, float projSpeed, float projDamage, int pierceAmount)
    {
        moveDirection = direction.normalized;
        moveDirection.y = 0; // İzometrikte yükseklik yok
        
        speed = projSpeed;
        damage = projDamage;
        pierceCount = pierceAmount;
        
        hitEnemies.Clear();
        currentLifeTime = 0f;
        
        gameObject.SetActive(true);
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > lifetime)
        {
            gameObject.SetActive(false); // Ömrü doldu, havuza geri dön
            return;
        }

        // Belirlenen yöne doğru ilerle
        transform.position += moveDirection * speed * Time.deltaTime;

        // Optimizasyonlu çarpışma testi (Physics Collider kullanmadan)
        // FindGameObjectsWithTag yerine Havuzdan aktif düşmanları çekiyoruz
        List<GameObject> enemies = EnemyPool.Instance != null ? EnemyPool.Instance.GetAllActiveEnemies() : new List<GameObject>();

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null || !enemyObj.activeInHierarchy || hitEnemies.Contains(enemyObj)) continue;

            Vector3 diff = transform.position - enemyObj.transform.position;
            diff.y = 0; // İzometrik oyunda yüksekliği görmezden gel, sadece X ve Z'ye bak
            float sqrDistance = diff.sqrMagnitude;
            
            if (sqrDistance <= hitDistanceSq)
            {
                // Hedefi vurduk!
                Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                    hitEnemies.Add(enemyObj); // Bu düşmana bir daha vurma
                    
                    pierceCount--;
                    if (pierceCount <= 0)
                    {
                        // Delip geçme hakkı bittiyse mermiyi yok et (havuza yolla)
                        gameObject.SetActive(false);
                        return; // Döngüden tamamen çık
                    }
                }
            }
        }
    }
}
