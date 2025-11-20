using Microsoft.EntityFrameworkCore;
using UnoApp1.Models;

namespace UnoApp1.Data;

/// <summary>
/// DbContext cho SQLite database, lưu trữ offline trên thiết bị
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<CartItem> CartItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });
    }
}

