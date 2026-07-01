using Microsoft.AspNetCore.Mvc;

namespace Headers.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    // whenever you want to recieve the date from header you have to explicitly add the [FromHeader] Attribute for it
    // and as convension use X-HeaderName to distinguish it
    [HttpGet("Products-Controller")]
    public IActionResult Get([FromHeader(Name = "X-Api-Version")] string Apiversion)
    {
        return Ok($"The api version is {Apiversion}.");
    }
}
