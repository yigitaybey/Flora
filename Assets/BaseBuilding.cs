using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BaseBuilding : MonoBehaviour
{
    public enum BuildingType { RadyoKulesi, Atolye }
    public BuildingType type;

    void Update()
    {
        // Yeni Input Sistemi ile sol tıklamayı (veya mobil dokunmayı) algıla
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Eğer fare bir UI (Arayüz/Menü) üzerindeyse binalara tıklamayı yoksay
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // Kameradan farenin olduğu yere sanal bir ışın (Ray) yolla
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // Eğer ışın bir şeye çarptıysa VE o çarptığı şey "bu" binaysa
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    OnBuildingClicked();
                }
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
