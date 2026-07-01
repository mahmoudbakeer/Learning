using Microsoft.AspNetCore.Mvc;

namespace ControllerIntroduction.Controllers;

/// <summary>
/// CONTROLLER-BASED APIs: What, When, and Why
///
/// What: An architectural pattern that groups related HTTP endpoints (actions) into a single C# class.
///
/// When to use:
/// - In medium-to-large scale applications where structured, predictable organization is required.
/// - When you want built-in support for features like action filters, model state validation, and authorization at a class level.
///
/// Why to use:
/// - Separation of Concerns: Keeps operations for a single domain (like Products) cleanly isolated in one place.
/// - Maintainability: Easy for large teams to navigate and test because endpoints are structured around resource controllers.
/// </summary>
/// <summary>
/// THE [ApiController] ATTRIBUTE: What and Why
///
/// What: A class-level attribute that enables API-specific behaviors and conventions for controllers.
///
/// Why we use it (Automatic Features):
/// 1. Enforced Attribute Routing: Requires you to use route attributes (like [Route]) instead of relying on default convention-based startup routing.
/// 2. Automatic HTTP 400 Bad Request: Automatically checks model state validation (e.g., [Required] fields). If validation fails, it immediately returns a 400 response, eliminating the need to write "if (!ModelState.IsValid)" in your actions.
/// 3. Binding Source Inference: Intelligently guesses where parameters come from:
///    - Complex types (like custom objects/DTOs) default to parsing from the request Body ([FromBody]).
///    - Form files (IFormFile) default to parsing from [FromForm].
///    - Primitives (string, int, double) default to parsing from Route/Query string ([FromRoute] or [FromQuery]).
/// 4. Problem Details Specification: Formats error responses using the standardized RFC 7807 Problem Details template.
/// </summary>
[ApiController]
// The main route will be shared across all the actions in the controller
[Route("Api/Products")]
public class ProductController : ControllerBase
{
    // Specific additional route on top of the shared route
    [HttpGet(Name = "blabla")]
    // Action 'endpoints' must be public, non-static instance methods.
    //
    // THE DIFFERENCE BETWEEN RETURN TYPES:
    //
    // 1. IActionResult (Interface)
    // - What: An interface representing the result of an action method.
    // - Pros: Ultimate runtime flexibility. You can return any HTTP status helper (e.g., Ok(), NotFound(), BadRequest()).
    // - Cons: It lacks compile-time type safety for the returned payload. Tooling like Swagger/OpenAPI cannot inspect the
    //         return schema model at design time unless you manually decorate the action with [ProducesResponseType].
    //
    // 2. ActionResult (Concrete Class)
    // - What: A concrete class implementation of IActionResult.
    // - Pros: Historically serves as the baseline concrete implementation of action helpers.
    // - Cons: Like IActionResult, it doesn't expose the underlying model data type to automated api-explorer metadata generation.
    //
    // 3. ActionResult<T> (Generic Wrapper - Recommended for APIs)
    // - What: A hybrid wrapper introduced in ASP.NET Core that allows returning either an Action Result helper OR the raw type T.
    // - Pros:
    //   - Compile-Time Type Safety: Ensures the action strictly yields either an HTTP result or a instance of T.
    //   - Self-Documenting Schema: Tools like Swagger automatically discover the exact schema of model type T without needing [ProducesResponseType].
    //   - Implicit Cast support: You can return the raw object directly (e.g., return "Some String Data") without wrapping it in Ok(), and C# handles the cast.
    public IActionResult GetProduct()
    {
        return Ok("The Product Name is Milk, and the price is 2.99$");
    }
}
