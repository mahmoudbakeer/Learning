using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperExceptionPage.Controllers;

[ApiController]
[Route("/api/controller")]
public class RequestController : ControllerBase
{
    [HttpGet("server-error")]
    public IActionResult ServerErrorExample()
    {
        System.IO.File.ReadAllText(@"C:\Settings\SomeSettings.json"); // not exist

        return Ok();
    }

    [HttpPost("bad-request")]
    public IActionResult BadRequestExample() =>
        BadRequest(
            new ProblemDetails
            {
                Title = "Product SKU is required",
                Type = "https://something/pleaseignore",
            }
        );

    [HttpPost("not-found")]
    public IActionResult NotFoundExample() =>
        NotFound(
            new ProblemDetails
            {
                Title = "Product Not Found",
                Type = "https://something/pleaseignore",
            }
        );

    [HttpPost("unauthorized")]
    public IActionResult UnauthorizedExample() => Unauthorized();

    [HttpPost("conflict")]
    public IActionResult ConflictExample() =>
        Conflict(
            new ProblemDetails
            {
                Title = "Product already exist",
                Type = "https://something/pleaseignore",
            }
        );

    [HttpPost("business-rule-error")]
    public IActionResult BusinessRuleExample() =>
        throw new ValidationException("A discontinued product cannot be put on promotion.");
}
