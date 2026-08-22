using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BoxOutline : MonoBehaviour
{
    [Header("Kutu Çizgi Ayarları")]
    public Color lineColor = new Color(0.8f, 0.6f, 0.1f, 1f); // Altın sarısı/Turuncu
    public float lineWidth = 0.05f;
    
    private GameObject lineObj;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) return;

        // Çizgi için yeni bir obje oluştur
        lineObj = new GameObject("Flora_BoxOutline");
        lineObj.transform.SetParent(transform);
        lineObj.transform.localPosition = Vector3.zero;
        lineObj.transform.localRotation = Quaternion.identity;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        
        // URP Unlit materyali kullan (parlak durması için)
        Material lineMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        lineMat.SetColor("_BaseColor", lineColor);
        lr.material = lineMat;

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 16; // Kutuyu çizmek için gereken köşe sayısı
        lr.loop = true;
        lr.useWorldSpace = true; // Sınırları dünya koordinatlarında çizeceğiz

        // Objenin sınırlarını (Bounding Box) al
        Bounds b = rend.bounds;
        
        // Sınırları birazcık şişir ki objenin içine girmesin
        b.Expand(0.1f);

        // Kutunun 8 köşesini hesapla
        Vector3 p0 = new Vector3(b.min.x, b.min.y, b.min.z);
        Vector3 p1 = new Vector3(b.max.x, b.min.y, b.min.z);
        Vector3 p2 = new Vector3(b.max.x, b.min.y, b.max.z);
        Vector3 p3 = new Vector3(b.min.x, b.min.y, b.max.z);
        
        Vector3 p4 = new Vector3(b.min.x, b.max.y, b.min.z);
        Vector3 p5 = new Vector3(b.max.x, b.max.y, b.min.z);
        Vector3 p6 = new Vector3(b.max.x, b.max.y, b.max.z);
        Vector3 p7 = new Vector3(b.min.x, b.max.y, b.max.z);

        // Çizgiyi sırayla köşelerden geçirerek 3D kutuyu (Hologramı) çiz
        Vector3[] points = new Vector3[]
        {
            p0, p1, p2, p3, p0, // Alt kareyi çiz
            p4, p5, p6, p7, p4, // Üst kareyi çiz
            p5, p1,             // Aşağı in
            p2, p6,             // Yukarı çık
            p7, p3              // Aşağı in
        };

        lr.SetPositions(points);
    }
}
