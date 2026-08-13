# Internship Daily Log

---

## YYYY-MM-DD (Gün)
**Görev:** 

**Yaptıklarım:**
-
-
-

**Öğrendiklerim:**
-
-

**Zorlandığım kısım:**
-

**Yarına not:**
-

---

## 2026-08-13 (Perşembe)

**Görev:** Ödeme sistemi mikroservisinde (GitTransactionsService) ilişkisel veritabanı (Foreign Key) bağlantılarını düzeltmek ve API uç noktalarındaki veri tipi uyuşmazlıklarını gidermek.

### Yaptıklarım
- `Transaction`, `Terminal` ve `Merchant` tabloları arasındaki Primary Key (PK) ve Foreign Key (FK) ilişkilerinde yaşanan `Datatype Mismatch` (int/string) hatalarını giderdim.
- Kod karmaşasını önlemek adına dışarıdan gelen iş birimi kimliklerini `TerminalNo` standardında birleştirirken, veritabanının arka planda kendi tam sayı (`int Id`) anahtarlarını kullanmasını sağladım.
- `TransactionsController` içerisindeki `CreateTerminal` ve `AddTransaction` endpoint'lerini Entity Framework standartlarına göre güncelleyerek, gelen istekleri önce veritabanında doğrulayan (`GetTerminalByNoAsync`) güvenli bir akış kurdum.
- Request DTO'larında büyük/küçük harf duyarlılığı ve tip dönüşümü (`int.Parse`) hatalarını çözdüm.

### Öğrendiklerim
- Fintech projelerinde veritabanının kendi anahtarı (`int Id`) ile iş birimlerinin kullandığı referans kodlarının (`string TerminalNo`) teknik olarak birbirinden nasıl ayrılması gerektiğini tecrübe ettim.
- Güvenlik ve veri tutarlılığı açısından, `AddTransaction` gibi işlemlerde dışarıdan ekstra `MerchantId` istemenin gereksiz ve riskli olduğunu; bu bilginin sisteme kayıtlı olan `TerminalNo` üzerinden arka planda çekilmesi gerektiğini öğrendim.

### Zorlandığım Kısım
- Modellerin birinde yapılan ufak bir veri tipi değişikliğinin (string'den int'e geçiş), Controller, Repository ve Entity Framework katmanlarında yarattığı zincirleme uyumsuzlukları tespit edip temizlemek oldukça yorucuydu.

### Yarına Not
- Veritabanı şemasındaki güncellemelerden sonra yeni bir migration (`InitialCleanSetup`) oluşturup veritabanını güncellemek.
- Postman veya Swagger üzerinden Terminal oluşturma ve Transaction ekleme uçtan uca (end-to-end) testlerini gerçekleştirmek.

---

## 2026-08-10 (Pazartesi)

**Görev:**
Transaction (İşlem) API'sinin DTO ve Repository Pattern ile iyileştirilmesi, güvenlik standartlarının artırılması ve iptal (cancellation) kurgusunun geliştirilmesi.

**Yaptıklarım:**
- İç veritabanı ID'lerinin dışarıdan gizlenmesi ve API yanıtlarında sadece sistemin ürettiği benzersiz referans numaralarının (ReferenceNumber) kullanılması sağlandı.
- API'ye gelen isteklerde güvenliği sağlamak için CreateTransactionRequest ve CancelTransactionRequest DTO (Data Transfer Object) sınıfları oluşturuldu.
- Kredi kartı bilgilerinin veritabanına kaydedilmeden önce ilk 8 ve son 4 hanesi görünecek şekilde maskelenmesi algoritması yazıldı.
- Repository arayüzüne ve implementasyonuna, işlemleri referans numarası ile bulmayı sağlayan GetByRefNumberAsync metodu eklendi.
- Kısmi (partial) ve tam (full) iptal senaryolarını yönetmek için veritabanı modeline CancelledAmount eklendi. İşlem sorgulandığında kalan tutarın (CurrentAmount) anlık olarak hesaplanıp dönmesi sağlandı.

**Öğrendiklerim:**
- Dışarıdan gelen verileri doğrudan Entity (veritabanı modeli) ile almak yerine DTO kullanmanın, API'yi Overposting (Mass Assignment) zafiyetinden korumadaki kritik rolü.
- HTTP GET isteklerinin standart gereği gövde (Body) alamayacağını, bu sebeple [FromBody] kullanımı yerine verilerin URL üzerinden (Route parameter) okunması gerektiğini.
- Veritabanında bir kaydın bulunamaması durumunda, duruma göre 404 (Not Found) yerine iş kuralı hatası olarak 400 (BadRequest) dönmenin API standartları açısından daha yönetilebilir olabileceğini.
- C# modellerine sonradan eklenen zorunlu (NOT NULL) alanların, SQLite üzerinde Entity Framework kaydı sırasında HTTP 500 hatalarına yol açtığını; bunun Nullable (?) tiplerle veya Migration komutlarıyla nasıl senkronize edileceğini.

