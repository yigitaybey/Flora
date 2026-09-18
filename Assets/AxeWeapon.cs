using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    [Header("Balta (Yörünge) Ayarları")]
    public int currentLevel = 1;

    public float damage = 20f;
    public float orbitRadius = 3.2f;           // Oyuncudan yeterince uzakta olması için 3.2 yapıldı
    public float orbitSpeed = 180f;  
    public float tickRate = 0.2f;    
    public float hitArea = 1.4f;     

    public enum AxeFacingMode { FaceOutward, FaceOrbitDirection, Fixed }

    [Header("Görsel Ayarlar")]
    public GameObject axeVisual; 
    public bool clockwise = true;              // Saat yönünde mi dönsün? (false = Saat yönünün tersi)
    public AxeFacingMode facingMode = AxeFacingMode.FaceOutward; // Varsayılan: Kafa tarafı dışarı baksın
    public Vector3 orbitCenterOffset = new Vector3(0f, 1.2f, 0f); // Dönüş merkezi: Karakterin göğüs/bel hizası (Ayaklar değil!)
    public float axeHeight = 0.8f;             // Yerden yükseklik
    public Vector3 visualRotationOffset = new Vector3(-90f, 0f, 0f); // Modelin kafa yönünü tam ayarlamak için
    public bool spinOnSelf = false;            // Kendi etrafında dönme kapalı
    public float selfSpinSpeed = 360f;
    
    private GameObject axeModelPrefab;
    private List<Transform> activeAxes = new List<Transform>();
    
    private float currentAngle = 0f;
    private float nextTickTime;

    [Header("Hasar Vuruş Noktası (Kafa/Bıçak Kısmı)")]
    public float bladeForwardDistance = 1.5f; // Baltanın sapından dışarıdaki kafasına olan gerçek mesafe (metre)
    public float bladeUpOffset = 0.2f;        // Yükseklik farkı

    void Start()
    {
        if (axeVisual != null && axeVisual.transform.childCount > 0)
        {
            axeModelPrefab = axeVisual.transform.GetChild(0).gameObject;
            activeAxes.Add(axeModelPrefab.transform); 
        }
        else
        {
            Debug.LogWarning("AxeWeapon için Axe Visual atanmamış veya içi boş!");
        }
    }

    private Dictionary<GameObject, float> enemyLastHitTime = new Dictionary<GameObject, float>();

    void Update()
    {
        if (!enabled || activeAxes.Count == 0)
        {
            if (axeVisual != null && axeVisual.activeSelf) axeVisual.SetActive(false);
            return;
        }
        
        if (axeVisual != null && !axeVisual.activeSelf) axeVisual.SetActive(true);

        // Yörünge yönü: Saat yönü (negatif) veya saat yönünün tersi (pozitif)
        float direction = clockwise ? -1f : 1f;
        currentAngle += direction * orbitSpeed * Time.deltaTime; 
        
        float angleStep = 360f / activeAxes.Count;

        for (int i = 0; i < activeAxes.Count; i++)
        {
            float angle = currentAngle + (i * angleStep);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * orbitRadius;
            float z = Mathf.Sin(rad) * orbitRadius;

            // Karakterin ayaklarına değil, göğüs/bel merkezine oturtuyoruz
            activeAxes[i].localPosition = new Vector3(x + orbitCenterOffset.x, orbitCenterOffset.y, z + orbitCenterOffset.z);
            
            // YÖNELİM (ROTASYON)
            if (facingMode == AxeFacingMode.FaceOutward)
            {
                // Baltanın kafasını merkezden (bizden) tam dışarıya bakacak şekilde çevir
                Vector3 outwardDir = new Vector3(x, 0f, z).normalized;
                Quaternion lookRot = Quaternion.LookRotation(outwardDir);
                activeAxes[i].localRotation = lookRot * Quaternion.Euler(visualRotationOffset);
            }
            else if (facingMode == AxeFacingMode.FaceOrbitDirection)
            {
                // Baltanın kafasını yörüngede gidiş yönüne çevir
                Vector3 tangentDir = new Vector3(clockwise ? z : -z, 0f, clockwise ? -x : x).normalized;
                Quaternion lookRot = Quaternion.LookRotation(tangentDir);
                activeAxes[i].localRotation = lookRot * Quaternion.Euler(visualRotationOffset);
            }
            else
            {
                activeAxes[i].localRotation = Quaternion.Euler(visualRotationOffset);
            }

            // İsteğe bağlı kendi etrafında fırıldak gibi dönme
            if (spinOnSelf)
            {
                activeAxes[i].Rotate(Vector3.up, selfSpinSpeed * Time.deltaTime, Space.Self); 
            }
        }

        // HASAR KONTROLÜNÜ (ÇARPIŞMAYI) HER KARE YAP
        CheckAxeCollisions();
    }

    public Vector3 GetBladeWorldPosition(Transform axe)
    {
        if (axe == null) return transform.position;
        // Karakter merkezinden baltaya doğru olan yön vektörü
        Vector3 playerCenter = transform.position + orbitCenterOffset;
        Vector3 outwardDir = (axe.position - playerCenter);
        outwardDir.y = 0f;
        if (outwardDir.magnitude > 0.01f) outwardDir.Normalize();
        else outwardDir = transform.forward;

        // Baltanın pozisyonundan dışarıya (kafa ucuna) doğru öteleme
        return axe.position + (outwardDir * bladeForwardDistance) + (Vector3.up * bladeUpOffset);
    }

    void CheckAxeCollisions()
    {
        float hitRadiusSqr = hitArea * hitArea;
        float finalDamage = damage * (UpgradeManager.Instance != null ? UpgradeManager.Instance.globalDamageMultiplier : 1f);

        for (int i = 0; i < Enemy.ActiveEnemies.Count; i++)
        {
            Enemy enemyScript = Enemy.ActiveEnemies[i];
            if (enemyScript == null || !enemyScript.gameObject.activeInHierarchy) continue;

            for (int a = 0; a < activeAxes.Count; a++)
            {
                Transform axe = activeAxes[a];
                if (axe == null) continue;

                // Sapı değil, dışarıdaki metal kafa ucunun dünyadaki gerçek noktasını alıyoruz
                Vector3 bladeWorldPos = GetBladeWorldPosition(axe);
                float sqrDistance = (bladeWorldPos - enemyScript.transform.position).sqrMagnitude;
                
                if (sqrDistance <= hitRadiusSqr)
                {
                    GameObject enemyObj = enemyScript.gameObject;
                    // Düşman baltaya DEĞDİ! Soğuma (Cooldown) kontrolü yap:
                    if (!enemyLastHitTime.ContainsKey(enemyObj) || Time.time >= enemyLastHitTime[enemyObj] + tickRate)
                    {
                        enemyScript.TakeDamage(finalDamage);
                        
                        // Bu düşmanın hasar yediği anı kaydet (Cooldown başlasın)
                        enemyLastHitTime[enemyObj] = Time.time;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Yörünge çemberi (Sarı)
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + orbitCenterOffset;
        int segments = 32;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0) * orbitRadius, 0f, Mathf.Sin(0) * orbitRadius);

        for (int i = 1; i <= segments; i++)
        {
            float rad = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad) * orbitRadius, 0f, Mathf.Sin(rad) * orbitRadius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // Baltaların hasar vuruş küreleri (Kırmızı Top)
        Gizmos.color = Color.red;
        if (activeAxes != null && activeAxes.Count > 0)
        {
            foreach (Transform axe in activeAxes)
            {
                if (axe != null)
                {
                    Vector3 bladePos = GetBladeWorldPosition(axe);
                    Gizmos.DrawWireSphere(bladePos, hitArea);
                }
            }
        }
        else if (axeVisual != null)
        {
            Transform sampleAxe = (axeVisual.transform.childCount > 0) ? axeVisual.transform.GetChild(0) : axeVisual.transform;
            if (sampleAxe != null)
            {
                Vector3 bladePos = GetBladeWorldPosition(sampleAxe);
                Gizmos.DrawWireSphere(bladePos, hitArea);
            }
        }
    }

    // --- UPGRADE SİSTEMİ ÇAĞRILARI ---

    public void UnlockWeapon()
    {
        enabled = true;
        currentLevel = 1;
        Debug.Log("Balta Silahı Açıldı!");
    }

    public void LevelUpWeapon()
    {
        if (currentLevel >= 8) return;

        currentLevel++;
        PlayerMovement playerMovement = UpgradeManager.Instance.playerMovement;

        switch (currentLevel)
        {
            case 2:
                damage *= 1.25f; // Hasar +%25
                break;
            case 3:
                orbitSpeed *= 1.20f; // Dönüş Hızı +%20
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.05f); // SİNERJİ: Hız +%5
                break;
            case 4:
                SetAxeCount(2); // 2 Balta
                orbitSpeed *= 1.10f; // Dönüş Hızı hafif artar
                break;
            case 5:
                // Tüm baltaların boyutunu (scale) %25 büyüt
                if (axeModelPrefab != null)
                {
                    Vector3 newScale = axeModelPrefab.transform.localScale * 1.25f;
                    axeModelPrefab.transform.localScale = newScale;
                    foreach (Transform axe in activeAxes)
                    {
                        axe.localScale = newScale;
                    }
                }
                break;
            case 6:
                damage *= 1.30f; // Hasar +%30
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.10f); // SİNERJİ: Hız +%10
                break;
            case 7:
                orbitSpeed *= 1.30f; // Dönüş Hızı +%30
                break;
            case 8:
                SetAxeCount(4); // 4 Balta
                if (playerMovement != null) playerMovement.IncreaseSpeed(0.10f); // SİNERJİ: Hız +%10
                break;
        }

        Debug.Log("Balta Seviye Atladı! Yeni Level: " + currentLevel);
    }

    void SetAxeCount(int count)
    {
        if (axeModelPrefab == null) return;

        // Öncekileri temizle (0. index hariç çünkü o bizim ana modelimiz)
        for (int i = 1; i < activeAxes.Count; i++)
        {
            if (activeAxes[i] != null) Destroy(activeAxes[i].gameObject);
        }
        
        activeAxes.Clear();
        activeAxes.Add(axeModelPrefab.transform); // Ana modeli tekrar koy

        // Eksik olanları üret
        for (int i = 1; i < count; i++)
        {
            GameObject newAxe = Instantiate(axeModelPrefab, axeVisual.transform);
            // Referans aldığımız objenin scale değerini yeni üretilenlere de ata
            newAxe.transform.localScale = axeModelPrefab.transform.localScale;
            activeAxes.Add(newAxe.transform);
        }
    }
}
