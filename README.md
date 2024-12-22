2D-Platformer
Unity ile geliştirdiğim 2D Pixel Art platformer oyunudur. Oyunun assetleri üçüncü parti bir kaynağa aittir, bu nedenle projenin sadece C# scriptleri bulunmaktadır. Oyunu oynamak için Google Drive linkindeki dosyaları indirebilirsiniz.

Proje Hakkında
Bu oyun, Pixel Art bir sanat tarzına sahip bir 2D platform oyunudur. Kullanılan assetler çoğunlukla üçüncü parti bir kaynağa aittir, ancak bazı sprite'lar yapay zeka yardımıyla ya da tarafımca çizilmiştir. Oyuncu, basit kontrollerle platform engellerini aşmaya, düşmanları öldürmeye ve "Coin" toplama amacına odaklanır.

Oyun, ilerledikçe zorluk seviyesi artan 3 farklı ana sahneden oluşmaktadır. Her sahnede karakter yeni bir özellik kazanır ve yeni düşmanlar veya objeler eklenir. Her bölümde belirli noktalarda Checkpoint noktaları bulunur. Oyuncu buradan ilerlemesini kaydedebilir. Oyuncular oyun içinde sahip oldukları canların ve coinlerin sayısı görsel olarak görür. Aynı zamanda Ok atma ve dash yeteneklerinin de bekleme süreleri dolduğunu görsel olarak görür.

Oynanış
Başlangıç: Karakter 0 coin ve 3 can ile başlar.
Coin ve Can Sistemi: Oyuncu, coin toplar ve bu coinlerle market sahnelerinde can satın alabilir.
Market Sahnesi: 1. ve 2. sahneler ile 2. ve 3. sahneler arasında bulunan market sahnelerinde oyuncu, topladığı her 3 coinle 1 adet can satın alabilir.
Bölüm Geçişi: Sahne geçişlerinde yeni düşmanlar, engeller ve karakter özellikleri eklenir. Karakter her sahnede yeni bir yetenek kazanır.
Game Over: Oyuncunun tüm canları tükendiğinde, Game Over ekranı gösterilir.
You Win: Oyuncu oyunu tamamladığında, You Win ekranı gösterilir.
Sahne Yapısı ;Oyun 3 ana sahneden oluşur.


İlk Sahne: Karakterin temelleri öğrenmeye başladığı ve ilk coinleri toplamaya çalıştığı başlangıç sahnesi. Basit kontrollere sahiptir. Zıplama, ok atma, koşma, merdivene tırmanma gibi.
Market Sahnesi: Coinleri harcayarak can satın alınan geçiş sahnesi
İkinci Sahne; Karakterimize Çift zıplama özelliği gelir, sahnenin ilerleyen kısmında bir sandığı tetikleyerek yeni bir geçiş yolu açar ancak kendisini yeni bir öldürülemez düşman tipi kovalamaya başlar.
Son Sahne: Oyunun final kısmı, karaktere dash yeteneği eklenir. Sadece kafasına zıplanılarak öldürülebilen yeni bir düşman çeşidi eklenir. Hareket eden platformlar ve temas edildikten kısa bir süre sonra patlayan bombalar eklenir.

