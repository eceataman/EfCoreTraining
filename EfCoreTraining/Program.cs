// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

#region SaveChanges'i Verimli Kullanalım
// SaveChanges fonksiyonu her tetiklendiğinde bir transaction oluşturacağından dolayı EF Core ile yapılan her bir
// işlem özel transaction’a eşdeğer olarak veritabanına gönderilir. Bu durum veritabanı açısından ekstradan maliyet
// oluşturabilir. Bu yüzden birden fazla işlem için SaveChanges'i tek seferde kullanmak daha verimli olabilir.
// Yani her bir işlem için ayrı ayrı SaveChanges kullanmak yerine, tüm işlemleri tek bir transaction eşliğinde
// veritabanına göndermek, hem maliyet hem de yönetilebilirlik açısından katkı sağlayacaktır.

//ETicaretContext context = new(); // Veri tabanı context'i oluşturuluyor.

//Product product1 = new()
//{
//    ProductName = "A Ürünü",
//    Price = 2000
//}; // İlk ürün nesnesi oluşturuluyor ve özellikleri atanıyor.

//Product product2 = new()
//{
//    ProductName = "B Ürünü",
//    Price = 2000
//}; // İkinci ürün nesnesi oluşturuluyor.

//Product product3 = new()
//{
//    ProductName = "C Ürünü",
//    Price = 2000
//}; // Üçüncü ürün nesnesi oluşturuluyor.

// Veritabanına ürün ekleme işlemleri async olarak yapılıyor. Ancak, her bir ekleme işlemi ardından SaveChanges çağrılmıyor.
//await context.AddAsync(product1);
//await context.AddAsync(product2);
//await context.AddAsync(product3);
//await context.Products.AddRangeAsync(product1,product2, product3);

// SaveChangesAsync çağrısı tek seferde, tüm eklenen ürünlerin veritabanına yazılmasını sağlıyor.
//await context.SaveChangesAsync();
//Console.WriteLine(product3.ProductId);
#endregion

#region veri ekleme
//ETicaretContext context = new();
//Product product = new()
//{
//    ProductName = "Test",
//    Price = 1,
//};
//// await context.AddAsync(product);
// await context.Products.AddAsync(product); // ikisini de kullanabilirsin

//await context.SaveChangesAsync();   

#endregion

#region Veri nasıl güncellenir?
//ETicaretContext context = new();
//Product product = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 13);
//product.ProductName = "Hakan";
//product.Price = 999;

//await context.SaveChangesAsync();

#endregion


#region ChangeTracker nedir,kısaca açıkla
//ChangeTracker context üzeribden gelen verilerin takibinden sorumludur. Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler
//neticesinde update yahut delete sorgularının yapılacagı anlasılır.

#endregion

#region Takip edilmeyen veirler nasıl güncellenir
//ETicaretContext context = new();
//Product product = new()
//{
//    ProductId =13,
//    ProductName = "Test",
//    Price=1
//};
//context.Products.Update(product);
//await context.SaveChangesAsync();
//changetracker tarafından takip edilmeyenler için update mekanizması kullanılır.

#endregion

#region EntityState nedir?
//ETicaretContext context = new();
//Product u = new();
//Console.WriteLine(context.Entry(u).State);
//çıktı detached oldu
#endregion

#region EFCore açısından bir verinin güncellenmesi gerektigi nasıl anlasılıyor?
//ETicaretContext context = new();
//Product product = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 13);
//Console.WriteLine(context.Entry(product).State);
////çıktı unchanged

//product.ProductName = "Hakan";
//Console.WriteLine(context.Entry(product).State);

////çıktı modified değişikliğe hazır.

//await context.SaveChangesAsync();
//Console.WriteLine(context.Entry(product).State);

#endregion
#region Birden fazla veri güncellenirken nelere dikkat edilmelidir?
//ETicaretContext context = new();
//var products = await context.Products.ToListAsync();
//foreach(var product in products)
//{
//    product.ProductName = "*";
//    //yani buraya savechangeasync yazııomamalı tek bir transactionda yap ki maliyeti fazla olmasın
//}
//await context.SaveChangesAsync();
#endregion

#region veri nasıl silinmeli?
//ETicaretContext context = new();
//Product product= await context.Products.FirstOrDefaultAsync(u => u.ProductId == 13);
//context.Products.Remove(product);
//await context.SaveChangesAsync();

#endregion

#region takip edilmeyen nesneler nasıl siliinir?
//ETicaretContext context = new();
//Product u = new();
//{
//    u.ProductId = 12;
//}
//context.Products.Remove(u);
//await context.SaveChangesAsync();
#endregion

#region EntityState ile silme işlemi
//ETicaretContext context = new();
//Product u = new()
//{
//    ProductId = 14
//};
//context.Entry(u).State = EntityState.Deleted;
//await context.SaveChangesAsync();
#endregion 


#region Birden fazla veri silerken ne yapılmalıdır=
//ETicaretContext context = new();
//var products = await context.Products.ToListAsync();
//foreach(var product in products) { 
//    context.Products.Remove(product);
//}
//await context.SaveChangesAsync();
#endregion

