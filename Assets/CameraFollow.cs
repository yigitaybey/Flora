using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Edilecek Hedef (Sylva'yi buraya sürükle)")]
    public Transform target;

    [Header("Kamera Ayarlari")]
    public float smoothSpeed = 5f;
    public Vector3 baseOffset = new Vector3(-10f, 15f, -10f); // İzometrik açı için iyi bir varsayılan mesafe

    [Header("Zoom Out (Level Atlama Genişlemesi)")]
    public float maxZoomOutMultiplier = 1.6f; // Max levelde kamera %60 daha geniş bir alanı görecek

    public static CameraFollow Instance;

    private Camera cam;
    private float baseOrthographicSize;

    [Header("Kamera Sınırları (Boşluğu Görmeme Koruması)")]
    public bool useCameraBounds = false; // İsterse açıp kapatabilir
    public Vector2 minCameraBounds = new Vector2(-30f, -30f); // X ve Z için minimum sınır
    public Vector2 maxCameraBounds = new Vector2(30f, 30f);   // X ve Z için maksimum sınır

    [Header("Ekran Sarsıntısı (Screen Shake)")]
    private Vector3 shakeOffset = Vector3.zero;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.08f;
    private float currentShakeStrength = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;

        cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic)
        {
            baseOrthographicSize = cam.orthographicSize;
        }
    }

    public void Shake(float duration = 0.10f, float magnitude = 0.08f)
    {
        // Ayarlardan ekran sarsıntısı kapatılmışsa sarsma
        if (GameManager.Instance != null && !GameManager.Instance.isScreenShakeEnabled) return;
        if (CombatUIManager.Instance != null && !CombatUIManager.fallbackScreenShakeEnabled) return;

        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeStrength = magnitude;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Anlık level'ı al (Maksimum 20. levele kadar kamerayı genişletmeye devam et)
        float currentLevel = 1f;
        if (ExperienceManager.Instance != null)
        {
            currentLevel = Mathf.Clamp(ExperienceManager.Instance.currentLevel, 1, 20);
        }

        // Level 1 ile 20 arasında yumuşak bir çarpan hesapla (1.0x -> 1.6x arası)
        float t = (currentLevel - 1f) / 19f;
        float targetZoomMultiplier = Mathf.Lerp(1.0f, maxZoomOutMultiplier, t);

        Vector3 desiredPosition;

        if (cam != null && cam.orthographic)
        {
            // İZOMETRİK (Orthographic) Kamera için uzaklaşma işlemi "Size" büyütülerek yapılır
            float targetSize = baseOrthographicSize * targetZoomMultiplier;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);

            // Kameranın konumu her zaman sabit offset'te kalabilir
            desiredPosition = target.position + baseOffset;
        }
        else
        {
            // 3D PERSPEKTİF Kamera kullanılıyorsa, kamerayı fiziksel olarak karakterden uzaklaştır
            Vector3 targetOffset = baseOffset * targetZoomMultiplier;
            desiredPosition = target.position + targetOffset;
        }

        // İsteğe bağlı kamera sınırlandırması (Kameranın kendisinin de dışarı kaymasını engeller)
        if (useCameraBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minCameraBounds.x, maxCameraBounds.x);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minCameraBounds.y, maxCameraBounds.y);
        }

        // Ekran Sarsıntısı (Screen Shake) hesapla - Kamera ekran düzleminde yumuşak ve tok
        if (shakeDuration > 0f)
        {
            Vector2 random2D = Random.insideUnitCircle * currentShakeStrength;
            shakeOffset = (transform.right * random2D.x) + (transform.up * random2D.y);
            shakeDuration -= Time.unscaledDeltaTime;
            currentShakeStrength = Mathf.Lerp(currentShakeStrength, 0f, 12f * Time.unscaledDeltaTime);
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime) + shakeOffset;
    }

    private void OnDrawGizmosSelected()
    {
        if (!useCameraBounds) return;

        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minCameraBounds.x + maxCameraBounds.x) * 0.5f, transform.position.y, (minCameraBounds.y + maxCameraBounds.y) * 0.5f);
        Vector3 size = new Vector3(Mathf.Abs(maxCameraBounds.x - minCameraBounds.x), 2f, Mathf.Abs(maxCameraBounds.y - minCameraBounds.y));
        Gizmos.DrawWireCube(center, size);
    }
}
