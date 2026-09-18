using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCamera;

    [Header("Animasyon ve Görsel")]
    public Animator animator;
    public Transform characterModel;

    private FloatingJoystick joystick;
    public Vector3 lastMoveDirection { get; private set; } = Vector3.right; // Varsayılan olarak sağa baksın

    public enum BoundaryType { Circle, NavMesh, Rectangle }

    [Header("Harita Sınırları (Görünmez Duvarlar)")]
    public bool useMapBounds = true;
    public BoundaryType boundaryType = BoundaryType.Circle;

    [Header("Dairesel Ada Sınırı (Organik Haritalar İçin İdeal)")]
    public Vector3 islandCenter = Vector3.zero;
    public float islandRadius = 35f;

    [Header("Kutu Sınırı")]
    public Vector2 minBounds = new Vector2(-40f, -40f);
    public Vector2 maxBounds = new Vector2(40f, 40f);

    private Vector3 previousValidPosition;

    void Start()
    {
        // Ana kamerayı bulup değişkene atıyoruz
        mainCamera = Camera.main;
        // Sahnede Joystick varsa otomatik bul
        joystick = FindFirstObjectByType<FloatingJoystick>();
        previousValidPosition = transform.position;
    }

    void Update()
    {
        Vector3 input = Vector3.zero;

        // 1. Önce PC (Klavye) kontrolü (Senin test etmen için)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input += Vector3.forward;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input += Vector3.back;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input += Vector3.right;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input += Vector3.left;
        }

        // 2. Eğer ekranda Joystick kullanılıyorsa, klavye girdisini ez (Mobil için)
        if (joystick != null && (joystick.Horizontal() != 0 || joystick.Vertical() != 0))
        {
            input = new Vector3(joystick.Horizontal(), 0f, joystick.Vertical());
        }

        if (input.magnitude > 0)
        {
            input.Normalize();
        }

        // Kameranın baktığı yönü hesaplıyoruz (Y eksenini yani yüksekliği sıfırlıyoruz ki havaya uçmasın)
        Vector3 camForward = mainCamera != null ? mainCamera.transform.forward : Vector3.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = mainCamera != null ? mainCamera.transform.right : Vector3.right;
        camRight.y = 0;
        camRight.Normalize();

        // Bastığımız tuşları kameranın bakış açısına göre büküyoruz (İzometrik hareket sihri burada)
        Vector3 movement = (camForward * input.z) + (camRight * input.x);

        if (movement.magnitude > 0.01f)
        {
            lastMoveDirection = movement.normalized;
            
            // Yürüme animasyonunu tetikle
            if (animator != null)
            {
                animator.SetFloat("Speed", movement.magnitude);
            }

            // Modeli hareket yönüne doğru döndür (Sadece modeli döndürüyoruz ki silahlar bozulmasın)
            if (characterModel != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
        else
        {
            // Durma animasyonuna geç
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
        }

        // Karakteri hareket ettiriyoruz
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // --- MATEMATİKSEL GÖRÜNMEZ DUVAR (BOUNDS CLAMP) ---
        if (useMapBounds)
        {
            ApplyMapBounds();
        }
    }

    void ApplyMapBounds()
    {
        if (boundaryType == BoundaryType.Circle)
        {
            // Dairesel sınır: Merkezden olan mesafeyi yarıçap ile sınırla
            Vector3 offset = transform.position - islandCenter;
            offset.y = 0f; // Yüksekliği sıfırla
            if (offset.magnitude > islandRadius)
            {
                Vector3 clamped = islandCenter + offset.normalized * islandRadius;
                clamped.y = transform.position.y;
                transform.position = clamped;
            }
        }
        else if (boundaryType == BoundaryType.NavMesh)
        {
            // NavMesh sınır: Karakterin harita dışına çıkmasını engeller
            // 1.5f arama yarıçapı zemin eğimlerinde veya karakterin -1 olan Y yüksekliğinde false verip oyuncuyu kilitliyordu.
            // Yarıçapı 5.0f yaparak ve Y toleransını düzelterek görünmez duvar hissini tamamen kaldırdık!
            UnityEngine.AI.NavMeshHit hit;
            Vector3 testPos = new Vector3(transform.position.x, previousValidPosition.y, transform.position.z);
            if (UnityEngine.AI.NavMesh.SamplePosition(testPos, out hit, 5.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                // Karakteri zemine oturt
                transform.position = new Vector3(transform.position.x, hit.position.y, transform.position.z);
                previousValidPosition = transform.position;
            }
            else
            {
                // Yalnızca harita sınırının tamamen dışına çıkıldıysa geri çek
                transform.position = previousValidPosition;
            }
        }
        else if (boundaryType == BoundaryType.Rectangle)
        {
            // Kutu sınır
            Vector3 clampedPos = transform.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, minBounds.x, maxBounds.x);
            clampedPos.z = Mathf.Clamp(clampedPos.z, minBounds.y, maxBounds.y);
            transform.position = clampedPos;
        }
    }

    // Unity Scene ekranında sınırları görsel olarak sarı/yeşil renkle çizer
    private void OnDrawGizmosSelected()
    {
        if (!useMapBounds) return;

        Gizmos.color = Color.yellow;

        if (boundaryType == BoundaryType.Circle)
        {
            // Daire çiz
            int segments = 40;
            float angleStep = 360f / segments;
            Vector3 prevPoint = islandCenter + new Vector3(Mathf.Cos(0) * islandRadius, 0.5f, Mathf.Sin(0) * islandRadius);

            for (int i = 1; i <= segments; i++)
            {
                float rad = i * angleStep * Mathf.Deg2Rad;
                Vector3 nextPoint = islandCenter + new Vector3(Mathf.Cos(rad) * islandRadius, 0.5f, Mathf.Sin(rad) * islandRadius);
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }
        else if (boundaryType == BoundaryType.Rectangle)
        {
            Vector3 center = new Vector3((minBounds.x + maxBounds.x) * 0.5f, transform.position.y, (minBounds.y + maxBounds.y) * 0.5f);
            Vector3 size = new Vector3(Mathf.Abs(maxBounds.x - minBounds.x), 2f, Mathf.Abs(maxBounds.y - minBounds.y));
            Gizmos.DrawWireCube(center, size);
        }
    }

    // YETENEK SİSTEMİ: Karakterin yürüme hızını artırır
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log("Hız Artırıldı! Yeni Hız: " + moveSpeed);
    }
}
