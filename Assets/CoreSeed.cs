using UnityEngine;

public class CoreSeed : MonoBehaviour
{
    [Header("Toplama Ayarları")]
    public float magnetRadius = 3f; // Ne kadar yakından çekilmeye başlayacak
    public float moveSpeed = 5f;    // Çekilirkenki hızı
    private Transform playerTarget;
    private bool isAttracted = false;

    public static System.Action OnCoreSeedCollected; // UI'ı haberdar etmek için Event

    void OnEnable()
    {
        isAttracted = false;
        playerTarget = null;
    }

    void Update()
    {
        // Eğer çekilme işlemi başladıysa oyuncuya doğru git
        if (isAttracted && playerTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
            
            // Oyuncuya çok yaklaştıysa topla
            if (Vector3.Distance(transform.position, playerTarget.position) < 0.5f)
            {
                Collect();
            }
        }
        else
        {
            float currentMagnetRadius = magnetRadius;
            if (PlayerPassives.Instance != null)
            {
                currentMagnetRadius = PlayerPassives.Instance.GetMagnetRadius();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, currentMagnetRadius);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    playerTarget = col.transform;
                    isAttracted = true;
                    break;
                }
            }
        }
    }

    // GameManager olmadan test yaparken kullanılacak geçici sayaç
    public static int fallbackRunCount = 0;

    void Collect()
    {
        int amount = 1;
        if (GameManager.Instance != null)
        {
            float multiplier = GameManager.Instance.VitalSeedBonusMultiplier * GameManager.Instance.SunshineCurseRewardMultiplier;
            amount = Mathf.Max(1, Mathf.RoundToInt(amount * multiplier));
            GameManager.Instance.AddCoreSeed(amount);
        }
        else
        {
            // GameManager yoksa (Test sahneleri için) statik sayacı kullan
            fallbackRunCount += 1;
        }
        
        // UI'a haber ver
        OnCoreSeedCollected?.Invoke();

        // Konsola bilgi yazdır
        Debug.Log("Core Seed Toplandı!");

        // Havuza geri gönder
        gameObject.SetActive(false);
    }
}
