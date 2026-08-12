# Halı Yıkama Takip Otomasyonu 🧼

Bu proje, halı yıkama işletmeleri için geliştirilmiş; müşteri kayıtlarını ve sipariş detaylarını tek bir ekrandan yönetmeyi sağlayan bir masaüstü otomasyon sistemidir.

Proje, **C# Windows Forms** kullanılarak tasarlanmış ve arka planda **Microsoft SQL Server** ile ilişkisel veritabanı (Müşteriler ve Siparişler tabloları) mantığına uygun olarak geliştirilmiştir.

## 🚀 Özellikler

* **Müşteri Yönetimi:** Yeni müşteri ekleme, iletişim ve adres bilgilerini kayıt altında tutma.
* **Sipariş Takibi:** Müşteriye özel sipariş oluşturma (Metrekare üzerinden otomatik tutar hesaplama).
* **Dinamik Liste (INNER JOIN):** İki farklı veritabanı tablosundaki verileri (Müşteri ve Sipariş) anlık olarak tek bir DataGridView üzerinde görüntüleme.
* **CRUD İşlemleri:** Kayıt Ekleme, Güncelleme ve ilişkisel kurallara uygun Silme işlemleri.
* **Karanlık Tema Arayüzü:** Göz yormayan, kullanıcı dostu ve modern form tasarımı.

## 💻 Kullanılan Teknolojiler

* **Dil:** C#
* **Platform:** .NET Framework (Windows Forms)
* **Veritabanı:** MS SQL Server (ADO.NET)
* **Geliştirme Ortamı:** Visual Studio 2022

## 🛠️ Kurulum ve Çalıştırma

Projeyi kendi bilgisayarınızda çalıştırmak için şu adımları izleyebilirsiniz:

1. Bu depoyu bilgisayarınıza klonlayın veya `.zip` olarak indirin.
2. Klasör içerisinde bulunan `HaliYikamaDB.sql` script dosyasını SQL Server Management Studio (SSMS) üzerinde açıp çalıştırarak (Execute) veritabanını ve test verilerini oluşturun.
3. Visual Studio üzerinden projeyi açın.
4. `Form1.cs` kod satırlarında bulunan `baglantiCumlesi` değişkenindeki `Data Source=localhost` kısmını kendi SQL Server sunucu adınıza göre güncelleyin.
5. Projeyi başlatın.

---
**Geliştirici:** Ömer Asaf Cenneter
