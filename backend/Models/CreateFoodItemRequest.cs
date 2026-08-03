using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Backend.Models;

public class CreateFoodItemRequest
{
    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    /// <summary>Date the dish will be served (no time).</summary>
    [Required]
    public DateOnly MenuDate { get; set; }

    /// <summary>Optional image upload (.jpg, .png, .webp).</summary>
    public IFormFile? Image { get; set; }
}
