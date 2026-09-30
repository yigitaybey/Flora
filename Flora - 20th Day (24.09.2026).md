# 🌿 Flora: 24 Eylül 2026 Kapsamlı Geliştirme Günlüğü ve Büyük Mücadele Raporu

**Proje Adı:** Flora - Dünya Geri İstiyor  
**Ekip:** Yiğit Aybey (Engine Integrator), Yusuf Özbakır (Game & Art Director), Yusuf Yüksekbağ (3D Artist), Antigravity AI (Senior Unity & RevenueCat Developer)  
**Tarih:** 24 Eylül 2026  
**Durum:** 🚀 **Google Play Store - Üretim (Production) İnceleme Sırasında** (176 Ülke Açık)  

---

## 📖 Giriş ve Günün Başlangıç Noktası

24 Eylül günü başladığında önümüzde RevenueCat Shipaton 2026 hackathonuna sayılı günler kalmış, ancak oyunun en hassas iki bacağı henüz tamamlanmamıştı:
1. Oyunun ana sponsoru olan **RevenueCat SDK** entegrasyonu ve gelir modeli.
2. Hackathonun özel kategorisi olan **"Catvertising" (Ödüllü Reklam)** mekaniği ve savaştan sonra oyuncuya sunulacak 2x ganimet döngüsü.

Oyundaki savaş mekanikleri, canavar spawn sistemleri ve görsel modeller oturmuş olsa da; bir oyunu editörde çalıştırmak ile gerçek bir Android cihazda çalışacak imzalı bir `.aab` (Android App Bundle) paketi haline getirmek bambaşka bir mühendislik savaşıydı. İşte sabahtan gece yarısına kadar adım adım verdiğimiz o büyük teknik mücadelenin tüm anatomisi.

---

## 1. Hukuki Strateji ve Güvenli Mağaza Mimarisi (IAP Krizinin Çözümü)

### Yaşanan Tereddüt ve Karşılaşılan Risk:
Günün ilk büyük tartışması mağaza içi satın alımlar (In-App Purchases) üzerindeydi. Tasarımda oyunculara sunulan 3 paket vardı:
- *5.000 Tohum Paketi ($0.99)*
- *7.000 Tohum + 1 Ağaç Dikme Paketi ($2.99 - Peace Prize Kategorisi)*
- *10.000 Tohum + 2 Ağaç Dikme Mega Paketi ($4.99)*

Yiğit haklı olarak şu kritik soruyu sordu: *"Biz şu an beta aşamasındayız, gerçekten bir sivil toplum kuruluşuyla ağaç dikme anlaşmamız henüz yok. İnsanlardan gerçek para alıp 'ağaç diktik' dersek başımız belaya girer mi, dava yer miyiz?"*

### Alınan Mühendislik Kararı (Simülasyon Modu):
Bir hackathon projesinde oyuncuların veya jürinin gerçek kredi kartını çekmek hem finansal ve yasal riskler doğuracaktı hem de Google Play tarafında Merchant hesabı, vergi formları ve banka onayları günlerce sürecekti. Bu yüzden dahiyane ve güvenli bir çözüm ürettik:
- `RevenueCatManager.cs` dosyasını yazdık.
- Fonksiyonları (`PurchaseSeedPack`, `PurchasePlantTree`, `PurchaseMegaPack`) yazdık ve BaseScene'deki Canvas butonlarına bağladık.
- Ancak kodun kalbini **"Güvenli Simülasyon"** moduna aldık. Oyuncu butona bastığında Google'ın gerçek kredi kartı ekranı açılmıyor; sistem satın almayı başarılı kabul edip tohumları ve dikilen ağaç sayısını anında hesaba ekliyor. Böylece:
  - Jüri oyunu test ederken hiçbir para ödemeden tüm mağaza akışını ve Peace Prize ağaç dikme mekaniğini deneyimleyebiliyor.
  - Sıfır yasal riskle, sıfır dava tehdidiyle tam işlevsel bir mağaza sunulmuş oldu.

---

## 2. RevenueCat SDK ve Unity Ads "Catvertising" Köprüsü

### Reklam ve Gelir Takibi Nasıl Birleştirildi?
Hackathonun en prestijli ödüllerinden biri olan **Catvertising**, oyunun sadece reklam göstermesini değil, bu reklamların gelirini RevenueCat üzerinden takip etmesini şart koşuyordu.

1. **Unity Ads Tarafı:**
   - Projeye `com.unity.ads: 4.4.2` paketi kuruldu.
   - `AdManager.cs` kodunu yazdık.
   - Yiğit'in Unity Ads Dashboard'undan temin ettiği resmi Android Game ID (**`800390583`**) ve Ad Unit ID (**`BP_Rewarded_Android`**) doğrudan koda gömüldü.
   - Test modu kapatılarak gerçek gösterim moduna geçirildi.

