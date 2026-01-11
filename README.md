\# Yazılım Mimarileri ve Uygulama Geliştirme – Final Ödevi



\# Öğrenci Bilgileri

\- Ad Soyad: Batuhan Işık	

\- Öğrenci No: 210101023



--



\## Proje Tanımı

Bu proje, Yazılım Mimarileri ve Uygulama Geliştirme dersi kapsamında geliştirilmiş bir .NET 9 Minimal API uygulamasıdır.  

Katmanlı mimari kullanılarak kullanıcı, ürün ve sipariş yönetimi yapan bir backend sistemi oluşturulmuştur.



--



\## Kullanılan Teknolojiler

\- .NET 9

\- ASP.NET Core Minimal API

\- Entity Framework Core

\- SQLite

\- Swagger



--



\## Mimari Yapı

Proje aşağıdaki katmanlardan oluşmaktadır:

\- \*\*Api\*\*: Endpointler, hata yönetimi, Swagger

\- \*\*Application\*\*: İş kuralları, DTO’lar, servisler

\- \*\*Domain\*\*: Entity sınıfları

\- \*\*Infrastructure\*\*: Veritabanı ve migration işlemleri



--



\## Varlıklar ve İlişkiler

Projede aşağıdaki entity’ler bulunmaktadır:

\- User

\- Product

\- Order

\- OrderItem



İlişkiler:

\- User → Order (1-N)

\- Order → OrderItem (1-N)

\- Product → OrderItem (1-N)



--



\## API Özellikleri

\- Kullanıcı, ürün, sipariş ve sipariş kalemi için CRUD işlemleri

\- DTO kullanımı

\- Global hata yönetimi

\- Standart API response yapısı

\- Uygun HTTP status code kullanımı (400, 404, 409, 500)



--



\## Uygulamanın Çalıştırılması

```bash

dotnet restore

dotnet build

dotnet ef database update

dotnet run --project MyApp.Api



