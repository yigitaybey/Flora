using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum UpgradeType
{
    // Silahlar
    UnlockPollen, UpgradePollen,
    UnlockUV, UpgradeUV,
    UnlockAxe, UpgradeAxe,
    UnlockFlamethrower, UpgradeFlamethrower,
    
    // Pasifler
    UpgradeMagnet,
    UpgradeLuck,
    UpgradeCurse,
    UpgradeTurret,
    UpgradeCrit,
    
    // Tekrarlanabilir / Yedek Yetenek
    HealPotion
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("UI Bağlantıları (3 Butonun Text'i)")]
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;

    [Header("Oyuncu Scriptleri")]
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public PollenWeapon pollenWeapon;
    public UVLampWeapon uvWeapon;
    public AxeWeapon axeWeapon;
    public FlamethrowerWeapon flamethrowerWeapon;

    [Header("Global Sinerji")]
    public float globalDamageMultiplier = 1.0f; 

    private UpgradeType[] currentChoices = new UpgradeType[3];

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerMovement == null) playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (pollenWeapon == null) pollenWeapon = FindFirstObjectByType<PollenWeapon>();
        if (uvWeapon == null) uvWeapon = FindFirstObjectByType<UVLampWeapon>();
        if (axeWeapon == null) axeWeapon = FindFirstObjectByType<AxeWeapon>();
        if (flamethrowerWeapon == null) flamethrowerWeapon = FindFirstObjectByType<FlamethrowerWeapon>();

        if (pollenWeapon != null) pollenWeapon.enabled = false;
        
        if (uvWeapon != null) 
        {
            uvWeapon.enabled = false;
            if (uvWeapon.auraVisual != null) uvWeapon.auraVisual.gameObject.SetActive(false);
        }
        
        if (axeWeapon != null) 
        {
            axeWeapon.enabled = false;
            if (axeWeapon.axeVisual != null) axeWeapon.axeVisual.SetActive(false);
        }
        
        if (flamethrowerWeapon != null) 
        {
            flamethrowerWeapon.enabled = false;
            if (flamethrowerWeapon.flameVisual != null) flamethrowerWeapon.flameVisual.SetActive(false);
        }
    }

    public void GenerateUpgrades()
    {
        List<UpgradeType> validUpgrades = new List<UpgradeType>();
        List<UpgradeType> weaponUnlocks = new List<UpgradeType>();
        List<UpgradeType> weaponUpgrades = new List<UpgradeType>();
        List<UpgradeType> passiveUpgrades = new List<UpgradeType>();

        bool hasAnyWeapon = false;

        // Silah Kontrolleri
        if (pollenWeapon != null) { if (!pollenWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockPollen); else { hasAnyWeapon = true; if (pollenWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradePollen); } }
        if (uvWeapon != null) { if (!uvWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockUV); else { hasAnyWeapon = true; if (uvWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradeUV); } }
        if (axeWeapon != null) { if (!axeWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockAxe); else { hasAnyWeapon = true; if (axeWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradeAxe); } }
        if (flamethrowerWeapon != null) { if (!flamethrowerWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockFlamethrower); else { hasAnyWeapon = true; if (flamethrowerWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradeFlamethrower); } }

        // Pasif Eşya Kontrolleri (Seviye 8 maksimum)
        if (PlayerPassives.Instance != null)
        {
            if (PlayerPassives.Instance.magnetLevel < 8) passiveUpgrades.Add(UpgradeType.UpgradeMagnet);
            if (PlayerPassives.Instance.luckLevel < 8) passiveUpgrades.Add(UpgradeType.UpgradeLuck);
            if (PlayerPassives.Instance.curseLevel < 8) passiveUpgrades.Add(UpgradeType.UpgradeCurse);
            if (PlayerPassives.Instance.turretLevel < 8) passiveUpgrades.Add(UpgradeType.UpgradeTurret);
            if (PlayerPassives.Instance.critLevel < 8) passiveUpgrades.Add(UpgradeType.UpgradeCrit);
        }

        if (!hasAnyWeapon)
        {
            validUpgrades.AddRange(weaponUnlocks);
        }
        else
        {
            validUpgrades.AddRange(weaponUnlocks);
            validUpgrades.AddRange(weaponUpgrades);
            validUpgrades.AddRange(passiveUpgrades);
        }

        for (int i = 0; i < 3; i++)
        {
            if (validUpgrades.Count > 0)
            {
                int randomIndex = Random.Range(0, validUpgrades.Count);
                currentChoices[i] = validUpgrades[randomIndex];
                validUpgrades.RemoveAt(randomIndex); 
            }
            else
            {
                // Havuz boşalırsa (Her şey max olursa) 
                currentChoices[i] = UpgradeType.HealPotion; // Can iksiri verelim
            }
        }

        if (button1Text != null) button1Text.text = GetUpgradeDescription(currentChoices[0]);
        if (button2Text != null) button2Text.text = GetUpgradeDescription(currentChoices[1]);
        if (button3Text != null) button3Text.text = GetUpgradeDescription(currentChoices[2]);
    }

    string GetUpgradeDescription(UpgradeType type)
    {
        switch (type)
        {
            // YEDEK / TEKRARLANABİLİR
            case UpgradeType.HealPotion:
                return "Health Potion (Max Lvl Reward):\nRestores +30 HP";

            // PASİFLER
            case UpgradeType.UpgradeMagnet:
                int nextMag = PlayerPassives.Instance.magnetLevel + 1;
                return (nextMag == 1) ? "NEW PASSIVE: Seed Magnet (Lvl 1)" : $"Seed Magnet (Lvl {nextMag}): Seed Collection Radius Increased";
            
            case UpgradeType.UpgradeLuck:
                int nextLuck = PlayerPassives.Instance.luckLevel + 1;
                return (nextLuck == 1) ? "NEW PASSIVE: Luck (Lvl 1)" : $"Luck (Lvl {nextLuck}): Item & Turret Drop Rate Increased";
            
            case UpgradeType.UpgradeCurse:
                int nextCurse = PlayerPassives.Instance.curseLevel + 1;
                return (nextCurse == 1) ? "NEW PASSIVE: Curse (Lvl 1)" : $"Curse (Lvl {nextCurse}): Enemy Density & Dropped XP Increased";
            
            case UpgradeType.UpgradeTurret:
                int nextTurret = PlayerPassives.Instance.turretLevel + 1;
                return (nextTurret == 1) ? "NEW PASSIVE: Root Turret (Lvl 1)" : $"Root Turret (Lvl {nextTurret}): Turret Damage & Duration Increased";
            
            case UpgradeType.UpgradeCrit:
                int nextCrit = PlayerPassives.Instance.critLevel + 1;
                return (nextCrit == 1) ? "NEW PASSIVE: Deadly Focus (Lvl 1)\nCrit Chance +5%" : $"Deadly Focus (Lvl {nextCrit}): Crit Chance +5% / Multiplier +0.125x";


            // POLLEN
            case UpgradeType.UnlockPollen: return "NEW WEAPON: Pollen Injector (Lvl 1)";
            case UpgradeType.UpgradePollen:
                int nextPollenLevel = pollenWeapon.currentLevel + 1;
                switch (nextPollenLevel)
                {
                    case 2: return "Pollen (Lvl 2): Fire Rate +15%";
                    case 3: return "Pollen (Lvl 3): Damage +20% | SYNERGY: Max HP +10";
                    case 4: return "Pollen (Lvl 4): 3-Way Spread Projectiles";
                    case 5: return "Pollen (Lvl 5): Projectile Speed & Range +20%";
                    case 6: return "Pollen (Lvl 6): Damage +25% | SYNERGY: Max HP +20";
                    case 7: return "Pollen (Lvl 7): Fire Rate +25%";
                    case 8: return "Pollen (MAX): 5 Piercing Projectiles | SYNERGY: Max HP +20";
                    default: return "Pollen Upgraded";
                }

            // UV LAMP
            case UpgradeType.UnlockUV: return "NEW WEAPON: UV Lamp (Lvl 1)";
            case UpgradeType.UpgradeUV:
                int nextUVLevel = uvWeapon.currentLevel + 1;
                switch (nextUVLevel)
                {
                    case 2: return "UV Lamp (Lvl 2): Area Damage +25%";
                    case 3: return "UV Lamp (Lvl 3): Hit Frequency Increased | SYNERGY: +0.5 HP Regen";
                    case 4: return "UV Lamp (Lvl 4): Aura Radius +50%";
                    case 5: return "UV Lamp (Lvl 5): Area Damage +30%";
                    case 6: return "UV Lamp (Lvl 6): Aura Radius +20% | SYNERGY: +1.0 HP Regen";
                    case 7: return "UV Lamp (Lvl 7): Rapid Pulse Frequency";
                    case 8: return "UV Lamp (MAX): 30% Slow Aura | SYNERGY: +1.5 HP Regen";
                    default: return "UV Lamp Upgraded";
                }

            // AXE
            case UpgradeType.UnlockAxe: return "NEW WEAPON: Orbiting Axe (Lvl 1)";
            case UpgradeType.UpgradeAxe:
                int nextAxeLevel = axeWeapon.currentLevel + 1;
                switch (nextAxeLevel)
                {
                    case 2: return "Axe (Lvl 2): Axe Damage +25%";
                    case 3: return "Axe (Lvl 3): Orbit Speed +20% | SYNERGY: Move Speed +5%";
                    case 4: return "Axe (Lvl 4): Twin Orbiting Axes";
                    case 5: return "Axe (Lvl 5): Axe Size +25%";
                    case 6: return "Axe (Lvl 6): Axe Damage +30% | SYNERGY: Move Speed +10%";
                    case 7: return "Axe (Lvl 7): Orbit Speed +30%";
                    case 8: return "Axe (MAX): 4-Axe Kinetic Sawblade | SYNERGY: Move Speed +10%";
                    default: return "Axe Upgraded";
                }

            // FLAMETHROWER
            case UpgradeType.UnlockFlamethrower: return "NEW WEAPON: Flamethrower (Lvl 1)";
            case UpgradeType.UpgradeFlamethrower:
                int nextFlameLevel = flamethrowerWeapon.currentLevel + 1;
                switch (nextFlameLevel)
                {
                    case 2: return "Flame (Lvl 2): Cooldown -20%";
                    case 3: return "Flame (Lvl 3): Damage +25% | SYNERGY: All Damage +10%";
                    case 4: return "Flame (Lvl 4): Range & Damage Extended";
                    case 5: return "Flame (Lvl 5): Flame Cone +30%";
                    case 6: return "Flame (Lvl 6): Cooldown -30% | SYNERGY: All Damage +10%";
                    case 7: return "Flame (Lvl 7): Flame Damage +35%";
                    case 8: return "Flame (MAX): Scorched Earth Ground Fire | SYNERGY: All Damage +15%";
                    default: return "Flame Upgraded";
                }

            default: return "Unknown Upgrade";
        }
    }

    public void SelectUpgrade1() { ApplyUpgrade(currentChoices[0]); }
    public void SelectUpgrade2() { ApplyUpgrade(currentChoices[1]); }
    public void SelectUpgrade3() { ApplyUpgrade(currentChoices[2]); }

    void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            // YEDEK / TEKRARLANABİLİR
            case UpgradeType.HealPotion:
                if (playerHealth != null) playerHealth.Heal(30f);
                break;

            // PASİFLER
            case UpgradeType.UpgradeMagnet:
                if (PlayerPassives.Instance != null) PlayerPassives.Instance.magnetLevel++;
                break;
            case UpgradeType.UpgradeLuck:
                if (PlayerPassives.Instance != null) PlayerPassives.Instance.luckLevel++;
                break;
            case UpgradeType.UpgradeCurse:
                if (PlayerPassives.Instance != null) PlayerPassives.Instance.curseLevel++;
                break;
            case UpgradeType.UpgradeTurret:
                if (PlayerPassives.Instance != null) PlayerPassives.Instance.turretLevel++;
                break;
            case UpgradeType.UpgradeCrit:
                if (PlayerPassives.Instance != null) PlayerPassives.Instance.critLevel++;
                break;

            // SİLAHLAR
            case UpgradeType.UnlockPollen:
                if (pollenWeapon != null) pollenWeapon.UnlockWeapon();
                break;
            case UpgradeType.UpgradePollen:
                if (pollenWeapon != null) pollenWeapon.LevelUpWeapon();
                break;

            case UpgradeType.UnlockUV:
                if (uvWeapon != null) uvWeapon.UnlockWeapon();
                break;
            case UpgradeType.UpgradeUV:
                if (uvWeapon != null) uvWeapon.LevelUpWeapon();
                break;

            case UpgradeType.UnlockAxe:
                if (axeWeapon != null) axeWeapon.UnlockWeapon();
                break;
            case UpgradeType.UpgradeAxe:
                if (axeWeapon != null) axeWeapon.LevelUpWeapon();
                break;

            case UpgradeType.UnlockFlamethrower:
                if (flamethrowerWeapon != null) flamethrowerWeapon.UnlockWeapon();
                break;
            case UpgradeType.UpgradeFlamethrower:
                if (flamethrowerWeapon != null) flamethrowerWeapon.LevelUpWeapon();
                break;
        }

        ExperienceManager.Instance.ResumeGame();
    }
}
