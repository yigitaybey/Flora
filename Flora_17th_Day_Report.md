# Flora - 17th Day (21.09.2026) Günlük Geliştirici & Post-Mortem Raporu

**Proje:** Flora - Dünya Geri İstiyor  
**Tarih:** 21 Eylül 2026 (Gece Oturumu: 22 Eylül 02:45)  
**Rol:** Senior Unity Developer (AI) & Engine Integrator (Yiğit Aybey)  
**Hedef:** RevenueCat Shipaton 2026  

---

## 1. Genel Özet ve Günün Başlangıç Hedefleri
Bugün (17. Gün), projenin teknik temelini sağlamlaştırmak, oyun hissini (game feel) cilalamak, yeni düşman varyasyonlarını savaş alanına entegre etmek ve 5 dakikalık (10 Wave) yarışma döngüsünü dengelemek için masaya oturduk.

### Planlanan Başlıklar:
1. **Level 1 (Game Feel & Polish):** Mobil haptik titreşimler, ekran sarsıntısı (screen shake), görünmez duvar / spawn bugfix'i ve İngilizce yerelleştirme.
2. **Level 2 (Pacing & Balance):** 10 Dalga / 5 dakikalık akıcı savaş ritmi, mobil için sıfır bellek tahsisli (Zero-Allocation) düşman takip sistemi.
3. **Level 3 (Düşman Mekanikleri & Çeşitlilik):** SporeHead, Iyv ve Fast düşmanlarının savaşa hazırlanması.
4. **UI & Loading Entegrasyonu:** Asenkron yükleme ekranı ve Yusuf'un yeni UI tasarımlarının projeye dahil edilmesi.

---

## 2. Neler Başarıyla Tamamlandı? (Yapılanlar)

### A. Game Feel ve Fizik / Kamera Cilası (Level 1)
- **Ekran Sarsıntısı (Screen Shake):** Oyuncu darbe aldığında veya büyük patlamalar olduğunda kameranın dinamik şekilde sarsılması sağlandı (`CameraFollow.cs`).
- **Mobil Haptik Geri Bildirim:** iOS & Android için düşük seviyeli titreşim altyapısı kuruldu (`HapticFeedback.cs`). Kritik vuruşlarda ve patlamalarda hissedilir tokluk sağlandı.
- **Sınır Duvarı Spawn Hatası Çözüldü:** Haritanın uç noktalarında doğan düşmanların görünmez duvarlara takılıp kalma sorunu `NavMesh.SamplePosition` ile harita içine zorlanarak giderildi.
- **Global İngilizce Çeviri:** Tüm UI metinleri, yükseltme kartları (Upgrade Cards) ve sistem mesajları uluslararası yarışma standartlarına uygun akıcı bir İngilizceye çevrildi.

### B. Performans & Savaş Döngüsü (Level 2)
- **Zero-Allocation Düşman Takibi:** Yüzlerce düşmanın olduğu anlarda `FindObjectsOfType<Enemy>()` çağrısı çöpe atıldı; statik `Enemy.ActiveEnemies` hashset/listesi ile 60 FPS kilitlendi.
- **10 Wave / 5 Dakika Döngüsü:** Oyun 20 dalgadan, tam hackathon jürisinin sıkılmadan test edebileceği 10 dalgalık dinamik ve yoğun bir tempoya uyarlandı.

### C. Düşman Mekaniklerinin Hayata Geçirilmesi (Level 3 - Part 1)
- **SporeHead (Kamikaze Mantarı):**
  - Animator Controller (`SporeHeadController.controller`) kuruldu, yürüme animasyonu bağlandı.
  - Oyuncuya 2.5 metre yaklaştığında 0.35 saniye boyunca %145 oranında şişme (swell/inflate) efekti eklendi.
  - Şişme sonrası kamerayı titreten, haptik vuran ve etrafa hasar saçan sıcak turuncu/alev tonlarında prosedürel patlama partikülü kodlandı (`SporeExplosionVFX.cs`).
- **Iyv (Menzilli Sarmaşık Atıcı):**
  - Model ölçeği insan boyutlarına çekildi.
  - Saldırı anında `Attack` trigger'ı ile menzilli tükürük fırlatma döngüsü bağlandı.
  - Oyuncu mermisi ile düşman mermisinin karışmaması için yeşil/zehir tonlu `EnemyBullet.prefab` hazırlandı.

### D. Zorluk ve Eşya Dengelemesi (+%20 Buff)
- **Oyun Zorluğu Arttırıldı:**
  - Sylva'nın canı: 25 -> 30 HP
  - Boss Chinar'ın canı: 500 -> 600 HP
  - Düşman taban hasarı: 8 -> 9.6
  - Düşman doğma sıklığı (SPS): 1.1 -> 1.32
