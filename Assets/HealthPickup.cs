using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Can İksiri Ayarları")]
    public float healAmount = 30f; // İksir kaç can verecek?
    public float magnetRadius = 4f; // Kaç metre yakınına gelince oyuncuya çekilsin?
    public float magnetSpeed = 10f; // Oyuncuya doğru uçma hızı

    private Transform playerTransform;
    private bool isMagnetized = false;

    void OnEnable()
    {
        isMagnetized = false;
        
        // Sylva'yı (Player) sahnede bul
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // İksir ile oyuncu arasındaki mesafeyi ölç (Optimizasyon için karesini kullanıyoruz)
        float sqrDistance = (transform.position - playerTransform.position).sqrMagnitude;

        // 1. Oyuncu yeterince yaklaştıysa mıknatıs modunu aç
        if (!isMagnetized && sqrDistance <= (magnetRadius * magnetRadius))
        {
            isMagnetized = true;
        }

        // 2. Mıknatıs modu açıksa oyuncuya doğru uç
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);

            // 3. Oyuncuya değdiyse canı ver ve kendini yok et
            if (sqrDistance <= 2.5f) // Yaklaşık 1.5 metre (oyuncunun boyundan dolayı pay bıraktık)
            {
                PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(healAmount);
                }
                
                // Obje sahnede kalabalık yapmasın diye silinir
                Destroy(gameObject);
            }
        }
    }
}
