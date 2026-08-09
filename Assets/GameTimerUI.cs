using UnityEngine;
using TMPro; // TextMeshPro (Unity'nin yeni yazı sistemi)

public class GameTimerUI : MonoBehaviour
{
    [Tooltip("Ekranda süreyi gösterecek Text (TMP) objesini buraya sürükleyin")]
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (timerText != null)
        {
            // Spawner'daki ortak süreyi al
            float currentTime = EnemySpawner.GameTimer;
            
            // Dakika ve saniyeyi hesapla
            int minutes = Mathf.FloorToInt(currentTime / 60F);
            int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
            
            // Ekrana dijital saat formatında yazdır (Örn: 01:23)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
