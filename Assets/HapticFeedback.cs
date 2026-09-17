using UnityEngine;

/// <summary>
/// Flora mobil haptic titreşim sistemi.
/// Android ve iOS cihazlarda hafif, orta ve güçlü titreşim efektleri sağlar.
/// </summary>
public static class HapticFeedback
{
    public static bool isEnabled = true;

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrator;
    private static AndroidJavaClass vibrationEffectClass;
    private static int sdkVersion = -1;
    private static bool isInitialized = false;

    private static void InitAndroid()
    {
        if (isInitialized) return;
        try
        {
            using (AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                sdkVersion = versionClass.GetStatic<int>("SDK_INT");
            }

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            }

            if (sdkVersion >= 26)
            {
                vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
            }

            isInitialized = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[HapticFeedback] Android vibrator başlatılamadı: " + e.Message);
            isInitialized = true;
        }
    }

    private static void VibrateAndroid(long milliseconds, int amplitude)
    {
        InitAndroid();
        if (vibrator == null)
        {
            Handheld.Vibrate();
            return;
        }

        try
        {
            if (sdkVersion >= 26 && vibrationEffectClass != null)
            {
                // API 26+ Android tek atımlık yumuşak titreşim (createOneShot)
                using (AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude))
                {
                    vibrator.Call("vibrate", effect);
                }
            }
            else
            {
                // Eski Android sürümleri için standart süre tabanlı titreşim
                vibrator.Call("vibrate", milliseconds);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[HapticFeedback] Titreşim hatası: " + e.Message);
            Handheld.Vibrate();
        }
    }
#endif

    /// <summary>
    /// Hafif dokunmatik titreşim (Tohum toplama, buton tıklama, menü etkileşimleri)
    /// </summary>
    public static void TriggerLight()
    {
        if (!IsHapticActive()) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        VibrateAndroid(20, 70); // 20ms, yumuşak şiddet
#elif UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }

    /// <summary>
    /// Orta şiddetli titreşim (Hasar alma, yakın dövüş darbesi)
    /// </summary>
    public static void TriggerMedium()
    {
        if (!IsHapticActive()) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        VibrateAndroid(45, 160); // 45ms, orta şiddet
#elif UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }

    /// <summary>
    /// Güçlü titreşim (Level atlama, ölüm, boss zemin darbesi)
    /// </summary>
    public static void TriggerHeavy()
    {
        if (!IsHapticActive()) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        VibrateAndroid(80, 255); // 80ms, tam güç
#elif UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }

    private static bool IsHapticActive()
    {
        if (!isEnabled) return false;
        if (GameManager.Instance != null && !GameManager.Instance.isHapticsEnabled) return false;
        return true;
    }
}