#region Verileri daha verimli bir şekilde silme: RemoveRange kullanarak
//ETicaretContext context = new();
//List<Product> products= await context.Products.Where(u => u.ProductId >= 16 && u.ProductId <= 18).ToListAsync();
//context.Products.RemoveRange(products);
//await context.SaveChangesAsync();

#endregion

// IQueryable ve IEnumerable Nedir? Basit Olarak!

ETicaretContext context = new();



#region IQueryable
// Sorguya karşılık gelir.
// C# core üzerinden yapılmış olan sorgunun execute edilmemiş halini ifade eder.
#endregion

#region IEnumerable
// Sorgunun çalıştırılıp/execute edilip verilerin in memory'e yüklenmiş halini ifade eder.
#endregion



#region method syntax
//var products = await context.Products.ToListAsync();
#endregion

#region query syntax
//var product2= await (from product in  context.Products select product).ToListAsync();
#endregion

#region Sorguyu Execute Etmek İçin Ne Yapmamız Gerekmektedir?
// ToListAsync()

//int Productid = 5;
//string pName = "2";

//var products = from product in context.Products
//              where product.ProductId > Productid && product.ProductName.Contains(pName)
//              select product;

//Productid = 200;
//pName = "3";

//// Sorgu bu noktada execute edilir
//foreach (var product in products)
//{
//    Console.WriteLine(product.ProductId);
//}

//await products.ToListAsync();
#endregion

#region Foreach
// foreach (var urun in urunler)
// {
//    Console.WriteLine(urun.UrunId);
// }
#endregion

#region Deferred Execution (Ertelenmiş Çalışma)
// IQueryable çalışmasında ilgili kod yazıldığı noktada tetiklenmez/çalıştırılmaz yani
// ilgili kod yazıldığı noktada sorguyu generate etmez. Nerede eder? Çalıştırıldığı/execute
// edildiği noktada tetiklenir, işte bu durumda ertelenmiş çalışma denir.
#endregion

#region ToListAsynnc
//üretilen sorguyu execute etmemize yarayan fonksiyondur. iquerayabledan ienumerable a geçmsini sağlar.

#endregion

#region where
//oluşturulan sorguya where şartı atanır
//var products=await context.Products.Where(u=>u.ProductId>500).ToListAsync();
//var products2 = await context.Products.Where(u => u.ProductName.StartsWith("a")).ToListAsync();
#endregion

#region querysyntax
//var products= from product in context.Products
//              where product.ProductId > 500 && product.ProductName.EndsWith("7")
//              select product;

//var data = await products.ToListAsync();
#endregion
#region Çoğul Veri Getiren Sorgulama Fonksiyonları
// ToListAsync()
#endregion

#region OrderBy
// Sorgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur. (Ascending) artarak
#endregion

#region Method Syntax
//var products = context.Products
//    .Where(u => u.ProductId > 500 || u.ProductName.EndsWith("2"))
//    .OrderBy(u => u.ProductName);
#endregion

#region Query Syntax
//var products2 = from product in context.Products
//               where product.ProductId > 500 || product.ProductName.StartsWith("2")
//               orderby product.ProductName
//               select product;
#endregion

#region ThenBy
// orderby üzerinde yapılan ssıralama işlemlerini farklı kolonlara da uygulamamızı sğalar. (Ascending)
//var products = context.Products
//    .Where(u => u.ProductId > 500 || u.ProductName.EndsWith("2"))
//    .OrderBy(u => u.ProductName)       // Öncelikle UrunAdi'na göre sıralama
//    .ThenBy(u => u.Price)          // Ardından Fiyat'a göre sıralama
//    .ThenBy(u => u.ProductId);            // Son olarak Id'ye göre sıralama

//await products.ToListAsync();

#endregion
#region OrderByDescending
// Descending olarak sıralama yapmamızı sağlayan bir fonksiyondur.
#endregion

#region Method Syntax
//var products = await context.Products
//    .OrderByDescending(u => u.Price)
//    .ToListAsync();
#endregion

#region Query Syntax
//var products = await (from product in context.Products
//                     orderby product.ProductName descending
//                     select product).ToListAsync();
#endregion

#region ThenByDescending
// OrderByDescending üzerinde yapılan sıralama işlemini farklı kolonlarda uygulamamızı sağlayan bir fonksiyondur. (Ascending)

//var products = await context.Products
//    .OrderByDescending(u => u.ProductId)            // İlk olarak Id'ye göre azalan sırada sıralanır
//    .ThenByDescending(u => u.Price)          // Sonra Fiyat'a göre azalan sırada sıralanır
//    .ThenBy(u => u.ProductName)                  // En son UrunAdi'na göre artan sırada sıralanır
//    .ToListAsync();

#endregion
#region Tekil Veri Getiren Sorgulama Fonksiyonları
// Yapılan sorgularda sadece tek bir verinin gelmesi amaçlanıyorsa Single ya da SingleOrDefault fonksiyonları kullanılabilir.

