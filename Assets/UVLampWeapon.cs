using UnityEngine;

public class UVLampWeapon : MonoBehaviour
{
    [Header("UV Lamba Ayarları")]
    public int currentLevel = 1;

    public float damagePerTick = 10f;
    public float damageRadius = 2.5f;
    public float tickRate = 1.0f; // Başlangıçta saniyede 1

    [Header("Görsel Ayarlar")]
    public Transform auraVisual;
    
    [Tooltip("Oyun başlamadan önce ayarladığınız o mükemmel boyut (Örn: 0.06)")]
    public float baseVisualScale = 0.06f; 

    private float nextTickTime;

    void Start()
    {
        UpdateVisualSize();
    }

    void Update()
    {
        if (!enabled) return;

        if (Time.time >= nextTickTime)
        {
            nextTickTime = Time.time + tickRate;
            DealDamageInAura();
        }
    }

    void DealDamageInAura()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float sqrRadius = damageRadius * damageRadius;

        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeInHierarchy) continue;

            float sqrDistance = (transform.position - enemyObj.transform.position).sqrMagnitude;

            if (sqrDistance <= sqrRadius)
            {
                Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Global hasar çarpanı eklendi
                    float finalDamage = damagePerTick * UpgradeManager.Instance.globalDamageMultiplier;
                    enemyScript.TakeDamage(finalDamage);

                    // MAX Level (8) ise %30 Yavaşlatma (Slow) uygula
                    if (currentLevel >= 8)
                    {
                        enemyScript.ApplySlow(30f, tickRate + 0.1f);
                    }
                }
            }
        }
    }

    void UpdateVisualSize()
    {
        if (auraVisual != null)
        {
            // Başlangıç yarıçapımız 2.5'ti. Mevcut yarıçapın ona oranını buluyoruz.
            float scaleRatio = damageRadius / 2.5f;
            
            // Senin bulduğun o mükemmel boyutu (0.06) bu oranla çarpıyoruz.
            float finalScale = baseVisualScale * scaleRatio;
            
            // X ve Y eksenlerine senin bulduğun o kusursuz değeri veriyoruz
            auraVisual.localScale = new Vector3(finalScale, finalScale, 1f);

            // Ayak altında titrememesi için milimetrik yükseklik
            auraVisual.localPosition = new Vector3(0f, 0.05f, 0f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    // --- UPGRADE SİSTEMİ ÇAĞRILARI ---

    public void UnlockWeapon()
    {
        enabled = true;
        currentLevel = 1;
        if (auraVisual != null) auraVisual.gameObject.SetActive(true);
        UpdateVisualSize();
        Debug.Log("UV Lamba Açıldı!");
    }

    public void LevelUpWeapon()
    {
        if (currentLevel >= 8) return;

        currentLevel++;
        PlayerHealth playerHealth = UpgradeManager.Instance.playerHealth;

        switch (currentLevel)
        {
            case 2:
                damagePerTick *= 1.25f; // Hasar +%25
                break;
            case 3:
                tickRate = 0.66f; // Saniyede 1.5 vuruş
                if (playerHealth != null) playerHealth.healthRegenPerSecond += 0.5f; // SİNERJİ
                break;
            case 4:
                damageRadius *= 1.5f; // Önceden 2 katıydı, şimdi 1.5 katına düşürüldü (Dengeleme)
                UpdateVisualSize();
                break;
            case 5:
                damagePerTick *= 1.30f; // Hasar +%30
                break;
            case 6:
                damageRadius *= 1.20f; // Önceden %25 artıyordu, şimdi %20 artıyor (Dengeleme)
                UpdateVisualSize();
                if (playerHealth != null) playerHealth.healthRegenPerSecond += 1f; // SİNERJİ
                break;
            case 7:
                tickRate = 0.5f; // Saniyede 2 vuruş
                break;
            case 8:
                // Slow efekti DealDamageInAura içinde currentLevel >= 8 ile kontrol ediliyor
                if (playerHealth != null) playerHealth.healthRegenPerSecond += 1.5f; // SİNERJİ
                break;
        }

        Debug.Log("UV Lamba Seviye Atladı! Yeni Level: " + currentLevel);
    }
}
