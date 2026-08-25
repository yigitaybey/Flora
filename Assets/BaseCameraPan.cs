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
            
            // YENİ: Kameranın açısına (Y rotasyonuna) göre kaydırma (Çapraz/İzometrik destekli)
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0;
            right.Normalize();

            // Fare hareketini dünya yönlerine uyarla
            Vector3 move = (right * -pos.x * panSpeed) + (forward * -pos.y * panSpeed);
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
            // Tekerlek değerini normalize edelim (-1, 0, 1) ki fare çok hızlı çevrildiğinde kamera fırlamasın
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0.1f) zoomDelta = zoomSpeedMouse * Time.deltaTime;
            else if (scroll < -0.1f) zoomDelta = -zoomSpeedMouse * Time.deltaTime;
        }

        // 2. MOBİL ÇİFT PARMAK (PINCH)
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
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

                zoomDelta = (currentMag - prevMag) * zoomSpeedTouch * Time.deltaTime;
            }
        }

        // ZOOM UYGULAMA
        if (Mathf.Abs(zoomDelta) > 0.01f)
        {
            if (cam.orthographic)
            {
                // İZOMETRİK (Orthographic) ZOOM:
                cam.orthographicSize -= zoomDelta;
                // Orthographic için Inspector'daki zoomLimit değerlerini (örn: 10 ile 80 arası) kullanabilirsin.
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomLimit.x / 10f, zoomLimit.y / 10f); 
            }
            else
            {
                // PERSPEKTİF ZOOM: Fiziksel olarak kamerayı ileri/geri götürür.
                Vector3 targetPos = transform.position + (transform.forward * zoomDelta);
                
                // Yüksekliği (Y ekseni) tam olarak sınırlara (zoomLimit) dayamak için matematiksel hesap:
                if (targetPos.y < zoomLimit.x)
                {
                    float diffY = zoomLimit.x - transform.position.y;
                    targetPos = transform.position + (transform.forward * (diffY / transform.forward.y));
                }
                else if (targetPos.y > zoomLimit.y)
                {
                    float diffY = zoomLimit.y - transform.position.y;
                    targetPos = transform.position + (transform.forward * (diffY / transform.forward.y));
                }

                Vector3 diff = targetPos - transform.position;
                transform.position = targetPos;
                
                // BUG FIX: Kaydırma merkezi (startPos) güncellemesi
                // Eğer ileride kamerayı sağa/sola döndürürsek (Y rotasyonu) X ekseninde de ilerler,
                // bu yüzden hem X hem Z merkezini güncellemeliyiz.
                startPos.x += diff.x;
                startPos.z += diff.z;
            }
        }
    }
}
