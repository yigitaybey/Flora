using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BaseBuilding : MonoBehaviour
{
    public enum BuildingType { RadyoKulesi, Atolye }
    public BuildingType type;

    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isTouching = false;
    private const float MAX_TAP_DURATION = 0.35f; // Saniye cinsinden maksimum dokunma süresi
    private const float MAX_TAP_MOVEMENT = 30f;   // Piksel cinsinden maksimum parmak kayma mesafesi

    void Update()
    {
        // 1. MOBİL / TABLET DOKUNMATİK EKRAN
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                // UI üzerindeyse dokunmayı başlatma
                int touchId = touch.touchId.ReadValue();
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touchId))
                    return;

                touchStartPos = touch.position.ReadValue();
                touchStartTime = Time.time;
                isTouching = true;
            }
            else if (isTouching && touch.press.wasReleasedThisFrame)
            {
                isTouching = false;
                Vector2 endPos = touch.position.ReadValue();
                float duration = Time.time - touchStartTime;
                float distance = Vector2.Distance(touchStartPos, endPos);

                // Eğer ekranı kaydırma (Pan) değil de binaya kısa bir dokunmaysa (Tap)
                if (duration <= MAX_TAP_DURATION && distance <= MAX_TAP_MOVEMENT)
                {
                    CheckRaycastHit(endPos);
                }
            }
        }

        // 2. PC / EDİTÖR FARE TIKLAMASI
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            CheckRaycastHit(Mouse.current.position.ReadValue());
        }
    }

    void CheckRaycastHit(Vector2 screenPosition)
    {
        if (Camera.main == null) return;

        // Kameradan dokunulan / tıklanan noktaya ışın yolla
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Eğer ışın bu binaya veya bu binanın alt parçalarına çarptıysa
            if (hit.transform == this.transform || hit.transform.IsChildOf(this.transform))
            {
                OnBuildingClicked();
            }
        }
    }

    // Tıklanma başarılı olduğunda çalışacak kod
    void OnBuildingClicked()
    {
        if (BaseUIManager.Instance != null)
        {
            if (type == BuildingType.RadyoKulesi)
            {
                BaseUIManager.Instance.OpenWorldMap();
            }
            else if (type == BuildingType.Atolye)
            {
                BaseUIManager.Instance.OpenWorkshop();
            }
        }
    }
}
