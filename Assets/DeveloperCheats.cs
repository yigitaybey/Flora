using UnityEngine;
using System.Collections.Generic;

public class DeveloperCheats : MonoBehaviour
{
    public static DeveloperCheats Instance;

    [Header("Debug Menü Ayarları")]
    public bool showMenu = false;
    public bool showFloatingDevButton = true;
    public KeyCode toggleMenuKey = KeyCode.F1;

    [Header("Durumlar")]
    public bool isGodMode = false;
    public float customGameSpeed = 1f;

    // FPS Hesaplama
    private float fps = 60f;
    private float fpsDeltaTime = 0f;

    // Bildirim Toast Mesajı
    private string lastMessage = "Dev Konsolu Hazır. (Kısayollar: G, K, L, P, H, T, B, F1)";
    private float messageTimer = 4f;

    // GUI Stilleri
    private GUIStyle windowStyle;
    private GUIStyle headerStyle;
    private GUIStyle statLabelStyle;
    private GUIStyle buttonStyle;
    private GUIStyle activeButtonStyle;
    private GUIStyle warningButtonStyle;
    private GUIStyle toastStyle;
    private bool stylesInitialized = false;

    private Rect windowRect = new Rect(20, 20, 320, 520);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // FPS Ölçümü
        fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
        fps = 1.0f / fpsDeltaTime;

        // Bildirim süresini say
        if (messageTimer > 0)
        {
            messageTimer -= Time.unscaledDeltaTime;
        }

        // Kısayol Tuşlarını Kontrol Et
        CheckKeyboardInputs();

