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

# Internship Daily Log

---

## 2026-08-19 (Çarşamba)
**Görev:** Backend API servisleri için Swagger'a alternatif, tüm işlemleri görsel olarak yönetebileceğim kullanıcı dostu bir HTML kontrol paneli (Dashboard) geliştirmek.

**Yaptıklarım:**
- Proje dizininde `wwwroot` klasörü oluşturup içine tek sayfalık (SPA) bir `index.html` dosyası ekledim.
- `Program.cs` dosyasına `app.UseDefaultFiles()` ve `app.UseStaticFiles()` middleware'lerini ekleyerek API sunucusunun doğrudan bu HTML sayfasını ayağa kaldırmasını sağladım.
- JavaScript `fetch` API kullanarak Üye İşyeri (Merchant) oluşturma, Terminal ekleme, İşlem (Transaction) yapma, İptal (Refund) ve Listeleme gibi tüm Controller endpoint'lerini arayüze bağladım.
- Arayüzü Apple iOS tasarım yönergelerine (Card yapıları, gölgeler, yuvarlatılmış köşeler) uygun şekilde CSS ile şekillendirdim ve CSS değişkenleri (`:root`) kullanarak sistemi varsayılan olarak "Dark Mode" açılacak şekilde yapılandırdım.

**Öğrendiklerim:**
- Frontend HTML dosyası ile Backend API'ın aynı sunucuda (`wwwroot` üzerinden) çalıştırılmasının, tarayıcılardaki meşhur CORS (Cross-Origin Resource Sharing) hatalarını nasıl ortadan kaldırdığını öğrendim.
- C# Controller'dan dönen JSON formatındaki verileri (özellikle array/dizi formatındaki tüm işlem listelerini) JavaScript ile karşılayıp, ekranda okunabilir dinamik HTML tablolarına ve listelerine nasıl dönüştüreceğimi kavradım.
- `data-theme` attribute'u ve CSS `:root` değişkenleri ile sayfa temasının (Dark/Light mode) ne kadar pratik bir şekilde yönetilebileceğini gördüm.

**Zorlandığım kısım:**
- C# tarafındaki Route yapıları (`[Route("api/[controller]")]`, path parametreleri) ile JavaScript'teki `fetch` adreslerinin (URL'lerin) tam olarak eşleşmesini ve API'ın beklediği JSON yapısını doğru bir şekilde göndermeyi sağlamak.

**Yarına not:**
- Geliştirdiğim bu yeni arayüz üzerinden uçtan uca bir senaryo testi yap (Merchant yarat -> Terminal ata -> Kartla işlem yap -> İşlemi iptal et) ve tüm hata mesajlarının (Status 400 Bad Request vs.) ekranda doğru renkte (kırmızı) göründüğünden emin ol.

---


## 2026-08-17 (Pazartesi)
**Görev:** Terminal ve Üye İşyeri (Merchant) durum kontrolü (Status Check) mantığının kurulması, arayüz (Interface) uyumsuzluklarının giderilmesi.

**Yaptıklarım:**
- Terminal ve Merchant nesnelerindeki `Status` isimlendirme çakışmasını çözmek için özel `TerminalStatus` ve `MerchantStatus` Enum'ları oluşturuldu ve "Composition" yapısı kullanılarak veriler izole edildi.
- `ITransactionRepository` ve `EfTransactionRepository` içerisindeki metot imzaları (`CheckTerminalStatusValidAsync`, `CheckStatusAsync` vb.) asenkron `Task` ve `Task<bool>` dönüş tiplerine uygun şekilde senkronize edildi.
- API Controller'daki `CheckStatus` metodu, statik enum'lar yerine doğrudan veritabanından `GetMerchantByIdAsync` ve `GetTerminalByNoAsync` ile güncel verileri çekecek şekilde yeniden yazıldı.

