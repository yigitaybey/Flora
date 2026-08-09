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

    private Camera cam;
    private float baseOrthographicSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic)
        {
            baseOrthographicSize = cam.orthographicSize;
        }
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

        if (cam != null && cam.orthographic)
        {
            // İZOMETRİK (Orthographic) Kamera için uzaklaşma işlemi "Size" büyütülerek yapılır
            float targetSize = baseOrthographicSize * targetZoomMultiplier;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);

            // Kameranın konumu her zaman sabit offset'te kalabilir
            Vector3 desiredPosition = target.position + baseOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            // 3D PERSPEKTİF Kamera kullanılıyorsa, kamerayı fiziksel olarak karakterden uzaklaştır
            Vector3 targetOffset = baseOffset * targetZoomMultiplier;
            Vector3 desiredPosition = target.position + targetOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
