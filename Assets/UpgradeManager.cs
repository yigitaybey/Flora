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
                return "İksir (Max Lvl Ödülü):\n+30 Can Yeniler";

            // PASİFLER
            case UpgradeType.UpgradeMagnet:
                int nextMag = PlayerPassives.Instance.magnetLevel + 1;
                return (nextMag == 1) ? "YENİ PASİF: Mıknatıs (Lvl 1)" : $"Mıknatıs (Lvl {nextMag}): Tohum Çekme Menzili Artar";
            
            case UpgradeType.UpgradeLuck:
                int nextLuck = PlayerPassives.Instance.luckLevel + 1;
                return (nextLuck == 1) ? "YENİ PASİF: Şans (Lvl 1)" : $"Şans (Lvl {nextLuck}): Eşya/Taret Çıkma İhtimali Artar";
            
            case UpgradeType.UpgradeCurse:
                int nextCurse = PlayerPassives.Instance.curseLevel + 1;
                return (nextCurse == 1) ? "YENİ PASİF: Lanet (Lvl 1)" : $"Lanet (Lvl {nextCurse}): Düşman Sıklığı ve Düşen XP Artar";
            
            case UpgradeType.UpgradeTurret:
                int nextTurret = PlayerPassives.Instance.turretLevel + 1;
                return (nextTurret == 1) ? "YENİ PASİF: Kök Taret (Lvl 1)" : $"Kök Taret (Lvl {nextTurret}): Taret Hasarı ve Süresi Uzar";
            
            case UpgradeType.UpgradeCrit:
                int nextCrit = PlayerPassives.Instance.critLevel + 1;
                return (nextCrit == 1) ? "YENİ PASİF: Ölümcül Odak (Lvl 1)\nKritik +%5" : $"Ölümcül Odak (Lvl {nextCrit}): Kritik İhtimali +%5 / Çarpan +0.125x";


            // POLLEN
            case UpgradeType.UnlockPollen: return "YENİ SİLAH: Polen Enjektörü (Lvl 1)";
            case UpgradeType.UpgradePollen:
                int nextPollenLevel = pollenWeapon.currentLevel + 1;
                switch (nextPollenLevel)
                {
                    case 2: return "Polen (Lvl 2): Atış Hızı +%15";
                    case 3: return "Polen (Lvl 3): Hasar +%20 | SİNERJİ: Max Can +10";
                    case 4: return "Polen (Lvl 4): V Şeklinde 3 Mermi";
                    case 5: return "Polen (Lvl 5): Mermi Hızı ve Menzili +%20";
                    case 6: return "Polen (Lvl 6): Hasar +%25 | SİNERJİ: Max Can +20";
                    case 7: return "Polen (Lvl 7): Atış Hızı +%25";
                    case 8: return "Polen (MAX): 5 Delici Mermi | SİNERJİ: Max Can +20";
                    default: return "Polen Geliştirildi";
                }

            // UV LAMP
            case UpgradeType.UnlockUV: return "YENİ SİLAH: UV Lamba (Lvl 1)";
            case UpgradeType.UpgradeUV:
                int nextUVLevel = uvWeapon.currentLevel + 1;
                switch (nextUVLevel)
                {
                    case 2: return "UV Lamba (Lvl 2): Alan Hasarı +%25";
                    case 3: return "UV Lamba (Lvl 3): Vuruş Sıklığı Artar | SİNERJİ: +0.5 Can Yenileme";
                    case 4: return "UV Lamba (Lvl 4): Menzil (Çap) %50 Uzar";
                    case 5: return "UV Lamba (Lvl 5): Alan Hasarı +%30";
                    case 6: return "UV Lamba (Lvl 6): Menzil +%20 | SİNERJİ: +1 Can Yenileme";
                    case 7: return "UV Lamba (Lvl 7): Vuruş Sıklığı Muazzam Artar";
                    case 8: return "UV Lamba (MAX): %30 Yavaşlatma | SİNERJİ: +1.5 Can Yenileme";
                    default: return "UV Lamba Geliştirildi";
                }

            // AXE
            case UpgradeType.UnlockAxe: return "YENİ SİLAH: Dönen Balta (Lvl 1)";
            case UpgradeType.UpgradeAxe:
                int nextAxeLevel = axeWeapon.currentLevel + 1;
                switch (nextAxeLevel)
                {
                    case 2: return "Balta (Lvl 2): Balta Hasarı +%25";
                    case 3: return "Balta (Lvl 3): Dönüş Hızı +%20 | SİNERJİ: Hareket Hızı +%5";
                    case 4: return "Balta (Lvl 4): Çift Balta (2 Adet)";
                    case 5: return "Balta (Lvl 5): Balta Boyutu +%25";
                    case 6: return "Balta (Lvl 6): Balta Hasarı +%30 | SİNERJİ: Hareket Hızı +%10";
                    case 7: return "Balta (Lvl 7): Dönüş Hızı +%30";
                    case 8: return "Balta (MAX): 4 Balta Kinetik Testere | SİNERJİ: Hız +%10";
                    default: return "Balta Geliştirildi";
                }

            // FLAMETHROWER
            case UpgradeType.UnlockFlamethrower: return "YENİ SİLAH: Alev Makinesi (Lvl 1)";
            case UpgradeType.UpgradeFlamethrower:
                int nextFlameLevel = flamethrowerWeapon.currentLevel + 1;
                switch (nextFlameLevel)
                {
                    case 2: return "Alev (Lvl 2): Bekleme Süresi -%20";
                    case 3: return "Alev (Lvl 3): Hasar +%25 | SİNERJİ: Tüm Hasarlar +%10";
                    case 4: return "Alev (Lvl 4): Menzil ve Hasar Uzar";
                    case 5: return "Alev (Lvl 5): Alev Genişliği +%30";
                    case 6: return "Alev (Lvl 6): Bekleme Süresi -%30 | SİNERJİ: Tüm Hasarlar +%10";
                    case 7: return "Alev (Lvl 7): Alev Hasarı +%35";
                    case 8: return "Alev (MAX): Yanık Toprak Efekti | SİNERJİ: Tüm Hasarlar +%15";
                    default: return "Alev Geliştirildi";
                }

            default: return "Bilinmeyen Yetenek";
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
