// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
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

}