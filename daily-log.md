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
**Görev:** Interface ekleme ve Swagger aktif etme

**Yaptıklarım:**
- `ITransactionRepository` interface'i yazdım
- `TransactionRepository` class'ını implement ettim
- `TransactionsController` ile endpoint'leri bağladım
- .NET 10'da Swagger paket çakışması sorununu çözdüm (Microsoft.OpenApi vs Swashbuckle)
- GitHub'da divergent branch / merge conflict sorununu çözdüm

**Öğrendiklerim:**
- Interface = sözleşme (contract), class = gerçek implementasyon
- Dependency Injection'ın temel mantığı (`AddSingleton<Interface, Class>`)
- Controller'ın interface/class ile Swagger arasındaki köprü olduğu
- `git pull --no-rebase` ile diverged branch'leri birleştirme

**Zorlandığım kısım:**
- Swagger sayfası 404 dönüyordu — sebebi iki farklı OpenAPI paketinin (`Microsoft.AspNetCore.OpenApi` ve `Swashbuckle.AspNetCore`) aynı anda kurulu olup çakışmasıymış

**Yarına not:**
-

---
