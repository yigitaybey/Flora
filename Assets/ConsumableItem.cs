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
        if (playerTransform == null) return;

        // Eşya ile oyuncu arasındaki mesafeyi ölç (Unity Physics/Collider kullanmadan)
        float sqrDistance = (transform.position - playerTransform.position).sqrMagnitude;
        
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
                        // 9999 Hasar vurarak öldür
                        if (eScript != null) eScript.TakeDamage(9999f); 
                    }
                }
                Debug.Log("Güneş Işığı (Screen Wipe) Alındı! Tüm düşmanlar yok edildi.");
                break;
                
            case ConsumableType.Vacuum:
                // Sahnedeki tüm XP tohumlarını merkeze (oyuncuya) çek!
                Seed[] allSeeds = FindObjectsByType<Seed>(FindObjectsSortMode.None);
                foreach (Seed s in allSeeds)
                {
                    if (s.gameObject.activeInHierarchy)
                    {
                        // Seed.cs'teki Update mantığı sayesinde anında toplanacak
                        s.transform.position = playerTransform.position; 
                    }
                }
                Debug.Log("Vakum (Vacuum) Alındı! Tüm XP'ler çekildi.");
                break;
        }
        
        // Objeyi yok et (Kullanıldı)
        Destroy(gameObject);
    }
}
