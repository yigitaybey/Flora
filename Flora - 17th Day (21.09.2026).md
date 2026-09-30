# Flora Mega Sprint: Güncellenmiş Yol Haritası & Check-List

> [!IMPORTANT]
> **Tekli Görev Kuralı:** Bir iş tamamen bitmeden ve sen **"tamam bu bitti"** demeden asla sonraki göreve geçilmeyecektir. Adım adım, test ederek ilerleyeceğiz.

---

### ✅ TAMAMLANANLAR (Seviye 1 & 2)
- [x] **Mobil Haptic Titreşim & Screen Shake:** Hasar, boss ve level up titreşimleri ile kamera sarsıntısı.
- [x] **Spawn Görünmez Duvar Bugfixi:** NavMesh örnekleme yarıçapı düzeltildi.
- [x] **Mobil Optimizasyon & Sıfır Bellek Çöpü:** 60 FPS kilidi, GPU Instancing, statik düşman listesi.
- [x] **Global Standartta İngilizce Çevirisi:** UI, kartlar ve buton metinleri.
- [x] **10 Dalga x 30 Saniye (5 Dk Run) Dengelemesi:** Üstel can/hasar formülü, Level 13-15 tatlı noktası.

---

### 🎯 ŞU ANKİ ODAK (SEVİYE 3: Düşmanlar & Yeni UI & Sahne)

