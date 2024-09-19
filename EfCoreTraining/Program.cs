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
ETicaretContext context = new();
List<Product> products= await context.Products.Where(u => u.ProductId >= 16 && u.ProductId <= 18).ToListAsync();
context.Products.RemoveRange(products);
await context.SaveChangesAsync();

#endregion
public class ETicaretContext : DbContext
{
    public DbSet <Product> Products { get; set; }
  
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=ECE\\MSSQLSERVER02; Initial Catalog=TrainingDb; Integrated Security=True; Connect Timeout=30; Encrypt=True; Trust Server Certificate=True; Application Intent=ReadWrite; Multi Subnet Failover=False");
        }
    }

}



public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } // Ürün adı için yeni özellik
    public decimal Price { get; set; }

}