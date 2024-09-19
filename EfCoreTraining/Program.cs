// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

#region SaveChanges'i Verimli Kullanalım
// SaveChanges fonksiyonu her tetiklendiğinde bir transaction oluşturacağından dolayı EF Core ile yapılan her bir
// işlem özel transaction’a eşdeğer olarak veritabanına gönderilir. Bu durum veritabanı açısından ekstradan maliyet
// oluşturabilir. Bu yüzden birden fazla işlem için SaveChanges'i tek seferde kullanmak daha verimli olabilir.
// Yani her bir işlem için ayrı ayrı SaveChanges kullanmak yerine, tüm işlemleri tek bir transaction eşliğinde
// veritabanına göndermek, hem maliyet hem de yönetilebilirlik açısından katkı sağlayacaktır.

ETicaretContext context = new(); // Veri tabanı context'i oluşturuluyor.

Product product1 = new()
{
    ProductName = "A Ürünü",
    Price = 2000
}; // İlk ürün nesnesi oluşturuluyor ve özellikleri atanıyor.

Product product2 = new()
{
    ProductName = "B Ürünü",
    Price = 2000
}; // İkinci ürün nesnesi oluşturuluyor.

Product product3 = new()
{
    ProductName = "C Ürünü",
    Price = 2000
}; // Üçüncü ürün nesnesi oluşturuluyor.

// Veritabanına ürün ekleme işlemleri async olarak yapılıyor. Ancak, her bir ekleme işlemi ardından SaveChanges çağrılmıyor.
//await context.AddAsync(product1);
//await context.AddAsync(product2);
//await context.AddAsync(product3);
await context.Products.AddRangeAsync(product1,product2, product3);

// SaveChangesAsync çağrısı tek seferde, tüm eklenen ürünlerin veritabanına yazılmasını sağlıyor.
await context.SaveChangesAsync();
Console.WriteLine(product3.ProductId);
#endregion

//ETicaretContext context = new();
//Product product = new()
//{
//    ProductName = "Test",
//    Price = 1,
//};
//// await context.AddAsync(product);
// await context.Products.AddAsync(product); // ikisini de kullanabilirsin

//await context.SaveChangesAsync();   
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