#### 👾 1. Bölüm: Düşmanların Kod, AI, Prefab ve Animasyon Stabilizasyonu
- [x] **Düşman Kod & AI Mantığı İyileştirmesi:** NavMesh durma mesafeleri, `Attack` trigger senkronizasyonu tamamlandı.
- [x] **SporeHead (Kamikaze) Paketi:** Yürüme animasyonu, 2.2m'de durup %45 şişme sekansı ve alevli küresel patlama partikülü tamamlandı.
- [x] **Iyv (Menzilli Zehir) Paketi:** Scale ayarı, Run-Attack animasyon geçişi ve zehirli asit mermisi VFX'i tamamlandı.
- [x] **Fast (Wolfey):** Model eksikliği sebebiyle elendi, spawn şansları diğer 3 düşmana sorunsuz paylaştırıldı.
- [x] **%20 Zorluk & %20 Eşya Dengelemesi:** Düşman canı (+%20), hasarı (+%20), spawn hızı (+%20) artırıldı; iksir spawn süresi 30s -> 25s yapıldı, düşme şansları %20 yükseltildi.
- [ ] **Moss (Melee) & Boss (Chinar):** (Sonraya bırakıldı / Seviye 4'te deprem ve sinematikle ele alınacak).

#### 🎨 2. Bölüm: Yusuf'ların Yeni UI Tasarımlarının Entegrasyonu
*Tasarım ekibinin hazırladığı yeni arayüzlerin Unity'ye aktarılıp giydirilmesi:*
- [ ] **5. Savaş Arayüzü (In-Game Combat HUD):**
  - Yeni Can Barı (HP Bar), XP Barı, Seviye Göstergesi, Dalga/Süre Sayacı ve Kill Counter giydirmesi.
- [ ] **6. Seviye Atlama & Kart Seçim Ekranı (Upgrade UI):**
  - Yeni kart çerçeveleri, ikon yerleşimleri, nadirlik efektleri ve seçim butonları.
- [ ] **7. Oyun Sonu Ekranları (Victory & Game Over):**
  - Arındırma/Zafer ekranı, Yenilgi ekranı, istatistik tablosu ve buton tasarımı.
- [ ] **8. Base (Radyo Kulesi) & Menü Ekranları:**
  - Ana menü butonları, tohum/kaynak göstergeleri ve bina yükseltme panelleri.

#### ⏳ 3. Bölüm: Sahne Geçişi
- [x] **9. Asenkron Yükleme Ekranı (Loading Screen):** Sahneler arası (BaseScene <-> CombatScene) donmayı önleyen, kararmalı, bar dolumlu, oyun içi ipuçlu ve asenkron yükleme sistemi (`LoadingScreen.cs`) tamamlandı.

---

### ⏳ EN SON YAPILACAKLAR (Seviye 4 & 5: Boss Mekaniği, Sinematik & Hackathon Backend)

#### 🔴 Seviye 4: Boss Mekanikleri ve Sinematik Atmosfer
- [ ] **10. Boss Chinar Animasyonları & Deprem/Kaya Saldırısı:**
  - Yere vurma şok dalgası, `Skill_01` / `Skill_03` ve oyuncunun altından kaya fırlama mekaniği.
- [ ] **11. Boss Ölümünde Terraforming (Yeşillenme) Efekti:**
  - 0.5 sn sessizlik -> sarı-yeşil zehirli sisin kalkması -> masmavi gökyüzü ve "Region Purified" sinematiği.
- [ ] **12. Ara Harita Sahnesi (World Map / Exploration):**
  - BaseScene ("Explore") -> WorldMapScene (Bölge Seçimi) -> CombatScene.

#### 🟣 Seviye 5: Hackathon Ödül Paketleri & Gelir Modeli
- [ ] **13. Canlı Çevrimiçi Ağaç Dikme Sayacı (Online Live Counter):**
  - Bulut servisinden anlık dikilen fidan sayısını çeken sistem (`TreeCounterService.cs`) ve UI göstergesi.
- [ ] **14. RevenueCat (IAP Peace Prize) ve Catvertising (Rewarded Ads):**
  - "Plant a Tree" In-App Purchase akışı, ölüm ekranında zorunlu olmayan "Loot'u 2'ye Katla" ödüllü video reklamı.

# 🌿 Flora - 17th Day (21.09.2026) Günlük Geliştirici & Post-Mortem Raporu

### 1. Neler Yaptık? (Tamamlananlar)

- **Mobil Haptik & Ekran Sarsıntısı (Screen Shake):** Hasar aldığımızda veya patlamalarda kameranın sarsılması ve telefonda tokluk hissi veren titreşim sistemi kuruldu (`CameraFollow.cs`, `HapticFeedback.cs`).
- **Harita Sınırı Bugfix'i:** Düşmanların spawn olurken görünmez duvarlara takılıp donması NavMesh sınırlandırmasıyla çözüldü.
- **Mobil 60 FPS Optimizasyonu:** Sahadaki yüzlerce düşmanı ararken telefonu kasan yöntemler kaldırıldı, sıfır bellek harcayan `Enemy.ActiveEnemies` listesine geçildi.
- **Global İngilizce Desteği:** Tüm kartlar, UI yazıları ve bildirimler hackathon standartlarında İngilizceye çevrildi.
- **10 Dalga / 5 Dakika Hackathon Döngüsü:** 20 wave yerine jürinin sıkılmadan tam deneyim yaşayabileceği 5 dakikalık akıcı savaş ritmi oluşturuldu.
- **SporeHead (Kamikaze Mantarı):** Yürüme animasyonu bağlandı, oyuncuya yaklaşınca **0.35 saniye boyunca %145 şişip** ardından ekranı titreten sıcak alevli parçacıklarla patlaması sağlandı (`SporeExplosionVFX.cs`).
- **Iyv (Menzilli Düşman):** Boyutu düzeltildi, animatörüne `Attack` bağlandı ve yeşil zehir mermisi fırlatması sağlandı.
- **Zorluk & Consumable Dengelemesi:** Oyun zorluğu %20 artırıldı (Canlar ve hasarlar buff'landı), tohum ve can düşme sıklığı da aynı şekilde %20 artırıldı.
- **Asenkron Yükleme Ekranı (Loading Screen):** Oyun sahneleri arası donmayı engelleyen, ipuçları gösteren ve Yusuf'un açık yeşil sarmaşıklarıyla süslenen modern yükleme ekranı yazıldı (`LoadingScreen.cs`).

---

### 2. Neler Yapamadık ve Neden Ertelendi?

1. **Fast Düşmanı (Wolfey):** Art ekibinden 3D model ve animasyonlar henüz hazır gelmediği için oyuna eklenemedi; wave sistemi Moss, SporeHead ve Iyv arasında dengelendi.
2. **Yeni UI Tasarımlarının Giydirilmesi:** Yusuf'un attığı tüm görseller Drive'dan başarıyla indirildi (`Assets/UI-...`), ancak saat 02:30'u geçtiği ve yorgunluk kaynaklı hata yapmamak adına yarına ertelendi.
3. **Moss ve Boss Cilası:** Kararlaştırdığımız gibi bilinçli olarak en son aşamaya bırakıldı.

---

### 3. Nerelerde Hata Yaptık / Tıkandık? (Dersler)

1. **SporeHead Patlama Partikülü:** İlk başta Unity'nin varsayılan materyali yüzünden mor renkte görünmüştü. Harici assete ihtiyaç duymayan dinamik bir C# partikül scripti (`SporeExplosionVFX.cs`) yazılarak düzeltildi.
2. **Sarmaşık PNG'si Görünmeme Sorunu:** Unity dışarıdan indirilen PNG'yi `Texture2D` olarak algıladığı için UI'da görünmedi. Meta dosyasından `Sprite (2D and UI)` formatına çevrilerek çözüldü.
3. **Moss Y-Scale Uçma Sorunu:** Model incelendiğinde FBX pivot noktasının ayaklarda ($Y=0$) değil, göbekte kaldığı ve $X$ ekseninin de kayık export edildiği anlaşıldı. Gelecekte Blender'da _Apply All Transforms_ yapılarak düzeltilmek üzere not alındı.

---

### 4. Yarın (18. Gün) İlk Ne Yapacağız?

- İndirdiğimiz UI paketini açıp **Combat HUD (Can barı, XP barı)**, **Seviye Atlama Kartları** ve **Radyo Kulesi (Base Menüsü)** tasarımlarını sırayla Canvas'a entegre edeceğiz.

Detaylı rapora istediğin zaman 

![](vscode-file://vscode-app/c:/Users/YigiT3545/AppData/Local/Programs/Antigravity%20IDE/resources/app/extensions/theme-symbols/src/icons/files/markdown.svg)

Flora_17th_Day_Report.md dosyasından ulaşabilirsin. Bugün gerçekten devasa bir ilerleme kaydettik, eline koluna sağlık! Şimdi güzelce dinlen, yarın kaldığımız yerden aynen devam ediyoruz! 🌿💤