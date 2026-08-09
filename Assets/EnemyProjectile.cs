using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Transform playerTarget;
    private float speed;
    private float damage;
    
    private Vector3 moveDirection; // Merminin gideceği sabit yön
    private float lifeTimer; // Merminin havada kalma süresi
    private float maxLifeTime = 5f; // 5 saniye sonra kaybolsun (Sonsuza kadar uçup oyunu kastırmasın)
    
    private float hitDistanceSq = 0.5f * 0.5f;

    public void Fire(Transform target, float projSpeed, float projDamage)
    {
        playerTarget = target;
        speed = projSpeed;
        damage = projDamage;
        
        // Ateş edildiği andaki yönü bir kere hesapla ve KİLİTLE (Güdümlü olmaması için)
        // Yüksekliğini de Sylva'nın boyuna sabitleyebiliriz ki yere çarpmasın
        Vector3 targetPos = target.position;
        moveDirection = (targetPos - transform.position).normalized;
        
        lifeTimer = 0f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        // 1. Ömür Kontrolü: 5 saniye geçtiyse mermiyi havuza geri gönder (Boşa uçmasın)
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifeTime)
        {
            gameObject.SetActive(false);
            return;
        }

        // 2. Düz bir çizgide (sabit yönde) uç
        transform.position += moveDirection * speed * Time.deltaTime;

        // 3. Çarpışma Kontrolü (Sadece PlayerTarget varsa)
        if (playerTarget != null)
        {
            float sqrDistance = (transform.position - playerTarget.position).sqrMagnitude;
            if (sqrDistance <= hitDistanceSq)
            {
                PlayerHealth health = playerTarget.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
