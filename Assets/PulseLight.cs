using UnityEngine;

[RequireComponent(typeof(Light))]
public class PulseLight : MonoBehaviour
{
    [Header("Nefes Alma Ayarları")]
    [Tooltip("Işığın en kısık halindeki gücü")]
    public float minIntensity = 0.5f;
    
    [Tooltip("Işığın en parlak halindeki gücü")]
    public float maxIntensity = 3.0f;
    
    [Tooltip("Nefes alıp verme hızı")]
    public float pulseSpeed = 2.5f;

    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        // Sinüs dalgası ile yumuşak bir in-çık dalgası yarat (-1 ile 1 arası)
        float wave = Mathf.Sin(Time.time * pulseSpeed);
        
        // Dalga değerini 0 ile 1 arasına sıkıştır (matematiksel yumuşatma)
        float normalizedWave = (wave + 1f) / 2f;
        
        // Işığın gücünü min ve max arasında bu dalgaya göre oynat
        myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedWave);
    }
}
