using Backend.Auth;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/fooditem")]
public class FoodItemController : ControllerBase
{
    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public FoodItemController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] DateOnly? date = null)
    {
        var query = _db.FoodItems
            .AsNoTracking()
            .Include(f => f.MenuDates)
            .AsQueryable();

        if (date is not null)
        {
            query = query.Where(f => f.MenuDates.Any(d => d.Date == date.Value));
        }

        var foods = await query
            .OrderBy(f => f.Name)
            .ToListAsync();

        var items = foods.Select(f => new
        {
            f.FoodItemId,
            f.Name,
            f.Description,
            f.ImageUrl,
            MenuDates = f.MenuDates
                .OrderBy(d => d.Date)
                .Select(d => d.Date)
                .ToList(),
        });

        return Ok(items);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.CanEditMenu)]
    [RequestSizeLimit(5_000_000)]
    public async Task<IActionResult> Create([FromForm] CreateFoodItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Name is required." });
        }

        string? imageUrl = null;
        if (request.Image is { Length: > 0 })
        {
            var saved = await SaveImageAsync(request.Image);
            if (saved.Error is not null)
            {
                return BadRequest(new { message = saved.Error });
            }

            imageUrl = saved.Url;
        }

        var item = new FoodItem
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            ImageUrl = imageUrl,
            MenuDates =
            [
                new FoodItemMenuDate { Date = request.MenuDate }
            ],
        };

        _db.FoodItems.Add(item);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = item.FoodItemId }, new
        {
            item.FoodItemId,
            item.Name,
            item.Description,
            item.ImageUrl,
            MenuDates = item.MenuDates.Select(d => d.Date).ToList(),
        });
    }

    private async Task<(string? Url, string? Error)> SaveImageAsync(IFormFile image)
    {
        var extension = Path.GetExtension(image.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            return (null, "Image must be a .jpg, .png, or .webp file.");
        }

        var contentType = image.ContentType?.ToLowerInvariant() ?? "";
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(contentType))
        {
            return (null, "Image must be a .jpg, .png, or .webp file.");
        }

        var imagesPath = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "images");
        Directory.CreateDirectory(imagesPath);

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var fullPath = Path.Combine(imagesPath, fileName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await image.CopyToAsync(stream);
        }

        return ($"/images/{fileName}", null);
    }
}
