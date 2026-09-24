using UnityEngine;

/// <summary>
/// RevenueCat API Key'lerini güvenli şekilde tutan ScriptableObject.
/// Unity'de Assets > Create > Flora > RevenueCat Config ile oluşturulur.
/// ÖNEMLİ: Bu dosya .gitignore'da olmalıdır!
/// </summary>
[CreateAssetMenu(fileName = "RevenueCatConfig", menuName = "Flora/RevenueCat Config")]
public class RevenueCatConfig : ScriptableObject
{
    [Header("RevenueCat Public SDK Keys")]
    [Tooltip("RevenueCat Dashboard > API Keys > Public SDK Key (appl_ ile başlar)")]
    public string appleAPIKey = "";

    [Tooltip("RevenueCat Dashboard > API Keys > Public SDK Key (goog_ ile başlar)")]
    public string googleAPIKey = "";
}