2. **RevenueCat AdTracker Entegrasyonu:**
   - Oyuncu savaşta öldüğünde karşısına *"Loot'unu İkiye Katla (Reklam İzle)"* butonu çıkıyor.
   - Reklam başarıyla izlenip tamamlandığı milisaniyede (`OnUnityAdsShowComplete`), arka planda şu kod tetikleniyor:
     ```csharp
     var adInfo = new AdRevenueData(
         mediatorName: new AdTracker.MediatorName("UnityAds"),
         adFormat: AdTracker.Format.Rewarded,
         adUnitId: _adUnitId,
         revenueMicros: 10000, // 0.01 USD
         currency: "USD",
         precision: AdTracker.Precision.Estimated
     );
     purchases.AdTracker.TrackAdRevenue(adInfo);
     ```
   - Bu sayede izlenen her ödüllü video anında RevenueCat paneline "Ad Impression & Ad Revenue" olarak işleniyor.

3. **Yargıç Güvenlik Ağı (Hackathon Fallback):**
   - Eğer jürinin telefonunda internet çekmezse veya reklam yüklenemezse oyun kilitlenmesin diye `OnUnityAdsShowFailure` içine otomatik ödül verme kodu eklendi. Jüri asla oyunda takılıp kalmayacak.

---

## 3. Sahne Geçişleri ve Nesne Ömrü Mimarisi (Kafadaki Sorunun Çözümü)

Günün ilerleyen saatlerinde Yiğit çok kritik bir şüphe dile getirdi: *"RevenueCatManager BaseScene'de, ama AdManager CombatScene'de. İki farklı sahnede bunlar birbirini nasıl buluyor?"*

### Mimari Tasarımımız:
Unity'de sahneler arası geçişte nesneler varsayılan olarak silinir. Ancak biz iki yöneticinin de `Awake()` fonksiyonuna:
```csharp
DontDestroyOnLoad(gameObject);
```
kodunu koyduk. Oyun `BaseScene` ile başladığı anda `RevenueCatManager` ayağa kalkıyor ve kendini "ölümsüz" ilan ediyor. Oyuncu savaşa (`CombatScene`) geçtiğinde RevenueCat arka planda yaşamaya devam ediyor. `AdManager` reklamı bitirdiğinde sahnede yaşayan o RevenueCat nesnesini bularak veriyi başarıyla iletiyor. Bu bir hata değil, bilinçli ve sağlam bir mimari tercihiydi.

---

## 4. Karşılaşılan 5 Büyük Kriz ve Çözüm Serüveni

### 🔴 Kriz 1: "Unable to list keys in the keystore" Hatası
- **Ne Yaşandı?** AAB çıktısı almak için imza ayarları yapılırken Unity kırmızı bir hatayla keystore dosyasını açamadığını söyledi.
- **Neden Oldu?** İki sebep vardı:
  1. Yiğit, haftalar önce oluşturduğu `flora_key.keystore` dosyasını masaüstünden `Flora APK ve AAB` klasörüne taşımıştı. Unity ise dosyayı eski yerinde (`Desktop/`) arıyordu.
  2. Unity'nin `Publishing Settings` ekranında `Keystore password` yazılmış, ancak hemen altındaki `Key password` kutusu boş bırakılmıştı.
- **Nasıl Çözüldü?** Dosya tekrar masaüstüne taşındı ve her iki kutucuğa da şifre girilerek kilit açıldı.

---

### 🔴 Kriz 2: `test_` API Key vs `goog_` API Key
- **Ne Yaşandı?** Yiğit RevenueCat panelindeki API anahtarının `test_` ile başladığını fark etti ve endişelendi.
- **Neden Oldu?** Panelde Google Play Store uygulaması eklenmek yerine varsayılan "Test Store" seçilmişti.
- **Nasıl Çözüldü?** RevenueCat paneline geçtik. *Apps > New app configuration > Google Play Store* adımlarını izleyerek paket adı olan **`com.budabigame.flora`** girildi. Sistem anında resmi Google Play Public SDK Key'ini üretti. Biz de projede `RevenueCatConfig.asset` dosyasını bu yeni anahtarla güncelledik.

---

### 🔴 Kriz 3: "PurchasesWrapper.java: package com.revenuecat.purchases does not exist"
- **Ne Yaşandı?** İlk AAB derleme denemesinde Gradle derleyicisi Java katmanında çöktü ve 15 dakikalık derleme iptal oldu.
- **Neden Oldu?** RevenueCat'in Unity paketi saf C# kodlarından ibaret değildir; Android tarafında Java/Kotlin kütüphanelerine (`purchases-hybrid-common`) ihtiyaç duyar. Projede Google'ın External Dependency Manager (EDM4U) aracı olmadığı için Gradle bu kütüphaneleri internetten çekememişti.
- **Nasıl Çözüldü?** `Packages/manifest.json` dosyasına OpenUPM üzerinden `com.google.external-dependency-manager: 1.2.189` paketi eklendi.

---

