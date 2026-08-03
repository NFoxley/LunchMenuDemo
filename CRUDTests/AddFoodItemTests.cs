using Backend.Controllers;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace CRUDTests;

public class AddFoodItemTests
{
    /// <summary>
    /// Builds a throwaway in-memory database so tests never touch lunchmenu.db.
    /// A unique name per call keeps tests isolated from each other.
    /// </summary>
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static TestWebHostEnvironment CreateEnv() => new();

    [Fact]
    public async Task AddFoodItem_Should_Return_Created()
    {
        // Arrange — set up dependencies and input (no real HTTP or SQLite)
        await using var db = CreateDbContext();
        var env = CreateEnv();
        var controller = new FoodItemController(db, env);

        await using var imageStream = new MemoryStream("fake-image"u8.ToArray());
        var image = new FormFile(imageStream, 0, imageStream.Length, "Image", "tomato_soup.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg",
        };

        var request = new CreateFoodItemRequest
        {
            Name = "  Tomato Soup  ",
            Description = "A warm lunch option.",
            MenuDate = new DateOnly(2026, 8, 2),
            Image = image,
        };

        // Act — call the same Create method the API uses
        var result = await controller.Create(request);

        // Assert — check the HTTP-style result
        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(FoodItemController.Get), created.ActionName);

        // Create returns an anonymous projection; inspect via JSON-like dynamic dictionary
        var value = created.Value!;
        var name = (string)value.GetType().GetProperty("Name")!.GetValue(value)!;
        var description = (string?)value.GetType().GetProperty("Description")!.GetValue(value);
        var imageUrl = (string?)value.GetType().GetProperty("ImageUrl")!.GetValue(value);
        var foodItemId = (int)value.GetType().GetProperty("FoodItemId")!.GetValue(value)!;
        var menuDates = (List<DateOnly>)value.GetType().GetProperty("MenuDates")!.GetValue(value)!;

        Assert.Equal("Tomato Soup", name); // whitespace was trimmed
        Assert.Equal("A warm lunch option.", description);
        Assert.StartsWith("/images/", imageUrl);
        Assert.EndsWith(".jpg", imageUrl);
        Assert.True(foodItemId > 0);
        Assert.Equal([new DateOnly(2026, 8, 2)], menuDates);

        // Assert — verify it was actually saved with its menu date
        var saved = await db.FoodItems.Include(f => f.MenuDates).SingleAsync();
        Assert.Equal(foodItemId, saved.FoodItemId);
        Assert.Equal("Tomato Soup", saved.Name);
        Assert.Equal(new DateOnly(2026, 8, 2), Assert.Single(saved.MenuDates).Date);

        // Assert — image file landed under wwwroot/images
        var savedFileName = Path.GetFileName(imageUrl);
        Assert.True(File.Exists(Path.Combine(env.WebRootPath, "images", savedFileName!)));
    }

    [Fact]
    public async Task AddFoodItem_Should_Reject_Invalid_Image_Type()
    {
        await using var db = CreateDbContext();
        var controller = new FoodItemController(db, CreateEnv());

        await using var imageStream = new MemoryStream("not-an-image"u8.ToArray());
        var image = new FormFile(imageStream, 0, imageStream.Length, "Image", "notes.gif")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/gif",
        };

        var request = new CreateFoodItemRequest
        {
            Name = "Bad Image Dish",
            MenuDate = new DateOnly(2026, 8, 2),
            Image = image,
        };

        var result = await controller.Create(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Empty(db.FoodItems);
        Assert.NotNull(badRequest.Value);
    }

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public TestWebHostEnvironment()
        {
            ContentRootPath = Path.Combine(Path.GetTempPath(), "lunchmenu-tests", Guid.NewGuid().ToString("N"));
            WebRootPath = Path.Combine(ContentRootPath, "wwwroot");
            Directory.CreateDirectory(WebRootPath);
        }

        public string ApplicationName { get; set; } = "Backend";
        public string EnvironmentName { get; set; } = "Development";
        public string ContentRootPath { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    }
}
