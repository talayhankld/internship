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