**Öğrendiklerim:**
- C#'ta Interface (Arayüz) ve onu implemente eden sınıflar (Class) arasında parametre tiplerinin, isimlerin ve asenkron dönüş tiplerinin (`Task`) birebir aynı olması gerektiği.
- Sadece `Task` dönen asenkron metotların bir değer döndürmek yerine hata (Exception) fırlatarak (örneğin `InvalidOperationException`) kontrol akışını nasıl yönetebileceği.
- Entity Framework Core'da model değişikliklerinin veritabanına nasıl yansıtılacağı ve SQLite'ın yerel geliştirme ortamında nasıl inceleneceği (`sqlite3` komutları).

**Zorlandığım kısım:**
- Başlangıçta Interface ve Repository arasındaki metot imzalarının (özellikle parametre tiplerinin string yerine Enum olması ve asenkron `Task` dönüşleri) tam eşleşmesini sağlamak ve derleme hatalarını çözmek zaman aldı.

**Yarına not:**
- Yazılan durum kontrolü (CheckStatus) endpoint'ini Swagger veya Postman üzerinden farklı senaryolarla (Aktif/Pasif üye işyeri, Bakımdaki terminal vb.) test et.

## 2026-08-14 (Cuma)
**Görev:**  Transaction API'sinde Repository Kalıbı ve Model Doğrulama (Validation) Entegrasyonu

**Yaptıklarım:**
- `ITransactionRepository` ve `EfTransactionRepository` arasındaki arayüz (interface) ve uygulama (implementation) yapısı kuruldu. Controller'ların veritabanı bağlamından (`AppDbContext`) bağımsız çalışması sağlandı.
- Mükerrer Merchant (Üye İşyeri) ve Terminal kayıtlarını engellemek amacıyla, Entity Framework `AnyAsync` metodu kullanılarak performanslı veritabanı varoluş kontrolleri eklendi.
- C# 8.0 ve üzeri sürümlerdeki Nullable Reference Types özelliği ile ortaya çıkan olası null referans hataları (CS8618 vb.) `!` (null-forgiving) operatörü ve model seviyesinde `required` kullanımı ile giderildi.
- Entity Framework Core Migrations kullanılarak veritabanı tamamen sıfırlandı ve güncel model yapılarına (Nullable kurallarına) uygun olarak SQLite üzerinde yeniden inşa edildi.
- Repository sınıfı içerisindeki kullanılmayan veya birebir aynı işlevi gören (örneğin `CreateTerminalAsync` ve `AddTerminalAsync`) gereksiz metotlar temizlenerek kod sadeleştirildi.

**Öğrendiklerim:**
- Arayüz (Interface) metodlarında `async` anahtar kelimesinin bulunmaması gerektiği; bu kelimenin yalnızca metodun gövdesini uygulayan (implement eden) somut sınıf içerisinde yer aldığı.
- Veri varlığını doğrulamak için `FirstOrDefaultAsync` kullanıp dönen sonucun `null` olup olmadığını kontrol etmek yerine, `AnyAsync` metodunu kullanmanın hem kod okunabilirliği hem de sorgu performansı açısından daha verimli olduğu.
- C# modellerinde yapılan değişikliklerin (örneğin bir alana `?` veya `required` eklenmesi) veritabanına yansıması için mutlaka yeni bir Migration oluşturulması (`dotnet ef migrations add`) ve veritabanının güncellenmesi (`dotnet ef database update`) gerektiği.

**Zorlandığım kısım:**
- Controller katmanındaki veri kontrolleri (örneğin Currency'nin null gelme ihtimali) ile Entity Framework'ün veri getirme metodlarının (bulamadığında null dönmesi) neden olduğu derleyici uyarılarını (sarı çizgileri) gidermek ve bu uyarıların mantığını kavramak başlangıçta kafa karıştırıcıydı.

**Yarına not:**
- Yeni bir API ucu eklerken Repository'de gereksiz bir kopya metot oluşturup oluşturmadığıma dikkat etmeliyim.
- Controller ve Repository arasındaki bağımlılık yapısı oturdu; bir sonraki aşamada hata yönetimi (exception handling) ve loglama yapıları üzerine yoğunlaşılabilir.

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
