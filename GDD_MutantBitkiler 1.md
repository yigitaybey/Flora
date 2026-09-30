# 🌿 OYUN TASARIM BELGESİ (GDD)
## "FLORA: Dünya Geri İstiyor" — Ekip Referans Dökümanı
**Hazırlayan:** Proje Lideri & Yazılım Ekibi  
**Versiyon:** 1.0 — MaxVP + MVP Birleşik Rapor  
**Tarih:** Temmuz 2026

---

> **Bu belge hakkında:**  
> Bu döküman, oyunun tüm tasarım kararlarını, hikayesini, mekaniklerini ve sanat yönelimini bir araya getiren "tek kaynak of truth" belgesidir. Yusuf (Hikaye & Lore) ve Grafik Tasarımcı için her bölüm ayrıca işaretlenmiştir. Teknik kodlama detayları bu belgede yer almaz; bu bir sanat ve tasarım belgesidir.

---

## İÇİNDEKİLER

1. [Oyun Kimliği & Vizyon](#1-oyun-kimliği--vizyon)
2. [Evren, Dünya İnşası & Lore](#2-evren-dünya-i̇nşası--lore)
3. [Karakterler](#3-karakterler)
4. [Sanat Yönetimi (Art Direction)](#4-sanat-yönetimi-art-direction)
5. [Çekirdek Oynanış Döngüsü](#5-çekirdek-oynanış-döngüsü)
6. [Mekanik 1 — Tutorial: Üssü Ele Geçirme](#6-mekanik-1--tutorial-üssü-ele-geçirme)
7. [Mekanik 2 — Savaş Sistemi (Bullet Haven)](#7-mekanik-2--savaş-sistemi-bullet-haven)
8. [Mekanik 3 — Base Building (Üs İnşası)](#8-mekanik-3--base-building-üs-i̇nşası)
9. [Mekanik 4 — Idle Tycoon & Sığınmacı Yönetimi](#9-mekanik-4--idle-tycoon--sığınmacı-yönetimi)
10. [Mekanik 5 — Bölge Arındırma (Reclamation Sistemi)](#10-mekanik-5--bölge-arındırma-reclamation-sistemi)
11. [Silahlar & Yetenek Havuzu (RNG Sistemi)](#11-silahlar--yetenek-havuzu-rng-sistemi)
12. [Düşman Kataloğu](#12-düşman-kataloğu)
13. [Gelir Modeli & Sosyal Sorumluluk](#13-gelir-modeli--sosyal-sorumluluk)
14. [MVP — Shipaton 2026 Sürümü (10 Dakika)](#14-mvp--shipaton-2026-sürümü-10-dakika)
15. [Proje Yol Haritası (Lifecycle)](#15-proje-yol-haritası-lifecycle)

---

## 1. Oyun Kimliği & Vizyon

| Alan | Detay |
|---|---|
| **Geçici İsim** | FLORA: Dünya Geri İstiyor |
| **Tür** | Roguelite Bullet Haven + Base Building + Idle Tycoon |
| **Platform** | Mobil (iOS / Android) — İleride PC (Steam) |
| **Motor** | Unity (C#) |
| **Hedef Kitle** | 16–35 yaş, Vampire Survivors / Fallout hayranları |
| **Referans Oyunlar** | Vampire Survivors, Fallout Shelter, Last Shelter: Survival |
| **Özet** | Distopik bir dünyada botanik bilimci olarak hem savaşıyor hem üs kuruyorsunuz hem de dünyanın kendisini iyileştiriyorsunuz. |

### 🎯 Oyunun Özü — Tek Cümle
> *"Seni öldürmeye çalışan doğaya, yine doğanın kendi silahlarıyla hayat geri veriyorsun."*

### 💡 Neden Bu Oyun Çalışacak?
Oyunun üç ana mekaniği (Savaş, İnşa, Bölge Arındırma) birbirini sürekli besler ve oyuncuyu döngü içinde tutar:

```
Savaş et → Loot topla → Üssünü geliştir → Yeni bölge aç → Daha zor savaş et
```

Bu döngü **asla bitmez.** Oyuncu her döngüde hem daha güçlü hisseder hem de dünyayı görsel olarak yeşillendirdiğini görür.

---

## 2. Evren, Dünya İnşası & Lore

> 📝 **Yusuf için not:** Bu bölüm senin ana oyun alanın. Her başlık altına kendi yorumunu ve genişletmeni ekleyebilirsin.

### 2.1 Kıyamet — Nasıl Başladı?

Dünya bir günde değil, on yıllar içinde öldü. İnsanlar sera gazlarını salmaya, ormanları yakmaya ve okyanusları zehirlemeye devam etti. Uyarılar görmezden gelindi. Anlaşmalar kağıt üzerinde kaldı.

Sonunda doğa cevap verdi.

**Yıl 2041:** Karbon konsantrasyonu kritik eşiği geçti. Atmosferin alt katmanları artık kalın ve sarımsı-yeşil bir gaz örtüsüyle kaplıydı. Güneş ışığı filtrelendi, tarım çöktü, milyarlarca insan açlık ve zehirlenmeyle hayatını kaybetti.

**Yıl 2043 — "Uyanış":** Bilim insanları buna "Chlorophyll Awakening" (Klorofil Uyanışı) adını verdi. Başlangıçta küçük anomaliler dikkat çekti: Bitkiler normalde olduklarından çok daha hızlı büyüyordu. Sonra hareket etmeye başladılar. Sonra insanlara saldırmaya.

Evrimsel baskı altında bitkiler yüzyıllık süreci on yılda tamamladı. Motor nöronları geliştirdiler. Ağrıya tepki verdiler. Av peşinden koştular. Ve avladılar.

**Yıl 2047 — Günümüz (Oyunun zamanı):** Dünya nüfusunun %90'ı yok oldu. Kalanlar dağınık, iletişimsiz ve korkulu gruplar halinde hayatta. Şehirler bitki kütlelerinin altında ezilip kayboldu. Askerler işe yaramadı; kurşun klorofili delmez. Hayvanlar da bu süreçten nasibini aldı; çoğu tür karbon zehirlenmesiyle mutasyona uğradı, diğerleri ise bitkilerden de vahşi davranmaya başladı.

### 2.2 Oyunun Dünyası — Coğrafya

Oyuncunun keşfedeceği dünya 5 ana bölgeye ayrılmıştır (MaxVP vizyonu). Her bölgenin kendine özgü iklimi, mutant türleri ve görsel dili vardır:

| Bölge No | İsim | Atmosfer | Tehlike Seviyesi |
|---|---|---|---|
| 1 | **Zehirli Vadi** | İlk bölge, sarımsı-yeşil sis, bataklık bitkileri | Başlangıç |
| 2 | **Çürümüş Şehir Kalıntıları** | Binalar sarmaşıkla kaplanmış, karanlık sokaklar | Orta |
| 3 | **Kızıl Çöl** | Güneşin yakıp kuruttuğu mutant kaktüs orduları | Zor |
| 4 | **Buz Tutan Kuzey** | Donmuş ama yine de canlı bitkiler, sessiz ve ölümcül | Çok Zor |
| 5 | **Orijin Noktası** | İlk mutasyonun başladığı efsanevi botanik enstitüsü | Nihai Boss |

### 2.3 Lore — Neden "Tohum" ve "Polen"?

Botaniğin altın çağında bilim insanları binlerce bitki türünün tohumunu "Tohum Bankası"nda koruma altına almıştı. Bu tohumlar, mutasyonun başladığı zamandan öncesine ait; kirletilmemiş, saf DNA bilgisi taşıyorlar.

Ana karakterimiz bu bankalara ulaşabilen son insanlardan biri. Keşfettiği şey şu: Saf tohum özlerini bir mutant bitkiye enjekte ettiğinde, o bitki birkaç saniye içinde normale dönüyor. Saldırmayı bırakıyor. Büyüyor. Yeşilleniyor. Ve çevresindeki alanı da temizlemeye başlıyor.

Bu, insanlığın elindeki tek umut.

### 2.4 Önemli Lore Notları (Yusuf için genişletme noktaları)

- **Mutant bitkiler acı çekiyor mu?** — Biyolojik olarak "evet" denilebilir. Bu oyunun ahlaki derinliğini oluşturabilir. Düşman yok etmek, bir anlamda bir varlığı acısından kurtarmak olabilir.
- **İnsanlar neden birleşmiyor?** — Güven sorunu, iletişim altyapısının çökmesi ve kaynaklar için şiddetli rekabet.
- **Başka hayatta kalanlar var mı?** — Evet. Ve bazıları sığınmacı olarak üsse katılacak.
- **Mutant hayvanlar düşman mı, müttefik mi?** — Bazıları olası müttefik. Bu, ilerleyen bölümler için bir storyline kapısı.

---

## 3. Karakterler

### 3.1 Ana Karakter — "Botanik"

> 📝 **Yusuf & Tasarımcı için not:** Karakterin net bir ismi henüz yok. Çalışma adı "Botanik." Yusuf'un isim ve kişilik önerisi bekleniyor.

**Kim?** Bu kıyamete başından beri şahit olmuş, o karanlık yıllarda büyümüş bir bilim insanı. Çocukluğunda hâlâ yeşil olan bir dünyayı hatırlıyor — ama o dünya bir rüya gibi uzakta.

**Motivasyonu:** Salt hayatta kalmak değil. Dünyanın iyileştirilebileceğine dair bir inancı var. Saf tohumların işe yaradığını gördüğünde bu inanç güce dönüştü.

**Karakter Özellikleri (tasarım için):**
- Orta yaş, yıpranmış ama güçlü
- Üzerinde laboratuvar önlüğü kalıntıları ve doğaçlama zırh
- Sırtında bir "Tohum Çantası" (hem hikaye hem UI elemanı)
- Elinde ya bir Polen Fırlatıcı ya da Tohum Enjektörü

**Karakter Sesi & Ton:** Sakin, gözlemci. Panik yapmaz. Bilim insanı mantığıyla düşünür. Ancak yalnız savaşmanın yorgunluğu yüzünde okunur.

---

### 3.2 Sığınmacılar (NPC'ler)

> 📝 **Yusuf için not:** Her sığınmacının bir arka plan hikayesi olacak. Bunlar oyun içi "profil kartlarında" gösterilecek. Yusuf'un bu profilleri yazması bekleniyor.

Dışarıdan gelen sığınmacılar rastgele oluşturulur ama her birinin kendine özgü bir geçmişi vardır. Oyuna **Fallout SPECIAL** sisteminden ilham alınarak 7 temel stat atanır:

| Stat | Türkçe | Oyun İçi Anlamı |
|---|---|---|
| **S**trength | Güç | Ağır loot taşıyabilme kapasitesi |
| **P**erception | Algı | Tehlikeyi erkenden sezme, loot kalitesi |
| **E**ndurance | Dayanıklılık | Görev sırasında hayatta kalma şansı |
| **C**harisma | Karizm | Diğer sığınmacıları motive etme bonusu |
| **I**ntelligence | Zeka | Üste çalışırken araştırma/üretim hızı |
| **A**gility | Çeviklik | Kaçma başarısı, görev süresi azalması |
| **L**uck | Şans | Kritik loot bulma ihtimali |

**Örnek Sığınmacı Profili (Yusuf için şablon):**

> **"Eski Çiftçi Mete"**  
> *"Tohumları benden daha iyi kimse tanımaz. Benim tarlam bitkiler ayaklanmadan önce ülkenin tahıl ambarıydı. Şimdi mi? Şimdi onlar benim ambarımı yiyor."*  
> S:6 | P:5 | E:7 | C:4 | I:4 | A:3 | L:5

---

### 3.3 Boss Karakterleri

> 📝 **Tasarımcı için not:** Her boss'un hem görsel bir kimliği hem de kısa bir lore açıklaması olacak. Bunlar savaştan önce bir "loading screen" kartında gösterilebilir.

| Boss İsmi (Geçici) | Görsel Konsept | Bölge |
|---|---|---|
| **Kök-Ata** | Dev, yüzlerce yıllık bir meşe ağacının mutant hali. Kökleri canlı ve saldırgan. | Zehirli Vadi |
| **Zehir Dul** | Devasa bir sinek kapan bitkisi. Ağzı bir kapı büyüklüğünde, asit salgılıyor. | Çürümüş Şehir |
| **Kaktüs Kraliçesi** | Çölün hâkimi. Dikenlerini mermi gibi ateşliyor, sırtında bir "kaktüs ordusu" taşıyor. | Kızıl Çöl |

---

## 4. Sanat Yönetimi (Art Direction)

> 📝 **Tasarımcı için not:** Bu bölüm sana yönelik. Her başlık altında referans ve yönelim bilgisi var.

### 4.1 Genel Görsel Ton

**Hedef His:** Karanlık ama canlı. Tehlikeli ama güzel. Distopik ama umut vaat eden.

Oyun iki zıt görsel kutup arasında gidip gelen bir renk dili kullanır:

| Durum | Renk Paleti | His |
|---|---|---|
| **Mutant Bölge** | Zehirli yeşil (#4a7c59), sarı-yeşil sis (#b5c467), koyu mor gölgeler | Boğucu, klostrofobik, tehlikeli |
| **Arındırılmış Bölge** | Sıcak yeşil (#56ab2f), altın sarısı güneş ışığı, mavi gökyüzü | Nefes açan, huzurlu, kazanılmış |
| **Base (Üs)** | Turuncu ampul ışığı, ahşap tonları, demir gri | Güvenli, ev hissi, yorgun ama sıcak |
| **UI & Arayüz** | Koyu arka plan (#1a1a2e), neon yeşil vurgu (#39ff14) | Bilim-fi, laboratuvar ekranı estetiği |

### 4.2 Karakter & Düşman Tasarım Yönergeleri

**Ana Karakter:**
- Silueti her açıdan anında tanınabilir olmalı (Vampire Survivors karakterlerine bak: tek bakışta kim olduğu belli)
- Hareketsizken bile "hazır" duran bir duruş
- Sırtındaki Tohum Çantası animasyonlu olmalı (sallanmalı, içi doluşta şişik, boş olunca sarkık)

**Mutant Bitkiler (Düşmanlar):**
- Gerçek bitki anatomisinden ilham al ama distort et
- Her düşman türünün ayırt edici bir silueti olmalı (gece görüşünde bile karışmamalı)
- Hareket animasyonları doğal bitki hareketinden ilham almalı: sürüklenen kökler, eğilen saplar, salınan yapraklar
- Mutant oldukları belli olmalı ama hâlâ "bitki" oldukları anlaşılmalı — tam yaratık değil

**Referans Oyunlar (Görsel):**
- Vampire Survivors (karakter ölçeği ve savaş kamerası)
- Hades (atmosferik parçacık efektleri)
- Don't Starve (karakter stil distorsiyonu)

### 4.3 Harita & Ortam Tasarımı

Her bölge kendi renk ve doku setini taşımalı:

**Zehirli Vadi (Bölge 1):**
- Zemin: Çatlak toprak, sararmış çimen
- Arka plan katmanları (parallax): Uzakta devasa mutant ağaçlar
- Hava efekti: Sürekli yavaşça dönen zehirli gaz partikülleri
- Işık: Yok denecek kadar az. Oyuncunun etrafında küçük bir "temiz hava" ışık balonu

**Arındırılmış Bölge (Post-Reclaim görsel):**
- Zemin: Sürpriz ve tatmin edici şekilde yeşil
- Partiküller: Yeşil yaprak partikülleri yavaşça yukarı yükseliyor
- Işık: Sıcak, altın sarısı güneş ışığı kırılımı
- Ses (tasarımcıya not): Bu görsel değişim için auditory bir "ahhh" anı yaratılmalı

### 4.4 UI & Arayüz Tasarımı

**Savaş Ekranı (Bullet Haven modu) UI Elemanları:**
- Sol üst: Can barı (yeşilden kırmızıya, bitki kökü tasarımlı)
- Sağ üst: Deneyim/level barı (şişe içindeki sıvı gibi dolup taşıyor)
- Ekranın altı: Aktif tohum/silah ikonları
- Dalga sayacı: Sağ alt, "Dalga 3/20" formatında
- Mini harita: Yok — Oyuncu etrafını göremez, bu kasıtlı

**Base Ekranı UI:**
- İzometrik veya top-down görünüm
- Binalar tıklanabilir, üzerine gelince tooltip gösteriyor
- Sığınmacı listesi: Sol kenar çubuğu, profil fotoğrafı + stat ikonları

---

## 5. Çekirdek Oynanış Döngüsü

Oyunun ruhu şu döngüde saklı:

```
┌─────────────────────────────────────────────┐
│                                             │
│   BASE'DE:                                  │
│   Lootu harca → Bina yap → Sığınmacı yönet │
│            ↑                     ↓          │
│            │                     │          │
│   Savaştan dönersin       Yeni bölge açılır │
│            │                     │          │
│            ↑                     ↓          │
│   OUTPOST'TA:                               │
│   20 wave dayanıyorsun → Boss yeniyorsun    │
│   → Bölge yeşilleniyor → Loot topluyorsun  │
│                                             │
└─────────────────────────────────────────────┘
```

Bu döngü şu duygusal yayı yaratır:
1. **Gerginlik** (savaş sırasında)
2. **Tatmin** (yeşillenme anında)
3. **Strateji** (base'de)
4. **Merak** (yeni bölge açıldığında)
5. **Tekrar → Gerginlik** (döngü devam eder)

---

## 6. Mekanik 1 — Tutorial: Üssü Ele Geçirme

### 6.1 Sahne Açıklaması

Oyun başladığında hiçbir açıklama ekranı yok. Hiçbir "Bu oyun şöyle oynanır" metni yok. Oyuncu direkt olarak aksiyonun içine düşer.

**Ortam:** Terk edilmiş, büyük bir cam-sera yapısı. Camların çoğu kırık, içeri yeşil sis sızıyor. Zemin çatlak beton. Köşelerde devrilmiş deney tezgahları. Duvarlar sarmaşıklarla kaplı.

**İlk An:** Oyuncu karanlıktan aydınlığa doğru koşarak giriyor. Sahne animasyonla açılıyor. Arka planda uzaktan mutant çığlıkları.

### 6.2 Tutorial'ın Oynanışı

| Adım | Ne Olur? | Oyuncu Ne Öğrenir? |
|---|---|---|
| 1 | Karakter ekrana girer, otomatik olarak en yakın düşmana nişan alır | "Ateş etmek için bir şey yapmama gerek yok" |
| 2 | 5 zayıf mutant karanlıktan süzülür | Hareket tuşlarını keşfeder |
| 3 | Öldürülen düşmandan küçük bir tohum loot düşer | "Düşman öldürmek bir şeyler veriyor" |
| 4 | Seviye atlar, ekran durur, 3 yetenek kartı belirir | RNG sistem tanıtımı |
| 5 | 15-20 düşman temizlendikten sonra sera kilitlenir | "Buraya temizledim" |
| 6 | Işıklar açılır, sarmaşıklar solar ve geriye çekilir | Görsel ödül, "yeşillendirme" önizlemesi |
| 7 | Ortadaki Telsiz Kulesi aktifleşir, harita açılır | Oyunun macro-amacı tanıtılır |

### 6.3 Bu Sahnenin Duygusal Amacı

> 📝 **Yusuf için not:** Tutorial, oyuncuya şunu hissettirmelidir: *"Ben bu dünyanın sahibiyim. Savaşırsam düzelir."* Başarılmışlık hissi çok erken gelmeli ki oyuncu "devam et" tuşuna bassin.

Tutorial boyunca isteğe bağlı bir iç ses/günlük sesi çalabilir (Yusuf'un yazacağı). Örnek:

> *"Eski sera. Babam beni buraya getirirdi, küçükken. Camlar kırılmadan önce. Şimdi burası benim. Temizlersem... belki burada bir başlangıç olur."*

---

## 7. Mekanik 2 — Savaş Sistemi (Bullet Haven)

### 7.1 Genel Mantık

Vampire Survivors'tan ilham alan bu sistemde oyuncu **hareket dışında hiçbir tuşa basmak zorunda değildir.** Silahlar ve yetenekler otomatik olarak ateşlenir. Oyuncunun tek görevi: Düşmanların arasından sağ çıkmak.

Bu basitlik yanıltıcıdır. Gerçek derinlik, hangi yetenekleri seçtiğine ve ne zaman hangi yöne kaçtığına bağlıdır.

### 7.2 Hareket & Kontrol

- **Mobilde:** Sanal joystick (sol alt)
- **PC'de:** WASD veya ok tuşları
- Karakter her zaman 360° hareket edebilir
- Hız, karakterin temel statına bağlı (ve upgrade edilebilir)
- **Dash yok** — kaçış salt hareketle sağlanır, bu gerilimi artırır

### 7.3 Dalga Sistemi (Wave System)

Her Outpost operasyonu **20 Dalgadan** oluşur:

| Dalga Aralığı | Düşman Yoğunluğu | Özel Etkinlik |
|---|---|---|
| 1–5 | Az ve yavaş | Yok |
| 6–10 | Orta, çeşitli türler | 7. dalgada: ilk "Zırhlı" düşman |
| 11–15 | Yoğun, hızlı | 12. dalgada: "Uğursuz Sis" efekti (görüş azalır) |
| 16–19 | Kaotik, çok türlü | Random event: "Böcek Sürüsü" gibi ani olaylar |
| 20 | **BOSS DALGASI** | Devasa boss sahnesi |

**Dalga arası (10 saniye mola):**
- Ekranda "Hazırlan..." sayacı
- Oyuncu bu sürede alanı dolaşıp düşen XP/loot toplar
- Kısa nefes alma anı (gerilimi düşürür, sonraki dalgayı daha gergin yapar)

### 7.4 RNG Yetenek Seçimi (Leveling Up)

Oyuncu düşman öldürdükçe **Tohum Enerjisi** (XP) toplar. Yeterince toplandığında:

1. Ekran kısa bir süre **yavaşlar** (Bullet Time efekti — oyuncu güvende hisseder)
2. 3 adet yetenek kartı belirir
3. Oyuncu birini seçer, kart seçilir ve efekt aktifleşir
4. Oyun normale döner

**Kart Tasarımı (Grafik Tasarımcı için):**
- Her kart: Büyük ikon + yetenek adı + tek satır açıklama
- 3 nadir seviye: Yaygın (gri çerçeve), Nadir (mavi çerçeve), Efsanevi (altın çerçeve)
- Efsanevi bir kart çıktığında hafif ekran titremesi + altın parıltı efekti

### 7.5 Ölüm & Devam Mekaniği

Oyuncu can puanı sıfıra düştüğünde:

**"OPERASYON BAŞARISIZ"** ekranı belirir. İki seçenek:

```
┌────────────────────────────────────────────────────────┐
│                 ❌ OPERASYON BAŞARISIZ                  │
│                                                        │
│  Topladıkların: 47 Tohum Özü, 12 Gıda, 3 Metal Parçası│
│                                                        │
│  [📺 Reklam İzle → Tüm Lootuı 2 Katına Çıkar]         │
│  [🏠 Üsse Dön → Normal Lootla Dön]                     │
│                                                        │
└────────────────────────────────────────────────────────┘
```

Bu seçim **asla zorla çıkmaz.** Oyuncu isterse direk üsse döner. Reklam kendi isteğiyle izlenir — bu, sinirlenmeyi değil, teşvik hissetmeyi sağlar.

---

## 8. Mekanik 3 — Base Building (Üs İnşası)

### 8.1 Üssün Görsel Evrimi

Oyun başında üs; birkaç kırık masa, bir Telsiz Kulesi ve boş bir alandan ibaret. Oyun ilerledikçe, oyuncunun her inşaatıyla bu alan yaşayan bir yerleşim yerine dönüşür.

> 📝 **Tasarımcı için not:** Üssün "evrim katmanlarını" gösterebilirsin: Level 1 üs vs. Level 5 üs görsel karşılaştırması. Oyuncu bu farkı görmek için oyunu oynayacak.

### 8.2 İnşa Edilebilir Binalar

#### 🔧 Atölye (Workshop)
- **Ne yapar?** Savaşa başlarken sahip olunan başlangıç değerlerini kalıcı olarak artırır (Perma-upgrade)
- **Upgrade edilince:** Daha fazla yükseltme slotu açılır
- **Görsel:** Dağınık ama çalışan bir tamir atölyesi, duvara asılı eski botanik diyagramları

**Atölye Perma-Upgrade Örnekleri:**

| Upgrade | Etki |
|---|---|
| Tohum Çantası Kapasitesi | Aynı anda daha fazla aktif silah |
| Can Zırhı | Başlangıç maksimum canı artırır |
| Polen Verimliliği | Saldırılar daha sık gerçekleşir |
| Adaptasyon | Savaşta XP kazanımı hızlanır |

#### 🌾 Tarla (Farm)
- **Ne yapar?** Sığınmacıları besler, üssün nüfus kapasitesini artırır
- **Upgrade edilince:** Daha fazla sığınmacı barındırılabilir
- **Görsel:** Küçük seralar içinde büyüyen saf (mutasyonsuz) bitkiler — üssün en renkli ve umudu hissettiren binası

#### 🏕️ Sığınmacı Çadırları
- **Ne yapar?** Dışarıdan gelen her sığınmacı için barınak sağlar
- **Görsel:** Çeşitli boyutlarda barikat-çadır kombinasyonları, içlerinde ışık yanan pencereler

#### 📡 Telsiz Kulesi (Radar)
- **Ne yapar?** Haritadaki "Savaş Sisi"ni araladırarak yeni Outpost'ları keşfeder
- **Upgrade edilince:** Daha uzak ve daha büyük bölgeler görünür hale gelir
- **Görsel:** Dönen radar diski + antenler + sürekli yanıp sönen yeşil ışık

#### ⏳ Zaman Kapsülü (End-Game Binası)
- **Ne yapar?** İnşa edilince **Endless (Sonsuz) Mod** açılır
- **İnşa için gerekenler:** Oyundaki en nadir materyaller, tüm diğer binaların belirli seviyeleri
- **Görsel:** Eski bir koruma kabının içinde, saf bir tohumun korunduğu cam bir kapsül. Işıklı. Kutsal görünümlü.
- **Lore önemi:** Bu kapsül, dünyanın "fabrika ayarlarına" dönmesini sağlayacak son umut.

> 📝 **Yusuf için not:** Zaman Kapsülü'nün inşa edildiği an, oyunun duygusal doruk noktalarından biri. Kısa bir cutscene veya iç ses metni bu ana yazılmalı.

---

## 9. Mekanik 4 — Idle Tycoon & Sığınmacı Yönetimi

### 9.1 Nasıl Çalışır?

Sığınmacılar üsse kabul edildikten sonra üç tip göreve gönderilebilir:

| Görev Türü | Açıklama | Risk |
|---|---|---|
| **Loot Toplama** | Temizlenmiş bölgelere gönderilir, kaynak getirir | Düşük (temiz alan) |
| **Keşif** | Henüz açılmamış alanlara gözetleme | Orta |
| **Tehlikeli Alan** | Mutant bölgelerine kaynak toplamaya | Yüksek — ölüm riski var |
| **Outpost İşletmeciliği** | Yeşillendirilmiş Outpost'lardaki tarlalara atanır ve sürekli üretim yapar | Yok (Güvenli) |

### 9.2 Ölüm Mekaniği

Sığınmacı ölebilir. Bu ağır bir karardır ve oyuncu bunu hisseder.

**Ölüm bildirimi:**
> *"📨 Haber aldın: Kemal, Kızıl Çöl'deki görevden geri dönemedi."*

Bu kayıp, oyuncuyu daha dikkatli görev seçmeye zorlar. Statları zayıf birini tehlikeli göreve göndermek o insanı kaybetmek demektir.

> 📝 **Yusuf için not:** Sığınmacı ölüm haberlerinin her biri farklı ve kişiselleştirilmiş olmalı. "Kemal öldü" değil, "Kemal'den haber gelmedi. Arama partisi sadece çantasını buldu."

### 9.3 Idle (Pasif) Gelir

Oyuncu oyun dışındayken bile sığınmacılar çalışmaya devam eder. Oyuna döndüğünde:

> *"Hoş geldin! 3 saatliğine ayrıydın. Sığınmacıların 284 Tohum Özü, 47 Gıda ve 12 Metal topladı."*

Bu, oyuncuyu her gün geri getiren temel Idle Tycoon kancasıdır.

---

## 10. Mekanik 5 — Bölge Arındırma (Reclamation Sistemi)

### 10.1 Kavram

Bu mekanik, oyunun **makro amacını** (dünyayı kurtarmak) somut ve görünür kılan sistemdir. Oyuncu sadece hayatta kalmak için savaşmıyor — bir şeyi değiştiriyor.

**Dünya Haritası:** Oyun haritası yukarıdan bakışta dünya benzeri bir alan. Kırmızı-siyah bölgeler: mutant kontrolü. Yeşil bölgeler: arındırılmış, güvenli, canlı.

Oyun ilerledikçe bu harita görsel olarak yeşillenir. Bu, oyuncunun "etki alanını" somut hale getirir.

### 10.2 Outpost Operasyon Akışı

```
1. Telsiz Kulesi'nden harita açılır
         ↓
2. Outpost seçilir → İstihbarat ekranı gösterilir
   ("Bölge tehlike seviyesi: Orta / Tahmini düşman sayısı: 200+")
         ↓
3. Operasyon başlatılır
         ↓
4. 20 Dalga Savaşı
         ↓
5a. BAŞARILI: Boss yenilir
    → Yeşillenme animasyonu
    → Pasif gelir noktası açılır
    → Haritada yeşil işaret belirir
         ↓
5b. BAŞARISIZ: Ölüm ekranı
    → Reklam ile x2 loot seçeneği
    → Outpost tekrar denenebilir (kilidi kapanmaz)
```

### 10.3 Yeşillenme (Terraforming) Sahnesi

Bu oyunun en duygusal anı. Tasarım açısından bu ana çok dikkat çekilmeli:

1. Boss düşer
2. **0.5 saniye sessizlik** — son derece önemli, en kritik game feel anı
3. Ekran hafifçe **beyaza yakar**
4. Müzik birden değişir — savaş müziği yerini huzurlu bir melodiye bırakır
5. Zehirli sis partikülleri hızla yok olur
6. Zemin texture'ı çatlak topraktan yemyeşil çimene dönüşür
7. Küçük çiçekler anında açılır (partikel efekti)
8. Kamera hafifçe yükselir, alanın tamamını gösterir
9. Ekranda: **"BÖLGE ARINDIRIlDI"** + loot özeti

> 📝 **Yusuf için not:** Bu an için bir "zafer metni" yazılabilir. Her bölge için farklı. Örnek:  
> *"Zehirli Vadi bir zamanlar çocukların koşturduğu bir yermiş. Şimdi yeniden olabilir."*

### 10.4 Outpost Gelişimi (Uydu Üsler ve İleri Idle Tycoon)

Arındırılan bir Outpost, sadece haritada yeşil bir leke olarak kalmaz, aynı zamanda ana üssün bir uzantısı (uydu üs) haline gelir.

1. **Tarla ve Tesis Kurma:** Yeşillenen bu temiz topraklara oyuncu tarafından yeni tarlalar, su kuyuları ve küçük üretim çadırları kurulabilir.
2. **Sığınmacı Atama:** Ana üste boşta duran sığınmacılar, bu yeni Outpost'lara kalıcı olarak yerleştirilebilir. Özel statlarına (Örn: Intelligence/Zeka veya Endurance/Dayanıklılık) göre farklı üretim bonusları sağlarlar.
3. **Kusursuz Idle Geliri:** Tehlikeli keşif görevlerinin aksine, arındırılmış bir Outpost'a atanan sığınmacıların **ölüm riski yoktur**. Orada yaşarlar ve oyuncu oyunda olmasa bile her gün düzenli olarak büyük miktarda saf gıda ve tohum özü üretirler.

Bu mekanik, Idle Tycoon hissini sadece tek bir base'de sıkışmaktan kurtarır ve haritanın geneline yayarak oyuncunun *"Ben bu dünyayı ilmek ilmek yeniden inşa ediyorum"* duygusunu perçinler.

---

## 11. Silahlar & Yetenek Havuzu (RNG Sistemi)

> 📝 **Tasarımcı için not:** Her silahın savaşta görsel olarak belirgin ve özgün bir "imzası" olmalı. Oyuncu silahını görmeden bile kimliği tanınmalı.

### 11.1 Temel Silahlar (Başlangıç Havuzu)

| Silah | Görsel | Nasıl Çalışır |
|---|---|---|
| **Tohum Atıcı** | Küçük parlak noktalar | Doğrudan ateşleme, hızlı, az hasar |
| **Polen Bulutu** | Sarı partiküller halka şeklinde yayılır | Çevreye alan hasarı, orta hız |
| **Dikenli Sarmaşık Kalkanı** | Karakterin etrafında dönen dikenler | Yaklaşan düşmana temas hasarı |
| **Su Jeti** | Mavi lazer çizgisi | Uzun menzil, düşük hasar ama delip geçer |

### 11.2 Nadir Silahlar (Oyun ortasında açılır)

| Silah | Efekt | Nadir Seviye |
|---|---|---|
| **Mantar Patlaması** | Öldürülen düşman zehirli mantar bırakır | 🔵 Nadir |
| **Işık Fotosentezi** | Güneş ışığı hasarı, bossları yavaşlatır | 🔵 Nadir |
| **Kök Ağı** | Yere kök serper, düşmanları yavaşlatır | 🔵 Nadir |
| **Spor Patlayıcı** | Belirli aralıklarla büyük patlama | 🔵 Nadir |

### 11.3 Efsanevi Silahlar (Oyun sonu için)

| Silah | Efekt | Nadir Seviye |
|---|---|---|
| **Orijin Tohumu** | Düşmanı normale döndürür — öldürmez, mutasyonu geri alır | 🟡 Efsanevi |
| **Klorofil Dalgası** | Tüm ekrana yayılan alan saldırısı | 🟡 Efsanevi |
| **Ağaç Kalkan** | Geçici olarak devasa bir ağaç dalı kalkan olarak fırlar | 🟡 Efsanevi |

> 📝 **Yusuf için not:** "Orijin Tohumu" oyunun lore'uyla mükemmel uyuşuyor — düşmanı öldürmüyor, kurtarıyor. Bu efsanevi silah, oyunun ana temasının oynanış içindeki somut hali.

### 11.4 Pasif Yetenek Örnekleri

| Pasif | Etki |
|---|---|
| **Klorofil Zırhı** | Her X saniyede bir hasar emici kabuk oluşur |
| **Fotosentez Döngüsü** | Düşman öldükçe küçük can yenilenir |
| **Hızlı Kökler** | Hareket hızı artar |
| **Çift Polen** | Her ateşte ek bir tohum fırlar |
| **Zehir Bağışıklığı** | Zehirli düşman tipleri hasar vermez |

---

## 12. Düşman Kataloğu

> 📝 **Tasarımcı için not:** Her düşman türü için ayrı bir sprite seti gerekli. Hareket animasyonu en az 4-6 frame olmalı.

### 12.1 Temel Düşmanlar

| İsim | Görsel Konsept | Davranış |
|---|---|---|
| **Koşan Sarmaşık** | Uzun sürünen, ince gövdeli mutant | Hızlı ama zayıf, sürü halinde gelir |
| **Zırhlı Kaktüs** | Yavaş, diken zırhıyla kaplı | Yavaş ama çok dayanıklı |
| **Zehir Çiçeği** | Renkli ama ölümcül | Mesafeden zehir püskürtür, yakına gelmez |
| **Mantar Adamı** | Şişman, yavaş | Öldüğünde patlar ve alan zehiri yayar |
| **Hızlı Fide** | Küçük, çok hızlı | Tek başına zayıf, 20+ grupta gelir |

### 12.2 Özel Düşmanlar (Oyun ortasında açılır)

| İsim | Özellik |
|---|---|
| **Köklü Dev** | Dev ama yavaş, çok can. Etrafa taş fırlatır. |
| **Asalak Sarmaşık** | Oyuncuya yapışır, hasar çarpanı verir |
| **Sürü Lideri** | Öldürülünce etraftaki düşmanlar hızlanır |

### 12.3 Mutant Hayvanlar

> Unutma: Hayvanlar da mutasyon geçirdi.

| İsim | Temel Hayvan | Mutasyon |
|---|---|---|
| **Köklü Kurt** | Kurt | Sırtından ağaç kökleri çıkmış, bölge gaz yayıyor |
| **Zehir Böceği** | Büyük böcek | Dev boyuta ulaşmış, asit salgısı var |
| **Kabuğun Kabuğu** | Kaplumbağa | Kabuğu mutant kaya gibi, çok dayanıklı |

---

## 13. Gelir Modeli & Sosyal Sorumluluk

### 13.1 Rewarded Ads (Ödüllü Reklam)

**Ne zaman çıkar?**
- Operasyon başarısız olduğunda (ölüm)
- Operasyon başarılı olduğunda (isteğe bağlı bonus olarak)
- Sığınmacı gönderirken (görevi hızlandırmak için)

**Asla:**
- Reklam başlangıçta zorla çıkmaz
- Reklam oyun içinde gelişigüzel pop-up olmaz
- Oyuncu her zaman "Hayır" diyebilir

Bu yaklaşım, **Catvertising Award** kategorisini doğrudan hedefler.

### 13.2 Uygulama İçi Satın Alım (IAP)

| Paket | İçerik | Fiyat | Özel Özellik |
|---|---|---|---|
| **Başlangıç Paketi** | 500 Altın + Ekstra Tohum Kapasitesi | 29 TL | — |
| **Kaşif Paketi** | Sığınmacı slotu +2 + 1000 Altın | 79 TL | — |
| **Dünya Kurtarıcı** | 2000 Altın + "Kuantum Radar" | 199 TL | 🌳 Gerçek Ağaç |
| **Orman Paketi** | 5000 Altın + Özel Karakter Kostümü | 499 TL | 🌳🌳🌳 3 Gerçek Ağaç |

### 13.3 Gerçek Dünyaya Etki — Ağaç Dikme Sistemi

Bu, oyunun en güçlü pazarlama silahı ve RevenueCat Peace Prize için doğrudan aday.

**Nasıl Çalışır:**
1. Oyuncu "Dünya Kurtarıcı" paketini satın alır
2. RevenueCat ödemeyi doğrular
3. Sistem otomatik olarak bir ağaç dikme platformuna (örn: Trees for the Future) bağışı iletir
4. Oyuncuya oyun içinde bir **"Ağaç Sertifikası"** verilir (paylaşılabilir)
5. Oyundaki haritada dünyanın gerçek bir noktasında yeşil bir pin belirir

**Oyun İçi Yansıması:**
- Ana menüde "Bugüne kadar dikilen ağaçlar: 1,247" gibi canlı bir sayaç
- Bu sayaç arttıkça oyunun loading screen arka planı daha yeşil hale gelir

---

## 14. MVP — Shipaton 2026 Sürümü (10 Dakika)

### 14.1 MVP Nedir?

MVP, oyunun sınırlı ama **tam işlevsel** ilk sürümüdür. 1 Ağustos - 30 Eylül 2026 tarihleri arasında Google Play Store'a yüklenmesi planlanmaktadır.

### 14.2 MVP Kapsamı — Ne VAR, Ne YOK?

| Özellik | MVP'de Var mı? |
|---|---|
| Tutorial (Sera ele geçirme) | ✅ Evet |
| 1 Outpost + 20 Wave Savaş | ✅ Evet |
| Boss Savaşı + Yeşillenme Animasyonu | ✅ Evet |
| 5 Temel Silah | ✅ Evet |
| 5 Pasif Yetenek | ✅ Evet |
| Atölye (Perma-upgrade) | ✅ Basit versiyon |
| Reklam İzle (x2 Loot) | ✅ Evet (RevenueCat) |
| Ağaç Dikme IAP paketi | ✅ Evet (RevenueCat) |
| Sığınmacı Yönetimi | ❌ Sonraki güncelleme |
| Tarla / Çadır İnşası | ❌ Sonraki güncelleme |
| Çoklu Harita | ❌ Sonraki güncelleme |
| Endless Mod | ❌ Sonraki güncelleme |

### 14.3 10 Dakikalık MVP Oynanış Senaryosu

```
[DAKİKA 0:00 - 1:30] TUTORIAL
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Oyuncu karanlık seraya girer.
15-20 zayıf mutant temizlenir.
Sera güvenli hale gelir.
Işıklar açılır, Telsiz Kulesi aktifleşir.

[DAKİKA 1:30 - 2:00] HARITA AÇILIŞI
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Oyuncu Telsiz Kulesi'ne tıklar.
Haritada "Zehirli Vadi Outpost" belirir.
Oyuncu operasyonu başlatır.

[DAKİKA 2:00 - 9:00] SAVAŞ (20 WAVE)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Dalga dalga düşman.
Her seviye atlamasında 3 yetenek kartı.
Oyuncu kendi build'ini kurar.
Gerilim tırmanır.

[DAKİKA 9:00 - 10:00] BOSS & YEŞİLLENME
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Kök-Ata boss sahnesi.
Boss yenilir.
Yeşillenme animasyonu şöleni.
Harita güncellenir.

[OYUN SONU EKRANI]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Loot özeti.
[📺 Reklam İzle → x2 Loot] — RevenueCat Ads
[🌳 Dünyayı Kurtarmaya Katıl] — IAP butonu
[🏠 Üsse Dön] — Atölye upgrade ekranı
```

---

## 15. Proje Yol Haritası (Lifecycle)

### Phase 1: MVP & Shipaton (Şimdiki — Eylül 2026)

- Tutorial + 1 Outpost + Boss
- RevenueCat entegrasyonu (Reklam + IAP)
- Google Play yayını
- Shipaton başvurusu + 2 dakikalık demo videosu

### Phase 2: MaxVP & Büyük Lansman (2026 Son Çeyrek)

- Sığınmacı ve Idle Tycoon sistemi
- Tarla + Çadır inşası
- 3 Outpost + 2 Yeni Harita
- Endless Mod (Zaman Kapsülü)
- Ağaç dikme canlı sayacı
- iOS sürümü

### Phase 3: İçerik Güncellemeleri (2027 — 2028)

- Yeni boss'lar (Mutant hayvanlar)
- Yeni bölgeler (Çöl, Kuzeyde Buz)
- Sığınmacı hikaye arkleri (Yusuf'un uzun soluklu lore'u)
- Steam (PC) versiyonu

### Phase 4: Nihai Vizyon (2029 — 2030)

- 5 tam bölge, her biri kendine özgü boss ve atmosfer
- 50+ silah + evrim formu
- Tam oyuncu topluluğu (sezonluk etkinlikler)
- Dünya genelinde gerçekten dikilen ağaçlar için canlı sayaç entegrasyonu

---

## 📎 EKLER

### Ekip Sorumlulukları

| Ekip Üyesi | Rol | Ana Sorumluluk |
|---|---|---|
| **Sen (Proje Lideri)** | Yazılım & Oyun Direktörlüğü | Unity geliştirme, RevenueCat entegrasyonu, Google Play yayını |
| **Yusuf** | Hikaye & Lore Direktörü | Karakter profilleri, dünya lore'u, iç ses metinleri, bölge açıklama yazıları |
| **Grafik Tasarımcı** | Sanat Direktörlüğü | Karakter spriteleri, düşman animasyonları, UI tasarımı, harita arka planları, efektler |

### Yusuf'a Öncelikli Yapılacaklar Listesi

1. [ ] Ana karakterin ismi ve kişilik belgesi (1 sayfa)
2. [ ] 10 adet sığınmacı profil kartı (isim + kısa hikaye + alıntı)
3. [ ] Tutorial için iç ses / günlük metni (max 5 cümle)
4. [ ] Her Outpost için "zafer metni" (Yeşillenme anı için, max 2 cümle)
5. [ ] 3 Boss için "loading screen" lore açıklaması

### Grafik Tasarımcıya Öncelikli Yapılacaklar Listesi (MVP için)

1. [ ] Ana karakter spriti (4 yön, idle + hareket animasyonu)
2. [ ] 3 temel düşman spriti (Koşan Sarmaşık, Zırhlı Kaktüs, Zehir Çiçeği)
3. [ ] Sera harita arka planı (Tutorial)
4. [ ] Zehirli Vadi harita arka planı (1. Outpost)
5. [ ] "Yeşillenme" öncesi/sonrası harita tonu (aynı harita, 2 versiyon)
6. [ ] 5 silah ikonu
7. [ ] UI elementleri: can barı, XP barı, dalga sayacı
8. [ ] 3 yetenek kartı şablonu (Yaygın, Nadir, Efsanevi)

---

*Bu belge yaşayan bir döküman olarak güncellenmeye devam edecektir.*  
*Son güncelleme: Temmuz 2026*
