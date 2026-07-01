using Microsoft.AspNetCore.Mvc;
using UrlControllerVersioning.Data;
using UrlControllerVersioning.Responses.V1;

namespace UrlControllerVersioning.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/Products")]
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
