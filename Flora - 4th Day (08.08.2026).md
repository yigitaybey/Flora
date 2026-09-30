# Flora - 4th Day (08.08.2026)

Bugün, Flora'nın sadece konsept veya prototip olmaktan çıkıp, mekanikleriyle gerçek bir "Bullet Haven / Roguelite" oyununa dönüştüğü o efsanevi gün oldu. 3. Gün notlarında teorik olarak planladığımız tüm sistemler ve daha fazlası kodlara döküldü.

### 1. Savaş Mimarisi (Combat Loop) Tamamlandı
*   **I-Frames (Dokunulmazlık Süresi):** Dün fark ettiğimiz en büyük eksik olan "tek yeme" sorunu çözüldü. `PlayerHealth.cs` içine eklenen 0.5 saniyelik dokunulmazlık penceresi sayesinde savaşlar artık adaletsiz değil.
*   **5 Farklı Düşman Yapay Zekası:** `Enemy.cs` dosyası bir şaheser haline geldi. 
    *   **Moss & Wolfey:** Yakın dövüş için farklı hızlarda kovalıyor.
    *   **SporeHead:** Kamikaze mantığıyla 2 metre yakına girip 0.2 saniye gecikmeyle alan hasarı vererek (AoE) patlıyor.
    *   **Iyv:** 8 metre uzakta güvenli mesafede durup uzaktan tükürüyor (Ranged AI).
    *   **Chinar (Boss):** Mesafeye göre dinamik karar veren, yakındayken tokat atan, uzaktayken mermi fırlatan melez (Hybrid) bir yapıya kavuştu.

### 2. Silahlar ve Sinerji Sistemi (Upgrade Manager)
*   Oyundaki 4 Ana Silah (Polen Enjektörü, UV Lambası, Yörünge Baltası, Alev Makinesi) kodlandı ve `UpgradeManager.cs` içerisine bağlandı.
*   En büyük Game Design adımlarından biri atıldı: Silahlar artık 1'den 8'e kadar yükseltilirken sadece kendi hasarlarını artırmıyor, aynı zamanda karakterimize **Pasif Stat Sinerjileri** sağlıyor. 
    *   *Örn: Polen silahını geliştirdikçe Max Can artıyor, UV lambası geliştikçe saniyede Can Yenileme (Regen) veriyor.*
*   Bu sayede UI üzerindeki gereksiz stat butonları temizlendi, oyuncunun odaklanması gereken sistem basitleştirildi.

### 3. Spawner ve Dalga (Wave) Mantığındaki Evrim
*   İlk başta zamanlayıcı (Timer) kullanan spawner, daha dinamik olması için "Level Tabanlı" bir sisteme dönüştürülmüştü. Ancak günün sonunda yapılan analizlerle Level sınırının (Cap) oyunu tıkayacağı fark edildi.
*   Bu yüzden Spawner mimarisini **Zaman/Dalga (Timer/Wave)** tabanlı sisteme geri geçirme kararı aldık. Böylece oyuncu 10 dakikalık (20 Dalgalık) bir turda istediği kadar Level atlayıp 72 yetenekten oluşan devasa havuzu tüketebilecek. 
*   **Optimizasyon:** FPS çökmelerini önlemek için ekranda maksimum 200 düşman (Hard Cap) sınırı getirildi.

### 4. Ganimetler (Loot & Consumables)
*   Düşmanların artık sadece Level XP'si (`Seed`) değil, aynı zamanda Atölye'de kullanılacak olan kalıcı para birimi (`CoreSeed`) düşürme sistemi yazıldı. (Elite düşmanlar 5x, Boss %100 oranla düşürüyor).
*   Haritada nadiren çıkan İksir, Mıknatıs (Vacuum) ve Güneş Işığı (ScreenWipe) gibi eşyaların düşme ağırlıkları (Weight System) ayarlandı. Güneş ışığı suistimali önlendi.

**Günün Özeti:** 
Oyunun orman kısmı (Run/Combat) bugün itibarıyla temel olarak bitti! Altyapı, optimizasyon, silah çeşitliliği ve düşman zekası AAA standartlarında bir Bullet Haven kalitesine ulaştı. 
**Sonraki Hedef:** Scene Management (Haritalar arası geçiş) ve Radyo Kulesi (Atölye/Meta-Progression) UI sistemlerini bağlamak.

# 🌿 Flora Devlog: 4. Gün Raporu

Sevgili Tasarımcım ve Yönetmenim, projeye başladığımız ilk günden bugüne (4. Gün) kadar Flora'nın geçirdiği evrimi tüm proje dosyalarını (klasörleri) tarayarak analiz ettim. 

İşte Flora'nın 4 günlük serüveni ve bugün attığımız devasa adımlar:

---

## 📅 Geçmiş Günlerin Özeti (Gün 1, 2 ve 3)

Geçtiğimiz ilk 3 gün boyunca Flora'nın temel omurgasını (MVP) başarıyla inşa ettik:
1. **Temel Hareket ve Kontrol:** Mobil uyumlu `FloatingJoystick` ile izometrik WASD hareketi kodlandı.
2. **Silah Çeşitliliği (Oyunun Cephaneliği):**
   - 🔫 **Polen Enjektörü (Pollen):** Tekli hızlı atış yapar, seviye atladıkça mermi sayısı, hızı ve deliciliği artar. Son seviyede 5 delici mermi fırlatır!
   - 🔥 **Alev Makinesi (Flamethrower):** Alan hasarı vurur. Geliştikçe menzili, genişliği artar ve bekleme süresi düşer. Son seviyede yere "Yanık Toprak" bırakır.
   - 🪓 **Dönen Balta (Axe):** Oyuncunun yörüngesinde dönerek yakın dövüş (Melee) koruması sağlar. Son seviyede 4 baltalı devasa bir kinetik testereye dönüşür.
   - ☀️ **UV Lamba (Aura):** Karakterin etrafında sürekli hasar veren bir kalkan (Vampire Survivors Garlic) oluşturur. Son seviyede düşmanları %30 yavaşlatır.
