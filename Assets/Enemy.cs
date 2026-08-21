using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Moss,
    SporeHead,
    Iyv,
    Wolfey,
    Chinar // Boss
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.Moss;
    
    // Normal düşmanların buff'lanmış (Büyümüş/Güçlenmiş) Elite versiyonu mu?
    public bool isElite = false;

    private Transform playerTarget;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;
    private Vector3 initialScale;

    [Header("Düşman Ayarları")]
    public float baseMaxHealth = 25f; // Başlangıç canı, level ile artacak (50'den 25'e düşürüldü)
    private float currentMaxHealth; // O anki levela göre hesaplanmış max can
    private float currentHealth;

    [Header("Sabit Statlar (Level İle Artmaz)")]
    public float moveSpeed = 3.2f; // Hız (3.5'ten 3.2'ye düşürüldü)
    public float baseDamage = 8f; // Temel Hasar (10'dan 8'e düşürüldü)
    private float currentDamage; // Hasar da artık Wave ile artacak
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private float lastAttackTime;

    private bool isExploding = false; // SporeHead'in birden fazla kez patlamasını engellemek için

    [Header("Efektler (Debuffs)")]
    private float defaultSpeed;
    private float slowEndTime;
    private float burnEndTime;
    private float burnDamagePerSecond;
    private float nextBurnTick;

    [Header("Ödül Ayarları")]
    public float coreSeedDropChance = 5f; // Yüzde 5 ihtimalle Core Seed düşürsün

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 20f;
        agent.angularSpeed = 360f;
        defaultSpeed = moveSpeed;
        agent.speed = defaultSpeed; // Hız statik ve level ile artmıyor

        initialScale = transform.localScale; // Editor'de ayarlanan boyutu kaydet
    }

    public void Spawn(Transform target, bool makeElite = false)
    {
        isElite = makeElite;
        isExploding = false; // Yeniden doğduğunda patlama durumunu sıfırla

        // Düşman canını artık oyuncunun Level'ına değil, bulunulan Dalga (Wave) sayısına göre artırıyoruz
        int currentWave = EnemySpawner.CurrentWave;
        
        // Boss için özel devasa can havuzu
        if (enemyType == EnemyType.Chinar)
        {
            currentMaxHealth = 2000f * Mathf.Pow(1.15f, currentWave);
        }
        else
        {
            currentMaxHealth = baseMaxHealth * Mathf.Pow(1.15f, currentWave);
        }
        
        // Eğer Elite ise canı 2 katına çıkar (Önceden 3 kattı, çok dayanıklı olduğu için 2'ye çektim) ve boyutunu büyüt!
        if (isElite)
        {
            currentMaxHealth *= 2f;
            transform.localScale = initialScale * 1.5f; // %50 daha büyük
        }
        else
        {
            transform.localScale = initialScale;
        }

        currentHealth = currentMaxHealth; // Doğduğunda canı fulle
        
        // Hasarı da wave'e göre artır (Her wave %5 daha fazla hasar)
        currentDamage = baseDamage * (1f + (currentWave * 0.05f));
        if (isElite) currentDamage *= 1.5f; // Elite'ler daha çok vurur
        
        playerTarget = target;
        playerHealth = target.GetComponent<PlayerHealth>();
        
        // --- YAPAY ZEKA DAVRANIŞINA GÖRE DURMA MESAFESİ (Stopping Distance) AYARLARI ---
        switch (enemyType)
        {
            case EnemyType.Iyv:
                agent.stoppingDistance = 8.0f; // Menzilli düşman uzakta durur
                break;
            case EnemyType.Chinar:
                agent.stoppingDistance = 2.8f; // Boss biraz daha geride durabilir
                break;
            case EnemyType.SporeHead:
                agent.stoppingDistance = 0.5f; // Kamikaze dibine kadar girmeli
                break;
            default:
                agent.stoppingDistance = attackRange - 0.2f; // Moss, Wolfey vb.
                break;
        }

        agent.enabled = true; // Object pool'dan çıkınca aktif et
        lastAttackTime = Time.time;
    }

    void Update()
    {
        if (playerTarget == null || !agent.enabled) return;

        // Hedefi takip et (Ranged olan Iyv bile takip eder ama 8.0 birim kala durur)
        agent.SetDestination(playerTarget.position);

        float sqrDistance = (transform.position - playerTarget.position).sqrMagnitude;
        
        // --- YAPAY ZEKA DAVRANIŞ (BEHAVIOR) KONTROLLERİ ---
        switch (enemyType)
        {
            case EnemyType.Moss:
            case EnemyType.Wolfey:
                // Standart ve Hızlı Yakın Dövüş (Melee)
                if (sqrDistance <= (attackRange * attackRange))
                {
                    if (Time.time >= lastAttackTime + attackCooldown)
                    {
                        Attack();
                    }
                }
                break;

            case EnemyType.SporeHead:
                // Kamikaze: 2.0 birim yakına girince tetiklenir
                if (sqrDistance <= (2.0f * 2.0f))
                {
                    if (!isExploding)
                    {
                        isExploding = true;
                        Invoke(nameof(Explode), 0.2f); // 0.2 saniye gecikmeli patla
                    }
                }
                break;

            case EnemyType.Iyv:
                // Menzilli (Ranged): 8.5 birim mesafedeyken (8'de durduğu için tolerans payı 0.5) ateş et
                if (sqrDistance <= (8.5f * 8.5f))
                {
                    if (Time.time >= lastAttackTime + attackCooldown)
                    {
                        Attack();
                    }
                }
                break;

            case EnemyType.Chinar:
                // Hibrit Boss: Yakındaysa tokat atar, Uzaktaysa Kazık/Mermi fırlatır
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    if (sqrDistance <= (3.0f * 3.0f))
                    {
                        // Melee attack
                        if (playerHealth != null) playerHealth.TakeDamage(currentDamage * 1.5f); // Yakın vuruşu sert olsun
                        lastAttackTime = Time.time;
                    }
                    else
                    {
                        // Ranged attack
                        FireProjectile();
                        lastAttackTime = Time.time;
                    }
                }
                break;
        }

        // --- YAVAŞLATMA (SLOW) KONTROLÜ ---
        if (Time.time > slowEndTime && agent.speed < defaultSpeed)
        {
            agent.speed = defaultSpeed;
        }

        // --- YANMA (BURN) KONTROLÜ ---
        if (Time.time <= burnEndTime)
        {
            if (Time.time >= nextBurnTick)
            {
                nextBurnTick = Time.time + 1f;
                TakeDamage(burnDamagePerSecond);
            }
        }
    }

    void Explode()
    {
        if (playerHealth != null)
        {
            // Direkt oyuncuya hasar ver (Alan hasarı için ilerde OverlapSphere konabilir)
            playerHealth.TakeDamage(currentDamage);
        }
        
        // Kendini yok et (Tohum/XP bırakması için Die çağrılır)
        Die();
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        if (playerHealth == null) return;

        switch (enemyType)
        {
            case EnemyType.Moss:
            case EnemyType.Wolfey:
                // Yakın dövüş hasarı
                float distance = Vector3.Distance(transform.position, playerTarget.position);
                if (distance < 2.5f) 
                {
                    playerHealth.TakeDamage(currentDamage);
                }
                break;

            case EnemyType.Iyv:
                // Menzilli atış
                FireProjectile();
                break;
        }
    }

    void FireProjectile()
    {
        if (EnemyProjectilePool.Instance != null)
        {
            GameObject projObj = EnemyProjectilePool.Instance.GetProjectile();
            projObj.transform.position = transform.position;
            EnemyProjectile proj = projObj.GetComponent<EnemyProjectile>();
            if (proj != null)
            {
                proj.Fire(playerTarget, 10f, currentDamage);
            }
        }
    }

    public void ApplySlow(float percent, float duration)
    {
        agent.speed = defaultSpeed * (1f - (percent / 100f));
        slowEndTime = Time.time + duration;
    }

    public void ApplyBurn(float totalDamagePerSec, float duration)
    {
        burnDamagePerSecond = totalDamagePerSec;
        burnEndTime = Time.time + duration;
    }

    public void TakeDamage(float amount, bool canCrit = false)
    {
        bool isCrit = false;

        // Eğer silah kritik vurabiliyorsa ve şansımız tutarsa hasarı katla
        if (canCrit && PlayerPassives.Instance != null)
        {
            if (Random.value < PlayerPassives.Instance.GetCritChance())
            {
                amount *= PlayerPassives.Instance.GetCritMultiplier();
                isCrit = true;
            }
        }

        currentHealth -= amount;
        
        if (DamagePopupPool.Instance != null)
        {
            GameObject popupObj = DamagePopupPool.Instance.GetPopup();
            if (popupObj != null)
            {
                // Düşmanın üzerinden çıkarken hem yüksekliği azalttım hem de üst üste binmemesi için hafif rastgele sağ/sol yaptım
                float randomX = Random.Range(-0.3f, 0.3f);
                float randomY = Random.Range(0.6f, 1.0f);
                popupObj.transform.position = transform.position + new Vector3(randomX, randomY, 0f);
                
                DamagePopup popupScript = popupObj.GetComponent<DamagePopup>();
                if (popupScript != null)
                {
                    popupScript.Setup(amount, false, isCrit);
                }
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // 1. Tohum (XP) Düşür
        if (SeedPool.Instance != null)
        {
            GameObject seedObj = SeedPool.Instance.GetSeed();
            if (seedObj != null)
            {
                seedObj.transform.position = transform.position;
                
                Seed seedScript = seedObj.GetComponent<Seed>();
                if (seedScript != null)
                {
                    if (enemyType == EnemyType.Chinar || isElite)
                    {
                        seedScript.baseXpAmount = 50f;
                        seedObj.transform.localScale = new Vector3(2f, 2f, 2f); 
                    }
                    else
                    {
                        seedScript.baseXpAmount = 10f; 
                        seedObj.transform.localScale = Vector3.one; 
                    }
                }
                
                seedObj.SetActive(true);
            }
        }

        // 2. Core Seed (Kalıcı Güçlendirme Parası) Düşür
        float chance = isElite ? coreSeedDropChance * 5f : coreSeedDropChance; 
        if (enemyType == EnemyType.Chinar) chance = 100f; 

        if (Random.Range(0f, 100f) <= chance)
        {
            if (CoreSeedPool.Instance != null)
            {
                GameObject coreSeedObj = CoreSeedPool.Instance.GetCoreSeed();
                if (coreSeedObj != null)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                    coreSeedObj.transform.position = transform.position + offset;
                    coreSeedObj.SetActive(true);
                }
            }
        }

        // 3. Consumable (Harita Eşyası) Düşürme Şansı (Luck pasifi etkiler)
        if (PlayerPassives.Instance != null)
        {
            if (Random.value < PlayerPassives.Instance.GetConsumableDropChance())
            {
                // Sonraki adımda yazılacak Consumable scripti üzerinden çağrılacak
                if (Consumable.Instance != null) Consumable.Instance.SpawnRandom(transform.position);
            }
        }

        // 4. Kök Taret (Turret) Çıkma Şansı (Turret pasifi + Luck pasifi etkiler)
        if (PlayerPassives.Instance != null && PlayerPassives.Instance.turretLevel > 0)
        {
            if (Random.value < PlayerPassives.Instance.GetTurretSpawnChance())
            {
                // Sonraki adımda yazılacak TurretManager üzerinden çağrılacak
                if (TurretManager.Instance != null) TurretManager.Instance.SpawnTurret(transform.position);
            }
        }

        // 5. Düşmanı yok etme (Object Pooling - Havuza Geri Gönder)
        agent.enabled = false;
        gameObject.SetActive(false);
    }
}
