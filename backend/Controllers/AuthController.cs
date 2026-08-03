using System.Security.Claims;
using Backend.Auth;
using Backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DemoAuthOptions _demoAuth;

    public AuthController(IOptions<DemoAuthOptions> demoAuth)
    {
        _demoAuth = demoAuth.Value;
    }

    /// <summary>
    /// Demo login. Later: replace with Okta redirect/callback that issues the same role claims.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = _demoAuth.Users.FirstOrDefault(u =>
            string.Equals(u.Username, request.Username, StringComparison.OrdinalIgnoreCase)
            && u.Password == request.Password);

        if (user is null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            });

        return Ok(ToUserDto(user.Username, user.Role));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Signed out." });
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public IActionResult Me()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Ok(new { authenticated = false });
        }

        var username = User.Identity.Name ?? "";
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        return Ok(ToUserDto(username, role));
    }

    private static object ToUserDto(string username, string role) => new
    {
        authenticated = true,
        username,
        role,
        canEditMenu = role == AppRoles.FoodAdmin,
    };
}
