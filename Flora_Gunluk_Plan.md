# 🗓️ Flora - Tam Kapsamlı Günlük Plan (3 Ağustos - 30 Eylül)

Bu belge, oyunun 30 Eylül'deki son teslimine kadar her gün tam olarak ne yapılacağını gösteren "Master Plan"dır.

### 🟢 FAZ 1: Mekaniklerin Tamamlanması (Sadece Kod)
*   **3 Ağustos:** Collider sistemini sil. `EnemyManager.cs` (Fiziksiz sistem) kurulumunu yap.
*   **4 Ağustos:** Mermi ve Ateş etme kodlarını `Vector3.Distance` ile çalışacak şekilde güncelle.
*   **5 Ağustos:** `BaseScene` (Üs) ve `CombatScene` (Savaş) sahnelerini ayır. Geçiş kodlarını yaz.
*   **6 Ağustos:** `SaveManager.cs` yaz. Toplanan tohumların (XP/Para) diske kaydedilmesini sağla.
*   **7 Ağustos:** Üs sahnesine "Harabe Atölye" görseli ekle. Tamir etme butonunu ve kodunu yaz.
*   **8 Ağustos:** Atölye tamir edilince açılan menüyü yap (Max Can +10 satın alma).
*   **9 Ağustos:** Telsiz Kulesini yap. Kuleye tıklayınca kilitli "Dünya Haritası" UI'ını açtır.
*   **10 Ağustos:** `TerraformingManager.cs` yaz. Post-Processing ile ekranı yavaşça renklendirme efektini kur.
*   **11 Ağustos:** Zafer Ekranını yap. "Bölge Arındırıldı" yazısı ve toplanan tohum miktarını göster.
*   **12 Ağustos:** Ölüm Ekranını yap. "Operasyon Başarısız" menüsü ve "Devam Et" butonunu kur.
*   **13 Ağustos:** RevenueCat entegrasyonuna başla. Mağaza menüsünü kodla.
*   **14 Ağustos:** Sahte satın alma (IAP) ve Reklam (Ad) UI butonlarının fonksiyonlarını bağla.
*   **15 Ağustos:** BAŞTAN SONA TEST (Playtest). Oyunu üsten başlat, savaş, öl, geri dön, can al, tekrar savaş. Bug'ları çöz. (KODLAMA BİTTİ).

### 🟡 FAZ 2: Varlık (Asset) Bekleme ve Cila (Polishing)
*   **16 Ağustos - 18 Ağustos:** Menüleri güzelleştir. Butonlara tıklama sesleri ekle. Arayüzün telefonda düzgün durması için Anchor (Hizalama) ayarlarını yap.
*   **19 Ağustos - 21 Ağustos:** Yusuf'tan ilk çizimleri (Karakter ve Düşman) iste. Gelince Unity'ye at, boyutlarını (Scale) ve merkez noktalarını (Pivot) ayarla.
*   **22 Ağustos - 24 Ağustos:** Gelen çizimlerin Animator (Yürüme, Ölme) geçişlerini ayarla.
*   **25 Ağustos:** Yusuf'tan Harita ve UI çizimlerinin teslim alınması (Deadline).
*   **26 Ağustos - 28 Ağustos:** Tüm gri küpleri sil, oyuna gerçek haritayı ve çevre modellerini oturt.
*   **29 Ağustos - 31 Ağustos:** Görsel cila. Partikül efektleri (Kan, Zehir Gazı, Patlamalar) ve oyun içi Müzik/SFX eklemeleri.

### 🔴 FAZ 3: Dengeleme (Balancing) ve Bug Fix
*   **1 Eylül:** Oyunu oyna. Düşmanların canı çok mu fazla? Oyuncunun hasarı az mı? Değerleri değiştir.
*   **2 Eylül:** Savaş 5. dakikada sıkıcı mı oluyor? Düşman doğma (Spawn) sıklığını ayarla.
*   **3 Eylül:** Atölyedeki geliştirmelerin fiyatlarını (Tohum maliyetlerini) ayarla.
*   **4 Eylül - 9 Eylül:** Ekipçe (Yusuf'la birlikte) oyunu saatlerce oynayın. Duvarın içine girme, menünün takılması gibi bulunan her hatayı (Bug) çözün. Yeni özellik EKLEME.

### 🟣 FAZ 4: Google Play 14 Günlük Test Süreci
*   **10 Eylül:** Oyunu Unity'den **.aab** olarak Build al. Google Play Console'a yükle.
*   **11 Eylül:** Discord ve Reddit'ten 20 kişilik testçi grubuna davet at.
*   **12 Eylül:** Test başlasın. Oyuncuların şikayetlerini dinle.
*   **13 Eylül - 23 Eylül:** Testçilerin bulduğu kritik hatalar olursa düzeltip Google Play'e yeni güncelleme yolla. Geri kalan günlerde sadece dinlen.
*   **24 Eylül:** 14 günlük test süresi bitti. Google'a "Test bitti, oyunu Production'a (Herkese açık) al" başvurusunu yap.

### 🏁 FAZ 5: Yayın, Fragman ve Shipaton Teslimi
*   **25 Eylül:** Ekran kaydediciyi (OBS) aç, oyundan en havalı anları (Boss kesimi, Yeşillenme anı) kaydet.
*   **26 Eylül:** CapCut veya Premiere ile bu videonun arkasına telifsiz epik bir müzik koy. Araya "Dünyayı Geri Al" gibi yazılar ekle (Max 2 dakika).
*   **27 Eylül:** Fragmanı YouTube'a yükle.
*   **28 Eylül:** Oyun Google Play'de herkese açık (Public) olarak onaylanmış olsun. Oyunun linkini kopyala.
*   **29 Eylül:** Devpost formunu (Additional Info) aç. Oyunun Google Play linkini ve YouTube fragman linkini yapıştır.
*   **30 Eylül:** SUBMIT butonuna bas ve Shipaton'u tamamla! 🎉
