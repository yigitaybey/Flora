using UnityEngine;

public class PlayerPassives : MonoBehaviour
{
    public static PlayerPassives Instance;

    [Header("Pasif Eşya Seviyeleri (0 = Yok, 8 = Max)")]
    public int magnetLevel = 0;
    public int luckLevel = 0;
    public int curseLevel = 0;
    public int turretLevel = 0;
    public int critLevel = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- MIKNATIS (MAGNET) ---
    // Tohum çekme mesafesini hesaplar. Base: 2.5f. Max Lvl 8'de: 5.0f (2 Katı).
    public float GetMagnetRadius()
    {
        float baseRadius = 2.5f;
        if (magnetLevel == 0) return baseRadius;

        // Her seviye %12.5 artırır (8 * 12.5 = %100 = 2x Katı)
        float multiplier = 1f + (magnetLevel * 0.125f);
        return baseRadius * multiplier;
    }

    // --- ŞANS (LUCK) ---
    // Consumable eşyaların (İksir, Güneş, Vakum) düşme ihtimali
    public float GetConsumableDropChance()
    {
        float baseChance = 0.005f; // %0.5 Temel İhtimal (Eski haline göre %50 düşürüldü)
        if (luckLevel == 0) return baseChance;

        // Her seviye %0.25 ekler (Max %2.5 İhtimal)
        return baseChance + (luckLevel * 0.0025f); 
    }

    // --- LANET (CURSE) ---
    // Düşman sayısını ve Spawn Hızını artırır
    public float GetCurseMultiplier()
    {
        if (curseLevel == 0) return 1f;
        // Her seviye %10 artırır (Max %80 daha fazla ve hızlı düşman)
        return 1f + (curseLevel * 0.10f);
    }

    // Düşen XP Tohumlarının değerini artırır (Risk/Ödül)
    public float GetCurseXPMultiplier()
    {
        if (curseLevel == 0) return 1f;
        // Her seviye %15 XP artırır (Max %120 Daha fazla XP)
        return 1f + (curseLevel * 0.15f);
    }

    // --- KÖK TARET (TURRET) ---
    // Düşman ölünce taret çıkma ihtimali
    public float GetTurretSpawnChance()
    {
        if (turretLevel == 0) return 0f;
        
        // Base %2. Her seviye %1 ekler. Max %10.
        float chance = 0.02f + (turretLevel * 0.01f);
        
        // Şans (Luck) statı doğrudan bu ihtimali katlar
        if (luckLevel > 0)
        {
            chance *= (1f + (luckLevel * 0.1f)); 
        }
        
        return chance;
    }

    public float GetTurretDuration()
    {
        // Temel 3 saniye, her seviye +1 saniye
        return 3f + (turretLevel * 1f); 
    }

    public float GetTurretDamage()
    {
        // Temel hasar 10. Her seviye +5 hasar.
        return 10f + (turretLevel * 5f);
    }

    // --- ÖLÜMCÜL ODAK (CRIT) ---
    // Kritik vurma ihtimali
    public float GetCritChance()
    {
        if (critLevel == 0) return 0f;
        // Her seviye %5 (Max Lvl 8 = %40)
        return critLevel * 0.05f; 
    }

    // Kritik vurunca katlanacak hasar çarpanı
    public float GetCritMultiplier()
    {
        if (critLevel == 0) return 1f; // Base hasar
        // Her seviye +0.125x ekler (Lvl 8 = 2.0x Hasar)
        return 1f + (critLevel * 0.125f);
    }
}
