using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperExceptionPage.Controllers;

public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult GetError()
    {
        var problem = new ProblemDetails
        {
            Type = "https://example/error/errorpage",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "Unexpected error happened",
            Title = "Internal Server Error",
            Instance = HttpContext.Request.Path,
        };
        return new ObjectResult(problem) { StatusCode = problem.Status };
    }

    [Route("/development-error")]
    public IActionResult GetDevError([FromServices] IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }
        else
        {
            var exceptionhandlerdetails = HttpContext
                .Features.Get<IExceptionHandlerFeature>()!
                .Error;
            var problem = new ProblemDetails
            {
                Type = "https://example/error/errorpage",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = exceptionhandlerdetails.StackTrace,
                Instance = HttpContext.Request.Path,
            };

            return new ObjectResult(problem) { StatusCode = problem.Status };
        }
    }
}
