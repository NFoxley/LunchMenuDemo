namespace Backend.Auth;

/// <summary>
/// Demo-only users. Replace this login path with Okta (or another IdP) later;
/// keep authorizing on role claims (FoodAdmin / FoodStaff).
/// </summary>
public class DemoAuthOptions
{
    public const string SectionName = "DemoAuth";

    public List<DemoUser> Users { get; set; } = [];
}

public class DemoUser
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    /// <summary>FoodAdmin can edit; FoodStaff can view only.</summary>
    public required string Role { get; set; }
}
