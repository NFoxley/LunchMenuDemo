using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("Vue", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("Vue");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // serves files from wwwroot at URLs like /images/...
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Applies pending migrations (creates/updates tables). Prefer this over EnsureCreated().
    db.Database.Migrate();

    if (!db.MenuMessages.Any())
    {
        db.MenuMessages.Add(new MenuMessage
        {
            Message = "Hello from SQLite!"
        });
    }

    if (!db.FoodItems.Any())
    {
        db.FoodItems.Add(new FoodItem
        {
            Name = "Pizza",
            Description = "Cheesy, cheesy goodness.",
            ImageUrl = "/images/pizza_1.jpg"
        });
        db.FoodItems.Add(new FoodItem
        {
            Name = "Chicken Tikka Masala",
            Description = "Tomato base, many spices, adjustable spiciness.",
            ImageUrl = "/images/chicken_tikka_masala_1.webp"
        });
    }

    db.SaveChanges();
}

app.Run();
