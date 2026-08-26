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

    void Start()
    {
        // Ana kamerayı bulup değişkene atıyoruz
        mainCamera = Camera.main;
        // Sahnede Joystick varsa otomatik bul
        joystick = FindFirstObjectByType<FloatingJoystick>();
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
        Vector3 camForward = mainCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = mainCamera.transform.right;
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
    }

    // YETENEK SİSTEMİ: Karakterin yürüme hızını artırır
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log("Hız Artırıldı! Yeni Hız: " + moveSpeed);
    }
}
