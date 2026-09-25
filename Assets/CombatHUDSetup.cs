using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// GÖREV 5: Savaş Arayüzü (In-Game Combat HUD) Giydirmesi.
/// Yusuf'un hazırladığı yeni UI görsellerini Canvas üzerine otomatik yerleştirir
/// ve dolum barlarını (HP, XP) + sayaçları (Seed, Timer, Wave, Level) bağlar.
/// 
/// KULLANIM:
/// 1) CombatScene → Canvas objesine bu scripti ekle (Add Component → CombatHUDSetup).
/// 2) Inspector'da 3 Sprite slotunu Yusuf'un görsellerinden sürükle-bırak.
/// 3) Play tuşuna bas → HUD otomatik kurulur!
/// </summary>
public class CombatHUDSetup : MonoBehaviour
{
    [Header("=== YUSUF'UN YENİ GÖRSELLERİ (Sprite Slotları) ===")]
    [Tooltip("Assets/UI-.../InGame/Can+Seed+CoreSeed.png dosyasını buraya sürükle")]
    public Sprite statusFrameSprite; // Can + Seed + CoreSeed çerçevesi

    [Tooltip("Assets/UI-.../InGame/XPBar.png dosyasını buraya sürükle")]
    public Sprite xpFrameSprite; // XP barı çerçevesi

    [Tooltip("Assets/UI-.../InGame/Settings.png dosyasını buraya sürükle")]
    public Sprite settingsSprite; // Ayarlar (dişli çark) ikonu

    [Header("=== SOL ÜST: DURUM PANELİ KONUM AYARLARI ===")]
    [Tooltip("Sol üst köşe panel pozisyonu (X, Y)")]
    public Vector2 statusPanelPosition = new Vector2(20f, -20f);
    [Tooltip("Çerçeve genişliği (piksel)")]
    public float statusFrameWidth = 420f;
    [Tooltip("Çerçeve yüksekliği (piksel)")]
    public float statusFrameHeight = 200f;

    [Header("--- CAN BARI (HP) Pozisyon Ayarları ---")]
    [Tooltip("Can barı çerçeve içindeki X kayması")]
    public float hpBarOffsetX = 10f;
    [Tooltip("Can barı çerçeve içindeki Y kayması (üstten)")]
    public float hpBarOffsetY = -18f;
    [Tooltip("Can barı genişliği")]
    public float hpBarWidth = 350f;
    [Tooltip("Can barı yüksekliği")]
    public float hpBarHeight = 36f;
    [Tooltip("Can barı dolum rengi")]
    public Color hpFillColor = new Color(0.2f, 0.85f, 0.3f, 1f); // Parlak yeşil

    [Header("--- TOHUM SAYACI (Seed) Pozisyon Ayarları ---")]
    [Tooltip("Tohum yazısının çerçeve içindeki Y kayması")]
    public float seedTextOffsetY = -72f;
    [Tooltip("Tohum yazısı genişliği")]
    public float seedTextWidth = 200f;

    [Header("--- ÇEKİRDEK SAYACI (CoreSeed) Pozisyon Ayarları ---")]
    [Tooltip("Çekirdek yazısının çerçeve içindeki Y kayması")]
    public float coreSeedTextOffsetY = -110f;

    [Header("=== ALT: XP BARI KONUM AYARLARI ===")]
    [Tooltip("XP barı ekranın altından yükseklik (Y)")]
    public float xpBarBottomOffset = 30f;
    [Tooltip("XP barı genişliği")]
    public float xpBarWidth = 600f;
    [Tooltip("XP barı yüksekliği")]
    public float xpBarHeight = 50f;
    [Tooltip("XP barı dolum rengi")]
    public Color xpFillColor = new Color(0.3f, 0.7f, 1f, 1f); // Açık mavi

    [Header("=== SAĞ ÜST: SÜRE, DALGA VE SEVİYE ===")]
    [Tooltip("Süre yazısı pozisyonu (sağ üst köşeden X kayması)")]
    public float timerOffsetX = -20f;
    [Tooltip("Süre yazısı pozisyonu (üstten Y kayması)")]
    public float timerOffsetY = -20f;

