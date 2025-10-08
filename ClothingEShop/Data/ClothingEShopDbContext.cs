using Microsoft.EntityFrameworkCore;
using ClothingEShop.Models;

namespace ClothingEShop.Data;

public class ClothingEShopDbContext : DbContext
{
    public ClothingEShopDbContext(DbContextOptions<ClothingEShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Seed data
        modelBuilder.Entity<Product>().HasData(
            new Product 
            { 
                Id = 1, 
                Name = "Classic White T-Shirt", 
                Description = "Comfortable cotton t-shirt perfect for everyday wear", 
                Price = 19.99m, 
                ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product 
            { 
                Id = 2, 
                Name = "Blue Denim Jeans", 
                Description = "Classic fit denim jeans with premium quality fabric", 
                Price = 59.99m, 
                ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=400",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product 
            { 
                Id = 3, 
                Name = "Black Hoodie", 
                Description = "Warm and cozy hoodie perfect for cooler weather", 
                Price = 39.99m, 
                ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=400",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}