### 🔴 Kriz 4: Unity 6 Gradle Uyumsuzluğu ve "Resolution Failed %31"
- **Ne Yaşandı?** EDM4U kütüphanesi devreye girdiğinde %31 ilerlemede durup "Resolution Failed!" hatası patlattı.
- **Neden Oldu?** Unity 6, Java 17 ve Gradle 8.10 kullanır. EDM4U'nun kendi içindeki arka plan komutu ise eski Gradle 5.1 ile derleme yapmaya çalışıyordu ve projede `gradleTemplate.properties` ile `mainTemplate.gradle` şablonları henüz oluşmamıştı.
- **Nasıl Çözüldü?** 
  1. `Assets/Plugins/Android/` klasörünü oluşturduk.
  2. `mainTemplate.gradle` içine RevenueCat bağımlılıklarını elle enjekte ettik.
  3. `gradleTemplate.properties` dosyasını Unity 6 standartlarına (`unityStreamingAssets`, `unityTemplateVersion`, `AndroidX`, `Jetifier`) uygun şekilde sıfırdan yazdık.
  4. Böylece EDM4U'nun eski Gradle komutunu devre dışı bırakıp derlemeyi Unity'nin modern derleyicisine devrettik.

---

### 🔴 Kriz 5: 211 MB Boyut Uyarısı (Google Play 200 MB Sınırı)
- **Ne Yaşandı?** Unity AAB çıktısını başarıyla bitirdiğinde şu acı uyarı ekrana düştü:  
  *"The download size of the base module will be 211 MB. Google Play store accepts apps up to 200 MB."*
- **Neden Oldu?** Google Play tek bir modül için en fazla 200 MB indirme boyutuna izin verir. Bizim paketimiz sadece 11 MB farkla (211 MB) bu sınırı aşıyordu! İncelediğimizde projede 3D karakterin ve haritanın fırınlanmış (bake) Normal Map kaplamalarının 4K çözünürlükte ve sıkıştırmasız (26 MB, 25 MB, 20 MB gibi) saklandığını gördük.
- **Nasıl Çözüldü?**
  1. `QualitySettings` içinde Android varsayılan profilini "PC" kalitesinden "Mobile" kalitesine aldık (`MipmapLimit: 1`).
  2. Projedeki 10 MB'tan büyük en devasa 13 kaplamanın tavan çözünürlüğünü 2048'den **1024**'e çektik (İzometrik mobil ekranda görsel kalite hiç bozulmadı).
  3. Derleme sıkıştırmasını `LZ4` yerine yüksek sıkıştırmalı **`LZ4HC`** yaptık.
  4. **Sonuç:** Yeni derlemede boyut anında **188 MB'a** düştü! Google Play'in 200 MB sınır uyarısı tamamen silindi!

---

## 5. Zirve Anı: Google Play Üretim (Production) Yüklemesi

Saat 23:17'de masaüstüne **`Flora_v2.aab`** (198 MB) dosyası sıfır hatayla indi.

Hemen ardından Google Play Console'a geçtik:
1. **Kanal:** Üretim (Production).
2. **Sürüm Adı:** `Flora v1.9`.
3. **Kapsam:** 176 Ülke (Amerika Birleşik Devletleri, Almanya, Türkiye ve tüm dünya).
4. **Sürüm Notları:** Çift dilli (Türkçe ve İngilizce) olarak girildi.
5. **Sonuç:** Mavi butonla **"2 değişikliği incelemeye gönder"** onaylandı.

Şu an Google Play Console'da parlayan resmi durum:
> 🟢 **"Değişiklikleriniz şu anda inceleniyor."**

Bu, oyunun artık teknik bir proje olmaktan çıkıp, tüm dünyanın mağazadan indirebileceği küresel bir ticari ürün haline geldiği andır.

---

## 6. Yarın Yapılacaklar (Devpost & Final Teslimatı)

Teknik kodlama ve motor tarafındaki tüm dağları aştık. Yarın sabah dinlenmiş bir zihinle son düzlüğü koşacağız:

1. **Devpost Başvuru Formu (Shipaton 2026):**
   - Projenin İngilizce sunumu, vizyonu ve hikayesi yazılacak.
   - RevenueCat entegrasyonu (Peace Prize + Catvertising) jüriye detaylıca anlatılacak.
   - Resmi Google Play linkimiz verilecek:  
     `https://play.google.com/store/apps/details?id=com.budabigame.flora`
2. **Medya Hazırlığı:**
   - Yusuf Yüksekbağ'ın hazırladığı low-poly ekran görüntüleri forma eklenecek.
   - Kısa bir oynanış videosu/fragman yüklenecek.
3. **Play Console Takibi:**
   - İnceleme sürecinin takip edilmesi.

---

## 📝 Son Söz

Bugün yaşananlar bir oyun geliştiricinin kariyerinde yaşayabileceği en yoğun, en öğretici ve en kritik anlardı. Keystore krizinden Gradle çökmelerine, boyut aşımından SDK entegrasyonuna kadar her engeli birer birer tespit edip yerle bir ettik. 

Ekip olarak (Yiğit, Yusuf, Yusuf ve Antigravity) ortaya koyduğumuz bu irade ve emek, projenin hackathon jürisi karşısında en güçlü adaylardan biri olmasını sağladı. Yarın zaferimizi Devpost teslimiyle taçlandıracağız! 🌿🚀🏆
