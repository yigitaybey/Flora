using UnityEngine;
using UnityEngine.UI; // Can barı için gerekli
using TMPro; // TextMeshPro (Yazılar) için gerekli

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Arayüz (UI)")]
    [Tooltip("Can Barı (Filled Image) buraya sürüklenecek")]
    public Image healthBarFill;
    [Tooltip("Can Barının üstündeki metin (Örn: 100/100)")]
    public TextMeshProUGUI healthText;

    [Header("I-Frames (Dokunulmazlık) Ayarları")]
    public float invincibilityDuration = 0.5f; // 0.5 saniye dokunulmazlık
    private float lastDamageTime = -100f;

    [Header("Sinerji Ayarları")]
    public float healthRegenPerSecond = 0f;
    private float accumulatedRegen = 0f;
    private float nextRegenPopupTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    void Update()
    {
        if (healthRegenPerSecond > 0f && currentHealth < maxHealth)
        {
            float regenAmount = healthRegenPerSecond * Time.deltaTime;
            Heal(regenAmount, true); // Artık true yapıyoruz ki barda görelim
            
            accumulatedRegen += regenAmount;
            if (Time.time >= nextRegenPopupTime && accumulatedRegen >= 1f)
            {
                nextRegenPopupTime = Time.time + 1f;
                ShowHealPopup(accumulatedRegen);
                accumulatedRegen = 0f;
            }
        }
    }

    void ShowHealPopup(float amount)
    {
        // Ayarlardan Hasar/Can yazıları kapatılmışsa çıkarma
        if (GameManager.Instance != null && !GameManager.Instance.isDamageNumEnabled) return;

        if (DamagePopupPool.Instance != null)
        {
            GameObject popupObj = DamagePopupPool.Instance.GetPopup();
            if (popupObj != null)
            {
                popupObj.transform.position = transform.position + Vector3.up * 2f;
                DamagePopup popupScript = popupObj.GetComponent<DamagePopup>();
                if (popupScript != null)
                {
                    popupScript.Setup(amount, true); // true = isHeal (yeşil ve + işaretli)
                }
            }
        }
    }

    private bool isDead = false;

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        // Eğer son hasar alma zamanının üzerinden yeterli süre geçmediyse hasarı yok say (I-Frames)
        if (Time.time < lastDamageTime + invincibilityDuration)
        {
            return;
        }

        lastDamageTime = Time.time; // Hasar aldığımız anı kaydet
        currentHealth -= amount;
        Debug.Log("Sylva Hasar Aldı! Kalan Can: " + currentHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Sylva Öldü! Toplanan tohumlar kaydedilip Base'e dönülüyor...");
        
        // GameManager varsa run'ı bitir ve base'e dön
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadBaseScene();
        }
        else
        {
            // Fallback: Test sırasında GameManager yoksa
            UnityEngine.SceneManagement.SceneManager.LoadScene("BaseScene");
        }
    }

    // YETENEK SİSTEMİ: Max canı artırır ve mevcut cana da ekler
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; // Max can arttığında mevcut can da o kadar dolsun
        UpdateUI();
        Debug.Log("Max Can Artırıldı! Yeni Max Can: " + maxHealth);
    }

    // CAN İKSİRİ: Mevcut canı artırır ama Max Canı geçemez
    public void Heal(float amount, bool updateUI = true)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        if (updateUI) 
        {
            UpdateUI();
            Debug.Log("Can Yenilendi! Mevcut Can: " + currentHealth);
        }
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + maxHealth.ToString();
        }
    }
}
