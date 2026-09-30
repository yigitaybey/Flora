# KAFES OMEGA (Level 10) PDD: Bees in The Tree (Flora)

*DİKKAT: Bu doküman, güncellenmiş `.agyrules` standartlarına göre yeniden derlenmiş; KAFES ekosisteminin "Bullet Haven + Base Building + Idle" türlerini birleştiren en kapsamlı (Hibrit) Ticari Oyun projesidir. Sistem Tasarımı, Idle Hileleri ve Finansal (Charity) Hukuk krizleri 8. Madde'de arşivlenmiştir.*

---

## BÖLÜM 1: KİŞİSEL ANALİZ VE FİZİBİLİTE (4 Madde)

### 1. Zorluk Derecesi: 9 / 10
Bu proje standart bir aksiyon oyunu değildir. Bir yanda 2000 düşmanın saldırdığı saf bir Savaş Motoru (Bullet Haven), diğer yanda yapay zekaların (NPC) binalar arasında dolaştığı bir Şehir Kurma (Base Builder) sistemi vardır. İki devasa mekaniği aynı RAM ve İşlemci üzerinde çökmeden çalıştırmak ve üzerine gerçek dünya API (Ağaç Dikme) entegrasyonu yapmak kodlama zorluğunu 9/10 seviyesine çıkarır.

### 2. Yapılabilirlik Analizi
Projenin kapsamı (Scope) tek bir geliştirici için çok tehlikeli sulardadır ("Kapsam Kayması - Scope Creep" riski). Ancak tüm modları aynı anda yapmaya çalışmak yerine belgedeki "Faz 1 (Sadece Savaş ve Küçük Atölye)" stratejisine sadık kalınırsa proje tamamen yapılabilirdir. Tycoon kısmı mutlaka demo sonrasına bırakılmalıdır.