#region SingleAsync
// Eğer ki, sorgu neticesinde birden fazla veri geliyorsa ya da hiç gelmiyorsa her iki durumda da exception fırlatır.
//var urun = await context.Urunler.SingleAsync(u => u.Id == 55);
#endregion

#region Hiç Kayıt Gelmediğinde
// var urun = await context.Urunler.SingleAsync(u => u.Id == 5555);
#endregion

#region Çok Kayıt Geldiğinde
// var urun = await context.Urunler.SingleAsync(u => u.Id == 55);
#endregion
#endregion

#region SingleOrDefaultAsync
// Eğer ki, sorgu neticesinde birden fazla veri geliyorsa exception fırlatır, hiç veri gelmiyorsa null döner.
//var urun = await context.Urunler.SingleOrDefaultAsync(u => u.Id == 55);
#endregion

#region Hiç Kayıt Gelmediğinde
// var urun = await context.Urunler.SingleOrDefaultAsync(u => u.Id == 5555);
#endregion

#region Çok Kayıt Geldiğinde
// var urun = await context.Urunler.SingleOrDefaultAsync(u => u.Id == 55);
#endregion
#region Tekil Veri Getiren Sorgulama Fonksiyonları
// Yapılan sorgularda sadece tek bir verinin gelmesi amaçlanıyorsa Single ya da SingleOrDefault fonksiyonları kullanılabilir.

#region SingleAsync
// Eğer ki, sorgu neticesinde birden fazla veri geliyorsa ya da hiç gelmiyorsa her iki durumda da exception fırlatır.
// var urun = await context.Urunler.SingleAsync(u => u.Id == 55);
#endregion

#region Hiç Kayıt Gelmediğinde
// var urun = await context.Urunler.SingleAsync(u => u.Id == 5555);
#endregion

#region Çok Kayıt Geldiğinde
// var urun = await context.Urunler.SingleAsync(u => u.Id == 55);
#endregion
#endregion

#region SingleOrDefaultAsync
// Eğer ki, sorgu neticesinde birden fazla veri geliyorsa exception fırlatır, hiç veri gelmiyorsa null döner.
// var urun = await context.Urunler.SingleOrDefaultAsync(u => u.Id == 55);
#endregion

#region Hiç Kayıt Gelmediğinde
// var urun = await context.Urunler.SingleOrDefaultAsync(u => u.Id == 5555);
#endregion

#region Çok Kayıt Geldiğinde
// var urun = await context.Urunler.SingleOrDefaultAsync(u => u.Id == 55);
#endregion

#region Tekil Veri Getiren Sorgulama Fonksiyonları
// Yapılan sorgularda tek bir verinin gelmesi amaçlanıyorsa First ya da FirstOrDefault fonksiyonları kullanılabilir.

#region FirstAsync
// Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa hata fırlatır.
#region Tek Kayıt Geldiğinde
// var urun = await context.Urunler.FirstAsync(u => u.Id == 55);
#endregion

#region Hiç Kayıt Gelmediğinde
// var urun = await context.Urunler.FirstAsync(u => u.Id == 5555);
#endregion

#region Çok Kayıt Geldiğinde
// var urun = await context.Urunler.FirstAsync(u => u.Id > 55);
#endregion
#endregion

#region FirstOrDefaultAsync
// Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa null değerini döndürür.
#endregion
#endregion

// SingleAsync, SingleOrDefaultAsync, FirstAsync, FirstOrDefaultAsync Comparison

#region FindAsync
// The Find function is a special function that allows you to query quickly using the primary key.
// Example:
// var product = await context.Products.FirstOrDefaultAsync(p => p.Id == 55);
//var product = await context.Products.FindAsync(15);
//Console.WriteLine();
#endregion

// Eksik olan #endregion'ları bu şekilde kapattığınızdan emin olun.

#region LastAsync
// Returns the last result from the query. If no record is found, it throws an exception. 
// Using OrderBy is mandatory.
// Example:
// var product = await context.Products.OrderBy(u => u.Price).LastAsync(u => u.Id > 55);
#endregion

#region LastOrDefaultAsync
// Returns the last result from the query. Using OrderBy is mandatory.
// If no record is found, it returns null.
#endregion

#region Other Query Functions
#region CountAsync
#endregion


public class ETicaretContext : DbContext
{
    public DbSet <Product> Products { get; set; }
    public DbSet <Part> Parts { get; set; }
    public DbSet <ProductPart> ProductParts { get; set; }
  
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=ECE\\MSSQLSERVER02; Initial Catalog=TrainingDb; Integrated Security=True; Connect Timeout=30; Encrypt=True; Trust Server Certificate=True; Application Intent=ReadWrite; Multi Subnet Failover=False");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductPart>().HasKey(up => new {up.ProductId, up.PartId });
    }


}



public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } // Ürün adı için yeni özellik
    public decimal Price { get; set; }

}
public class Part
{
    public int PartId { get; set; }
    public string PartName { get; set; }
}

public class ProductPart
{
    public int ProductId { get; set; }
    public int PartId { get; set; }

    public Product Product { get; set; }
    public Part Part { get; set; }
}
