using Microsoft.AspNetCore.Mvc;

namespace Cookies.Controllers;

[ApiController]
[Route("Prefrences")]
public class PrefrencesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(
            new
            {
                theme = HttpContext.Request.Cookies["theme"],
                language = HttpContext.Request.Cookies["language"],
            }
        );
    }
}
