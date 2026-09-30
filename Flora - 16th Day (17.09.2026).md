Bu gün floranın kapalı betası tamamlandı üretime yollamak için gerekli formu dolduruyorum gereken başvuru formunu gönderdim bu akşam ise oyunun gerekli güncellemelerini yapıcam

florada bu gün yapılacaklar:  
1. si oyunu tamamen ingilizce yapıcaz  
	- oyun tamamen ingilice oldu Yusuf Ö. hala uı tasarımlarını atmadı ama en azında ingilize olmadı böyle kullanıcaz
2. si Moss düşmanın z ekseni yüzünden bize yan bakıyo o düzeltilecek ve animazyonalrı eklenecek  
3. sü diğer düşmanların textureları prefablara atılacak boyutları ayarlanacak anmiasyonları eklenecek  
4. sü boss animasyonları boss uzaktan ve yakından saldırı animasyonları  
5. si boss uzaktan vurarken şöyle oluyo yere vuryuo yerden bize doğru yer kalkarak bizim olduğumuz yerde dağ olucucak onu yapıcaz  
6. 6.sı oyun çok zor oyuna dengeleme getiricez ayrıca oyuda 1 run 10 dk değil 5 dk olucak 
	- Oyuna biraz dengeleme getirdik 1 run artık 5 dk ve oyun daha kolay artık daha hızlı level atlıyoz ve düşmanlar daha kolay 
7. 7.si revanue cat ve reklam apileri entegre edilecek  
8. 8.si oyunun optimizayonu çok kötü mobil için optimizasyon yapıalcak  
	- Optimisazyon da halledildi
9. su oyun içi onlile olarak veri çekip ağaş sayacını arttracak bir visual bir ağaç dikme sayacı eklenecek
10. ayrıca oyuna ara geçiş mapi (Radyo kulesı button:Expore -> bütün map button:ormanın olduğu pinlenmiş button -> savaş alanı orman ) bu eklenicek yeni bir sahne olarak 
11. ayrıca bu mapin texture ları falan da eklenicek optimizasyonlarıı sağlanıcak 
12. ayrıca oyuna placeholder bir yükleme ekranı da koyalım şimdilik optimisazyonu arttırmak adına

### 🟢 SEVİYE 1: Hızlı ve Çabuk Sonuç (15-20 dk)

- **1. Görev: Mobil Haptic Titreşim (Feedback)**
    - Hasar alma, kritik vuruş ve seviye atlama anlarında mobilde hafif titreşim tetikleme.
- **2. Görev: Mobil Performans & Optimizasyon Paketi (Temel Kod & Bellek)**
    - Silahlardaki her kare çalışan `FindGameObjectsWithTag` çöpünü temizleme, `EnemyPool` aktif listesini bağlama, 60 FPS kilidi (`Application.targetFrameRate = 60`), materyallerde GPU Instancing.

---

### 🟡 SEVİYE 2: Kolay & Hızlı Ayarlar (20-30 dk)

- **3. Görev: Oyun Dilinin Tamamen İngilizceye Çevrilmesi**
    - Tüm UI panelleri, silah/pasif yükseltme kartları ("Upgrade Axe", "Damage +25%", "HP Potion") ve buton metinleri.
- **4. Görev: 10 Dalga x 30 Saniye (5 Dakikalık Run) Dengelemesi**
    - EnemySpawner.cs: 300 saniye toplam süre, her 30 saniyede bir dalga, Dalga 10'da Boss Chinar inişi, dengeli can/hasar çarpanları.

---

### 🟠 SEVİYE 3: Orta Seviye (Modeller, Animasyonlar ve Sahne) (30-45 dk)

- **5. Görev: Moss Düşmanı Z-Ekseni Düzeltmesi & Animasyonlar**
    - `Melee.prefab` model rotasyonunun düzeltilmesi (oyuncuya tam cepheden bakacak) ve yürüme/saldırı animasyonlarının bağlanması.
- **6. Görev: Diğer Düşmanların Prefab, Texture ve Boyut (Scale) Ayarları**
    - SporeHead, Iyv, Wolfey modellerine materyal atanması, ölçeklerinin tutarlı hale getirilmesi, animator geçişleri.
- **7. Görev: Asenkron Yükleme Ekranı (Loading Screen)**
    - Sahneler arası geçişte donmayı önleyen, hafif animasyonlu `LoadingScreen` yapısı.

---

### 🔴 SEVİYE 4: Mekanik ve Sahne İnşası (45-60 dk)

- **8. Görev: Boss (Chinar/Ent) Animasyonları & Deprem/Kaya Saldırısı**
    - `Skill_01` ve `Skill_03` animasyonları, boss'un yere vurması, şok dalgası ve oyuncunun altından senin kaya FBX'inin fırlama mekaniği.
