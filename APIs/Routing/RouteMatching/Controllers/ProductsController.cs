using Microsoft.AspNetCore.Mvc;

namespace RouteMatching.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet("all")]
    public IActionResult GetProducts()
    {
        return Ok(new[] { "Product #1", "Product #2" });
    }
}
