Bugün Flora'nın MVP (Minimum Viable Product) aşaması için devasa bir yol kat ettik. Özellikle silah mekanikleri, görsel efektler ve harita eşyalarının (Consumables) temelini tamamen bitirip AAA oyun standartlarına çektik.

İşte adım adım bugün yaşadığımız zorluklar, çözümlerimiz ve eklediğimiz yepyeni özellikler:

---

### 1. Polen Silahı (Pollen Weapon) Temel Mekaniği ve Düzeltmeler

**Yapılanlar:**

- Silahın karakter (Slyvia) etrafında yumuşak bir yörüngede (Orbit) dönmesini sağlayan trigonometrik sistem kuruldu.
- Silahın menzile giren düşmanlara otomatik dönüp (LookAt) mermi ateşlemesi sağlandı.

**Zorluklar ve Yanlış Fikirler:**

- **Blender Aktarım Sorunları:** 3D model projeye aktarıldığında silah ters bakıyordu (Y=180 derece) ve içinde Blender'dan kalma gereksiz "Kamera" ve "Işık" objeleri barındırıyordu. Bu durum oyunun ana kamerasını bozuyordu.
- **Çözüm (Ultimate Auto-Fix):** Silahın koduna (`PollenWeapon.cs`) oyun başladığı an içindeki gereksiz ışık ve kameraları tarayıp otomatik yok eden bir sistem yazdık. Silahı görünmez bir parent objenin içine koyup eksenini düzelttik.

### 2. Vuruş Hissiyatı (Game Feel): Geri Tepme ve Namlu Ateşi

**Yapılanlar:**

- **Recoil (Geri Tepme):** Silah her ateş ettiğinde zıt yönde (`-gunModel.forward`) anlık bir tepme kuvveti uygulayıp `Vector3.Lerp` ile yumuşakça orijinal pozisyonuna dönmesini sağladık.
- **Muzzle Flash (Namlu Ateşi):** Ateş edildiğinde namlu ucunda bir parlama oluşturduk.

**Zorluklar ve Yanlış Fikirler:**

- **Görünmezlik Sorunu:** 2D Sprite olarak tasarladığımız namlu ateşi ilk başta ekranda görünmüyordu. Çünkü izometrik açıda yere paralel olsun diye X ekseninde 90 derece yatırdığımız resim, Blender'dan gelen 180 derecelik rotasyon sapması yüzünden **gökyüzüne değil, direkt toprağa bakıyordu!** Arka yüzü de (Backface Culling) görünmez olduğu için saatlerce hatayı aradık.
- **Çözüm & Particle System Upgrade:** Rotasyonu koda müdahale edip `-90` yaparak çözdük. Ancak 2D resmin kamera açısından dolayı gıcık etmesi üzerine fikrimizi değiştirip sistemi çok daha profesyonel olan **3D Particle System'e** (Parçacık Efekti) taşıdık.

### 3. Harita Eşyaları (Consumables) Fonksiyonelliği

Bugün oyuna 3 temel harita eşyası eklendi ve mantıkları Vampire Survivors standartlarına uyarlandı.

**Zorluklar ve Yanlış Fikirler:**

- **Prefab Kopyalama Hatası:** Yiğit küpleri (Cube) çoğaltarak 3 farklı eşya oluşturdu ancak hepsinin `ConsumableItem` scriptindeki "Eşya Tipi" (Type) Potion olarak kaldığı için üçü de sadece can veriyordu. (Sorun Inspector'dan tipler seçilerek saniyeler içinde çözüldü).

**Eklenen Sistemler:**

1. **Potion (Şifa):** Alındığı an oyuncuya (Slyvia) +30 HP veriyor.
2. **Vacuum (Vakum / Mıknatıs):**
    - _Eski Fikir:_ XP tohumlarını anında oyuncunun içine ışınlamak (Çok yapay duruyordu).
    - _Yeni Çözüm:_ `Seed.cs` scriptine `Magnetize()` fonksiyonu eklendi. Vakum alındığı an haritadaki tüm tohumlar havaya kalkıp mıknatıs gibi süzülerek oyuncuya uçuyor.
3. **Screen Wipe (Kavurma / Güneş Işığı):**
    - _Eski Fikir:_ Sahnede ne varsa 9999 hasar vurup herkese tek atmak.
    - _Yeni Çözüm (Balans):_ Koda Boss ve Elite koruması eklendi. Normal yaratıklar anında kül olurken, Boss (Chinar) tek yemiyor, sadece anlık canının %30'unu kaybediyor.

### 4. Ekran Efektleri ve 3D Asset Entegrasyonu

**Yapılanlar:**

- **Hit Stop & Flash:** Screen Wipe alındığı an nuke (kavurma) hissiyatını vermek için `Consumable.cs` içine bir Coroutine yazıldı. Zaman `0.01f`'e çekilerek oyun anlık donduruluyor (Hit Stop) ve kamerada bembeyaz bir flaş patlatılıyor.
- _Revize:_ Flaş %90 opaklıktan %65'e düşürüldü ki oyuncuyu kör etmesin ve arkada yanan düşmanlar seçilebilsin.
- **3D Asset (FBX) Entegrasyonu:** Ekip arkadaşından gelen FBX dosyasındaki objeler ("Cannot restructure Prefab instance" uyarısı aşılarak) `Unpack Completely` yöntemiyle tek tek ayrıldı.
- **PBR Texture Giyidilmesi:** URP (Lit) materyaline PBR haritaları bağlandı:
    - `_Diffuse` -> Base Map
    - `_Emission` -> Emission Map (Gücü HDR Intensity'den verildi)
    - `_Roughness` -> Metallic Map (Smoothness ile ters orantılı ayarlandı)

---

### Sonuç ve Sonraki Adımlar

Oyunun combat (savaş) döngüsü, vuruş hissi ve ödül sistemi inanılmaz sağlam bir temele oturdu. Hem kod mimarisi hem de görsel kalite açısından devasa bir adım atıldı.

**Yarınki Olası Hedeflerimiz:**

1. Alev Silahının (Flamethrower) animasyon ve partikül (ateş püskürtme) ayarlarını yapmak.
2. Level atlama (Upgrade) UI menüsünü XP toplayarak aktif edip test etmek.
3. Düşman XP (Tohum) modellerini projeye bağlamak.