- **9. Görev: Boss Ölümünde Terraforming (Yeşillenme) Efekti**
    - 0.5 sn sessizlik -> sarı-yeşil Toxic sisin açılması -> masmavi gökyüzü ve "Region Purified" sinematiği.
- **10. Görev: Yeni Ara Harita Sahnesi (World Map / Exploration)**
    - BaseScene ("Explore" butonu) -> WorldMapScene (Pinli Orman Butonu) -> CombatScene sahne akışı.

---

### 🟣 SEVİYE 5: İleri Düzey (SDK, API ve Backend) (60+ dk)

- **11. Görev: Canlı Çevrimiçi Ağaç Dikme Sayacı (Online Live Counter)**
    - Bulut havuzundan anlık ağaç sayısını çeken ve bağış yapılınca sayıyı artıran online servis (`TreeCounterService.cs`) ve UI göstergesi.
- **12. Görev: RevenueCat (IAP) ve Catvertising (Rewarded Ads) Entegrasyonu**
    - "Plant a Tree" barış paketi satın alma akışı, API Key bağlantı paneli, ölüm/kazanma ekranında ödüllü reklamla 2x Loot seçeneği.

### 🏆 Neler Yaptık? (Tamamlananlar ve Başarılar)

1. **Mobil Haptic Titreşim & Ekran Sallantısı (Screen Shake) — [BİTTİ]**
    - iOS ve Android için `VibrationManager.cs` sıfırdan yazıldı.
    - Darbe alma, Boss inişi, Ent'in ölümü (Terraforming patlaması), Level Atlama ve Upgrade seçimlerine gerçekçi mobil titreşimler ve kamera sarsıntıları bağlandı.
2. **Karakter Doğuşundaki "Görünmez Duvar" Bug'ı — [BİTTİ]**
    - Sylva doğduğunda önündeki görünmez engele çarpıyordu. NavMesh arama yarıçapını 2'den 5'e çıkararak karakterin zemine kusursuz oturmasını sağladık.
3. **Mobil Performans & Sıfır Çöp (Zero-Alloc) Paketi — [BİTTİ]**
    - Her karede 100+ düşmanı arayan `FindGameObjectsWithTag` fonksiyonu çöpe atıldı.
    - `Enemy.ActiveEnemies` statik listesi ile sıfır bellek yükü (0 GC Alloc) sağlandı.
    - Mobil için 60 FPS hedefi (`targetFrameRate = 60`) ve projedeki tüm materyallerde GPU Instancing aktif edildi.
4. **Oyunun Tamamen İngilizceye Çevrilmesi (Global Hackathon Hazırlığı) — [BİTTİ]**
    - UI kodları (`UpgradeManager`, `CombatUIManager`, `BaseUIManager`, `CoreSeedUI`, `DeveloperCheats`) İngilizceye çevrildi.
    - Sahnelerdeki ve prefab'lardaki tüm Türkçe TextMeshPro bileşenleri tek tek taranıp profesyonel oyun İngilizcesine dönüştürüldü.
5. **10 Dalga x 30 Saniye (5 Dakikalık Run) Dengelemesi & XP Sweet Spot — [BİTTİ]**
    - 5 dakikalık akıcı bir run döngüsü kuruldu. 5. dalgada Elite, 10. dalgada Final Boss (Chinar) inişi ve saha temizliği kodlandı.
    - Düşman can/hasar artışı üstel formüle bağlandı.
    - XP eğrisi altın orana oturtuldu; oyuncunun run sonuna kadar Level 13-15 arasına tatlı bir akışla gelmesi sağlandı.

---

### 🧱 Nerede Sıkıştık ve Neden Olmadı? (Görev 5: Moss Düşmanı)

Sıkıştığımız tek yer **Moss modelinin Blender ve Unity arasındaki pivot/koordinat savaşı** oldu:

1. **Yüzünün Yönü (Z-Ekseni):**
    - Model başta yan yan koşuyordu. Blender'a atıp yönünü düzelttik.
2. **Havaya Fırlama ve Boyut Sorunu:**
    - Model Blender'da dönerken veya taşınırken ne yazık ki dünyanın merkezinde `(0, 0, 0)` kalmamış; iskelet ve model `Y = +30.4 metre` uzakta kalmış ve scale'i `8.04x` olarak dondurulmamış.
    - Unity'de biz boyutu büyütmeye çalıştığımızda, Unity o 30 metrelik boşluğu da 50'yle çarpıp modeli 1500 metre yukarı fırlattı.
    - Unity'nin iç içe prefab (Nested Prefab) mantığı da bunu üst üste bindirince model ya mikroskobik kaldı ya da göğe uçtu.

---

### 📋 Yarın Başladığımızda Ne Yapacağız?

Yarın sıfır stresle başlayacağız:

