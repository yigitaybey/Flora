- Bu gün yapılacaklar:
	- [x] Map dekorasyonları ✅ 2026-08-26
	- [x] Savaş Alanına Pause Buttonu ✅ 2026-08-26
	- [x] kamera açısı belirleme ✅ 2026-08-26
	- [x] Yön tuşları entegre et ✅ 2026-08-26
	- [x] Base Map ayarlar oyun başlayınca son durumu kaydetme ✅ 2026-08-26
	- [ ] savaş mapi bugfix
	- [ ] Sylva animasyonu değiştirilecek
	- [ ] Işıklandırma bugfix
	- [ ] Ara map yapılması
	- [x] Ölüm/Kazanma Ekranı ✅ 2026-08-26
	- [ ] Düşman ve silah assetleri
	- [ ] FlameThrower Assetini ekle
	- [ ] FlameThrower Pariküllerini Unity üzerinden yap
	- [ ] Zaman yeterse terraforming

Map dekorasyonlarını yaparken sürekli game içine baktığım için canvas gözükmesin diye canvası prefab olarak ekledim ama bu çok büyük hataydı dekorasyonlar bittikten sonra canvası ekleyince scriptlerin hiç biri çalışmıyo du ama baktığımda her şey okeydi hiç bir porblem gözükmüyodu kapadım açtım bişeyleri değiştridim hiç bir şekile düzelmedi iyi ki github kullanıyorum çünkü hemen önceki checkponit e geri döndüm bütün dekorasyonlar gitti ama en azından scriptler geri geldi nomarl olarak çalışmaya başladı sonrasında tam gerçekten olucak kamera açılarını ayarladım ardından savaş mapi için ölüm ve kazanma ekranlarını yaptım base e dönabiliyoruz artık kazanınca veya kaybedince ayrıca iki ekrana da denemelik reklam izleme buttonları koydum artık reklam izleyip 2x tohum kazanabiliyoruz run larımızdan  ardından oyunun UX açısından daha iyi olması için yön tuşları ile de oynanabilir yaptım şu an 3 şekilde oynanabiliyor mouse,WASD ve yön tuşları ardından savaş alanına bir pause buttonu koydum bu button aynı zamanda ayarlar buttonu ile aynı işlevi görüyor bu çok iyi oldu aynı zamanda artık oyun içi yaptığımız ayarlar kaydediliyo ve hem base hemde savaş ekranının ayarları senkron çalışıyo ardından mapi dekore ettik daha önce patlayan save i yerine getirdik daha yapılcak çok iş var bu gün yapılmayanları not olarak bırakıyorum ayrıca öğrendiğim bir şey de var daha araştırmadım ama play store da uygulama yayınlarayn bir arkadaşım var aldığım duyumlara göre geliştirci hesabı açarken çok fazla form doldurcakmışız ve bunu yaparken kesinlikle AI kullan diyo çünkü kendisi kendi uygulamasını kendi doldurmaya çalışmış 4-5 defa red yemiş AI ile yapmış ve olmuş bunu not ettim ayrıca bir kullanıcı sözleşmesi gibi birşey hazırlamam gerekiyomuş dedi ki google formlardan form yapyıp geliştirici consolunda ki sorulan link yerine yapıştırmam lazımmış ayrıca en sorun çıkarabilecek olan şey bizim oyunumuzda sadece ads geliri olucaksa problem yok o zaman kişisel gelişirici hesabı ile yayınlayabiliyormuşuz ama eğer oyunun içinde mikro ödemeler olucaksa eğer play sotore bunun için şirket hesabı istiyormuş bu çok kötü eğer böyle olursa babamın şirktei üzerinden yayınlamamız lazım yoksa da başka birşey düşüncez ayrıca android studiyo yüklemem lazım bilgisayarıma 

# Flora - 11. Gün Raporu (Neler Başardık?)

Bugün (veya dün gece) teknik açıdan oyunun temel direklerini çok sağlamlaştırdık. Her ne kadar planın son kısımlarına (çarpışmalar, silahlar) yetişemesek de, yaptığımız işler oyunun profesyonel hissettirmesi için kritikti:

## 1. Kamera ve Kontroller (UX İyileştirmeleri)
*   **İzometrik Kamera:** Oyunun kamera açısı tam olarak hedeflenen izometrik/orthographic (2D-3D hibrit) hissiyatına kavuşturuldu.
*   **Genişletilmiş Kontroller:** Sadece WASD ve Mouse Tıklaması ile kısıtlı kalmadık; oyuncuların rahatlığı için **Yön Tuşları (Ok Tuşları)** entegrasyonu başarıyla yapıldı. Artık 3 farklı şekilde de pürüzsüz oynanabiliyor.

## 2. Arayüz (UI) ve Menü Senkronizasyonu
*   **Akıllı Ayarlar Menüsü:** Savaş sahnesine, ekranı kalabalıklaştırmayacak tek bir "Durdur/Ayarlar" butonu eklendi. Oyuncu bastığında oyun duruyor, kapattığında devam ediyor.
*   **Kalıcı Ayarlar:** Base haritadaki ayarlarla Savaş haritasındaki ayarlar (Ses, Müzik, Hasar Yazısı, Ekran Titremesi) birbirine bağlandı. Nereden değiştirirsen değiştir, oyun bunu hatırlıyor ve UI açıldığında doğru haliyle (kaydedilmiş şekliyle) geliyor.

## 3. Oyun Döngüsü ve Bug Fixler
*   **Ölüm/Kazanma Döngüsü:** Savaş haritasında ölünce veya kazanınca Base'e dönebilme (Scene Transition) mantığı oturtuldu.
*   **Reklam ve 2x Tohum:** Kazanma ve kaybetme ekranlarına "Reklam İzle 2x Tohum Kazan" sistemi eklendi (Catvertising kuralına uygun, zorunlu olmayan Rewarded Ad mantığı).
*   **Damage Popup Bug Fix:** Savaş sahnesini GameManager olmadan direkt test ettiğinde hasar yazılarının kapatılamaması sorunu, `Fallback` statik değişkenler yazılarak çözüldü.

## 4. Sanat ve Dekorasyon (Yusuf'un Şaheseri)
*   Haritanın görsel dizilimi, göllerin ve objelerin yerleşimi başarıyla tamamlandı. Harita artık sadece dümdüz bir zemin değil, yaşayan bir çevre oldu.

---
> [!NOTE]
> Geriye kalan görünmez duvarlar, silah entegrasyonları (FlameThrower/Pollen) ve ışıklandırma işlerini bir sonraki oturuşumuzda tam bıraktığımız yerden "Task Listesi" üzerinden adım adım devam ettireceğiz. Elinize, emeğinize sağlık!
