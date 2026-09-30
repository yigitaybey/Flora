bu gün tamamen düşman balance olaylatını araştırıyoprum 
karar kıldığım mekanik şu olucak 2 ana değerimiz var 1.si zorluk 2.si SPS(Spawn per second) bu değerler zamnan arttıkça exponensiyel artıcak ayrıca bütün düşmanlar aynı saniye kalmicek level yükseldikçe mesele her 10 levelde yeni düşman gelecek
**### 1. Hareket Hızları (Speed)

- **Karakterimiz (Sylva):** Hızı **5.0** veya **5.5** olmalı. Oyuncu _her zaman_ standart düşmanlardan daha hızlı olmalıdır ki kaçmak yeteneklerine bağlı olsun, çaresiz hissetmesin.
- **Avcı Bitkiler (Standart):** Hızı **3.0**. (Karakterden yavaş, sadece kalabalıkla sıkıştırmaya çalışırlar).
- **Spore Taşıyıcı (Hızlı Tür):** Hızı **4.5** veya **4.8**. (Karaktere çok yakın bir hız. Amacı oyuncuyu panikletip hareket etmeye zorlamak).
- **Boss (ENT):** Hızı **2.0**. Çok yavaş olmalı ama alanı kaplamalı.

### 2. Menziller (Range)

- **Sylva'nın (Karakter) Base Menzili:** **10 ile 12 birim** arası (Kamera açımıza göre ekranın %60'ı kadar uzağa vurabilmeli). Eğer menzil çok kısa olursa düşmanlar çok yaklaşır ve oyuncu haksızlığa uğradığını hisseder.
- **Menzilli Düşmanlar (Ranged):** Atış menzili **8 veya en fazla 10 birim** olmalı. Düşman **kesinlikle kamera ekranının dışından (kör noktadan) oyuncuya ateş etmemelidir!** Oyuncu mermiyi kimin attığını görmezse oyuna sinir olur.

### 3. Düşman Mermisi (Projectile) Hızı

- **Düşman Mermisi:** Hızı **6.0 ile 7.0** civarında olmalıdır. Asla gerçek bir mermi gibi (hız = 20) gitmemelidir. Oyuncu üzerine gelen asit topunu yavaşça süzülürken görebilmeli ve "refleksleriyle" o merminin arasından sıyrılabilmelidir. Bu, oyuncuya müthiş bir tatmin verir.

### 4. Unuttuğumuz En Önemli Şey (Gotcha!): "I-Frames"

Scriptlerimizi incelerken çok kritik bir eksik fark ettim. Bizim `PlayerHealth.cs` kodunda oyuncu hasar aldığında bir **"Dokunulmazlık Süresi" (Invincibility Frames - I-Frames)** yok. Eğer bu haliyle 5 tane düşman oyuncunun dibinde belirirse, hepsi saniyenin onda biri içinde aynı anda vurur ve oyuncumuz daha ne olduğunu anlamadan saniyesinde ölür (Tek yer).

**Çözüm:** Karakter hasar aldığında **0.5 saniye** boyunca başka kimseden hasar almamalı ve o sırada kırmızı yanıp sönmeli.**


