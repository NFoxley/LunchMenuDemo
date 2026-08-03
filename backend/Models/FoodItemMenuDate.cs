namespace Backend.Models;

/// <summary>
/// A calendar date when a dish is served (date only, no time).
/// </summary>
public class FoodItemMenuDate
{
    public int FoodItemMenuDateId { get; set; }
    public int FoodItemId { get; set; }
    public DateOnly Date { get; set; }

    public FoodItem FoodItem { get; set; } = null!;
}
