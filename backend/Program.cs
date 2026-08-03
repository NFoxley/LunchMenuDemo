using Backend.Auth;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DemoAuthOptions>(
    builder.Configuration.GetSection(DemoAuthOptions.SectionName));

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "LunchMenu.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPolicies.CanEditMenu, policy =>
        policy.RequireRole(AppRoles.FoodAdmin));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Vue", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors("Vue");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();

    if (!db.FoodItems.Any())
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        db.FoodItems.Add(new FoodItem
        {
            Name = "Pizza",
            Description = "Cheesy, cheesy goodness.",
            ImageUrl = "/images/pizza_1.jpg",
            MenuDates =
            [
                new FoodItemMenuDate { Date = today },
                new FoodItemMenuDate { Date = today.AddDays(7) },
            ],
        });
        db.FoodItems.Add(new FoodItem
        {
            Name = "Chicken Tikka Masala",
            Description = "Tomato base, many spices, adjustable spiciness.",
            ImageUrl = "/images/chicken_tikka_masala_1.webp",
            MenuDates =
            [
                new FoodItemMenuDate { Date = today },
            ],
        });
    }
    else if (!db.FoodItemMenuDates.Any())
    {
        // Existing DBs created before menu dates: schedule current items for today.
        var today = DateOnly.FromDateTime(DateTime.Today);
        foreach (var item in db.FoodItems.ToList())
        {
            db.FoodItemMenuDates.Add(new FoodItemMenuDate
            {
                FoodItemId = item.FoodItemId,
                Date = today,
            });
        }
    }

    db.SaveChanges();
}

app.Run();
