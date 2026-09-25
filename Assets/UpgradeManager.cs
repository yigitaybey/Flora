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

    [Header("UI Bağlantıları (3 Kartın İkon Resmi)")]
    public UnityEngine.UI.Image button1Icon;
    public UnityEngine.UI.Image button2Icon;
    public UnityEngine.UI.Image button3Icon;

    [Header("Yetenek İkonları (Sprite)")]
    public Sprite pollenIcon;
    public Sprite uvLampIcon;
    public Sprite axeIcon;
    public Sprite flamethrowerIcon;
    public Sprite healPotionIcon; // apple.png

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

        bool hasAnyWeapon = false;

        // Silah Kontrolleri
        if (pollenWeapon != null) { if (!pollenWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockPollen); else { hasAnyWeapon = true; if (pollenWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradePollen); } }
        if (uvWeapon != null) { if (!uvWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockUV); else { hasAnyWeapon = true; if (uvWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradeUV); } }
        if (axeWeapon != null) { if (!axeWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockAxe); else { hasAnyWeapon = true; if (axeWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradeAxe); } }
        if (flamethrowerWeapon != null) { if (!flamethrowerWeapon.enabled) weaponUnlocks.Add(UpgradeType.UnlockFlamethrower); else { hasAnyWeapon = true; if (flamethrowerWeapon.currentLevel < 8) weaponUpgrades.Add(UpgradeType.UpgradeFlamethrower); } }

        if (!hasAnyWeapon)
        {
            validUpgrades.AddRange(weaponUnlocks);
        }
        else
        {
            validUpgrades.AddRange(weaponUnlocks);
            validUpgrades.AddRange(weaponUpgrades);
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

        if (button1Icon != null) button1Icon.sprite = GetUpgradeIcon(currentChoices[0]);
        if (button2Icon != null) button2Icon.sprite = GetUpgradeIcon(currentChoices[1]);
        if (button3Icon != null) button3Icon.sprite = GetUpgradeIcon(currentChoices[2]);
    }

    Sprite GetUpgradeIcon(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.UnlockPollen:
            case UpgradeType.UpgradePollen:
                return pollenIcon;
            case UpgradeType.UnlockUV:
            case UpgradeType.UpgradeUV:
                return uvLampIcon;
            case UpgradeType.UnlockAxe:
            case UpgradeType.UpgradeAxe:
                return axeIcon;
            case UpgradeType.UnlockFlamethrower:
            case UpgradeType.UpgradeFlamethrower:
                return flamethrowerIcon;
            case UpgradeType.HealPotion:
                return healPotionIcon;
            default:
                return null;
        }
    }

    string GetUpgradeDescription(UpgradeType type)
    {
        switch (type)
        {
            // YEDEK / TEKRARLANABİLİR
            case UpgradeType.HealPotion:
                return "🧪 Acil Durum İksiri:\n+30 HP Canlandır";


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
                    case 8: return "⚡ EVRİM: ÇİÇEK GATLING'İ!\n5 Delici Mermi & Seri Tarama";
                    default: return "Pollen Upgraded";
                }

            // UV LAMP
            case UpgradeType.UnlockUV: return "YENİ SİLAH: UV Lambası (Lvl 1)";
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
                    case 8: return "⚡ EVRİM: SÜPERNOVA!\nDevasa Şok Dalgası & %40 Yavaşlatma";
                    default: return "UV Lamp Upgraded";
                }

            // AXE
            case UpgradeType.UnlockAxe: return "YENİ SİLAH: Dönen Balta (Lvl 1)";
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
                    case 8: return "⚡ EVRİM: TESTERE KALKANI!\n4 Dev Balta ile Geçilmez Kalkan";
                    default: return "Axe Upgraded";
                }

            // FLAMETHROWER
            case UpgradeType.UnlockFlamethrower: return "YENİ SİLAH: Alev Püskürtücü (Lvl 1)";
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
                    case 8: return "⚡ EVRİM: LAV TARLASI!\nYerde Sönmeyen Alev Göletleri";
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
