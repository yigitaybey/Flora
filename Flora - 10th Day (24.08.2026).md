- Mail kntrolü
### 1. Kırmızı Çizgili Kurallar ve Tarihler

- **Google Play 14 Gün Kuralı:** Android için kapalı test yapman zorunlu. Yayına almadan önce **en az 12 test kullanıcısının** katılıp onay vermesi ve testin 14 gün sürmesi gerekiyor. Bu 14 günlük sayacı başlatmak için 12 kişiyi bulmak en kritik eşik (Discord'daki `#post-engagement-boost` kanalını bunun için aktif kullanmalısın).
    
- **Son Teslim (Submission):** Flora'nın Devpost üzerindeki başvurusunu en geç **30 Eylül 2026, 23:45 PDT**'de eksiksiz olarak kesinleştirmen gerekiyor.
    

### 2. "Flora" İçin Devpost Görevleri

- **Katkı (Contribution) Beyanı:** Flora projesi şu an taslak durumunda. HTML/CSS ile hazırladığın arayüzleri, takımda üstlendiğin sorumlulukları veya arka planda kurduğun yapıları Devpost'taki "Describe your contribution" bölümüne mutlaka eklemelisin.
    

### 3. Kullanıma Hazır Ship Kit (Sponsor) Avantajları

- **Replit Pro:** İlk gönderilen kod hatalı olduğu için iptal edilmiş, e-postanda bekleyen yeni %40 indirim kodunu kullanman gerekiyor.
    
- **Tasarım ve Kodlama Araçları:** Geliştirme sürecini hızlandırmak için JetBrains Junie (2 ay ücretsiz), Mobbin (3 ay ücretsiz), AppScreens (%50 indirim) ve Bitrig (%60 indirim) gibi araçların fırsatları seni bekliyor. Bunları Devpost'taki Ship Kit bölümünden talep edebilirsin. _(Stripe kredisini zaten önceki planımızda çöpe attık, onu pas geçebilirsin)._
    

### 4. Ekstra Ödüller ve Görünürlük Fırsatları

- **HackerNoon Yazarlık Yarışması:** Projenin geliştirme sürecini (Build in Public kapsamında) anlatan bir makale yazarak 2.500 dolarlık ekstra ödül havuzuna aday olabilirsin.
    
- **Noise Haftalık Çekilişleri:** X (Twitter) üzerinden `@getnoise` hesabının verdiği haftalık görevleri yerine getirerek 200 dolar değerinde Noise kredisi kazanma şansın var.
    
- **Kampüs Etkinliği:** Kampüste ufak bir toplanma ayarlarsan, Austin Blake'in yiyecek ve hediye kiti (Meetup-in-a-Box) desteğini alabilirsin. Bu adım seni doğrudan öğrenci odaklı "Next Gen Award" ödülü için öne çıkarıyor.
    

### 5. Eğitim, Canlı Yayınlar ve Eğitim Kayıtları

- **AdMob ve OneSignal:** Maillerde uygulama içi reklamlar (AdMob) ve bildirimlerle kullanıcı tutma (OneSignal) konularında önemli eğitimlerin tekrarları olduğu belirtiliyor. AdMob entegrasyonu düşünen ilk 500 kişiye $1.000 bonus gibi fırsatlar da duyurulmuş (ağaç dikme modeline destekleyici bir gelir modeli olarak uyarsa bunu da inceleyebilirsin).
    
- **Etkinlik Tekrarları (Replays):** "Fikri doğrulama", "İlk 7 günü kazanma" ve "Etkileyici arayüz kurgulama" gibi daha önce yapılmış canlı yayınların kayıtları sitede duruyor, arka planda çalışırken açıp dinleyebilirsin.

# 🌲 Flora - Günlük Geliştirme Raporu

Bugün Flora'nın "MVP" (Minimum Viable Product) hedefine ulaşması için hem teknik hem de görsel anlamda devasa adımlar attık. Base (Radyo Kulesi) haritasını resmi olarak **tamamlayıp**, Savaş haritasının da (Orman) temel görsel kalitesini belirledik.

İşte bugün seninle birlikte başardıklarımızın tam listesi:

## 1. ⚙️ Arayüz (UI) ve Sistem Entegrasyonları

- **Ayarlar Menüsü Canlandı:** Ses, Müzik, Ekran Titremesi (ScreenShake) ve Hasar Yazıları (Damage Num) butonları doğrudan oyun motoruna bağlandı. Ayarların PlayerPrefs ile cihazda kayıtlı kalması sağlandı.
- **Damage Popup Fix:** Kapatılıp açıldığında hasar sayılarının ekranda çıkmaması sorunu çözüldü, obje havuzu (Object Pooling) ile entegre edildi.

## 2. 💰 RevenueCat (Peace Prize) Hazırlığı

- Hackathon'daki en büyük kozumuz olan **"Ağaç Dikme / Bağış Mağazası"** arayüzü kuruldu.
- API bağlanmadan önce UI testlerini yapabilmen için 3 farklı **Sahte Satın Alma (Fake IAP)** fonksiyonu yazıldı (300TL, 500TL, 800TL paketleri).
- Satın alımların sol üstteki "Tohum" ekonomisini anlık olarak güncellemesi sağlandı.

## 3. 🎥 Kamera ve Sanat Yönetimi (Görsel Evrim)

- **Sahte İzometrikten Gerçek İzometriğe Geçiş:** Kameradaki _Perspective + Düşük FOV_ hilesi terk edilip, profesyonel **Orthographic** kameraya geçildi. Bu sayede harita çok daha net ve optimize hale geldi.
- **3D Hacim Kazanımı:** Kameranın Y rotasyonu **-45 dereceye** çekilerek objelerin (konteynerler, kule) yan yüzeyleri görünür kılındı. Oyunun o "kağıt gibi (2D)" duran yapısı tamamen kırılıp, hacimli bir 3D görünüm elde edildi.
- **Kamera Kaydırma (Pan) Düzeltmesi:** Kamera 45 derece dönünce bozulan fare ile kaydırma matematiği (`BaseCameraPan.cs`) Vektör hesaplamalarıyla baştan yazıldı. Artık kamera hangi açıya dönerse dönsün kaydırma pürüzsüz çalışıyor.

## 4. 🌘 Gölge ve Işıklandırma Krizleri Çözüldü

- **URP Gölge Mesafesi:** Gölge çizim sınırları (Max Distance) optimize edilerek her iki haritanın da kusursuz gölge vermesi sağlandı.
- **Peter Pan & Gölge Aknesi Hataları:** Unity'nin o meşhur havada uçan veya haritayı kapkara yapan gölge problemleri; `Normal Bias` ve `Depth Bias` ayarları `1.0` yapılarak saniyesinde çözüldü.
- **Karanlık Harita (SSAO) Ayarı:** Bütün haritayı çamur gibi yapan Ambient Occlusion ayarlarının Şiddet (Intensity) ve Yarıçap (Radius) değerleri dengelenerek, gölgelerin sadece profesyonel temas noktalarında çıkması sağlandı.