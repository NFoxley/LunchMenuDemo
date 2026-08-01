using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/fooditem")]
public class FoodItemController : ControllerBase
{
    private readonly AppDbContext _db;

    public FoodItemController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var item = await _db.FoodItems.ToListAsync();

        return Ok(item);
    }
}