    [Header("=== SAĞ ÜST: AYARLAR BUTONU ===")]
    [Tooltip("Ayarlar butonu boyutu")]
    public float settingsButtonSize = 60f;
    [Tooltip("Ayarlar butonu sağdan X kayması")]
    public float settingsOffsetX = -20f;
    [Tooltip("Ayarlar butonu üstten Y kayması (timer'ın altına gelecek)")]
    public float settingsOffsetY = -90f;

    // === İÇ REFERANSLAR (Script otomatik bağlar, Inspector'da dokunma) ===
    [HideInInspector] public Image hpFillImage;
    [HideInInspector] public TextMeshProUGUI hpText;
    [HideInInspector] public Image xpFillImage;
    [HideInInspector] public TextMeshProUGUI levelText;
    [HideInInspector] public TextMeshProUGUI seedText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public TextMeshProUGUI waveText;

    void Awake()
    {
        BuildHUD();
    }

    /// <summary>
    /// Tüm HUD'ı sıfırdan oluşturur ve mevcut scriptlere bağlar.
    /// </summary>
    void BuildHUD()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[CombatHUDSetup] Bu script bir Canvas objesine eklenmelidir!");
            return;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // ============================================================
        // 1) SOL ÜST: DURUM PANELİ (HP Bar + Seed + CoreSeed Çerçevesi)
        // ============================================================
        GameObject statusPanel = CreateUIObject("HUD_StatusPanel", canvasRect);
        RectTransform statusRT = statusPanel.GetComponent<RectTransform>();
        statusRT.anchorMin = new Vector2(0, 1); // Sol üst
        statusRT.anchorMax = new Vector2(0, 1);
        statusRT.pivot = new Vector2(0, 1);
        statusRT.anchoredPosition = statusPanelPosition;
        statusRT.sizeDelta = new Vector2(statusFrameWidth, statusFrameHeight);

        // Çerçeve Görseli (Yusuf'un Can+Seed+CoreSeed.png'si)
        if (statusFrameSprite != null)
        {
            Image frameImg = statusPanel.AddComponent<Image>();
            frameImg.sprite = statusFrameSprite;
            frameImg.type = Image.Type.Simple;
            frameImg.preserveAspect = true;
            frameImg.raycastTarget = false;
        }

        // --- HP BAR (Dolum Çubuğu) ---
        // HP Bar Arka Plan (koyu şeffaf)
        GameObject hpBarBg = CreateUIObject("HUD_HP_Background", statusRT);
        RectTransform hpBgRT = hpBarBg.GetComponent<RectTransform>();
        hpBgRT.anchorMin = new Vector2(0, 1);
        hpBgRT.anchorMax = new Vector2(0, 1);
        hpBgRT.pivot = new Vector2(0, 1);
        hpBgRT.anchoredPosition = new Vector2(hpBarOffsetX, hpBarOffsetY);
        hpBgRT.sizeDelta = new Vector2(hpBarWidth, hpBarHeight);
        Image hpBgImg = hpBarBg.AddComponent<Image>();
        hpBgImg.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
        hpBgImg.raycastTarget = false;

        // HP Dolum Alanı (Filled Image)
        GameObject hpFillObj = CreateUIObject("HUD_HP_Fill", hpBgRT);
        RectTransform hpFillRT = hpFillObj.GetComponent<RectTransform>();
        hpFillRT.anchorMin = Vector2.zero;
        hpFillRT.anchorMax = Vector2.one;
        hpFillRT.offsetMin = new Vector2(4, 4);  // İç boşluk
        hpFillRT.offsetMax = new Vector2(-4, -4);
        hpFillImage = hpFillObj.AddComponent<Image>();
        hpFillImage.color = hpFillColor;
        hpFillImage.type = Image.Type.Filled;
        hpFillImage.fillMethod = Image.FillMethod.Horizontal;
        hpFillImage.fillAmount = 1f;
        hpFillImage.raycastTarget = false;