- **Düşen Eşyalar (Consumables) Arttırıldı:**
  - Doğma aralığı 30 saniyeden 25 saniyeye indirildi.
  - Can iksiri, mıknatıs ve tohum düşme şansları %20 artırıldı.

### E. Asenkron Yükleme Ekranı (Loading Screen)
- `LoadingScreen.cs` sıfırdan yazıldı. Kod üzerinden kendi Canvas'ını, arka planını, ilerleme çubuğunu ve oynanış ipuçlarını otomatik üreten modern bir sistem kuruldu.
- Yusuf'un çizdiği asma sarmaşık tasarımı (`Ekran Alıntısı-Photoroom.png`) açık neon/canlı yeşile boyanarak ekranın üst kısmına dekoratif olarak oturtuldu.

---

## 3. Neler Yapılamadı ve Neden Ertelendi?

1. **Fast Düşmanı (Wolfey / Hızlı Kurt):**
   - *Neden Yapılamadı:* 3D modeli ve animasyonları art ekibinden henüz hazır gelmediği için oyuna eklenemedi.
   - *Çözüm:* Dalga sistemi (Wave 1-10) sadece Moss, SporeHead ve Iyv arasında dengelendi. Model gelince 2 dakikada eklenebilecek altyapı hazır bırakıldı.
2. **Yusuf'un Yeni UI Tasarımlarının Tam Entegrasyonu:**
   - *Neden Yapılamadı:* Tasarımlar Drive'dan başarıyla indirildi (`Assets/UI-20260921T232430Z-1-001/UI/`), ancak saat 02:30'u geçtiği, aşırı yorgunluk ve dikkat dağınıklığı riski olduğu için yarına bırakıldı.
3. **Moss ve Boss Görsel/Pivot Cilası:**
   - *Neden Ertelendi:* Bilinçli olarak oyunun en son cilalama fazına bırakıldı.

---

## 4. Karşılaşılan Sorunlar, Hatalar ve Çözümleri (Post-Mortem)

### Hata 1: SporeHead Patlama Efektinin İlk Başta Mor/Görünmez Olması
- **Sorun:** Unity'de hazır materyalsiz oluşturulan dinamik partiküller "Missing Material" (mor kare) hatası verdi.
- **Ders & Çözüm:** Unity'nin `Sprites-Default` shader'ını runtime'da yakalayan, parçacıkları alev kırmızısı ve sarı gradyana boyayan `SporeExplosionVFX.cs` yazılarak harici asset gerektirmeyen kusursuz bir patlama üretildi.

### Hata 2: Yusuf'un Sarmaşık PNG'sinin UI'da Görünmemesi
- **Sorun:** Unity dışarıdan atılan PNG'yi varsayılan olarak `Texture2D (Default)` türünde içeri aldı. UI Image bileşeni ise yalnızca `Sprite (2D and UI)` kabul ediyordu.
- **Ders & Çözüm:** `.meta` dosyası güncellenerek `textureType: 8` (Sprite) yapıldı ve sorun anında çözüldü.

### Hata 3: Moss Düşmanının Y Scale Oynandığında Havaya Uçması
- **Sorun:** Moss'un Scale'i ile oynandığında modelin ayakları yerde kalmak yerine gökyüzüne doğru süzülmesi.
- **Teşhis:** `moss.fbx` binary analizi yapıldı. Diğer modellerin taban pivotu $Y = 0$ iken, Moss modelinin AI/Tripo export'u sebebiyle pivotunun göbeğinde kaldığı ve $X$ ekseninde de sağa kayık olduğu saptandı.
- **Karar:** Kullanıcı isteğiyle koda/modele dokunulmadı; yarın veya son fazda Blender'da "Apply All Transforms" yapılarak çözülecek notu düşüldü.

---

## 5. Yarın (18. Gün) İçin Aksiyon Planı

1. **Part 2: UI Overhaul (Öncelik #1):**
   - İndirilen UI paketindeki HUD elemanlarının (HP Barı, XP Barı, Seviye Göstergesi) bağlanması.
   - Vampire Survivors RNG Yükseltme Kartlarının Yusuf'un yeni kart tasarımlarına dönüştürülmesi.
   - Radyo Kulesi (Base Scene) menüsünün ve yetenek ağacının giydirilmesi.
2. **Kapsamlı Test & Mobil Derleme Hazırlığı:**
   - Dokunmatik kontroller (Virtual Joystick) ve performans testleri.

---

**Günün Notu:** Bugün projenin oyun içi omurgası, dengesi ve hissi seviye atladı. Hackathon teslimine hazır, temiz ve sağlam adımlarla ilerliyoruz! 🌿🚀
