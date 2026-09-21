using UnityEngine;

/// <summary>
/// SporeHead patladığında devreye giren dinamik, daha büyük ve tam alev/ateş tonlarında optimize patlama partikülü.
/// Tamamen sarı, turuncu ve kızıl tonlardadır (mavi/soğuk renkler sıfırlanmıştır).
/// </summary>
public class SporeExplosionVFX : MonoBehaviour
{
    private static Texture2D smoothCircleTex;

    private Light pointLight;
    private float lightDuration = 0.25f;
    private float timer = 0f;
    private float startIntensity;

    // Yumuşak, yuvarlak alev partikülü dokusu (Kare görünmesini ve garip renk sapmalarını engeller)
    private static Texture2D GetCircleTexture()
    {
        if (smoothCircleTex != null) return smoothCircleTex;
        int res = 64;
        smoothCircleTex = new Texture2D(res, res, TextureFormat.RGBA32, false);
        float center = res * 0.5f;
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center)) / center;
                float alpha = Mathf.Clamp01(1f - dist * dist);
                smoothCircleTex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }
        smoothCircleTex.Apply();
        return smoothCircleTex;
    }

    public static void Spawn(Vector3 position)
    {
        GameObject go = new GameObject("SporeExplosion_VFX");
        go.transform.position = position;

        // 1. Particle System Oluştur
        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        
        // Temel Ayarlar (Daha Büyük & Güçlü Patlama)
        var main = ps.main;
        main.duration = 0.7f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.45f, 0.75f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(5f, 10.5f); // Daha hızlı dışarı fırlama
        main.startSize = new ParticleSystem.MinMaxCurve(0.55f, 1.15f); // 2 kat daha büyük boyut!
        main.startColor = Color.white;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;

        // Emisyon: Tek seferlik yoğun patlama (50 parçacık)
        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 50) });

        // Şekil: Küresel saçılma
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.55f;

        // Sürükleme (Drag): Patlayıp duman gibi havada yavaşlama hissi
        var limitVelocity = ps.limitVelocityOverLifetime;
        limitVelocity.enabled = true;
        limitVelocity.drag = 1.8f;

        // Renk Geçişi: SADECE SICAK ALEV TONLARI (Mavi/Yeşil kesinlikle yok!)
        // Parlak Beyazımsı Sarı -> Ateşli Turuncu -> Kızıl Alev -> Koyu Duman -> Şeffaf
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 0.95f, 0.55f), 0.0f),  // 1. Parlak Beyaz-Sarı Alev
                new GradientColorKey(new Color(1f, 0.50f, 0.05f), 0.25f), // 2. Canlı Ateş Turuncusu
                new GradientColorKey(new Color(0.95f, 0.18f, 0.05f), 0.60f),// 3. Kızıl/Kırmızı Kor
                new GradientColorKey(new Color(0.30f, 0.15f, 0.10f), 0.90f) // 4. Koyu Sıcak Duman
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.9f, 0.6f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = grad;

        // Boyut Eğrisi: Patlama anında devleşip sonra yavaşça ufalarak sönme
        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.6f);
        curve.AddKey(0.18f, 1.4f);
        curve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);

        // Materyal ve Doku Ataması
        ParticleSystemRenderer renderer = go.GetComponent<ParticleSystemRenderer>();
        Shader s = Shader.Find("Universal Render Pipeline/Particles/Unlit") 
                   ?? Shader.Find("Particles/Standard Unlit") 
                   ?? Shader.Find("Sprites/Default");
        if (s != null)
        {
            Material mat = new Material(s);
            Texture2D circle = GetCircleTexture();
            mat.mainTexture = circle;
            if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", circle);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Color.white);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", Color.white);
            renderer.material = mat;
        }

        // 2. Sıcak Işık Patlaması (Flash) - Parlak sarı/turuncu
        Light l = go.AddComponent<Light>();
        l.type = LightType.Point;
        l.color = new Color(1f, 0.55f, 0.1f);
        l.range = 6.5f;
        l.intensity = 3.5f;

        // Kontrolcü Bileşeni Ekle
        SporeExplosionVFX vfx = go.AddComponent<SporeExplosionVFX>();
        vfx.pointLight = l;
        vfx.startIntensity = l.intensity;

        ps.Play();
        Destroy(go, 1.3f);
    }

    void Update()
    {
        if (pointLight != null)
        {
            timer += Time.deltaTime;
            pointLight.intensity = Mathf.Lerp(startIntensity, 0f, timer / lightDuration);
            if (timer >= lightDuration)
            {
                pointLight.enabled = false;
            }
        }
    }
}
