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
    private CanvasGroup backgroundCanvasGroup;

    void Start()
    {
        if (background != null)
        {
            backgroundCanvasGroup = background.GetComponent<CanvasGroup>();
            if (backgroundCanvasGroup == null)
            {
                backgroundCanvasGroup = background.gameObject.AddComponent<CanvasGroup>();
            }
            backgroundCanvasGroup.alpha = 0f; // Başlangıçta gizle
            backgroundCanvasGroup.blocksRaycasts = false; // Tıklamayı engellemesin
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (background != null)
        {
            // Ekranda dokunulan yere joystick'i taşı ve görünür yap
            background.position = eventData.position;
            if (handle != null) handle.anchoredPosition = Vector2.zero;
            if (backgroundCanvasGroup != null) backgroundCanvasGroup.alpha = 1f;
        }
        OnDrag(eventData); // Anında hareketi başlat
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null) return;

        // Parmağın joystick merkezine göre mesafesini hesapla
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position);

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
        // Parmak çekildiğinde sıfırla ve gizle
        inputVector = Vector2.zero;
        if (handle != null) handle.anchoredPosition = Vector2.zero;
        if (backgroundCanvasGroup != null) backgroundCanvasGroup.alpha = 0f;
    }

    // Dışarıdan okumak için
    public float Horizontal() { return inputVector.x; }
    public float Vertical() { return inputVector.y; }
}