### 3. Kullanılması Gereken Teknolojiler
*   **Oyun Motoru & Savaş:** Unity (C#), Object Pooling, Flocking / Boids (Sürü Matematiği).
*   **Üs ve Yapay Zeka:** Hücre Tabanlı (Grid-based) A* Pathfinding Algoritması.
*   **Veri ve Güvenlik:** JSON (Şifreli Kayıt Sistemi), Güvenli Dış Zaman Sunucusu (NTP Server).
*   **Monetizasyon & Gerçek Dünya:** RevenueCat SDK (IAP), Charity (Vakıf) API Bağlantısı.

### 4. Öğrenme Süreci (x4 Sindirme Çarpanı)
| Konu Başlığı | Öğrenme Zorluğu | Öğrenme Süresi (x4) | Odaklanılacak Kazanımlar |
| :--- | :--- | :--- | :--- |
| **Grid Pathfinding (A*)** | Zor | **1 Ay** | Sığınmacıların, oyuncunun sürekli yerini değiştirdiği duvar ve tarlalara çarpmadan yollarını bulması. |
| **Idle Zaman Matematiği** | Orta | **2 Hafta** | Oyun kapalıyken geçen sürenin hilesiz hesaplanıp loot (ganimet) matematiğine dökülmesi. |
| **RevenueCat & Vakıf API** | Zor | **1 Ay** | Mobil mağazalardan güvenli tahsilat yapıp, makbuz doğrulayarak (Receipt Validation) dış dünyada ağaç dikme tetikleyicisi yazmak. |

---

## BÖLÜM 2: KAFES OMEGA ŞABLONU (10 Madde)

### 1. Proje Özeti, Vizyonu ve Nihai Hedef
Flora (Bees in The Tree), sera gazıyla kaplanmış distopik bir dünyada geçen Hibrit bir Hayatta Kalma oyunudur. Oyuncu ormanda mutant bitkilerle savaşır (Bullet Haven), ganimetlerle üssünü geliştirir (Base Building) ve sığınmacıları keşfe yollayarak oyun kapalıyken bile gelişir (Idle Tycoon). Nihai vizyon; oyundaki dijital satın alımların (Altın) gerçek dünyada ağaç dikimine dönüşmesi (Sosyal Etki) ve geniş kitlelere ulaşmasıdır.

### 2. Matematik, Fizik ve Kinematik Çekirdeği
*   **Dalga Ölçeklemesi:** Düşman sayısı ve gücünün zamana bağlı üstel artışı: $N(t) = N_0 \cdot e^{k \cdot t}$
*   **Sığınmacı Hayatta Kalma (RNG):** Loot seferinde ölme riskinin stat bazlı hesabı: $P_{survival} = \left( \frac{\text{Endurance} + \text{Luck}}{\text{Danger Level}} \right) \cdot 100$
*   **Hasar Zırh Lojistiği:** $\text{Net Damage} = (\text{Base} + \text{Bonus}) \cdot \text{Crit} - \text{Armor}$

### 3. Nokta Atışı Malzeme (BOM) ve Tech-Stack
*   **Oyun Döngüsü Öğeleri:** Botanik Bilimci (Ana Karakter), Sığınmacılar (NPC), Tohum Mermileri, Mutant Bitkiler.
*   **Üs Yapıları:** Seralar, Tarlalar, İleri Seviye Laboratuvarlar, Savunma Duvarları.
*   **Entegrasyonlar:** RevenueCat (Ödeme Altyapısı), NTP Time (Zaman Güvenliği).

### 4. Veri Akış Şeması (Core Game Loop)
1.  Karakter ormana iner, Object Pooling ile üretilen binlerce mutantla savaşır.
2.  Ölür veya süre biterse kazanılan materyallerle "Güvenli Bölge"ye (Base) dönülür.
3.  Base'de binalar geliştirilir (Grid sistemi), yeni sığınmacılar (SPECIAL statlı) kampa alınır.
4.  Oyun kapatıldığında Idle sistemi başlar; sığınmacılar loota gider, sunucu saati (NTP) kaydı tutar.

### 5. KAFES (Zero-Trust) ve Ağ İzolasyonu
*   **Zaman İzolasyonu:** Idle mekaniği (Ganimet toplama süresi) asla kullanıcının cihaz saatine (Local Time) güvenmez (Zero-Trust). Sistem sadece bağımsız dış sunuculardan (NTP) gelen zamana göre ilerleme kaydeder.

### 6. Güç Tüketimi ve Yük Yönetimi (Memory & CPU)
*   **Durum Makinesi (State Machine) Sahne Yönetimi:** 2000 düşmanlı Savaş Sahnesi ile Arayüz ağırlıklı Üs Kurma Sahnesi arasında sürekli geçiş yapmak RAM'i dondurur. Cihaz bellek sızıntısı (Memory Leak) yaşamasın diye sahneler sürekli silinip baştan yüklenmez, "Additive (Eklemeli)" olarak bellekte tutulup görünmez yapılır (Disable).

### 7. Felaket Senaryoları (Disaster Recovery)
*   **Makbuz Uyuşmazlığı (Bağlantı Kopması):** Oyuncu 500 TL'lik Ağaç paketini aldığı anda interneti koparsa; oyun bu makbuzu (Receipt) şifreli olarak Cihaz Hafızasına yazar. İnternet bağlandığı an RevenueCat sunucularıyla tekrar konuşup işlemi tamamlar. Parası asla boşa gitmez.

### 8. ⚠️ Çözümsüz Açık Sorunlar (Ar-Ge Edge Case'leri)
Hibrit motorun zayıf noktası olan ve çözülmezse mobil cihazları kitleyip Hukuki krizler yaratacak 4 Ağır Kriz:

| Ertelemeye Alınan Sorun | Teknik/Hukuki Kriz | Muhtemel Çözüm (Sonra Tartışılacak) |
| :--- | :--- | :--- |
| **A* Navigasyon Tıkanması** | Base yaparken oyuncunun sürekli duvar eklemesinin yapay zekanın (NavMesh) haritayı anlık baştan hesaplamasını (Rebake) gerektirip telefonu dondurması. | Ağır NavMesh yerine, sadece o hücrenin (Tile) yürünemez yapıldığı hafif Grid-Based A* mimarisi kullanmak. |
| **Zaman Hilesi (Time Spoofing)** | Idle modunda oyuncunun telefonun saatini 1 ay ileri alarak beklemeden devasa ganimet kazanması ve oyun içi ekonomiyi çökertebilmesi. | Cihaz saatini yok sayıp süreyi tamamen dış NTP (Network Time Protocol) sunucularından çekmek. |
| **Sahne Sızıntısı (Memory Leak)** | Savaş modu ile Base modu arasında sürekli sahne (Scene) yüklenmesinin Unity'de Garbage Collection (Çöp Toplama) krizine yol açması. | Sahneleri silmek yerine, birbiri üstüne bindiren ve aktif/deaktif eden bir "State Machine (Durum Makinesi)" kurmak. |
| **Charity (Vakıf) İade Krizi** | Ağaç paketini alan oyuncunun parası vakfa gönderildikten sonra, oyuncunun mağazadan (App Store) parasını iade edip (Refund) stüdyoyu borca sokması. | API tetikleyicisini bekletmek (Escrow). Parayı 14 günlük yasal iade süresi dolduktan sonra vakfa topluca (Batch) yollamak. |

### 9. Bakım ve Kalibrasyon Döngüsü (Balancing & LiveOps)
*   Topluluk oynadıkça "Sığınmacıların Loot'ta çok çabuk ölmesi" veya "Belirli bir tohumun aşırı güçlü (OP) olması" gibi sorunlar baş gösterecektir. Statlar ve düşman zırhları tamamen ScriptableObjects üzerinden modüler tutulduğu için bu dengelemeler yamasız (Hotfix) anında halledilebilir.

### 10. Maliyet Tahmini ve x4 Öğrenme Eğrisi
RevenueCat komisyonları (Kazanılan paradan ufak yüzdeler) ve Charity (Ağaç) masrafları vardır. Geliştirme safhaları:
*   **Faz 1 (MVP & Demo - 6 Ay):** Base building hariç tutulacak. Sadece "Botanik Bilimci ormanda hayatta kalır, tohum patlatır ve Atölyede ufak upgrade yapar" döngüsü tamamlanıp test edilecek.
*   **Faz 2 (MaxVP & Büyük Lansman - 1 Yıl):** Idle Tycoon mekanikleri, sığınmacı statları, 15 Tohum evrimi ve "Ağaç Dikme API'si" eklenip mağazaya tam çıkış yapılacak.
*   **Faz 3 (Post-Launch - 2/3 Yıl):** İlk ay RNG dengelemeleri (Hotfix). 6-8. aylarda "Çöl" vb. yeni haritalarla yeni düşmanların ve tohumların oyuna dahil edilmesi (Major DLC).