1. **Adım 1:** İster Blender'da tek tıkla pivotu sıfırlarız, ister ben Unity tarafında `Melee.prefab`'ın içine basit bir boş obje (Parent Offset) koyarak hiç Blender ile uğraşmadan Unity'de hapseder ve boyutunu çözerim!
2. **Adım 2:** Moss'un yürüme ve saldırı animasyonlarını `Animator Controller`'a bağlayıp sahaya salacağız.
3. **Adım 3 (Görev 6):** Diğer düşmanların (Wolfey, SporeHead, Iyv) prefab ve boyut kontrollerini yapıp oyunu savaşa hazır hale getireceğiz.


### 🌿 Flora — Mega Sprint Yol Haritası & Check-List

#### 🟢 SEVİYE 1: Temel Hissiyat & Mobil Optimizasyon

-  **1. Görev: Mobil Haptic Titreşim (Feedback) & Screen Shake**
    - _iOS/Android titreşim sistemi (`VibrationManager.cs`), hasar alma, boss gelişi ve level up titreşimleri & kamera sarsıntısı tamamlandı._
-  **Ara Görev (Bugfix): Karakter Doğuşundaki Görünmez Duvar**
    - _NavMesh örnekleme yarıçapı 2'den 5'e çıkarılarak Sylva'nın takılma sorunu çözüldü._
-  **2. Görev: Mobil Performans & Sıfır Bellek Çöpü (Zero-Alloc) Paketi**
    - _`Enemy.ActiveEnemies` statik listesi ile `FindGameObjectsWithTag` çöpleri sıfırlandı, 60 FPS kilidi ve tüm materyallerde GPU Instancing aktif edildi._

---

#### 🟡 SEVİYE 2: Global Sürüm & Dengeleme

-  **3. Görev: Oyun Dilinin Tamamen İngilizceye Çevrilmesi**
    - _Tüm C# UI kodları (`UpgradeManager`, `CombatUIManager`, `BaseUIManager` vb.) ve sahnelerdeki TextMeshPro bileşenleri global standartta İngilizce yapıldı._
-  **4. Görev: 10 Dalga x 30 Saniye (5 Dakikalık Run) Dengelemesi & XP Tatlı Noktası**
    - _5 dakikalık tam run döngüsü, Dalga 5 Elite mini-boss, Dalga 10 Final Boss (Chinar) temizliği, üstel can/hasar formülü ve Level 13-15 XP tatlı noktası tamamlandı._

---

#### 🟠 SEVİYE 3: Düşman Modelleri, Animasyonlar ve Sahne (Şu Anki Durak)

-  **5. Görev: Moss Düşmanı Z-Ekseni, Scale Düzeltmesi & Animasyonlar** _(Şu an buradayız — Duraklatıldı)_
    -  Yüzünün ileri (-Y) bakması için Blender düzeltmesi.
    -  Pivot noktasının `(0,0,0)` merkezine oturtulması ve Scale'in sabitlenmesi.
    -  `Melee.prefab` üzerine Animator Controller ve Idle/Walk/Attack animasyonlarının bağlanması.
-  **6. Görev: Diğer Düşmanların Prefab, Texture ve Boyut (Scale) Ayarları**
    - _SporeHead, Iyv ve Wolfey modellerinin materyallerinin atanması, boyutlarının eşitlenmesi ve animator geçişleri._
-  **7. Görev: Asenkron Yükleme Ekranı (Loading Screen)**
    - _Sahneler arası geçişte donmayı engelleyen hafif animasyonlu yükleme barı/ekranı._

---

#### 🔴 SEVİYE 4: Boss Mekanikleri ve Sinematik Atmosfer

-  **8. Görev: Boss (Chinar/Ent) Animasyonları & Deprem/Kaya Saldırısı**
    - _Yere vurma şok dalgası, `Skill_01` / `Skill_03` geçişleri ve oyuncunun altından kaya fırlama mekaniği._
-  **9. Görev: Boss Ölümünde Terraforming (Yeşillenme) Efekti**
    - _Boss öldüğünde 0.5 sn sessizlik -> sarı-yeşil zehirli sisin dağılması -> masmavi gökyüzü ve "Region Purified" arınma hissi._
-  **10. Görev: Yeni Ara Harita Sahnesi (World Map / Exploration)**
    - _BaseScene ("Explore" butonu) -> WorldMapScene (Orman Bölgesi Seçimi) -> CombatScene sahne akışı._

---

#### 🟣 SEVİYE 5: Hackathon Ödül Paketleri & Gelir Modeli

-  **11. Görev: Canlı Çevrimiçi Ağaç Dikme Sayacı (Online Live Counter)**
    - _Bulut havuzundan anlık dikilen fidan sayısını çeken online servis (`TreeCounterService.cs`) ve UI göstergesi._
-  **12. Görev: RevenueCat (IAP Peace Prize) ve Catvertising (Rewarded Video) Entegrasyonu**
    - _"Plant a Tree" gerçek ağaç bağışı paketinin In-App Purchase akışı, ölüm ekranında zorunlu olmayan "Loot'u 2'ye katla" ödüllü reklam mekaniği._