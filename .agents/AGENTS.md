# Proje: Flora - Dünya Geri İstiyor

## Hedefler ve Kapsam
- **Hedef:** RevenueCat Shipaton 2026 Hackathon (Kategoriler: Peace Prize, Best Game, Catvertising, HAMM Award).
- **Tür:** İzometrik 3D Low-Poly, Bullet Haven + Base Building.
- **Teslim:** Ağustos-Eylül 2026 arası (iOS/Android).

## Ekip Rolleri ve Davranış Kuralları
1. **Yiğit Aybey (Kullanıcı):** Engine Integrator. Unity'de acemi. Asla C# kodu yazmaz. Kodları AI'dan kopyalayıp Unity arayüzünde bağlar.
2. **Yusuf Özbakır (Game & Art Director):** Hikaye yazarı, oyunun genel görsel ve tasarımsal patronu.
3. **Yusuf Yüksekbağ (Tasarımcı):** 3D Low-Poly model (.fbx/.png) sağlayıcısı, çevre ve karakter sanatçısı.
4. **AI (Antigravity):** Senior Unity Developer. Tüm C# kodlarını hatasız yazar, RevenueCat entegrasyonunu ve optimizasyonu sağlar.
   - **Önemli Davranış Kuralı:** AI, Yiğit'e her zaman "Şuraya tıkla, kodu şuraya sürükle" şeklinde basit, cesaret verici ve adım adım Unity arayüz (UI) talimatları vermelidir.

## Hikaye ve Oyunun Ruhu
- **Klorofil Uyanışı:** İklim krizi sonrası evrimleşen, avlanan bitkiler. Zehirli sarı-yeşil bir sis (Karbondioksit).
- **Ana Karakter (Sylva):** Dünyayı yok etmeden "Saf Tohumları" kullanarak bitkileri iyileştirmeyi amaçlayan eski bir botanikçi.
- **Terraforming (Yeşillenme):** Boss (Örn: Ent) kesildiğinde, 0.5 saniyelik sessizlik sonrası zehirli karanlık ortamın aniden masmavi gökyüzülü, parlak ve canlı bir doğaya dönüşmesi. "Bölge Arındırıldı" hissi.

## MVP Kapsamı ve Mekanikler
- **Haritalar:** Sadece "Radyo Kulesi (Base)" ve "Orman (Savaş Alanı)".
- **Silahlar (Otomatik Ateşlenir):** Polen Enjektörü (Tekli hızlı), Flamethrower (Alan hasarı), Balta (Yakın dövüş yörünge), **UV Lambası** (Karakterin etrafında sürekli hasar veren aura - Vampire Survivors Garlic benzeri).
- **Düşmanlar (NavMesh kullanır):** Avcı Bitkiler, Spore Taşıyıcı, Asit Salgılayan Türler, Fidanlar, Evrimleşmiş Kurt, Yarasa ve ENT (Boss).
- **NPC:** İnşaat Ustası (Base kısmında).
- **Savaş Döngüsü:** 20 Dalga (Wave). WASD ile hareket, otomatik ateş. Düşen tohumlar (XP) ile seviye atlama ve rastgele yetenek seçimi (Vampire Survivors RNG sistemi).

## Teknik ve Gelir Modeli Kuralları
1. **Optimizasyon:** Yüzlerce düşman için KESİNLİKLE "Object Pooling" kullanılacak. Rigidbody (fizik motoru) kullanımından olabildiğince kaçınılıp, hasar hesaplamaları matematiksel mesafe (Vector3.Distance veya sqrMagnitude) ölçümleriyle yapılacak.
2. **Kamera:** İzometrik (Orthographic) açı kullanılacak.
3. **RevenueCat Peace Prize:** In-App Purchase ile "Ağaç Dikme" paketi satılacak, oyun içi haritada dünyayı yeşillendirme bağışı yapılacak.
4. **Catvertising:** Oyuncu öldüğünde asla zorunlu reklam çıkmayacak. Sadece "Lootunu 2'ye katlamak için izle" (Rewarded Video) seçeneği sunulacak.
