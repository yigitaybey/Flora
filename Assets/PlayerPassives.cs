using UnityEngine;

public class PlayerPassives : MonoBehaviour
{
    public static PlayerPassives Instance;

    [Header("Pasif Statlar")]
    public int magnetLevel = 0;  // Oyun içi artık yükseltilmez, sadece Atölye kalıcı bonus için
    public int curseLevel = 0;   // Oyun içi artık yükseltilmez, sadece Atölye kalıcı bonus için

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- MIKNATIS (MAGNET) ---
    // Tohum çekme mesafesini hesaplar. Base: 2.5f.
    public float GetMagnetRadius()
    {
        float baseRadius = 2.5f;
        if (GameManager.Instance != null)
        {
            baseRadius *= GameManager.Instance.MagnetBonusRadiusMultiplier;
        }

        if (magnetLevel == 0) return baseRadius;

        // Her seviye %12.5 artırır
        float multiplier = 1f + (magnetLevel * 0.125f);
        return baseRadius * multiplier;
    }

    // --- ŞANS (LUCK) ---
    // Consumable eşyaların (İksir, Güneş, Vakum) düşme ihtimali
    public float GetConsumableDropChance()
    {
        float baseChance = 0.006f;
        return baseChance;
    }

    // --- LANET (CURSE) ---
    // Düşman sayısını ve Spawn Hızını artırır (Sunshine Curse dahil)
    public float GetCurseMultiplier()
    {
        float permCurse = (GameManager.Instance != null) ? GameManager.Instance.SunshineCurseSpeedMultiplier : 1f;
        if (curseLevel == 0) return permCurse;
        return permCurse + (curseLevel * 0.10f);
    }

    // Düşen XP Tohumlarının değerini artırır (Sunshine Curse dahil)
    public float GetCurseXPMultiplier()
    {
        float permReward = (GameManager.Instance != null) ? GameManager.Instance.SunshineCurseRewardMultiplier : 1f;
        if (curseLevel == 0) return permReward;
        return permReward + (curseLevel * 0.15f);
    }

    // --- DEVRE DIŞI PASİFLER (Derleme uyumluluğu için sıfır döndürür) ---
    public float GetTurretSpawnChance() => 0f;
    public float GetTurretDuration() => 3f;
    public float GetTurretDamage() => 10f;
    public float GetCritChance() => 0f;
    public float GetCritMultiplier() => 1f;
}