**Zorlandığım kısım:**
- Sisteme iptal nedeni, kur ve iptal edilen miktar gibi yeni alanlar ekledikten sonra aldığım HTTP 500 hatalarının kaynağını tespit etmek. Hatanın veritabanı şemasındaki (SQLite) uyumsuzluktan kaynaklandığını bulup NOT NULL constraint failed hatasını çözmek biraz zaman aldı. Ayrıca GET metodunda yanlışlıkla [FromBody] kullandığım için frontend/Swagger tarafında aldığım çökme hatasının sebebini bulmak karmaşıktı.

**Yarına not:**
- Yazdığım kısmi iptal (partial cancellation) mantığının testlerini yap ve tutarın sıfırlanma durumlarında sistemin "Fully Cancelled" durumuna sorunsuz geçtiğinden emin ol. Ayrıca endpoint gereksinimlerine bakarak açıkta ID veya hassas veri dönen başka bir yer kalıp kalmadığını kontrol et.

## 2026-08-04 (Salı)

**Görev:** 
Projenin temel mimarisini (Interface & Repository) kurup Swagger'ı ayağa kaldırmak ve ardından geçici (in-memory) yapıyı Entity Framework Core kullanarak kalıcı SQLite veritabanına taşımak.

**Yaptıklarım:**
- `ITransactionRepository` interface'ini yazıp, önce in-memory çalışacak `TransactionRepository` sınıfını implement ettim ve `TransactionsController` ile endpoint'leri bağladım.
- .NET 10'da yaşanan Swagger paket çakışması sorununu (`Microsoft.AspNetCore.OpenApi` vs `Swashbuckle`) çözerek API arayüzünü aktif ettim ve GitHub'daki divergent branch / merge conflict sorunlarını giderdim.
- Sistemin altyapısı oturduktan sonra EF Core paketlerini kurdum, `AppDbContext` ayarlarını yapıp Migration (`InitialCreate`) ile fiziksel SQLite veritabanı dosyasını oluşturdum.
- Dependency Injection (DI) ayarlarını güncelledim; In-Memory yapı için kullandığım `AddSingleton` ömrünü, veritabanı mimarisine uygun olan `AddScoped` ile değiştirdim.
- Controller'ı RESTful standartlara uygun hale getirip, metotların `[FromBody]` ile tam `Transaction` nesnesi almasını ve asenkron (`async/await`) çalışmasını sağladım.
- Güvenlik kuralı gereği, gelen isteklerdeki kart numaralarını veritabanına yazılmadan önce otomatik olarak maskeleyen bir mantık ekledim ve Swagger testlerini kolaylaştırmak için modellere `[DefaultValue]` atamaları yaptım.

**Öğrendiklerim:**
- **Interface ve DI Mantığı:** Interface'in bir sözleşme (contract), class'ın ise gerçek implementasyon olduğunu; Controller'ın bu ikisi ile Swagger arasındaki köprü görevini üstlendiğini kavradım. Bağımlılıkları soyutlayarak (Clean Architecture), Controller kodunu neredeyse hiç değiştirmeden tüm sistemi veritabanına taşıyabildim.
- **Git Yönetimi:** `git pull --no-rebase` komutu ile diverged (ayrılmış) branch'leri güvenli bir şekilde birleştirmeyi öğrendim.
- **RESTful ve Veritabanı Standartları:** URL tasarımlarında fiil (`/Add`) kullanılmaması gerektiğini ve ilişkisel veritabanlarındaki Foreign Key kısıtlamalarının (var olmayan bir ID ile işlem yapılamayacağı) pratikteki koruyucu etkisini gördüm.

**Zorlandığım kısım:**
- Günün ilk yarısında Swagger sayfasının 404 dönmesiyle uğraştım; sorunun iki farklı OpenAPI paketinin aynı anda kurulu olmasından kaynaklandığını buldum.
- Günün ikinci yarısında ise In-Memory'den EF Core'a geçerken eski Controller'ın `string` beklemesi ile yeni yapının `int` ve `Transaction` nesnesi beklemesi arasındaki çakışmalardan dolayı `500 Internal Server Error` hataları aldım. Ayrıca IDE'nin otomatik eklediği `System.Transactions` kütüphanesinin benim kendi modelimle çakışmasını ayıklamak biraz zaman aldı.

**Yarına not:**
- Temel mimari, Swagger, SQLite kayıt işlemleri ve kart maskeleme kusursuz çalışıyor.
- Yarın listeleme (GET) işlemlerinde `Transaction` verisiyle birlikte bağlı olduğu `Terminal` ve `Merchant` verilerini de getirme (`.Include()` mantığı) üzerine çalışabilirim.
- Yapılan tüm bu kapsamlı mimari değişiklikleri yöneticimle gözden geçireceğim (Code Review).
---
