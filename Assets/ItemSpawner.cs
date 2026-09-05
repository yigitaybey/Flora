using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Eşya Spawn Ayarları")]
    public GameObject itemPrefab; // Spawn edilecek iksir prefabı
    
    [Tooltip("Kaç saniyede bir yeni eşya spawn olacak?")]
    public float spawnInterval = 30f; 
    
    [Tooltip("Karakterden ne kadar uzakta oluşacaklar? (Minimum)")]
    public float minSpawnRadius = 10f;
    
    [Tooltip("Karakterden ne kadar uzakta oluşacaklar? (Maksimum)")]
    public float maxSpawnRadius = 25f;

    private Transform playerTransform;
    private float nextSpawnTime;

    void Start()
    {
        // Oyuncuyu bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // İlk spawn zamanını belirle
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // Oyuncu yoksa veya zaman gelmediyse bekle
        if (playerTransform == null || Time.time < nextSpawnTime) return;

        // Zamanı geldi, eşya spawn et!
        nextSpawnTime = Time.time + spawnInterval;
        SpawnItem();
    }

    void SpawnItem()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("ItemSpawner'a bir Prefab koymamışsın kankam!");
            return;
        }

        // Oyuncunun etrafında rastgele bir açı ve uzaklık (yarıçap) belirle
        float randomAngle = Random.Range(0f, Mathf.PI * 2f); // Çemberin etrafında 360 derece
        float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Sin ve Cos kullanarak X ve Z ekseninde oyuncunun etrafında nokta bul
        float spawnX = playerTransform.position.x + Mathf.Cos(randomAngle) * randomRadius;
        float spawnZ = playerTransform.position.z + Mathf.Sin(randomAngle) * randomRadius;

        Vector3 randomPosition = new Vector3(spawnX, playerTransform.position.y, spawnZ);

        // NavMesh üzerinde geçerli bir nokta mı kontrol et
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
        {
            // Objeyi sahneye yerleştir (hit.position Y eksenini zemine oturtur, bu yüzden biraz yukarı kaldırıyoruz)
            // Quaternion.identity yerine prefabın kendi rotasyonunu kullanıyoruz ki dik dursun!
            Instantiate(itemPrefab, hit.position + Vector3.up * 0.84f, itemPrefab.transform.rotation);
        }
    }
}
