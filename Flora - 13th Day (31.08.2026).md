# 🌿 Flora

## Dev Log — 14. Gün (01.09.2026)

---

## 🎯 Günün Hedefi

> "Hackathon bitimine ~30 Gün kala Google Play Console'a kayıt olup oyunu incelemeye sokmak ve ilk Android build'i almak."

---

## ✅ TAMAMLANAN İŞLER

---

### 🔧 Adım 1 — Geliştirici Test Hile Menüsü (DeveloperCheats.cs)

**Neden yapıldı:** Oyunun 20 wave'ini tek tek oynayarak test etmek imkansızdı. Hızlı debug için bir araç gerekiyordu.

**Ne yapıldı:**

- `Assets/DeveloperCheats.cs` sıfırdan yazıldı.
- **Klavye kısayolları:**
    - `G` → God Mode (hasar almama)
    - `K` → Tüm düşmanları anında öldür
    - `L` → Seviye atla (ForceLevelUp)
    - `P` → +1000 Tohum ekle
    - `H` → Canı tam doldur
    - `T` → +60 saniye dalga süresine ekle
    - `B` → Boss Chinar'ı anında doğur
    - `1` / `2` / `5` → Hareket hızı çarpanları
- **OnGUI Dev Panel:** Ekranın sol altında şık koyu cam efektli yüzen panel. İçinde:
    - Anlık FPS sayacı
    - Aktif düşman sayısı
    - Dalga zamanlayıcısı
    - Tüm kısayollar için tıklanabilir butonlar
    - `[🧹 SIFIRLA (0 SEED)]` butonu (BaseScene ve CombatScene'de)
- **Release Build Koruması:** `#if !UNITY_EDITOR && !DEVELOPMENT_BUILD` direktifi ile hile menüsü **Release build'lerde otomatik olarak kendini yok eder.** Google Play'e kesinlikle gitmez.

**Değiştirilen dosyalar:**

- `Assets/DeveloperCheats.cs` → YENİ DOSYA oluşturuldu
- `Assets/PlayerHealth.cs` → `isGodMode` boolean eklendi, `TakeDamage()` içinde kontrol
- `Assets/ExperienceManager.cs` → `ForceLevelUp()` public metodu eklendi

**Unity Adımları (Yiğit için):**

1. `DeveloperCheats.cs`'yi Hierarchy'deki `GameManager` objesine sürükle.
2. Play'e basıp `G` tuşuna bas → God Mode test et.

---

### 🔧 Adım 2 — "End Run" Butonu & Tohum Sıfırlama

**Neden yapıldı:** Test sırasında savaş sahnesinden çıkış yoktu. Ayrıca her test sonrası tohumları sıfırlamak için oyunu baştan başlatmak gerekiyordu, bu çok zaman kaybettiriyordu.

**Ne yapıldı:**

- `CombatUIManager.cs`:
    - `Update()` metodu eklendi → `Escape` tuşuna basınca `ToggleSettings()` çağrılır
    - `EndRun()` metodu eklendi → BaseScene'e geri döner
- `GameManager.cs`:
    - `ResetAllSeeds()` → CoreSeed ve TowerCore'u sıfırlar, kaydeder
    - `ResetAllSaveData()` → Tüm PlayerPrefs'i siler
- `BaseUIManager.cs`:
    - `ResetSeedsButton()` ve `ResetAllSaveDataButton()` metotları eklendi (UI butonlarına bağlanacak)

**Sonuç:** Test süreçleri dramatik şekilde hızlandı. Artık Escape'e bas → End Run → BaseScene → Sıfırla → tekrar test.

---

### 🔧 Adım 3 — Polen Enjektörü Yüzen Silah (Orbital) Mekaniği

**Neden yapıldı:** Önceki sistem silahı karakterin önüne sabitledi. Hedef sistem şuydu:

- Silah Sylva'nın etrafında belirli bir yarıçapta yüzer (drone gibi)
- En yakın düşmana doğru yavaşça döner ve ateş eder
- Düşman yokken **son durduğu açıda kalır** (sıfıra sıfırlamaz)
- Flamethrower gibi gelecek silahlar da aynı sistemi kullanacak

**Ne yapıldı:** `Assets/PollenWeapon.cs` tamamen yeniden yazıldı:

**Yeni mimari:**

Sylva (merkez)

  └── PollenWeapon (orbital yörüngede)

        ├── gunModel (3D silah modeli)

        ├── firePoint (namlu ucu — mermi buradan çıkar)

        └── gunAnimator (ateş animasyonu)

**Önemli detaylar:**

- `currentOrbitAngle` değişkeni → Silahın anlık açısını tutar
- Düşman varsa: `Mathf.Atan2()` ile hedef açısı hesaplanır, `Mathf.MoveTowardsAngle()` ile smooth kayar
- Düşman yokken: `currentOrbitAngle` değiştirilmez → Silah son açısında kalır! ✅
- Level 4 ve 8'de çoklu mermi: `spreadAngle` ile yayılır
- Scene Gizmos: `OnDrawGizmosSelected()` ile editor'da menzil ve orbit çemberleri görünür

**Değiştirilen dosyalar:**

- `Assets/PollenWeapon.cs` → Komple yeniden yazıldı

---

### 🏪 Adım 4 — Google Play Console Geliştirici Hesabı Açılışı

**Neden yapıldı:** Hackathon bitimine ~30 saat kalmışken oyunu Google Play incelemesine sokmak kritikti. İnceleme süreci 7 güne kadar sürebilir.

**Yapılan işlemler (sırasıyla):**

1. **Hesap Türü Kararı:**
    
    - Şirket henüz yok → Kişisel hesap açıldı
    - Geliştirici adı: **`Bu Da Bi Game`** (gelecekte BuDaBiGame şirketi açılınca aynı isim kullanılabilir)
    - Uzun vadeli plan: **BuDaBiArt** ana şirketi → **BuDaBiGame** alt kuruluşu
2. **Kayıt Formu:**
    
    - E-posta: Yiğit'in kişisel mail adresi
    - Web sitesi: `https://yigitaybey.com`
    - Ülke: Türkiye
    - Ödeme: **$25** kayıt ücreti ödendi ✅
3. **Doğrulamalar:**
    
    - Ödeme profili bağlandı ✅
    - Android cihaz üzerinden Play Console uygulamasıyla giriş doğrulandı ✅
    - Kimlik belgesi (ID/pasaport) Google'a yüklendi → **Onay bekleniyor (12-24 saat)** ⏳
4. **Android Paket Adı:**
    
    - `com.budabigame.flora` olarak kararlaştırıldı
    - `ProjectSettings/ProjectSettings.asset` dosyasında `companyName: BuDaBiGame` güncellendi

---

### 📄 Adım 5 — Google Play Gizlilik Politikası (Privacy Policy)

**Neden yapıldı:** Google Play, uygulama yayınlamak için geçerli ve erişilebilir bir Privacy Policy URL'si zorunlu tutuyor. Bu olmadan uygulama incelemeye alınmıyor.

**Ne yapıldı:**

- `Flora/PrivacyPolicy.html` sıfırdan yazıldı
- İçerik: Türkçe + İngilizce, **Bu Da Bi Game** ve **Flora: Dünya Geri İstiyor** markasına özel
- Kapsanan maddeler:
    - Toplanan veriler (Reklam ID - AdMob için)
    - Üçüncü taraf hizmetler (Google Play, AdMob, RevenueCat)
    - Çocuk güvenliği (COPPA / GDPR uyumu)
    - İletişim: `budabigame@gmail.com`
- URL: **`https://yigitaybey.com/PrivacyPolicy.html`**
- Durum: Yiğit tarafından FTP ile canlı siteye yüklendi ✅

---

### 📱 Adım 6 — İlk Android Build (APK) Alma Denemesi

**Neden yapıldı:** Oyunun mobilde çalışıp çalışmadığını görmek, grafiklerin nasıl durduğunu anlamak.

**Build ayarları:**

- Platform: **Android** (Active olarak geçildi)
- Build App Bundle: ❌ (APK alındı, .aab değil)
- Development Build: ❌ (temiz release build)
- Texture Compression: **ASTC** (zaten ayarlıydı)
- Graphics API: **Vulkan** eklendi, OpenGLES3 silindi

**Grafik sorunları araştırması (uzun süreç):**

Platform Android'e alınınca editörde görüntü soluklaştı. Araştırılan ve uygulanan düzeltmeler:

|Denenen Çözüm|Sonuç|
|---|---|
|Mobile_RPAsset → RenderScale 0.8→1.0|Kısmen iyileşti|
|Mobile_RPAsset → FastSRGB kapatıldı|Fark yok|
|QualitySettings → Android'e PC quality atandı|Editörde fark yok|
|OpenGLES3 silindi, Vulkan eklendi|Editörde fark yok|
|Mobile_Renderer'a SSAO eklendi|Fark yok|
|BaseScene kamerasında PostProcessing açıldı|Fark yok|

**Gerçek Sebep (Sonunda anlaşıldı):** Unity editörü her zaman **DX12** ile render eder. Android build target seçildiğinde editör önizlemesi değişebilir ama bu **gerçek cihaz görünümünü yansıtmaz.** Asıl test yöntemi gerçek cihaza kurmak.

**APK kurulumu:**

- Tablet'e WhatsApp/Drive üzerinden gönderildi
- "Bilinmeyen kaynak" izni verilerek kuruldu ✅
- Sonuç: Grafik soluk görünüyor, oyun performance sorunları var

---

## ⏳ TAMAMLANAMAYAN / ERTELENENLer

|İş|Sebebi|Öncelik|
|---|---|---|
|Grafik düzeltmesi|Editör preview yanıltıcı, gerçek cihaz testi gerekiyor|🔴 Yarın|
|Performans optimizasyonu|APK'da oyun neredeyse çalışmıyor, FPS sorunu|🔴 Yarın|
|Google kimlik onayı|Google'dan onay emaili bekleniyor|⏳ 12-24 saat|
|.aab yükleme (Play Console)|Kimlik onayı bekliyor|⏳ Onay sonrası|

---

## 🗓️ Yarın İçin Plan (02.09.2026)

1. **Google'dan kimlik onay maili geldi mi?** → Geldi ise Play Console'da SMS doğrulamasını bitir
2. **Performans krizi:** APK'da oyun neden kasıyor? → Object Pooling, Rigidbody sayısı, shader sayısı
3. **Grafik düzeltmesi:** Gerçek cihazda neden soluk? → URP Volume Profile, Tonemapping, Color Grading ayarları
4. **.aab upload:** Play Console'a yükleme ve iç test (Internal Testing) başlatma

---

## 📊 Genel Proje Durumu

|Alan|Durum|
|---|---|
|Google Play Hesabı|✅ Açıldı, ⏳ Kimlik onayı bekliyor|
|Privacy Policy|✅ Yayında|
|Android Build|✅ APK alındı, 🔴 Optimizasyon gerekli|
|Geliştirici Araçları|✅ Dev Console, End Run, Orbital Weapon|
|Hackathon Submission|⏳ ~30 saat|

---

_"Bitkin ama kararlıyız. Flora, dünyayı geri istiyor." 🌱_