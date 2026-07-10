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
    public IActionResult BadRequestExample() => BadRequest("Product SKU is required");

    [HttpPost("not-found")]
    public IActionResult NotFoundExample() => NotFound("Product not found.");

    [HttpPost("unauthorized")]
    public IActionResult UnauthorizedExample() => Unauthorized();

    [HttpPost("conflict")]
    public IActionResult ConflictExample() => Conflict("This Product already exists.");

    [HttpPost("business-rule-error")]
    public IActionResult BusinessRuleExample() =>
        throw new ValidationException("A discontinued product cannot be put on promotion.");
}
