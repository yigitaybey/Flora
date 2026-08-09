using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Tohum Ayarları")]
    public float baseXpAmount = 10f; // Temel XP (Curse ile çarpılacak)
    public float magnetSpeed = 8f; // Çekilme hızı
    
    private Transform playerTransform;
    private bool isMagnetized = false;
    private float currentXpAmount; // Düşman öldüğünde o anki Curse seviyesine göre hesaplanacak

    void OnEnable()
    {
        isMagnetized = false; 
        
        // Spawn olduğu andaki Curse (Lanet) seviyesini alıp tohumun XP değerine yazarız
        if (PlayerPassives.Instance != null)
        {
            currentXpAmount = baseXpAmount * PlayerPassives.Instance.GetCurseXPMultiplier();
        }
        else
        {
            currentXpAmount = baseXpAmount;
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("DİKKAT: Sahnede 'Player' etiketli (Tag) bir obje bulunamadı!");
            }
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float sqrDistance = (transform.position - playerTransform.position).sqrMagnitude;
        
        float currentMagnetRadius = 3f; // Default
        if (PlayerPassives.Instance != null)
        {
            currentMagnetRadius = PlayerPassives.Instance.GetMagnetRadius();
        }

        // 1. Mıknatıs alanına girdiyse çekilmeye başla
        if (!isMagnetized && sqrDistance <= (currentMagnetRadius * currentMagnetRadius))
        {
            isMagnetized = true;
        }

        // 2. Eğer çekiliyorsa, oyuncuya doğru uç
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);

            // 3. Yeterince yaklaştıysa topla ve XP ver
            if (sqrDistance <= 2.5f) 
            {
                if (ExperienceManager.Instance != null)
                {
                    ExperienceManager.Instance.AddExperience(currentXpAmount);
                }
                gameObject.SetActive(false); // Havuza (Pool) geri dön!
            }
        }
    }
}
