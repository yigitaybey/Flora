**Proje Özeti ve Hedefler** Bugün Flora projesi üzerinde yaklaşık **2.5 saat** yoğun bir şekilde çalışıldı. Oyunun genel hatları ve kaba taslağı zihinde oturtulup resmi olarak geliştirme sürecine geçildi.

**Oyunun Temel Yapısı:**

- **Kamera/Görüş:** İzometrik kamera açısı
- **Görsel Stil:** 3D Low-Poly tarzı
- **Ana Hedef:** Projeyi önümüzdeki **2 ay içerisinde MVP (Minimum Viable Product - Çalışan İlk Sürüm)** olarak tamamlayıp çıkarmak.

Öncelikli olarak neler yapılacağı planlandı ve Unity motoru üzerinde "Graybox" (Gri kutu ile bölüm prototipleme) aşamasıyla çok verimli bir temel atıldı.

---

### Günün Özeti (Tamamlanan Görevler)

Bugün geliştirme ortamı (Unity) kurulup test edildi ve yapay zeka (AI) destekli olarak oyunun temel mekanikleri kodlandı. Tamamlanan adımlar şunlardır:

- **Zemin ve Karakter Kurulumu:** Test amaçlı bir Plane (zemin) oluşturuldu. Ana karakterimiz **Slvia** (şu anlık bir kutu formunda) sahneye yerleştirildi.
- **Hareket Mekaniği:** Karakterin hareket kodları yazıldı ve doğru hareket şemasına (izometrik düzleme uygun şekilde) oturtuldu.
- **Kamera Sistemi:** Kameranın karakteri pürüzsüz bir şekilde takip etmesi sağlandı (Camera Tracking).
- **Düşman Sistemleri:** Düşman doğma (Spawn) algoritması yazıldı, algılama menzili (Range) ve temel davranış olayları halledildi.
- **Kullanıcı Arayüzü (UI):** Ekranda serbestçe dolaşan (Floating) parmak yönlendirme butonu (Sanal Joystick) eklendi ve karakter hareketine bağlandı.

---

### Neden Bu Stratejileri Seçtik? (Geliştirme Gerekçeleri)

#### 1. Graybox (Gri Kutu) Tasarım Mimarisi

- **Gerekçe:** Oyuna doğrudan yüksek kaliteli 3D modeller, ağaçlar veya kaplamalar eklemek yerine her şeyi kutular ve düzlemlerle tasarlamak.
- **Sonuç:** Karakter hareketi, kamera açısı ve düşman algoritması gibi oyunun "çekirdek mekaniklerini" (Core Loop) grafikleri beklemeden test edebilmeyi sağladı. Eğer bir hareket algoritması gri bir kutuyla düzgün çalışıyorsa, yarın o kutunun yerine Slvia modelini koyduğumuzda da kusursuz çalışacaktır.

---

### Alınan Aksiyonlar ve Kararlar (Günlük Çıkarımlar)

1. **İzometrik Kamera Hilesi:** Oyunun Low-Poly tarzını en iyi yansıtacak ve mekanı daha büyük hissettirecek izometrik açının temel formülü (45 derece dönüş vs.) Graybox testleriyle doğrulandı ve kilitlendi.
2. **Mobil Kontrol Altyapısı (Floating Joystick):** Eklenen serbest yönlendirme butonu, oyunun sadece klavyeyle değil, dokunmatik ekranlarda (Mobil) da rahatlıkla oynanabileceği modüler bir altyapıya sahip olduğunu gösterdi.