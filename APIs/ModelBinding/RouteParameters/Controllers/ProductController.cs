using Microsoft.AspNetCore.Mvc;

namespace RouteParameters.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    [HttpGet("Products-Controller/{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok($"the product with id {id} is exist.");
    }
}
