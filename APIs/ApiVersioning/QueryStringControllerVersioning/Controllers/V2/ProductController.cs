using Microsoft.AspNetCore.Mvc;
using UrlControllerVersioning.Data;
using UrlControllerVersioning.Responses.V2;

namespace UrlControllerVersioning.Controllers.V2;

[ApiController]
[Route("api/products")]
[ApiVersion("2.0")]
public class ProductController(ProductRepository pr) : ControllerBase
{
    [HttpGet("{Id:int}")]
    public IActionResult GetProductById(int Id)
    {
        var product = pr.GetProductById(Id);

        if (product is null)
            return NotFound();

        return Ok(ProductResponse.FromModel(product));
    }
}