        // HP Yazısı (Örn: "85 / 100")
        GameObject hpTextObj = CreateUIObject("HUD_HP_Text", hpBgRT);
        RectTransform hpTextRT = hpTextObj.GetComponent<RectTransform>();
        hpTextRT.anchorMin = Vector2.zero;
        hpTextRT.anchorMax = Vector2.one;
        hpTextRT.offsetMin = Vector2.zero;
        hpTextRT.offsetMax = Vector2.zero;
        hpText = hpTextObj.AddComponent<TextMeshProUGUI>();
        hpText.text = "100 / 100";
        hpText.fontSize = 16;
        hpText.alignment = TextAlignmentOptions.Center;
        hpText.color = Color.white;
        hpText.fontStyle = FontStyles.Bold;
        hpText.raycastTarget = false;

        // --- TOHUM SAYACI (Seed) ---
        GameObject seedObj = CreateUIObject("HUD_SeedCounter", statusRT);
        RectTransform seedRT = seedObj.GetComponent<RectTransform>();
        seedRT.anchorMin = new Vector2(0, 1);
        seedRT.anchorMax = new Vector2(0, 1);
        seedRT.pivot = new Vector2(0, 1);
        seedRT.anchoredPosition = new Vector2(hpBarOffsetX + 10f, seedTextOffsetY);
        seedRT.sizeDelta = new Vector2(seedTextWidth, 30f);
        seedText = seedObj.AddComponent<TextMeshProUGUI>();
        seedText.text = "Seeds: 0";
        seedText.fontSize = 16;
        seedText.alignment = TextAlignmentOptions.Left;
        seedText.color = new Color(1f, 0.85f, 0.2f, 1f); // Altın sarısı
        seedText.fontStyle = FontStyles.Bold;
        seedText.raycastTarget = false;

        // --- ÇEKİRDEK SAYACI (CoreSeed - kaldırılabilir, şimdilik Seed ile aynı) ---
        // (Tohum ve CoreSeed aynı ise bu bölüm boş bırakılabilir)

        // ============================================================
        // 2) ALT MERKEZ: XP BARI
        // ============================================================
        GameObject xpPanel = CreateUIObject("HUD_XPPanel", canvasRect);
        RectTransform xpPanelRT = xpPanel.GetComponent<RectTransform>();
        xpPanelRT.anchorMin = new Vector2(0.5f, 0);  // Alt merkez
        xpPanelRT.anchorMax = new Vector2(0.5f, 0);
        xpPanelRT.pivot = new Vector2(0.5f, 0);
        xpPanelRT.anchoredPosition = new Vector2(0, xpBarBottomOffset);
        xpPanelRT.sizeDelta = new Vector2(xpBarWidth, xpBarHeight);

        // XP Çerçeve Görseli (Yusuf'un XPBar.png'si)
        if (xpFrameSprite != null)
        {
            Image xpFrameImg = xpPanel.AddComponent<Image>();
            xpFrameImg.sprite = xpFrameSprite;
            xpFrameImg.type = Image.Type.Simple;
            xpFrameImg.preserveAspect = true;
            xpFrameImg.raycastTarget = false;
        }

        // XP Dolum Arka Plan
        GameObject xpBg = CreateUIObject("HUD_XP_Background", xpPanelRT);
        RectTransform xpBgRT = xpBg.GetComponent<RectTransform>();
        xpBgRT.anchorMin = Vector2.zero;
        xpBgRT.anchorMax = Vector2.one;
        xpBgRT.offsetMin = new Vector2(12, 8);   // Çerçeve iç boşluğu
        xpBgRT.offsetMax = new Vector2(-12, -8);
        Image xpBgImg = xpBg.AddComponent<Image>();
        xpBgImg.color = new Color(0.08f, 0.08f, 0.12f, 0.7f);
        xpBgImg.raycastTarget = false;

        // XP Dolum (Filled Image)
        GameObject xpFillObj = CreateUIObject("HUD_XP_Fill", xpBgRT);
        RectTransform xpFillRT = xpFillObj.GetComponent<RectTransform>();
        xpFillRT.anchorMin = Vector2.zero;
        xpFillRT.anchorMax = Vector2.one;
        xpFillRT.offsetMin = new Vector2(3, 3);
        xpFillRT.offsetMax = new Vector2(-3, -3);
        xpFillImage = xpFillObj.AddComponent<Image>();
        xpFillImage.color = xpFillColor;
        xpFillImage.type = Image.Type.Filled;
        xpFillImage.fillMethod = Image.FillMethod.Horizontal;
        xpFillImage.fillAmount = 0f;
        xpFillImage.raycastTarget = false;

