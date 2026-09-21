using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Ayarları")]
    public Transform playerTarget;
    public float baseSPS = 1.32f; // Saniyede doğan temel düşman sayısı (%20 artırıldı: 1.1 -> 1.32)
    public float spawnRadius = 25f;
    
    [Header("FPS Koruması")]
    public int maxEnemiesOnScreen = 120; // Mobil performans ve adil oynanış için 120 sınırı

    [Header("Debug & Test Zaman Kontrolleri")]
    [Tooltip("Oyun hız çarpanı (1 = Normal, 2 = 2x Hızlı, 5 = 5x Hızlı)")]
    [Range(0.5f, 10f)] public float gameSpeedMultiplier = 1f;

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
        Time.timeScale = gameSpeedMultiplier;
    }

    // Sahadaki tüm normal düşmanları temizler (Boss savaşı öncesi)
    public void ClearAllEnemies()
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

    // --- TEST / DEBUG KISAYOLLARI ---
    void CheckDebugInputs()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            // 'B' Tuşu: Direkt BOSS Dalgasına (Wave 10 - 270. saniyeye) Atla!
            if (UnityEngine.InputSystem.Keyboard.current.bKey.wasPressedThisFrame)
            {
                JumpToBoss();
            }

            // 'T' Tuşu: Süreyi +60 saniye (2 Dalga) İleri Sar
            if (UnityEngine.InputSystem.Keyboard.current.tKey.wasPressedThisFrame)
            {
                AddSeconds(60f);
            }

            // '1', '2', '5' Tuşları: Oyun Hızını Değiştir (1x, 2x, 5x Hızlı Oyna)
            if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame) SetGameSpeed(1f);
            if (UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame) SetGameSpeed(2f);
            if (UnityEngine.InputSystem.Keyboard.current.digit5Key.wasPressedThisFrame) SetGameSpeed(5f);
        }
    }

    public void JumpToBoss()
    {
        GameTimer = 270f; // 4.5 dakika -> Wave 10 Final Boss
        Debug.Log("⏩ DEBUG: Direkt Wave 10 (BOSS CHINAR) Dalgasına Atlandı!");
    }

    public void AddSeconds(float seconds)
    {
        GameTimer += seconds;
        Debug.Log($"⏩ DEBUG: Süre +{seconds}sn ileri sarıldı. Yeni Süre: {GameTimer:F0}sn | Wave: {CurrentWave}");
    }

    public void SetGameSpeed(float speed)
    {
        gameSpeedMultiplier = speed;
        Time.timeScale = speed;
        Debug.Log($"⚡ DEBUG: Oyun Hızı {speed}x yapıldı!");
    }

    void Update()
    {
        CheckDebugInputs();

        // Inspector'dan hız değiştirildiyse uygula
        if (Time.timeScale != 0f && Time.timeScale != gameSpeedMultiplier)
        {
            Time.timeScale = gameSpeedMultiplier;
        }

        if (playerTarget == null) return;

        // Oyun süresini say (Her 30 saniye = 1 Dalga/Wave)
        // 10 Dalga x 30 saniye = 300 saniye (5 Dakika Toplam Run)
        GameTimer += Time.deltaTime;
        CurrentWave = Mathf.FloorToInt(GameTimer / 30f) + 1; // 0-29sn = Wave 1, ..., 270sn+ = Wave 10 (Boss)

        // --- BOSS KONTROLÜ (5. Dakika / Wave 10) ---
        if (CurrentWave >= 10)
        {
            if (!bossSpawned)
            {
                Debug.Log("WAVE 10 (FINAL BOSS)! SAHA TEMİZLENİYOR, CHINAR GELİYOR!");
                ClearAllEnemies();
                SpawnEnemy(EnemyType.Chinar);
                bossSpawned = true;
            }
            return; // Boss indikten sonra normal düşman doğurmayı durdur
        }

        // --- ELITE KONTROLÜ (Wave 5: 2.5 Dakika Ara Kontrol Noktası) ---
        if (CurrentWave == 5 && lastSpawnedEliteWave != 5)
        {
            EnemyType randomEliteType = DetermineEnemyTypeByWave();
            SpawnEnemy(randomEliteType, true); // true = isElite
            lastSpawnedEliteWave = 5;
            Debug.Log("ELITE MİNİ-BOSS İNDİ! Wave: " + CurrentWave);
        }

        // --- FPS HARD CAP (LIMIT) KONTROLÜ ---
        if (EnemyPool.Instance != null && EnemyPool.Instance.GetActiveEnemyCount() >= maxEnemiesOnScreen)
        {
            return; // Sınır aşıldıysa yeni düşman doğurma (Sadece süre akar)
        }

        // Anlık SPS (Saniyede doğan düşman) hesaplaması (Dengeli üstel artış: %12 her wave)
        float sps = baseSPS * Mathf.Pow(1.12f, CurrentWave);
        
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
        // Wave 1-2 (0-60s): Sadece temel Moss (Yeni başlayan oyuncu rahatça tohum toplar)
        if (CurrentWave <= 2)
        {
            return EnemyType.Moss;
        }
        // Wave 3-4 (60-120s): Moss (%70) ve SporeHead (%30)
        else if (CurrentWave <= 4)
        {
            return (Random.value < 0.7f) ? EnemyType.Moss : EnemyType.SporeHead;
        }
        // Wave 5 (120-150s): Mid-run wave (Moss %60 + SporeHead %40 + Garantili Elite)
        else if (CurrentWave == 5)
        {
            return (Random.value < 0.6f) ? EnemyType.Moss : EnemyType.SporeHead;
        }
        // Wave 6-7 (150-210s): Menzilli Iyv savaşa katılır! (Moss %50, SporeHead %30, Iyv %20)
        else if (CurrentWave <= 7)
        {
            float roll = Random.value;
            if (roll < 0.50f) return EnemyType.Moss;
            if (roll < 0.80f) return EnemyType.SporeHead;
            return EnemyType.Iyv;
        }
        // Wave 8-9 (210-270s): 3 tür birden yoğun saldırır (Kaos & Boss öncesi zirve!)
        else
        {
            float roll = Random.value;
            if (roll < 0.40f) return EnemyType.Moss;
            if (roll < 0.70f) return EnemyType.SporeHead;
            return EnemyType.Iyv;
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
