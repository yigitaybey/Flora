using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Joystick Parçaları")]
    public RectTransform background;
    public RectTransform handle;

    [Header("Ayarlar")]
    public float handleRange = 100f; // Topuzun arka plandan ne kadar uzağa gidebileceği

    private Vector2 inputVector = Vector2.zero;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Başlangıçta görünmez yap
        canvasGroup.alpha = 0f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Ekrana dokunulduğunda joystick'i parmağın olduğu yere taşı ve görünür yap
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 1f; // Görünür yap
        OnDrag(eventData); // Anında hareketi algıla
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Parmağın arka plana göre ne kadar kaydırıldığını hesapla
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position);

        // Vektörü hesapla ve menzili sınırla
        inputVector = position / (background.sizeDelta.x / 2f);
        if (inputVector.magnitude > 1f)
        {
            inputVector = inputVector.normalized;
        }

        // Topuzu hareket ettir
        handle.anchoredPosition = inputVector * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Parmak kalktığında her şeyi sıfırla ve gizle
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 0f;
    }

    // Dışarıdan okumak için
    public float Horizontal() { return inputVector.x; }
    public float Vertical() { return inputVector.y; }
}