        // Level Yazısı (XP barının üstünde)
        GameObject levelObj = CreateUIObject("HUD_LevelText", xpPanelRT);
        RectTransform levelRT = levelObj.GetComponent<RectTransform>();
        levelRT.anchorMin = new Vector2(0.5f, 1);
        levelRT.anchorMax = new Vector2(0.5f, 1);
        levelRT.pivot = new Vector2(0.5f, 0);
        levelRT.anchoredPosition = new Vector2(0, 4f);
        levelRT.sizeDelta = new Vector2(200f, 28f);
        levelText = levelObj.AddComponent<TextMeshProUGUI>();
        levelText.text = "Level: 1";
        levelText.fontSize = 18;
        levelText.alignment = TextAlignmentOptions.Center;
        levelText.color = Color.white;
        levelText.fontStyle = FontStyles.Bold;
        levelText.raycastTarget = false;

        // ============================================================
        // 3) SAĞ ÜST: SÜRE + DALGA
        // ============================================================
        // Süre (Timer) Yazısı
        GameObject timerObj = CreateUIObject("HUD_TimerText", canvasRect);
        RectTransform timerRT = timerObj.GetComponent<RectTransform>();
        timerRT.anchorMin = new Vector2(1, 1);  // Sağ üst
        timerRT.anchorMax = new Vector2(1, 1);
        timerRT.pivot = new Vector2(1, 1);
        timerRT.anchoredPosition = new Vector2(timerOffsetX, timerOffsetY);
        timerRT.sizeDelta = new Vector2(160f, 36f);
        timerText = timerObj.AddComponent<TextMeshProUGUI>();
        timerText.text = "00:00";
        timerText.fontSize = 24;
        timerText.alignment = TextAlignmentOptions.Right;
        timerText.color = Color.white;
        timerText.fontStyle = FontStyles.Bold;
        timerText.raycastTarget = false;

        // Dalga (Wave) Yazısı (Timer'ın altında)
        GameObject waveObj = CreateUIObject("HUD_WaveText", canvasRect);
        RectTransform waveRT = waveObj.GetComponent<RectTransform>();
        waveRT.anchorMin = new Vector2(1, 1);
        waveRT.anchorMax = new Vector2(1, 1);
        waveRT.pivot = new Vector2(1, 1);
        waveRT.anchoredPosition = new Vector2(timerOffsetX, timerOffsetY - 36f);
        waveRT.sizeDelta = new Vector2(160f, 30f);
        waveText = waveObj.AddComponent<TextMeshProUGUI>();
        waveText.text = "Wave: 1 / 10";
        waveText.fontSize = 18;
        waveText.alignment = TextAlignmentOptions.Right;
        waveText.color = new Color(0.8f, 1f, 0.8f, 1f); // Hafif yeşil tonu
        waveText.fontStyle = FontStyles.Bold;
        waveText.raycastTarget = false;

        // ============================================================
        // 4) SAĞ ÜST: AYARLAR BUTONU
        // ============================================================
        if (settingsSprite != null)
        {
            GameObject settingsBtn = CreateUIObject("HUD_SettingsButton", canvasRect);
            RectTransform settingsBtnRT = settingsBtn.GetComponent<RectTransform>();
            settingsBtnRT.anchorMin = new Vector2(1, 1);
            settingsBtnRT.anchorMax = new Vector2(1, 1);
            settingsBtnRT.pivot = new Vector2(1, 1);
            settingsBtnRT.anchoredPosition = new Vector2(settingsOffsetX, settingsOffsetY);
            settingsBtnRT.sizeDelta = new Vector2(settingsButtonSize, settingsButtonSize);

            Image settingsImg = settingsBtn.AddComponent<Image>();
            settingsImg.sprite = settingsSprite;
            settingsImg.type = Image.Type.Simple;
            settingsImg.preserveAspect = true;

            Button btn = settingsBtn.AddComponent<Button>();
            btn.targetGraphic = settingsImg;

            // Buton rengini hafif transparan yap, basıldığında parlatıcı efekt
            ColorBlock cb = btn.colors;
            cb.normalColor = new Color(1, 1, 1, 0.85f);
            cb.highlightedColor = new Color(1, 1, 1, 1f);
            cb.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
            btn.colors = cb;

            // Butona tıklama olayını CombatUIManager.ToggleSettings'e bağla
            btn.onClick.AddListener(() =>
            {
                if (CombatUIManager.Instance != null)
                {
                    CombatUIManager.Instance.ToggleSettings();
                }
            });
        }

