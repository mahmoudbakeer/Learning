using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperExceptionPage.Controllers;

public class ErrorController : ControllerBase
{
    [Route("error")]
    public IActionResult GetError() =>
        new ObjectResult(new { StatusCode = 500, Message = "Internal Server Error" });

    [Route("development-error")]
    public IActionResult GetDevError([FromServices] IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }
        else
        {
            var exceptionhandlerdetails = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return new ObjectResult(
                new
                {
                    details = exceptionhandlerdetails.Error.StackTrace,
                    exceptionhandlerdetails.Error.Message,
                }
            );
        }
    }
}
