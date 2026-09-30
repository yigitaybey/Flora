using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkillSlotUI
{
    public PermanentSkillType skillType;
    public string skillName;
    [TextArea(2, 3)]
    public string baseDescription;
    public Sprite skillIcon;
    public Button slotButton;
    public Image iconDisplay;
    
    [Header("3 Kademeli Seviye Göstergesi (Yeşil Işıklar/Kutular)")]
    public GameObject[] levelIndicators = new GameObject[3]; // Lvl 1, 2, 3 kutuları
}

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;

    [Header("8 Aktif Atölye Yeteneği")]
    public List<SkillSlotUI> skillSlots = new List<SkillSlotUI>();

    [Header("Sağ Taraf Detay Paneli (SkillInfo)")]
    public Image detailIcon;
    public TextMeshProUGUI txtDetailName;
    public TextMeshProUGUI txtDetailDesc;
    public TextMeshProUGUI txtDetailLevel;
    public TextMeshProUGUI txtDetailCost;
    public Button btnUpgrade;
    public TextMeshProUGUI txtUpgradeBtnText;

    private int selectedSkillIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Butonların tıklama eventlerini bağla
        for (int i = 0; i < skillSlots.Count; i++)
        {
            int index = i;
            if (skillSlots[i].slotButton != null)
            {
                skillSlots[i].slotButton.onClick.AddListener(() => OnSkillSlotClicked(index));
            }
        }

        if (btnUpgrade != null)
        {
            btnUpgrade.onClick.AddListener(OnUpgradeButtonClicked);
        }

        RefreshAllSlots();
        SelectSkill(0); // İlk yeteneği seçili getir
    }

    void OnEnable()
    {
        RefreshAllSlots();
        SelectSkill(selectedSkillIndex);
    }

    // --- TÜM YETENEK KUTULARININ SEVİYELERİNİ GÜNCELLE ---
    public void RefreshAllSlots()
    {
        if (GameManager.Instance == null) return;

        for (int i = 0; i < skillSlots.Count; i++)
        {
            SkillSlotUI slot = skillSlots[i];
            int currentLevel = GameManager.Instance.GetSkillLevel(slot.skillType);

            // İkonu ayarla
            if (slot.iconDisplay != null && slot.skillIcon != null)
            {
                slot.iconDisplay.sprite = slot.skillIcon;
            }

            // 3 kademeli seviye kutucuklarını yak/söndür
            if (slot.levelIndicators != null)
            {
                for (int lvl = 0; lvl < slot.levelIndicators.Length; lvl++)
                {
                    if (slot.levelIndicators[lvl] != null)
                    {
                        slot.levelIndicators[lvl].SetActive(lvl < currentLevel);
                    }
                }
            }
        }
    }

    // --- BİR YETENEĞE TIKLANDIĞINDA SAĞ PANELİ DOLDUR ---
    public void OnSkillSlotClicked(int index)
    {
        SelectSkill(index);
    }

    public void SelectSkill(int index)
    {
        if (index < 0 || index >= skillSlots.Count) return;
        selectedSkillIndex = index;
        SkillSlotUI slot = skillSlots[index];

        if (GameManager.Instance == null) return;

        int currentLvl = GameManager.Instance.GetSkillLevel(slot.skillType);
        int targetLvl = currentLvl + 1;
        int cost = GameManager.Instance.GetSkillCost(slot.skillType, targetLvl);

        // İkon & Başlık
        if (detailIcon != null) detailIcon.sprite = slot.skillIcon;
        if (txtDetailName != null) txtDetailName.text = slot.skillName;

        // Seviye Yazısı
        if (txtDetailLevel != null)
        {
            txtDetailLevel.text = (currentLvl >= 3) ? "Seviye: 3 / 3 (MAKSİMUM)" : $"Seviye: {currentLvl} / 3";
        }

        // Açıklama Metni (Stat artışı bilgisiyle birlikte)
        if (txtDetailDesc != null)
        {
            txtDetailDesc.text = GetSkillDescriptionWithStats(slot.skillType, currentLvl);
        }

        // Maliyet ve Satın Al Butonu
        if (currentLvl >= 3)
        {
            if (txtDetailCost != null) txtDetailCost.text = "GELİŞTİRME TAMAMLANDI!";
            if (btnUpgrade != null) btnUpgrade.interactable = false;
            if (txtUpgradeBtnText != null) txtUpgradeBtnText.text = "MAX";
        }
        else
        {
            if (txtDetailCost != null) txtDetailCost.text = $"Maliyet: {cost} Tohum";
            
            bool canAfford = GameManager.Instance.coreSeedCount >= cost;
            if (btnUpgrade != null) btnUpgrade.interactable = canAfford;
            if (txtUpgradeBtnText != null) txtUpgradeBtnText.text = canAfford ? "YÜKSELT" : "YETERSİZ TOHUM";
        }
    }

    // --- YÜKSELT BUTONUNA BASILINCA ---
    public void OnUpgradeButtonClicked()
    {
        if (GameManager.Instance == null) return;

        SkillSlotUI slot = skillSlots[selectedSkillIndex];
        bool success = GameManager.Instance.PurchasePermanentSkill(slot.skillType);

        if (success)
        {
            Debug.Log($"🎉 {slot.skillName} upgraded successfully!");
            
            // Refresh screen and counters
            RefreshAllSlots();
            SelectSkill(selectedSkillIndex);

            if (BaseUIManager.Instance != null)
            {
                BaseUIManager.Instance.UpdateCurrencyUI();
            }
        }
        else
        {
            Debug.LogWarning("Insufficient seeds or maximum level reached!");
        }
    }

    // Level-based skill descriptions
    string GetSkillDescriptionWithStats(PermanentSkillType type, int currentLevel)
    {
        switch (type)
        {
            case PermanentSkillType.FlameMultishot:
                if (currentLevel == 0) return "Doubles projectile and axe counts for all weapons.\nNext: 2x Arsenal Count";
                if (currentLevel == 1) return "Triples projectile and axe counts.\nCurrent: 2x -> Next: 3x Arsenal";
                if (currentLevel == 2) return "Quadruples projectile and axe counts!\nCurrent: 3x -> Next: 4x Arsenal Frenzy!";
                return "All weapon counts multiplied by 4X! (Maximum Power)";

            case PermanentSkillType.SunshineCurse:
                if (currentLevel == 0) return "Forest Wrath: Enemies are 10% faster, but seed & XP yield increases by 25%.\nNext: +25% Loot / +10% Speed";
                if (currentLevel == 1) return "Enemies are 20% faster, loot yield increases by 50%.\nCurrent: +25% -> Next: +50% Loot";
                if (currentLevel == 2) return "Enemies are 30% faster, loot yield increases by 75%!\nCurrent: +50% -> Next: +75% Mega Loot!";
                return "Maximum Curse: Enemies 30% faster, Seed & XP earnings boosted by 75%!";

            case PermanentSkillType.AppleHealth:
                if (currentLevel == 0) return "Fortifies Sylva's root vitality, starting runs with +25 Max HP.";
                if (currentLevel == 1) return "Increases Maximum Health.\nCurrent: +25 HP -> Next: +50 HP";
                if (currentLevel == 2) return "Massively increases Maximum Health.\nCurrent: +50 HP -> Next: +100 HP";
                return "Maximum Root Vitality: Sylva starts combat with +100 Extra HP.";

            case PermanentSkillType.MagnetRadius:
                if (currentLevel == 0) return "Increases pickup radius for all XP seeds and cores by 40%.";
                if (currentLevel == 1) return "Expands magnetic collection radius.\nCurrent: +40% -> Next: +80% Area";
                if (currentLevel == 2) return "Massively expands magnetic radius.\nCurrent: +80% -> Next: +120% Area";
                return "Maximum Pollen Attraction: Seed pickup radius expanded by +120%.";

            case PermanentSkillType.VitalSeedGain:
                if (currentLevel == 0) return "Increases permanent Core Seed drops from combat by 20%.";
                if (currentLevel == 1) return "Increases Core Seed drops.\nCurrent: +20% -> Next: +40% Seeds";
                if (currentLevel == 2) return "Increases Core Seed drops.\nCurrent: +40% -> Next: +60% Seeds";
                return "Maximum Harvest Bounty: Permanent Core Seed yield increased by +60%.";

            case PermanentSkillType.PollenDamage:
                if (currentLevel == 0) return "Permanently increases base strike damage across all weapons by 15%.";
                if (currentLevel == 1) return "Increases base weapon damage.\nCurrent: +15% -> Next: +30% Damage";
                if (currentLevel == 2) return "Increases base weapon damage.\nCurrent: +30% -> Next: +50% Damage";
                return "Maximum Razor Sharpness: All weapon base damage increased by +50%.";

            case PermanentSkillType.AxeArmor:
                if (currentLevel == 0) return "Bark Armor: Directly mitigates 1 Damage from every incoming hit.";
                if (currentLevel == 1) return "Reinforces defensive bark.\nCurrent: -1 Damage -> Next: -2 Damage Absorbed";
                if (currentLevel == 2) return "Reinforces defensive bark.\nCurrent: -2 Damage -> Next: -3 Damage Absorbed";
                return "Maximum Bark Shield: Directly negates 3 Damage from every incoming hit.";

            case PermanentSkillType.UVSpeed:
                if (currentLevel == 0) return "Permanently increases Sylva's movement speed by 10%.";
                if (currentLevel == 1) return "Increases movement speed.\nCurrent: +10% -> Next: +20% Speed";
                if (currentLevel == 2) return "Increases movement speed.\nCurrent: +20% -> Next: +30% Speed";
                return "Maximum Photonic Velocity: Sylva sprints 30% faster.";

            default:
                return "Unknown Skill";
        }
    }
}
