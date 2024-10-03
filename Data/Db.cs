using Microsoft.EntityFrameworkCore;
using FrietSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FrietSite.Data
{
    public class Db : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }  // Nieuw toegevoegd
        public DbSet<IdentityUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = @"Data Source=Alex;Initial Catalog=Friettest8.0;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Menu
            modelBuilder.Entity<Menu>().Property(m => m.Title).HasMaxLength(50);
            Menu voorbeeldEntity = new Menu() { Id = 1, Title = "menu" };
            modelBuilder.Entity<Menu>().HasData(voorbeeldEntity);

            // Configure Category (optioneel: seed data)
            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(50);
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Friet" },
                new Category { CategoryId = 2, Name = "Snacks" },
                new Category { CategoryId = 3, Name = "Drankjes" }
            );

            // Configure Product
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50);
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Friet Groot", Price = 2.50m, CategoryId = 1 },
                new Product { Id = 2, Name = "Kroket", Price = 3.50m, CategoryId = 2 }
            );

            // Relatie tussen Product en Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Client seed data
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "John Doe", Password = "password123" },
                new Client { Id = 2, Name = "Jane Smith", Password = "mypassword" },
                new Client { Id = 3, Name = "Alice Johnson", Password = "alicepassword" }
            );

            // Order
            modelBuilder.Entity<Order>().Property(o => o.Description).HasMaxLength(100);
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, Description = "Bestelling 1" }
            );

            // Many-to-Many relatie tussen Client en Order
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Orders)
                .WithMany(o => o.Clients);

            // Many-to-Many relatie tussen Order en Product
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProduct",
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                    j => j.HasOne<Order>().WithMany().HasForeignKey("OrderId"),
                    j =>
                    {
                        j.HasKey("OrderId", "ProductId");

                        j.HasData(
                            new { OrderId = 1, ProductId = 1 },
                            new { OrderId = 1, ProductId = 2 }
                        );
                    }
                );
        }
    }
}