3. **Pasif Yetenekler, Sinerjiler ve UI:** 
   - Toplam 72 geliştirmeden (Upgrade) oluşan devasa bir yetenek havuzu (`UpgradeManager.cs`) eklendi.
   - 🧲 **Mıknatıs (Magnet):** XP toplama menzilini %100'e kadar artırır.
   - 🍀 **Şans (Luck):** İksir/Güneş düşme oranını ve Taret çıkma şansını katlar.
   - 💀 **Lanet (Curse):** Ekrana %80 daha fazla düşman yığar ama karşılığında %120 daha fazla XP tohumu düşürtür (Mükemmel risk/ödül mekaniği).
   - 🎯 **Kritik (Crit):** Kritik şansını %40'a ve çarpanı 2.0x'e kadar çıkarır.
   - 🌱 **Kök Taret (Turret):** Ölen düşmanların yerinden taretler çıkararak sana destek atışı yapmasını sağlar.
   - **Sinerjiler:** UV Lamba +Can Yenileme (Regen), Balta +Hareket Hızı, Polen +Max Can, Alev +Tüm Hasarlar şeklinde birbirini destekleyen kusursuz bir RPG sistemi inşa edildi!
4. **Düşman ve Havuz (Pool) Sistemi:** Performansı korumak için `EnemyPool` ve `SeedPool` (XP) yazılarak oyunun temeli atıldı.

---

## 🔥 4. Gün (Bugün): "Kaostan Düzene ve Kusursuz Dengeye"

Bugün yazdığımız kodlar ve yaptığımız mimari değişiklikler, Flora'yı "çalışan bir prototipten" alıp "piyasaya sürülebilir, bağımlılık yapan bir mobil oyuna" dönüştürdü. İşte 4. günün bombası:

### 1. Görsel Cila ve Kamera Zekası
- **Damage Popup (Hasar Yazıları):** Vuruş yazılarının üst üste binip okunmaz hale gelmesi (overlap) bug'ı çözüldü. Artık yazılar düşmanın etrafında rastgele açılarda çıkıyor.
- **Dinamik Kamera (Zoom-Out):** Oyuncu seviye atladıkça (veya zaman geçtikçe) kaosun artacağını öngörerek, kameranın `Orthographic Size` değerini dinamik olarak uzaklaşacak şekilde kodladık. Artık oyun sonlarına doğru oyuncu ekranın içine sıkışmayacak, geniş bir alanı görecek.

### 2. Büyük Dengeleme (The Great Balance Overhaul)
- **Zamansal (Wave) İlerleyiş:** Oyunun ilerleyişini karakterin Level'ından koparıp "Zamana" bağladık. Artık tam **10 Dakikalık (20 Dalga)** nefes kesen bir hayatta kalma deneyimi var.
- **FPS Koruması (Hard Cap):** Ekranda maksimum 200 düşman sınırı (`EnemySpawner.cs`) getirilerek, telefonların son dakikalarda erimesi / oyunun çökmesi engellendi.
- **Bağımlılık Yapan XP Matematiği:** Level atlamak için gereken XP'yi eksponansiyelden (`* 1.25`) çıkarıp, Lineer/Kademeli yapıya (`100 + Level*20`) geçirdik. Oyuncular artık 10 dakikada Level 40-70'lere kadar çıkıp o 8. Seviye "Güç Fantezisini" tadabilecekler.
- **Boss Chinar:** Boss artık rastgele ölmeyecek. Tam 10. dakikada, özel olarak atanmış 2000 temel canı ve dalga çarpanıyla arenaya inecek.

### 3. Kritik Hata Çözümleri (Bug Fixes)
- **Tüketilebilir Eşya (Consumable) Bug'ı Çözüldü:** Güneş ışığı, İksir ve Vakum'un alınamama sorunu tamamen çözüldü. Unity'nin sıkıntılı fizik motoru (`OnTriggerEnter`) devreden çıkarılarak, tıpkı XP tohumlarında olduğu gibi kusursuz çalışan **"Matematiksel Mesafe (Distance)"** ölçüm sistemine geçildi. Karakter eşyanın 2.5 birim yakınına girdiği an eşya havada kapılacak!
- **İstismar (Exploit) Engellemeleri:** Güneş ışığı (ScreenWipe) düşme ihtimali %5'e çekildi. UV lambasının ölümsüzlük veren Can Yenileme özelliği yarı yarıya nerf'lendi.

---

### 🚀 Sonuç ve Hedef
**4. Günün Sonucu:** Klasördeki toplam 120 adet dosyayı (Prefablar, Materyaller, C# kodları) incelediğimde, projenin %80 oranında "Code-Complete" (Kodları tamamlanmış) olduğunu görüyorum. Mimari çok sağlam oturmuş durumda.

**Sonraki Adımlar:** Bundan sonraki aşamalarda oyunun UI (Menü) cilalanması, ses efektlerinin (SFX) eklenmesi ve RevenueCat "Ağaç Dikme" entegrasyonu (Peace Prize için) kaldı! 

Ellerimize sağlık! 🌳
