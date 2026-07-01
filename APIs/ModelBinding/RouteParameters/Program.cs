using Microsoft.AspNetCore.Mvc;

/**
 * Model Binding from Route Parameters
 * (Minimal APIs and Controller-Based APIs)
 * ----------------------------------------
 * Model binding is the process by which ASP.NET Core
 * automatically extracts values from the URL and converts
 * them into .NET types.
 *
 * Route values are matched by parameter name and bound
 * to the endpoint or action method parameters.
 *
 * ASP.NET Core performs automatic type conversion,
 * eliminating the need for manual parsing.
 *
 * Minimal API Example:
 *
 *   app.MapGet(
 *       "/products/{id}",
 *       (int id) => $"Product {id}");
 *
 *   Request:
 *     /products/10
 *
 *   Result:
 *     id = 10
 *
 *
 * Controller-Based API Example:
 *
 *   [HttpGet("{id}")]
 *   public IActionResult GetProduct(int id)
 *   {
 *       return Ok($"Product {id}");
 *   }
 *
 *   Request:
 *     /products/10
 *
 *   Result:
 *     id = 10
 *
 *
 * Supported Data Types:
 *
 * - string
 * - int
 * - long
 * - decimal
 * - bool
 * - Guid
 * - DateTime
 * - DateOnly
 * - Enum types
 *
 *
 * Optional Route Parameters:
 *
 * Minimal API:
 *
 *   app.MapGet(
 *       "/users/{id?}",
 *       (int? id) => ...);
 *
 * Controller-Based API:
 *
 *   [HttpGet("{id?}")]
 *   public IActionResult GetUser(int? id)
 *   {
 *       ...
 *   }
 *
 *
 * Why Use Route Model Binding?
 *
 * - Eliminates manual parsing.
 * - Automatically converts data types.
 * - Produces cleaner and more readable code.
 * - Reduces boilerplate logic.
 *
 *
 * When to Use It
 * --------------
 *
 * Use route model binding when the value is part
 * of the resource's URL, such as:
 *
 * - Product IDs.
 * - User IDs.
 * - Order numbers.
 * - Category names.
 * - API version numbers.
 *
 *
 * Note:
 * Routing first determines which endpoint or action
 * should handle the request. Then model binding
 * extracts the route values and supplies them to
 * the handler method.
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.MapGet("/Products-minimal/{id:int}", (int id) => $"Product with id {id} exist.");

// using model with different name than the name of route parameter
app.MapGet(
    "/Products-minimal-1/{id:int}",
    ([FromRoute(Name = "id")] int identifier) =>
    {
        return $"Product with id {identifier} exist.";
    }
);

app.Run();
