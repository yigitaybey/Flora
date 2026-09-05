using UnityEngine;
using System.Collections.Generic;

// Unity'nin fizik motoruna (Rigidbody/Collider) bağımlı kalmamak için Seed.cs'teki gibi
// Update içinde mesafe ölçümü (Distance) kullanarak toplanma mantığına geçtik.
public class ConsumableItem : MonoBehaviour
{
    [Header("Eşya Tipi")]
    public ConsumableType type;
    
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 1. GÖRSEL CİLA: Eşyalar sürekli olarak kendi etrafında yavaşça döner (Space.World ekledik)
        transform.Rotate(0, 45f * Time.deltaTime, 0, Space.World);

        if (playerTransform == null) return;

        // Eşya ile oyuncu arasındaki mesafeyi ölç (Y eksenini yoksayarak hesapla)
        // Böylece Core Seed gibi çok büyük/uzun objelerin merkez noktası havada olsa bile oyuncuya değdiği algılanır.
        Vector3 flatItemPos = transform.position;
        flatItemPos.y = 0;
        Vector3 flatPlayerPos = playerTransform.position;
        flatPlayerPos.y = 0;

        float sqrDistance = (flatItemPos - flatPlayerPos).sqrMagnitude;
        
        // Eğer oyuncu yeterince yaklaştıysa (2.5f uzaklık karesi) topla
        if (sqrDistance <= 2.5f)
        {
            CollectConsumable();
        }
    }

    void CollectConsumable()
    {
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        
        switch (type)
        {
            case ConsumableType.Potion:
                if (playerHealth != null)
                {
                    playerHealth.Heal(30f); // 30 Can verir
                    Debug.Log("İksir (Potion) Alındı! +30 Can");
                }
                break;
                
            case ConsumableType.ScreenWipe:
                // Sahnedeki tüm düşmanlara devasa hasar ver (Kavur)
                if (EnemyPool.Instance != null)
                {
                    List<GameObject> activeEnemies = EnemyPool.Instance.GetAllActiveEnemies();
                    foreach (GameObject enemy in activeEnemies)
                    {
                        Enemy eScript = enemy.GetComponent<Enemy>();
                        if (eScript != null)
                        {
                            if (eScript.enemyType == EnemyType.Chinar || eScript.isElite)
                            {
                                // Boss veya Elit ise tek atma, canının %30'u kadar hasar vur
                                eScript.TakeDamage(eScript.currentHealth * 0.3f);
                            }
                            else
                            {
                                // Normal düşmanlara tek at
                                eScript.TakeDamage(9999f); 
                            }
                        }
                    }
                }
                
                // Ekran parlama ve zamanı durdurma efektini çağır!
                if (Consumable.Instance != null)
                {
                    Consumable.Instance.PlayScreenWipeEffect();
                }

                Debug.Log("Güneş Işığı (Screen Wipe) Alındı! Düşmanlar kavruldu.");
                break;
                
        case ConsumableType.Vacuum:
                // Sahnedeki tüm XP tohumlarını merkeze (oyuncuya) mıknatıs gibi çek!
                Seed[] allSeeds = FindObjectsByType<Seed>(FindObjectsSortMode.None);
                foreach (Seed s in allSeeds)
                {
                    if (s.gameObject.activeInHierarchy)
                    {
                        s.Magnetize(); // Işınlanmak yerine mıknatıs gibi oyuncuya uçacaklar!
                    }
                }
                Debug.Log("Vakum (Vacuum) Alındı! Tüm XP'ler çekiliyor.");
                break;
                
            case ConsumableType.CoreSeed:
                // Boss'un düşürdüğü Çekirdek alındı: OYUN BİTER VE KAZANILIR!
                Debug.Log("✨ KULE ÇEKİRDEĞİ ALINDI! ZAFER!");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.towerCoreCount++;
                }
                if (CombatUIManager.Instance != null)
                {
                    CombatUIManager.Instance.TriggerVictory(0.5f); // 0.5 saniye sonra ekran açılır
                }
                break;
        }
        
        // Objeyi yok et (Kullanıldı)
        Destroy(gameObject);
    }
}
