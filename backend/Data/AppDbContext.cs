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
}