        // God Mode durumunu karaktere yansıt
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null)
        {
            player.isGodMode = isGodMode;
        }
    }

    void CheckKeyboardInputs()
    {
        // 1. Yeni Input System (UnityEngine.InputSystem) Kontrolü
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null)
        {
            // F1 veya Tilde (~) -> Menü Aç/Kapat
            if (kb.f1Key.wasPressedThisFrame || kb.backquoteKey.wasPressedThisFrame)
            {
                ToggleMenu();
            }

            // G -> God Mode
            if (kb.gKey.wasPressedThisFrame)
            {
                ToggleGodMode();
            }

            // K -> Kill All Enemies
            if (kb.kKey.wasPressedThisFrame)
            {
                KillAllEnemies();
            }

            // L -> Level Up
            if (kb.lKey.wasPressedThisFrame)
            {
                ForceLevelUp();
            }

            // P -> +1000 Para (Core Seed)
            if (kb.pKey.wasPressedThisFrame)
            {
                AddMoney(1000);
            }

            // H -> Full Heal
            if (kb.hKey.wasPressedThisFrame)
            {
                FullHeal();
            }

            // T -> +60 Saniye Zaman Sar
            if (kb.tKey.wasPressedThisFrame)
            {
                AddTime(60f);
            }

            // B -> Direkt Boss Çağır
            if (kb.bKey.wasPressedThisFrame)
            {
                SpawnBoss();
            }

            // 1, 2, 5 -> Oyun Hızı
            if (kb.digit1Key.wasPressedThisFrame) SetSpeed(1f);
            if (kb.digit2Key.wasPressedThisFrame) SetSpeed(2f);
            if (kb.digit5Key.wasPressedThisFrame) SetSpeed(5f);
        }
        else
        {
            // 2. Legacy Input Fallback (Eski Giriş Sistemi için)
            if (Input.GetKeyDown(toggleMenuKey) || Input.GetKeyDown(KeyCode.BackQuote)) ToggleMenu();
            if (Input.GetKeyDown(KeyCode.G)) ToggleGodMode();
            if (Input.GetKeyDown(KeyCode.K)) KillAllEnemies();
            if (Input.GetKeyDown(KeyCode.L)) ForceLevelUp();
            if (Input.GetKeyDown(KeyCode.P)) AddMoney(1000);
            if (Input.GetKeyDown(KeyCode.H)) FullHeal();
            if (Input.GetKeyDown(KeyCode.T)) AddTime(60f);
            if (Input.GetKeyDown(KeyCode.B)) SpawnBoss();
            if (Input.GetKeyDown(KeyCode.Alpha1)) SetSpeed(1f);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SetSpeed(2f);
            if (Input.GetKeyDown(KeyCode.Alpha5)) SetSpeed(5f);
        }
    }

    public void ToggleMenu()
    {
        showMenu = !showMenu;
        ShowToast(showMenu ? "🛠️ Dev Konsolu Açıldı" : "🛠️ Dev Konsolu Kapatıldı");
    }

    public void ToggleGodMode()
    {
        isGodMode = !isGodMode;
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null) player.isGodMode = isGodMode;
        ShowToast(isGodMode ? "🛡️ GOD MODE: AÇIK (Ölümsüzlük Aktif)" : "🛡️ GOD MODE: KAPALI");
    }

    public void KillAllEnemies()
    {
        if (EnemyPool.Instance != null)
        {
            List<GameObject> activeEnemies = EnemyPool.Instance.GetAllActiveEnemies();
            int count = activeEnemies.Count;
            foreach (GameObject obj in activeEnemies)
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Die(); // Hem tohum düşürsün hem ölsün
                }
                else
                {
                    obj.SetActive(false);
                }
            }
            ShowToast($"💀 {count} Adet Düşman Yok Edildi!");
        }
        else
        {
            ShowToast("⚠️ EnemyPool bulunamadı!");
        }
    }

    public void ForceLevelUp()
    {
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.ForceLevelUp();
            ShowToast("⬆️ +1 Seviye Atlandı! Yetenek Seçimi Açıldı.");
        }
        else
        {
            ShowToast("⚠️ Sahnede ExperienceManager yok!");
        }
    }

    public void AddMoney(int amount)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoreSeed(amount);
            ShowToast($"💰 +{amount} Çekirdek Tohum (Para) Eklendi!");
            if (BaseUIManager.Instance != null) BaseUIManager.Instance.UpdateCurrencyUI();
        }
        else
        {
            ShowToast($"💰 +{amount} Tohum Eklendi (GameManager Yok)");
        }
    }

    public void ResetMoney()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetAllSeeds();
            ShowToast("🧹 Tohumlar Sıfırlandı (0 Seed)!");
            if (BaseUIManager.Instance != null) BaseUIManager.Instance.UpdateCurrencyUI();
        }
        else
        {
            ShowToast("🧹 Tohumlar Sıfırlandı!");
        }
    }

    public void FullHeal()
    {
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null)
        {
            player.Heal(player.maxHealth);
            ShowToast("🏥 Can Tamamen Fullendi (100/100)!");
        }
        else
        {
            ShowToast("⚠️ Sahnede Sylva / PlayerHealth bulunamadı!");
        }
    }

    public void AddTime(float seconds)
    {
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.AddSeconds(seconds);
            ShowToast($"⏩ Süre +{seconds:F0}sn İleri Sarıldı! (Wave: {EnemySpawner.CurrentWave})");
        }
        else
        {
            ShowToast("⚠️ Sahnede EnemySpawner bulunamadı!");
        }
    }

    public void SpawnBoss()
    {
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.JumpToBoss();
            ShowToast("🌲 WAVE 20: BOSS (CHINAR) İNDİRİLDİ!");
        }
        else
        {
            ShowToast("⚠️ EnemySpawner bulunamadı!");
        }
    }

    public void SetSpeed(float speed)
    {
        customGameSpeed = speed;
        Time.timeScale = speed;
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.gameSpeedMultiplier = speed;
        }
        ShowToast($"⚡ Oyun Hızı: {speed}x");
    }

    void ShowToast(string message)
    {
        lastMessage = message;
        messageTimer = 3.5f;
        Debug.Log("[DEV CHEAT] " + message);
    }

    // --- ONGUI ŞIK GELİŞTİRİCİ ARAYÜZÜ ---
    void InitStyles()
    {
        if (stylesInitialized) return;

        // Pencere Arka Planı
        windowStyle = new GUIStyle(GUI.skin.window);
        Texture2D winBg = MakeTex(2, 2, new Color(0.08f, 0.12f, 0.10f, 0.92f));
        windowStyle.normal.background = winBg;
        windowStyle.normal.textColor = new Color(0.4f, 1f, 0.6f);
        windowStyle.fontStyle = FontStyle.Bold;
        windowStyle.fontSize = 13;

        // Başlıklar
        headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = 12;

        // İstatistikler
        statLabelStyle = new GUIStyle(GUI.skin.label);
        statLabelStyle.normal.textColor = new Color(0.8f, 0.9f, 0.8f);
        statLabelStyle.fontSize = 11;

        // Standart Buton
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.background = MakeTex(2, 2, new Color(0.18f, 0.25f, 0.22f, 0.9f));
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fontSize = 11;

        // Aktif (Yeşil) Buton
        activeButtonStyle = new GUIStyle(GUI.skin.button);
        activeButtonStyle.normal.background = MakeTex(2, 2, new Color(0.15f, 0.65f, 0.35f, 0.95f));
        activeButtonStyle.normal.textColor = Color.white;
        activeButtonStyle.fontStyle = FontStyle.Bold;
        activeButtonStyle.fontSize = 11;

        // Uyarı (Kırmızı) Buton
        warningButtonStyle = new GUIStyle(GUI.skin.button);
        warningButtonStyle.normal.background = MakeTex(2, 2, new Color(0.7f, 0.2f, 0.2f, 0.95f));
        warningButtonStyle.normal.textColor = Color.white;
        warningButtonStyle.fontStyle = FontStyle.Bold;
        warningButtonStyle.fontSize = 11;

        // Toast Bildirim
        toastStyle = new GUIStyle(GUI.skin.box);
        toastStyle.normal.background = MakeTex(2, 2, new Color(0.05f, 0.2f, 0.1f, 0.85f));
        toastStyle.normal.textColor = new Color(0.4f, 1f, 0.5f);
        toastStyle.fontSize = 11;
        toastStyle.fontStyle = FontStyle.Bold;
        toastStyle.alignment = TextAnchor.MiddleCenter;

        stylesInitialized = true;
    }

    void OnGUI()
    {
        InitStyles();

        // 1. Ekrandaki Küçük Şık [⚡ DEV] Açma/Kapama Butonu
        if (showFloatingDevButton)
        {
            if (GUI.Button(new Rect(10, 10, 80, 26), showMenu ? "❌ KAPAT" : "⚡ DEV", showMenu ? warningButtonStyle : activeButtonStyle))
            {
                ToggleMenu();
            }
        }

        // 2. Alt Bildirim Toast Çubuğu
        if (messageTimer > 0)
        {
            float toastWidth = 420;
            float toastX = (Screen.width - toastWidth) / 2f;
            GUI.Box(new Rect(toastX, Screen.height - 45, toastWidth, 30), lastMessage, toastStyle);
        }

        // 3. Ana Sexy Debug Menü Penceresi
        if (showMenu)
        {
            windowRect = GUI.Window(9999, windowRect, DrawDevWindow, "🌿 FLORA DEV CHEATS & STATS", windowStyle);
        }
    }

    void DrawDevWindow(int windowID)
    {
        GUILayout.Space(5);

        // --- İSTATİSTİKLER PANELİ ---
        GUILayout.Label("📊 CANLI DURUM & PERFORMANS", headerStyle);
        
        string fpsColor = fps >= 45 ? "<color=#4dff88>" : (fps >= 25 ? "<color=#ffea4d>" : "<color=#ff4d4d>");
        int activeEnemies = EnemyPool.Instance != null ? EnemyPool.Instance.GetActiveEnemyCount() : 0;
        int currentWave = EnemySpawner.CurrentWave;
        float gameTime = EnemySpawner.GameTimer;
        int mins = Mathf.FloorToInt(gameTime / 60f);
        int secs = Mathf.FloorToInt(gameTime % 60f);
        int seeds = GameManager.Instance != null ? (GameManager.Instance.coreSeedCount + GameManager.Instance.currentRunCoreSeedCount) : 0;

        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        string hpText = player != null ? $"{player.name}: Canlı" : "Karakter Yok";

        GUILayout.Label($"FPS: {fpsColor}{fps:F0}</color> | Düşman: <b>{activeEnemies}</b> | Wave: <b>{currentWave}</b> ({mins:00}:{secs:00})", statLabelStyle);
        GUILayout.Label($"Bakiye: <b>{seeds}</b> Tohum | Durum: <b>{hpText}</b>", statLabelStyle);

        GUILayout.Space(8);
        GUILayout.Label("⚡ HIZLI HİLELER & AKSİYONLAR", headerStyle);

        // God Mode Butonu
        GUIStyle godBtnStyle = isGodMode ? activeButtonStyle : buttonStyle;
        string godBtnText = isGodMode ? "🛡️ GOD MODE: AKTİF [G]" : "🛡️ GOD MODE: KAPALI [G]";
        if (GUILayout.Button(godBtnText, godBtnStyle, GUILayout.Height(28)))
        {
            ToggleGodMode();
        }

        // Kill All Butonu
        if (GUILayout.Button("💀 TÜM DÜŞMANLARI YOK ET [K]", warningButtonStyle, GUILayout.Height(28)))
        {
            KillAllEnemies();
        }

        // Level Up Butonu
        if (GUILayout.Button("⬆️ ANINDA SEVİYE ATLA (+1 LEVEL) [L]", buttonStyle, GUILayout.Height(28)))
        {
            ForceLevelUp();
        }

        // Para Ekle / Sıfırla Butonları
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("💰 +1000 TOHUM [P]", buttonStyle, GUILayout.Height(28)))
        {
            AddMoney(1000);
        }
        if (GUILayout.Button("🧹 SIFIRLA (0 SEED)", warningButtonStyle, GUILayout.Height(28)))
        {
            ResetMoney();
        }
        GUILayout.EndHorizontal();

        // Full Heal Butonu
        if (GUILayout.Button("🏥 CANI FULLE (100/100) [H]", buttonStyle, GUILayout.Height(28)))
        {
            FullHeal();
        }

        // +60sn İleri Sar
        if (GUILayout.Button("⏩ +60 SANİYE İLERİ SAR (2 DALGA) [T]", buttonStyle, GUILayout.Height(28)))
        {
            AddTime(60f);
        }

        // Wave 20 Boss Çağır
        if (GUILayout.Button("🌲 DİREKT WAVE 20: BOSS ÇAĞIR [B]", activeButtonStyle, GUILayout.Height(28)))
        {
            SpawnBoss();
        }

        GUILayout.Space(6);
        GUILayout.Label("⏱️ OYUN HIZI (GAME SPEED)", headerStyle);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("1x", customGameSpeed == 1f ? activeButtonStyle : buttonStyle, GUILayout.Height(24))) SetSpeed(1f);
        if (GUILayout.Button("2x", customGameSpeed == 2f ? activeButtonStyle : buttonStyle, GUILayout.Height(24))) SetSpeed(2f);
        if (GUILayout.Button("3x", customGameSpeed == 3f ? activeButtonStyle : buttonStyle, GUILayout.Height(24))) SetSpeed(3f);
        if (GUILayout.Button("5x", customGameSpeed == 5f ? activeButtonStyle : buttonStyle, GUILayout.Height(24))) SetSpeed(5f);
        GUILayout.EndHorizontal();

        GUILayout.Space(6);
        if (GUILayout.Button("🚪 BASE'E DÖN (END RUN)", buttonStyle, GUILayout.Height(24)))
        {
            if (GameManager.Instance != null) GameManager.Instance.LoadBaseScene();
        }

        // Pencereyi sürüklenebilir yap
        GUI.DragWindow(new Rect(0, 0, 10000, 30));
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