        // ============================================================
        // 5) MEVCUT SCRIPTLERİ BAĞLA
        // ============================================================
        WireUpManagers();

        Debug.Log("[CombatHUDSetup] ✅ Yeni HUD başarıyla kuruldu! Yusuf'un görselleri giydirildi.");
    }

    /// <summary>
    /// Oluşturulan HUD elemanlarını PlayerHealth, ExperienceManager,
    /// CoreSeedUI ve GameTimerUI scriptlerine otomatik bağlar.
    /// </summary>
    void WireUpManagers()
    {
        // --- PlayerHealth → HP Bar Bağlantısı ---
        PlayerHealth playerHP = FindAnyObjectByType<PlayerHealth>();
        if (playerHP != null)
        {
            playerHP.healthBarFill = hpFillImage;
            playerHP.healthText = hpText;
            Debug.Log("[CombatHUDSetup] ✅ PlayerHealth → Yeni HP Barına bağlandı.");
        }
        else
        {
            Debug.LogWarning("[CombatHUDSetup] ⚠️ PlayerHealth bulunamadı! Sahneye Sylva'yı eklemeyi unutma.");
        }

        // --- ExperienceManager → XP Bar + Level Text Bağlantısı ---
        ExperienceManager xpManager = ExperienceManager.Instance;
        if (xpManager == null) xpManager = FindAnyObjectByType<ExperienceManager>();
        if (xpManager != null)
        {
            xpManager.xpBarFill = xpFillImage;
            xpManager.levelText = levelText;
            Debug.Log("[CombatHUDSetup] ✅ ExperienceManager → Yeni XP Barına bağlandı.");
        }
        else
        {
            Debug.LogWarning("[CombatHUDSetup] ⚠️ ExperienceManager bulunamadı!");
        }

        // --- CoreSeedUI → Seed Text Bağlantısı ---
        CoreSeedUI seedUI = FindAnyObjectByType<CoreSeedUI>();
        if (seedUI != null)
        {
            seedUI.coreSeedText = seedText;
            Debug.Log("[CombatHUDSetup] ✅ CoreSeedUI → Yeni Tohum Sayacına bağlandı.");
        }
        else
        {
            Debug.LogWarning("[CombatHUDSetup] ⚠️ CoreSeedUI bulunamadı! Canvas'ta CoreSeedUI scripti olmalı.");
        }

        // --- GameTimerUI → Timer Text Bağlantısı ---
        GameTimerUI timerUI = FindAnyObjectByType<GameTimerUI>();
        if (timerUI != null)
        {
            timerUI.timerText = timerText;
            Debug.Log("[CombatHUDSetup] ✅ GameTimerUI → Yeni Süre Sayacına bağlandı.");
        }
        else
        {
            Debug.LogWarning("[CombatHUDSetup] ⚠️ GameTimerUI bulunamadı!");
        }
    }

    /// <summary>
    /// Wave (Dalga) sayacını her frame günceller.
    /// Ayrı bir script (WaveUI) yoktu, biz burada güncelliyoruz.
    /// </summary>
    void Update()
    {
        // Dalga sayacını güncelle
        if (waveText != null)
        {
            int currentWave = EnemySpawner.CurrentWave;
            waveText.text = "Wave: " + currentWave + " / 10";
        }
    }

    /// <summary>
    /// Yardımcı: Boş bir UI GameObject oluşturur.
    /// </summary>
    GameObject CreateUIObject(string name, RectTransform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }
}
