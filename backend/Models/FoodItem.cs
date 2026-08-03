namespace Backend.Models;

public class FoodItem
{
    public int FoodItemId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public List<FoodItemMenuDate> MenuDates { get; set; } = [];
}
