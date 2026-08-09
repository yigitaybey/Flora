using UnityEngine;
using TMPro; // TextMeshPro için

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    public float floatSpeed = 2f;
    public float fadeSpeed = 2f;
    private Color textColor;
    private float disappearTimer;
    private Vector3 baseScale; // Prefab'ın orijinal boyutunu hafızada tut

    void Awake()
    {
        // 3D dünyada yazı göstermek için TextMeshPro (UI olmayan) componentini al
        textMesh = GetComponent<TextMeshPro>();
        baseScale = transform.localScale; // Unity'de ayarladığın orijinal boyutu kaydet
    }

    public void Setup(float amount, bool isHeal = false, bool isCrit = false)
    {
        // Önceki boyutu sıfırla (Ama Vector3.one yerine orijinal prefab boyutuna dön)
        transform.localScale = baseScale;

        if (isHeal)
        {
            textMesh.text = "+" + amount.ToString("F0");
            textMesh.color = Color.green;
        }
        else if (isCrit)
        {
            textMesh.text = amount.ToString("F0") + "!";
            textMesh.color = Color.yellow; // Kritik vuruş sarı olsun
            transform.localScale = baseScale * 1.5f; // Orijinal boyutun 1.5 katı olsun
        }
        else
        {
            textMesh.text = amount.ToString("F0");
            textMesh.color = Color.red; // Normal hasar kırmızı
        }

        textColor = textMesh.color;
        textColor.a = 1f;
        textMesh.color = textColor;
        disappearTimer = 1f; // 1 saniye sonra yok olmaya başlasın
    }

    void Update()
    {
        // Yukarı doğru süzül
        transform.position += new Vector3(0, floatSpeed, 0) * Time.deltaTime;

        // Zamanla saydamlaş (Fade Out)
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <= 0)
            {
                gameObject.SetActive(false); // Havuza geri gönder
            }
        }
    }
}
