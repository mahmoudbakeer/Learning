using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Body.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    [HttpPost("Products-Controller")]
    public IActionResult Post([FromBody] ProductRequest productRequest)
    {
        return Ok(productRequest);
    }
}
