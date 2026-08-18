using UnityEngine;
using UnityEngine.EventSystems; // UI (Menü) tıklamasını algılamak için
using UnityEngine.InputSystem;  // YENİ EKLENDİ: Yeni Input Sistemi için

public class BaseCameraPan : MonoBehaviour
{
    [Header("Pan Ayarları (Kaydırma)")]
    public float panSpeed = 250f;
    public Vector2 panLimit = new Vector2(300f, 300f);

    [Header("Zoom Ayarları (Yakınlaşma)")]
    public float zoomSpeedMouse = 100f; // Fare tekerleği hızı (350 yükseklik için artırıldı)
    public float zoomSpeedTouch = 10f; // Çift parmak (Pinch) hızı
    public Vector2 zoomLimit = new Vector2(100f, 600f); // Kameranın min ve max Y yüksekliği

    private Vector3 dragOrigin;
    private Camera cam;
    private bool isDragging = false; 
    private Vector3 startPos; // Sınırları belirlemek için oyun başındaki ana konum

    void Start()
    {
        cam = GetComponent<Camera>();
        startPos = transform.position; // Kameranın başlangıç konumunu kaydet
        
        // Sadece X rotasyonunu 24.3 yap, pozisyonu elleme (Unity Inspector'daki kalır)
        transform.rotation = Quaternion.Euler(24.3f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        // Fare ve Dokunmatik yoksa çık
        if (Mouse.current == null && Touchscreen.current == null) return;

        // Eğer fare bir UI (Buton, Panel) üzerindeyse kamerayı kaydırma!
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            // Mobilde 2 parmakla zoom yaparken UI üzerine gelse bile engellememek için
            if (Touchscreen.current == null || !Touchscreen.current.touches[1].isInProgress)
                return;
        }

        PanCamera();
        ZoomCamera();
    }

    void PanCamera()
    {
        // Mobilde 2 parmak dokunuyorsa Pan (Kaydırma) yapma, sadece Zoom yapsın.
        if (Touchscreen.current != null && Touchscreen.current.touches[1].isInProgress)
        {
            isDragging = false;
            return;
        }

        Vector2 currentScreenPos = Vector2.zero;
        bool isInputActive = false;
        bool inputBeganThisFrame = false;

        // Farenin veya Parmağın aktiflik durumunu al
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            currentScreenPos = Mouse.current.position.ReadValue();
            isInputActive = true;
            inputBeganThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
        }
        else if (Touchscreen.current != null && Touchscreen.current.touches[0].isInProgress)
        {
            currentScreenPos = Touchscreen.current.touches[0].position.ReadValue();
            isInputActive = true;
            inputBeganThisFrame = (Touchscreen.current.touches[0].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began);
        }

        // Tıklama bu karede (Frame) başladıysa ve UI'a takılmadıysa sürüklemeyi ONAYLA
        if (inputBeganThisFrame)
        {
            isDragging = true;
            dragOrigin = cam.ScreenToViewportPoint(currentScreenPos);
        }

        // Eğer tıklama devam ediyorsa VE sürükleme onaylandıysa kaydır
        if (isInputActive && isDragging)
        {
            Vector3 pos = cam.ScreenToViewportPoint(currentScreenPos) - dragOrigin;
            
            Vector3 move = new Vector3(-pos.x * panSpeed, 0, -pos.y * panSpeed);
            Vector3 newPos = transform.position + move;
            
            // Kameranın harita dışına uçmasını engelle (Oyun başındaki konumu referans alarak)
            newPos.x = Mathf.Clamp(newPos.x, startPos.x - panLimit.x, startPos.x + panLimit.x);
            newPos.z = Mathf.Clamp(newPos.z, startPos.z - panLimit.y, startPos.z + panLimit.y);
            
            transform.position = newPos;
            dragOrigin = cam.ScreenToViewportPoint(currentScreenPos);
        }

        // Tıklama bittiyse (Parmağını/Fareyi çektiyse) onaylamayı iptal et
        if (!isInputActive)
        {
            isDragging = false;
        }
    }

    void ZoomCamera()
    {
        float zoomDelta = 0f;

        // 1. MOUSE TEKERLEĞİ (SCROLL)
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                zoomDelta = scroll * zoomSpeedMouse * Time.deltaTime;
            }
        }

        // 2. MOBİL ÇİFT PARMAK (PINCH)
        if (Touchscreen.current != null)
        {
            var t0 = Touchscreen.current.touches[0];
            var t1 = Touchscreen.current.touches[1];

            if (t0.isInProgress && t1.isInProgress)
            {
                Vector2 t0Pos = t0.position.ReadValue();
                Vector2 t1Pos = t1.position.ReadValue();
                Vector2 t0Prev = t0Pos - t0.delta.ReadValue();
                Vector2 t1Prev = t1Pos - t1.delta.ReadValue();

                float prevMag = (t0Prev - t1Prev).magnitude;
                float currentMag = (t0Pos - t1Pos).magnitude;

                // Parmaklar açılıyorsa pozitif (zoom in), kapanıyorsa negatif (zoom out)
                zoomDelta = (currentMag - prevMag) * zoomSpeedTouch * Time.deltaTime;
            }
        }

        // ZOOM UYGULAMA (Kameranın baktığı yöne doğru ilerlemesi)
        if (Mathf.Abs(zoomDelta) > 0.01f)
        {
            Vector3 targetPos = transform.position + (transform.forward * zoomDelta);
            
            // Yüksekliğe göre sınırlandır (Limitleri aşmasın)
            if (targetPos.y > zoomLimit.x && targetPos.y < zoomLimit.y)
            {
                transform.position = targetPos;
            }
        }
    }
}
