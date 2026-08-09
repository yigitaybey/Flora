using UnityEngine;
using System.Collections.Generic;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance;
    
    [Header("Taret Ayarları")]
    public GameObject turretPrefab; // Unity'den atanacak
    public int maxTurrets = 3; // Dengeleme: Ekranda aynı anda en fazla 3 taret olabilir
    
    private List<Turret> activeTurrets = new List<Turret>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnTurret(Vector3 position)
    {
        // Eğer sahnede max taret varsa en eskisini (ilk çıkanı) yok et
        if (activeTurrets.Count >= maxTurrets)
        {
            Turret oldestTurret = activeTurrets[0];
            activeTurrets.RemoveAt(0);
            if (oldestTurret != null)
            {
                oldestTurret.gameObject.SetActive(false);
                Destroy(oldestTurret.gameObject);
            }
        }

        // Yeni taret oluştur (Şimdilik Instantiate/Destroy, ileride Object Pool'a alınabilir)
        if (turretPrefab != null)
        {
            GameObject turretObj = Instantiate(turretPrefab, position, Quaternion.identity);
            Turret turretScript = turretObj.GetComponent<Turret>();
            
            if (turretScript != null)
            {
                // PlayerPassives'ten güncel değerleri al
                float duration = PlayerPassives.Instance.GetTurretDuration();
                float damage = PlayerPassives.Instance.GetTurretDamage();
                
                // Hasara global sinerji çarpanını ekle
                if (UpgradeManager.Instance != null)
                {
                    damage *= UpgradeManager.Instance.globalDamageMultiplier;
                }
                    
                turretScript.Setup(duration, damage);
                activeTurrets.Add(turretScript);
            }
        }
        else
        {
            Debug.LogWarning("DİKKAT: TurretManager'a Taret Prefab'ı atanmamış!");
        }
    }

    public void RemoveTurret(Turret turret)
    {
        if (activeTurrets.Contains(turret))
        {
            activeTurrets.Remove(turret);
        }
    }
}
