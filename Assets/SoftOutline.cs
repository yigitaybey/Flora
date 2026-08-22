using UnityEngine;
using UnityEngine.Rendering;

public class SoftOutline : MonoBehaviour
{
    [Header("Çizgi Ayarları")]
    [Tooltip("Çizginin Rengi (Flora'nın ruhuna uygun yumuşak bir yeşil/mavi)")]
    public Color outlineColor = new Color(0.5f, 0.8f, 0.6f, 1f); 
    
    [Tooltip("Çizginin Kalınlığı (0.01 ile 0.05 arası idealdir - Dikkat: Artık 1.02 değil, 0.02 gibi ufak sayılar kullanmalısın!)")]
    public float outlineWidth = 0.02f;

    private GameObject outlineObj;

    void Start()
    {
        CreateOutline();
    }

    void CreateOutline()
    {
        // Kendi üzerindeki mesh (3D model) yapısını al
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        // Eğer bu objede bir model yoksa boşuna çalışma
        if (mf == null || mr == null) return;

        // Dış çizgi için yeni bir alt obje oluştur
        outlineObj = new GameObject("Flora_Outline");
        outlineObj.transform.SetParent(transform);
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one; // Transform boyutunu ellemiyoruz!

        // Yeni objeye Mesh Filter ekle
        MeshFilter outMf = outlineObj.AddComponent<MeshFilter>();
        
        // --- YENİ YÖNTEM: PİVOT KAYMASINI ENGELLEMEK İÇİN NORMALLERİ ŞİŞİR ---
        Mesh originalMesh = mf.mesh;
        Mesh inflatedMesh = new Mesh();
        inflatedMesh.name = originalMesh.name + "_Outline";
        
        Vector3[] vertices = originalMesh.vertices;
        Vector3[] normals = originalMesh.normals;
        
        // Köşeleri kendi "Baktığı yöne (Normal)" doğru şişir
        if (normals.Length == vertices.Length)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += normals[i] * outlineWidth;
            }
        }
        
        inflatedMesh.vertices = vertices;
        inflatedMesh.triangles = originalMesh.triangles;
        inflatedMesh.normals = normals;
        outMf.mesh = inflatedMesh;

        // Yeni objeye bir çizici (Renderer) ekle
        MeshRenderer outMr = outlineObj.AddComponent<MeshRenderer>();
        
        // --- İŞİN SIRRI BURADA ---
        // Işıktan etkilenmeyen (Unlit) düz bir materyal oluştur.
        Material outlineMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        
        // CullMode.Front -> Modelin "Dış" yüzeylerini sil, sadece "İç" yüzeylerini göster.
        // Bu sayede modelin kendisi, bu yeni büyütülmüş modelin önünü kapatır ve biz sadece kenarlardan taşan kısımları "dış çizgi" olarak görürüz.
        outlineMat.SetInt("_Cull", (int)CullMode.Front); 
        
        // Rengini ayarla
        outlineMat.SetColor("_BaseColor", outlineColor);
        
        // Materyali objeye ata
        outMr.material = outlineMat;
    }
}
