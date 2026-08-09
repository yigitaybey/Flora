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
            // Oyuncuyu etrafta ara (Optimizasyon için aslında Player referansını statik tutabiliriz ama MVP için OverlapSphere yeterli)
            Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRadius);
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

    void Collect()
    {
        // PlayerPrefs'e kaydet
        int currentCores = PlayerPrefs.GetInt("CoreSeedCount", 0);
        PlayerPrefs.SetInt("CoreSeedCount", currentCores + 1);
        PlayerPrefs.Save();
        
        // UI'a haber ver
        OnCoreSeedCollected?.Invoke();

        // Konsola bilgi yazdır
        Debug.Log("Core Seed Toplandı! Toplam Core Seed: " + PlayerPrefs.GetInt("CoreSeedCount"));

        // Havuza geri gönder
        gameObject.SetActive(false);
    }
}
