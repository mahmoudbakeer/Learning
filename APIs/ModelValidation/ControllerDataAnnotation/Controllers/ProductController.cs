using ControllerDataAnnotation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ControllerDataAnnotation.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpPost]
    public IActionResult Createproduct([FromBody] ProductRequest productRequest)
    {
        return Created($"api/products/{Guid.NewGuid}", productRequest);
    }
}
