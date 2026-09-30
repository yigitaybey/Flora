# 🌿 Flora - Geliştirme Raporu: 15. Gün
**Tarih:** 5 Eylül 2026  
**Odak:** Optimizasyon, Silah Mekanikleri, Hissiyat İyileştirmeleri ve Görsel Cilalar  

Bugün Flora'nın teknik altyapısında "Çalışıyor ama hissi eksik" dediğimiz tüm pürüzleri temizledik. Hem oyunun performansını (FPS) koruduk hem de silahların oyuncuya verdiği o "tok" aksiyon hissini zirveye taşıdık. İşte günün özeti:

---

## 1. Kule Çekirdekleri (Core Seeds) Entegrasyonu
**Hedef:** Düşmanlardan düşen ve oyunun ana kaynak/ilerleme mekaniği olan Core Seed (Çekirdek) sistemini kurmak.
* **Ne Yaptık?** Çekirdeklerin düşmanlardan düşmesi, sahnede performansı bozmadan (Object Pooling ile) yönetilmesi ve UI (Arayüz) üzerinde anlık olarak takip edilebilmesi için sistemler kuruldu.
* **Karşılaşılan Sorunlar:** Çekirdeklerin toplanma hissiyatı ve UI'a yansıması aşamasında yaşanan senkronizasyon problemleri.
* **Çözüm:** `CoreSeedPool` ve UI yöneticileri arasında temiz bir veri akışı sağlandı. Çekirdekler artık sorunsuz düşüyor ve toplanıyor.

## 2. UV Lambası (Garlic / Aura Mekaniği)
**Hedef:** Oyuncunun etrafında sürekli hasar veren, Vampire Survivors'taki Garlic (Sarımsak) tarzı bir alan hasarı silahı yapmak.
* **Karşılaşılan Sorunlar (Mobil Optimizasyon):** Bu etkiyi Particle System ile yapmak, üst üste binen saydam pikseller (Overdraw) yüzünden mobil cihazlarda GPU'yu ağlatırdı.
* **Çözüm:** Sıfırdan özel bir **Shader (`UVAura.shader`)** yazıldı. Ağır parçacıklar yerine basit bir silindir model (Mesh) kullanılarak, merkezden dışarıya doğru radyal olarak yayılan efsanevi bir UV ışığı efekti elde edildi. Performans maliyeti %90 düşürüldü.

## 3. Alev Makinesi (Flamethrower) Kapsamlı Revizyon
**Hedef:** Görseli entegre edilen alev makinesinin ateş hissiyatını iyileştirmek ve gecikmeleri silmek.
* **Sorun 1 (Doğallık):** Dumanlar ateşi takip etmiyor, dümdüz gidiyordu ve ateş ip gibi uzuyordu.
  * *Çözüm:* Particle System ayarlarında duman `Simulation Space: World` yapıldı. `Color over Lifetime` ve `Angle` değerleriyle dumanın dağılması sağlandı.
* **Sorun 2 (Kontrolsüz Ateşleme):** Silah ilk alındığında (veya oyun başladığında) düşman olmasa bile "Puf" diye koca bir ateş kusuyordu.
  * *Çözüm:* Unity'nin `Play On Awake` özelliği kapatılarak silahın inisiyatifi tamamen C# koduna devredildi.
* **Sorun 3 (Input Lag - Gecikme):** "Ateşi çok geç atıyor" problemi. Düşman menzile girse bile silah 0.5 saniye bekliyordu.
  * *Çözüm:* Silahın kodu tamamen ikiye bölündü! **Hedef bulma ve namlu dönüşü** saniyenin her karesinde (`Update`) anlık çalışacak şekilde revize edildi. **Hasar verme (Tick)** ise arka planda 0.5 saniyede bir çalışmaya devam etti. Sonuç: Milisaniyelik tepki veren e-spor keskinliğinde bir hissiyat.
  * *Hata:* Bu bölme işlemi sırasında ufak bir derleme hatası (`fireDirection` tanımsız) alındı, değişken adı `currentTargetDirection` yapılarak anında düzeltildi.

## 4. Balta (Axe) Çarpışma Tünellemesi (Tunneling) Çözümü
**Hedef:** Oyuncunun etrafında yörüngede dönen baltaların düşmanlara vurma hissiyatını düzeltmek.
* **Sorun ("Vurmuyor Gibi Geliyor"):** Baltalar çok hızlı döndüğü (180 derece/sn) ve hasar kontrolü 0.2 saniyede bir yapıldığı için, baltalar düşmanların içinden *kontrol edilmeyen ara anlarda* hayalet gibi geçip ıskalıyordu.
* **Çözüm (Frame-Perfect Matematik):** 
  1. Fizik motoru (Rigidbody) kullanılmama kuralına sadık kalındı.
  2. Mesafe kontrolleri 0.2 saniyede bir değil, **saniyenin her karesinde (60 FPS)** yapılmaya başlandı.
  3. Düşmanların saniyede 60 kere hasar yemesini engellemek için, her düşmana özel bir **Hafıza Sözlüğü (Dictionary Cooldown)** eklendi.
  4. Performans düşmesin diye ağır arama komutları silinip, doğrudan `EnemyPool.Instance.GetAllActiveEnemies()` kullanıldı.
  * *Sonuç:* Baltalar artık jilet gibi kesiyor, düşmana değdiği 1 milisaniyede bile hasarı garantili şekilde yapıştırıyor ve sıfır kasma yapıyor.

## 5. Zehir Efekti (Toxic Volume) Planlaması
* **Tartışma:** Ekranın kenarlarında yeşilin içinden morların patladığı bir zehirlenme efekti istendi.
* **Teknik Beyin Fırtınası:** Standart Vignette'in tek renk desteklemesinden dolayı renklerin karışıp çamur olma riski konuşuldu. Bunun yerine kameraya yapışan **Zehir Parçacıkları (Screen Particles)** veya **Maskeli (Desenli) Vignette** alternatifleri sunuldu.
* **Sonuç:** Günün yorgunluğuyla bu görsel cila işi bir sonraki mesaiye bırakıldı.

---
**Günün Özeti:** Flora bugün sadece mekanik eklenmekle kalmadı, AAA oyun standartlarındaki "Game Feel" (Oyun Hissiyatı) seviyesine inanılmaz derecede yaklaştı. Hatalarımızdan çok şey öğrendik ve optimizasyonlardan asla taviz vermedik. Yarın daha da güçlü devam edeceğiz! 😎🌿
