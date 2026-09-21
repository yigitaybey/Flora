using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Flora Asenkron Yükleme Ekranı (Asynchronous Loading Screen).
/// Sahneler (BaseScene <-> CombatScene) arası geçerken donmayı engeller,
/// pürüzsüz kararma, yükleme barı ve doğa temalı ipuçları gösterir.
/// Tamamen otomatik çalışır; sahnede canvas olmasa bile kendi UI'ını koddan kurar.
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen instance;
    public static LoadingScreen Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("LoadingScreen_Manager");
                instance = go.AddComponent<LoadingScreen>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Slider progressBar;
    private Text progressText;
    private Text titleText;
    private Text tipText;

    private readonly string[] gameplayTips = new string[]
    {
        "Tip: SporeHead enemies swell up before exploding. Keep your distance!",
        "Tip: Upgrading your Magnet allows you to absorb XP seeds from further away.",
        "Tip: Iyv shoots toxic spit from afar. Keep moving to dodge incoming projectiles.",
        "Tip: Core Seeds are permanent currency used to upgrade Sylva in the Radio Tower.",
        "Tip: Defeat the Elite at Wave 5 to earn massive XP and high-tier seeds.",
        "Tip: Slaying Boss Chinar at Wave 10 purifies the corrupted forest."
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CreateLoadingUI();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Herhangi bir scriptten tek satırla asenkron sahne yükler.
    /// Örn: LoadingScreen.LoadScene("CombatScene", "Entering Corrupted Forest...");
    /// </summary>
    public static void LoadScene(string sceneName, string customTitle = null)
    {
        Instance.StartCoroutine(Instance.LoadSceneAsyncRoutine(sceneName, customTitle));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName, string customTitle)
    {
        // Zamanı garantiye al (Game Over veya Pause'da kalmış olabilir)
        Time.timeScale = 1f;

        // UI Metinlerini Güncelle
        if (titleText != null)
        {
            titleText.text = string.IsNullOrEmpty(customTitle) ? "LOADING..." : customTitle.ToUpper();
        }
        if (tipText != null)
        {
            tipText.text = gameplayTips[Random.Range(0, gameplayTips.Length)];
        }
        if (progressBar != null) progressBar.value = 0f;
        if (progressText != null) progressText.text = "0%";

        canvas.gameObject.SetActive(true);

        // 1. Kararma / Açılma (Fade In)
        float fadeDuration = 0.25f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 2. Asenkron Sahne Yüklemesi Başlat
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Bar dolana kadar sahneyi hemen açma

        float targetProgress = 0f;
        float displayedProgress = 0f;

        // Unity AsyncOperation 0.0 ile 0.9 arasında yükleme yapar (0.9 = hazır)
        while (displayedProgress < 1f)
        {
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.unscaledDeltaTime * 1.5f);

            if (progressBar != null) progressBar.value = displayedProgress;
            if (progressText != null) progressText.text = $"{Mathf.RoundToInt(displayedProgress * 100f)}%";

            if (operation.progress >= 0.9f && displayedProgress >= 0.99f)
            {
                displayedProgress = 1f;
                if (progressBar != null) progressBar.value = 1f;
                if (progressText != null) progressText.text = "100%";
                yield return new WaitForSecondsRealtime(0.15f); // Kısa tatlı bir bekleme
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Sahnenin tam olarak oturmasını bekle
        while (!operation.isDone)
        {
            yield return null;
        }

        // 3. Kararmadan Çıkış (Fade Out)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sahnede önceden UI çizilmemişse, koddan tam ekran şık ve koyu yeşil/siyah bir Loading Canvas'ı üretir.
    /// </summary>
    private void CreateLoadingUI()
    {
        // 1. Root Canvas
        GameObject canvasGo = new GameObject("LoadingScreen_Canvas");
        canvasGo.transform.SetParent(transform);
        canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Her şeyin en üstünde görünsün

        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGo.AddComponent<GraphicRaycaster>();
        canvasGroup = canvasGo.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // 2. Koyu Post-Apokaliptik Arka Plan (Derin Yeşil-Siyah)
        GameObject bgGo = new GameObject("Background");
        bgGo.transform.SetParent(canvasGo.transform, false);
        Image bgImg = bgGo.AddComponent<Image>();
        bgImg.color = new Color(0.04f, 0.07f, 0.05f, 1f); // Doğal karanlık orman tonu
        RectTransform bgRect = bgGo.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        // 2.5. Tavandan Sarkan Doğa Sarmaşıkları (Yusuf'un Çizdiği Doğa Deseni)
        Sprite vineSprite = GetVineSprite();
        if (vineSprite != null)
        {
            GameObject vineGo = new GameObject("HangingVines_Top");
            vineGo.transform.SetParent(canvasGo.transform, false);
            Image vineImg = vineGo.AddComponent<Image>();
            vineImg.sprite = vineSprite;
            vineImg.preserveAspect = true;
            vineImg.color = Color.white;
            RectTransform vineRect = vineGo.GetComponent<RectTransform>();
            vineRect.anchorMin = new Vector2(0f, 1f);
            vineRect.anchorMax = new Vector2(1f, 1f);
            vineRect.pivot = new Vector2(0.5f, 1f);
            vineRect.anchoredPosition = Vector2.zero;
            vineRect.sizeDelta = new Vector2(0, 340); // Tavandan 340 birim aşağı sarksın
        }

        // 3. Başlık Metni (Örn: "ENTERING CORRUPTED FOREST...")
        GameObject titleGo = new GameObject("TitleText");
        titleGo.transform.SetParent(canvasGo.transform, false);
        titleText = titleGo.AddComponent<Text>();
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 38;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.85f, 0.95f, 0.85f, 1f);
        RectTransform titleRect = titleGo.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.6f);
        titleRect.anchorMax = new Vector2(0.5f, 0.6f);
        titleRect.sizeDelta = new Vector2(1000, 60);

        // 4. İpucu Metni (Gameplay Tip)
        GameObject tipGo = new GameObject("TipText");
        tipGo.transform.SetParent(canvasGo.transform, false);
        tipText = tipGo.AddComponent<Text>();
        tipText.font = titleText.font;
        tipText.fontSize = 22;
        tipText.fontStyle = FontStyle.Italic;
        tipText.alignment = TextAnchor.MiddleCenter;
        tipText.color = new Color(0.6f, 0.75f, 0.65f, 0.9f);
        RectTransform tipRect = tipGo.GetComponent<RectTransform>();
        tipRect.anchorMin = new Vector2(0.5f, 0.45f);
        tipRect.anchorMax = new Vector2(0.5f, 0.45f);
        tipRect.sizeDelta = new Vector2(1100, 80);

        // 5. Yükleme Barı (Slider)
        GameObject sliderGo = new GameObject("ProgressBar");
        sliderGo.transform.SetParent(canvasGo.transform, false);
        progressBar = sliderGo.AddComponent<Slider>();
        progressBar.minValue = 0f;
        progressBar.maxValue = 1f;
        RectTransform sliderRect = sliderGo.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.5f, 0.25f);
        sliderRect.anchorMax = new Vector2(0.5f, 0.25f);
        sliderRect.sizeDelta = new Vector2(700, 24);

        // Slider Arka Planı
        GameObject sBgGo = new GameObject("SliderBackground");
        sBgGo.transform.SetParent(sliderGo.transform, false);
        Image sBgImg = sBgGo.AddComponent<Image>();
        sBgImg.color = new Color(0.1f, 0.15f, 0.12f, 1f);
        RectTransform sBgRect = sBgGo.GetComponent<RectTransform>();
        sBgRect.anchorMin = Vector2.zero;
        sBgRect.anchorMax = Vector2.one;
        sBgRect.sizeDelta = Vector2.zero;

        // Slider Dolgu Alanı
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderGo.transform, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.sizeDelta = Vector2.zero;

        GameObject fillGo = new GameObject("Fill");
        fillGo.transform.SetParent(fillArea.transform, false);
        Image fillImg = fillGo.AddComponent<Image>();
        fillImg.color = new Color(0.22f, 0.85f, 0.35f, 1f); // Parlak Zümrüt Yeşili
        RectTransform fillRect = fillGo.GetComponent<RectTransform>();
        fillRect.sizeDelta = Vector2.zero;

        progressBar.fillRect = fillRect;
        progressBar.targetGraphic = fillImg;

        // 6. Yüzde Metni (Örn: "75%")
        GameObject percGo = new GameObject("PercentageText");
        percGo.transform.SetParent(canvasGo.transform, false);
        progressText = percGo.AddComponent<Text>();
        progressText.font = titleText.font;
        progressText.fontSize = 20;
        progressText.fontStyle = FontStyle.Bold;
        progressText.alignment = TextAnchor.MiddleCenter;
        progressText.color = new Color(0.7f, 0.9f, 0.75f, 1f);
        RectTransform percRect = percGo.GetComponent<RectTransform>();
        percRect.anchorMin = new Vector2(0.5f, 0.20f);
        percRect.anchorMax = new Vector2(0.5f, 0.20f);
        percRect.sizeDelta = new Vector2(200, 30);

        canvas.gameObject.SetActive(false);
    }

    private Sprite GetVineSprite()
    {
        string[] searchPaths = new string[]
        {
            Application.dataPath + "/UI-20260921T232430Z-1-001/UI/Assets/Ekran Alıntısı-Photoroom.png",
            Application.dataPath + "/UI/Assets/Ekran Alıntısı-Photoroom.png",
            Application.dataPath + "/UI/Ekran Alıntısı-Photoroom.png"
        };

        foreach (string path in searchPaths)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    byte[] fileData = System.IO.File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (tex.LoadImage(fileData))
                    {
                        // Sarmaşık piksellerini canlı, taze ve açık yaprak yeşiline dönüştür
                        Color[] pixels = tex.GetPixels();
                        Color lightGreen = new Color(0.38f, 0.90f, 0.45f); // Taze, aydınlık açık yeşil (#61E673)
                        for (int i = 0; i < pixels.Length; i++)
                        {
                            if (pixels[i].a > 0.03f)
                            {
                                float lum = (pixels[i].r + pixels[i].g + pixels[i].b) / 3f;
                                float tone = Mathf.Lerp(0.85f, 1.15f, lum);
                                pixels[i] = new Color(
                                    Mathf.Clamp01(lightGreen.r * tone),
                                    Mathf.Clamp01(lightGreen.g * tone),
                                    Mathf.Clamp01(lightGreen.b * tone),
                                    pixels[i].a
                                );
                            }
                        }
                        tex.SetPixels(pixels);
                        tex.Apply();

                        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 1f));
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[LoadingScreen] Sarmaşık yüklenemedi: " + e.Message);
                }
            }
        }
        return null;
    }
}
