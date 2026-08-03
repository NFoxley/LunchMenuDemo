using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<MenuMessage> MenuMessages => Set<MenuMessage>();
    public DbSet<FoodItem> FoodItems => Set<FoodItem>();
    public DbSet<FoodItemMenuDate> FoodItemMenuDates => Set<FoodItemMenuDate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodItemMenuDate>(entity =>
        {
            entity.HasIndex(d => new { d.FoodItemId, d.Date }).IsUnique();

            entity.HasOne(d => d.FoodItem)
                .WithMany(f => f.MenuDates)
                .HasForeignKey(d => d.FoodItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
