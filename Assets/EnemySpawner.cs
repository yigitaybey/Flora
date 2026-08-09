using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Ayarları")]
    public Transform playerTarget;
    public float baseSPS = 1f; // Saniyede doğan temel düşman sayısı
    public float spawnRadius = 25f;
    
    [Header("FPS Koruması")]
    public int maxEnemiesOnScreen = 200; // Ekranda aynı anda en fazla kaç düşman olabilir

    // UI (Ekran) üzerinden okuyabilmek için public static yaptık
    public static float GameTimer { get; private set; } = 0f;
    
    // Anlık olarak hangi dalgada olduğumuzu tutar
    public static int CurrentWave { get; private set; } = 1;
    
    private int lastSpawnedEliteWave = 0; // Aynı dalgada 2 kere elite doğmaması için

    private float currentSpawnInterval = 1f;
    private float nextSpawnTime;
    
    private bool bossSpawned = false;

    void Start()
    {
        GameTimer = 0f;
        CurrentWave = 1;
        lastSpawnedEliteWave = 0;
        bossSpawned = false;
    }

    // Sahadaki tüm normal düşmanları temizler (Boss savaşı öncesi)
    void ClearAllEnemies()
    {
        if (EnemyPool.Instance == null) return;
        
        List<GameObject> activeEnemies = EnemyPool.Instance.GetAllActiveEnemies();
        foreach (GameObject obj in activeEnemies)
        {
            Enemy e = obj.GetComponent<Enemy>();
            if (e != null && e.enemyType != EnemyType.Chinar)
            {
                obj.SetActive(false); // Havuza geri yolla
            }
        }
    }

    void Update()
    {
        if (playerTarget == null) return;

        // Oyun süresini say (Her 30 saniye = 1 Dalga/Wave)
        GameTimer += Time.deltaTime;
        CurrentWave = Mathf.FloorToInt(GameTimer / 30f) + 1; // 0-29sn = Wave 1, 30-59sn = Wave 2

        // --- BOSS KONTROLÜ (10. Dakika / Wave 20) ---
        if (CurrentWave >= 20)
        {
            if (!bossSpawned)
            {
                Debug.Log("WAVE 20! SAHA TEMİZLENİYOR, CHINAR GELİYOR!");
                ClearAllEnemies();
                SpawnEnemy(EnemyType.Chinar);
                bossSpawned = true;
            }
            return; // Boss indikten sonra normal düşman doğurmayı durdur
        }

        // --- ELITE KONTROLÜ (Wave 5, 10, 15) ---
        if (CurrentWave % 5 == 0 && CurrentWave != lastSpawnedEliteWave)
        {
            EnemyType randomEliteType = DetermineEnemyTypeByWave();
            SpawnEnemy(randomEliteType, true); // true = isElite
            lastSpawnedEliteWave = CurrentWave;
            Debug.Log("ELITE DÜŞMAN İNDİ! Wave: " + CurrentWave);
        }

        // --- FPS HARD CAP (LIMIT) KONTROLÜ ---
        if (EnemyPool.Instance != null && EnemyPool.Instance.GetActiveEnemyCount() >= maxEnemiesOnScreen)
        {
            return; // Sınır aşıldıysa yeni düşman doğurma (Sadece süre akar)
        }

        // Anlık SPS (Saniyede doğan düşman) hesaplaması (Level yerine Wave kullanıyoruz)
        float sps = baseSPS * Mathf.Pow(1.10f, CurrentWave);
        
        // Lanet (Curse) çarpanını uygula (Daha fazla ve hızlı düşman)
        if (PlayerPassives.Instance != null)
        {
            sps *= PlayerPassives.Instance.GetCurseMultiplier();
        }

        currentSpawnInterval = 1f / sps;

        // Normal düşman doğurma döngüsü
        if (Time.time >= nextSpawnTime)
        {
            EnemyType enemyToSpawn = DetermineEnemyTypeByWave();
            SpawnEnemy(enemyToSpawn);
            
            nextSpawnTime = Time.time + currentSpawnInterval;
        }
    }

    EnemyType DetermineEnemyTypeByWave()
    {
        if (CurrentWave >= 10 && CurrentWave < 20)
        {
            // Wave 10-19: Moss, SporeHead, Wolfey, Iyv karışık
            int rnd = Random.Range(0, 4); 
            if (rnd == 0) return EnemyType.Moss;
            if (rnd == 1) return EnemyType.SporeHead;
            if (rnd == 2) return EnemyType.Wolfey;
            return EnemyType.Iyv;
        }
        else if (CurrentWave >= 5 && CurrentWave < 10)
        {
            // Wave 5-9: Moss ve SporeHead
            int rnd = Random.Range(0, 2);
            return rnd == 0 ? EnemyType.Moss : EnemyType.SporeHead;
        }
        else
        {
            // Wave 1-4: Sadece Moss
            return EnemyType.Moss;
        }
    }

    void SpawnEnemy(EnemyType type, bool isElite = false)
    {
        if (EnemyPool.Instance == null) return;

        // İstenilen tipte kapalı bir düşmanı al
        GameObject enemyObj = EnemyPool.Instance.GetEnemy(type);
        
        if (enemyObj != null)
        {
            // Oyuncunun etrafında rastgele bir pozisyon hesapla
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 randomPosition = playerTarget.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // NavMesh üzerinde geçerli bir nokta mı kontrol et
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 2f, NavMesh.AllAreas))
            {
                enemyObj.transform.position = hit.position;
                enemyObj.SetActive(true);

                Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Spawn(playerTarget, isElite);
                }
            }
            else
            {
                Debug.LogWarning("Zemin çok küçük! Düşman havada doğamadı. Lütfen Plane (zemin) boyutunu büyütün.");
            }
        }
    }
}
