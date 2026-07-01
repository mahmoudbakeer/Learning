var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/**
 * Route Template
 * --------------
 * A route template is a URL pattern that ASP.NET Core uses
 * to match incoming HTTP requests to an endpoint.
 *
 * A route template can contain:
 *
 * 1. Literal segments
 *    Fixed text that must match exactly.
 *    Example:
 *      /products/all
 *
 * 2. Route parameters
 *    Values enclosed in {} that are extracted from the URL.
 *    Example:
 *      /products/{id}
 *
 * 3. Optional parameters
 *    Parameters followed by ? that may or may not exist.
 *    Example:
 *      /users/{id?}
 *
 * 4. Default values
 *    A value assigned with = that is used when
 *    the client does not provide one.
 *    Example:
 *      /{controller=Home}
 *
 * 5. Catch-all parameters
 *    Parameters prefixed with * or ** that capture
 *    the remaining part of the URL.
 *    Example:
 *      /files/{*path}
 *
 * Route templates are the foundation of ASP.NET Core routing,
 * allowing the framework to map URLs to the appropriate endpoint.
 */

/**
 * Route: /product/{id}
 *
 * {id} is a required route parameter.
 * ASP.NET Core automatically converts the value from the URL
 * to an integer because the handler expects an int.
 *
 * Example:
 *   /product/10
 * Result:
 *   Product 10
 */
app.MapGet("/product/{id}", (int id) => $"Product {id}");

/**
 * Route: /date/{year}-{month}-{day}
 *
 * Route parameters do not have to be separated by '/'.
 * Here they are separated by '-' inside the same URL segment.
 *
 * The values are automatically bound to the method parameters
 * and used to create a DateOnly object.
 *
 * Example:
 *   /date/2026-6-11
 * Result:
 *   Date is 06/11/2026
 */
app.MapGet(
    "/date/{year}-{month}-{day}",
    (int year, int month, int day) => $"Date is {new DateOnly(year, month, day)}"
);

/**
 * Route: /{controller=Home}
 *
 * This route has a default value.
 * If the user does not provide a value,
 * ASP.NET Core automatically uses "Home".
 *
 * Examples:
 *   /
 *   -> Home
 *
 *   /Products
 *   -> Products
 */
app.MapGet("/{controller=Home}", (string? controller) => controller);

/**
 * Route: /users/{id?}
 *
 * The '?' makes the route parameter optional.
 * The endpoint works whether an id is supplied or not.
 *
 * Examples:
 *   /users
 *   -> All users
 *
 *   /users/5
 *   -> User 5
 */
app.MapGet("/users/{id?}", (int? id) => id is null ? "All users" : $"User {id}");

/**
 * Route: /a{b}c{d}
 *
 * Route parameters can be mixed with literal text.
 * ASP.NET Core extracts only the variable parts.
 *
 * Example:
 *   /a123c456
 * Result:
 *   b: 123, d: 456
 */
app.MapGet("/a{b}c{d}", (string b, string d) => $"b: {b}, d: {d}");

/**
 * Route: /single/{*slug}
 *
 * '*' is a catch-all parameter.
 * It captures everything after /single/,
 * including multiple URL segments.
 *
 * A single-star catch-all escapes '/' characters
 * when generating URLs.
 *
 * Example:
 *   /single/blog/aspnet/routing
 * Result:
 *   Slug: blog/aspnet/routing
 */
app.MapGet("/single/{*slug}", (string slug) => $"Slug: {slug}");

/**
 * Route: /double/{**slug}
 *
 * '**' is also a catch-all parameter.
 * It captures multiple URL segments just like '*'.
 *
 * The difference is that a double-star catch-all
 * preserves '/' characters during URL generation,
 * making it useful for file paths and nested routes.
 *
 * Example:
 *   /double/images/2026/photo.jpg
 * Result:
 *   Slug: images/2026/photo.jpg
 */
app.MapGet("/double/{**slug}", (string slug) => $"Slug: {slug}");

/**
 * Route: /{id?}/name
 *
 * The first route segment is optional.
 * If an id is provided, it is captured.
 * If not, the endpoint still matches because
 * the parameter is optional.
 *
 * Examples:
 *   /name
 *   -> Id: none
 *
 *   /123/name
 *   -> Id: 123
 */
app.MapGet("/{id?}/name", (string? id) => $"Id: {id ?? "none"}");

app.Run();
