using Microsoft.AspNetCore.Mvc;
using RestfulApi.Filters;

namespace RestfulApi.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(new { Name = "Mahmoud", Price = 29.99m });
    }
}
