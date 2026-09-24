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
            Debug.Log($"🎉 {slot.skillName} başarıyla yükseltildi!");
            
            // Tüm ekranı ve sayacı güncelle
            RefreshAllSlots();
            SelectSkill(selectedSkillIndex);

            if (BaseUIManager.Instance != null)
            {
                BaseUIManager.Instance.UpdateCurrencyUI();
            }
        }
        else
        {
            Debug.LogWarning("Yetersiz tohum veya maksimum seviye!");
        }
    }

    // Yeteneklerin seviye bazlı açıklamaları
    string GetSkillDescriptionWithStats(PermanentSkillType type, int currentLevel)
    {
        switch (type)
        {
            case PermanentSkillType.FlameMultishot:
                if (currentLevel == 0) return "Tüm silahların mermi ve balta sayısını 2 KATINA çıkarır.\nSonraki: 2x Silah Adedi";
                if (currentLevel == 1) return "Silah adetlerini 3 KATINA çıkarır.\nMevcut: 2x -> Sonraki: 3x Silah";
                if (currentLevel == 2) return "Silah adetlerini 4 KATINA çıkarır!\nMevcut: 3x -> Sonraki: 4x Çılgın Saldırı!";
                return "Tüm silahların mermi ve balta sayısı 4 KATINA çıkarıldı! (Maksimum Güç)";

            case PermanentSkillType.SunshineCurse:
                if (currentLevel == 0) return "Ormanın Gazabı: Düşmanlar %10 hızlanır, ama tohum ve XP kazancı %25 artar.\nSonraki: +%25 Kazanç / +%10 Hız";
                if (currentLevel == 1) return "Düşmanlar %20 hızlanır, kazanç %50 artar.\nMevcut: +%25 -> Sonraki: +%50 Kazanç";
                if (currentLevel == 2) return "Düşmanlar %30 hızlanır, kazanç %75 artar!\nMevcut: +%50 -> Sonraki: +%75 Dev Kazanç!";
                return "Maksimum Lanet: Düşmanlar %30 hızlı, Tohum ve XP kazancı %75 artırıldı!";

            case PermanentSkillType.AppleHealth:
                if (currentLevel == 0) return "Sylva'nın kök sağlığını güçlendirerek savaşa +25 Maksimum Can ile başlamasını sağlar.";
                if (currentLevel == 1) return "Maksimum Canı artırır.\nMevcut: +25 HP -> Sonraki: +50 HP";
                if (currentLevel == 2) return "Maksimum Canı devasa artırır.\nMevcut: +50 HP -> Sonraki: +100 HP";
                return "Maksimum Kök Sağlığı: Sylva savaşa +100 Ekstra Can ile başlar.";

            case PermanentSkillType.MagnetRadius:
                if (currentLevel == 0) return "Yerdeki tüm XP tohumlarını ve çekirdekleri çekme menzilini %40 artırır.";
                if (currentLevel == 1) return "Çekim alanını genişletir.\nMevcut: +%40 -> Sonraki: +%80 Alan";
                if (currentLevel == 2) return "Çekim alanını devasa yapar.\nMevcut: +%80 -> Sonraki: +%120 Alan";
                return "Maksimum Polen Çekimi: Tohum çekim alanı +%120 artırıldı.";

            case PermanentSkillType.VitalSeedGain:
                if (currentLevel == 0) return "Savaş esnasında düşen kalıcı Core Seed (Tohum) miktarını %20 artırır.";
                if (currentLevel == 1) return "Tohum kazancını artırır.\nMevcut: +%20 -> Sonraki: +%40 Tohum";
                if (currentLevel == 2) return "Tohum kazancını artırır.\nMevcut: +%40 -> Sonraki: +%60 Tohum";
                return "Maksimum Bereket: Kalıcı tohum kazancı +%60 artırıldı.";

            case PermanentSkillType.PollenDamage:
                if (currentLevel == 0) return "Tüm silahların temel vuruş hasarını kalıcı olarak %15 artırır.";
                if (currentLevel == 1) return "Temel hasarı artırır.\nMevcut: +%15 -> Sonraki: +%30 Hasar";
                if (currentLevel == 2) return "Temel hasarı artırır.\nMevcut: +%30 -> Sonraki: +%50 Hasar";
                return "Maksimum Keskinlik: Tüm silahların temel hasarı +%50 artırıldı.";

            case PermanentSkillType.AxeArmor:
                if (currentLevel == 0) return "Ağaç Kabuğu: Alınan her darbeden doğrudan 1 Hasar siler.";
                if (currentLevel == 1) return "Zırhı güçlendirir.\nMevcut: -1 Hasar -> Sonraki: -2 Hasar Engelleme";
                if (currentLevel == 2) return "Zırhı güçlendirir.\nMevcut: -2 Hasar -> Sonraki: -3 Hasar Engelleme";
                return "Maksimum Kabuk: Alınan her darbeden 3 Hasar doğrudan silinir.";

            case PermanentSkillType.UVSpeed:
                if (currentLevel == 0) return "Sylva'nın koşu hızını kalıcı olarak %10 artırır.";
                if (currentLevel == 1) return "Koşu hızını artırır.\nMevcut: +%10 -> Sonraki: +%20 Hız";
                if (currentLevel == 2) return "Koşu hızını artırır.\nMevcut: +%20 -> Sonraki: +%30 Hız";
                return "Maksimum Işık Hızı: Sylva %30 daha hızlı koşar.";

            default:
                return "Bilinmeyen Yetenek";
        }
    }
}
