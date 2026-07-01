var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/**
 * LinkGenerator
 * -------------
 * LinkGenerator is a built-in ASP.NET Core service used
 * to generate URLs based on the application's routing
 * configuration instead of manually writing URL strings.
 *
 * It creates links by using route values, endpoint names,
 * or controller/action information, ensuring that generated
 * URLs remain correct even if route templates change later.
 *
 * Common Uses:
 *
 * 1. Generate a URL by endpoint name.
 *
 * 2. Generate a URL for a controller action.
 *
 * 3. Create links for APIs (HATEOAS).
 *
 * 4. Build redirect URLs.
 *
 * 5. Generate absolute URLs that include the scheme
 *    (http/https) and host.
 *
 * Why Use LinkGenerator?
 *
 * - Avoids hard-coded URLs.
 * - Makes applications easier to maintain.
 * - Reduces errors when route templates change.
 * - Uses the same routing system that matches requests.
 *
 * Example:
 *
 *   string url = linkGenerator.GetPathByName(
 *       "ProductDetails",
 *       new { id = 10 });
 *
 *   Result:
 *     /product/10
 *
 * When to Use LinkGenerator
 * -------------------------
 *
 * Use LinkGenerator when your application needs to
 * generate URLs programmatically, such as:
 *
 * - Returning links in Web API responses.
 * - Redirecting users to another endpoint.
 * - Building navigation links dynamically.
 * - Sending links in emails or notifications.
 * - Generating URLs outside controllers
 *   (middleware, services, background tasks).
 *
 * When Not to Use It
 * ------------------
 *
 * If you are simply defining routes, LinkGenerator
 * is not needed. It is only used when you want to
 * create URLs from existing routes.
 */
app.MapGet(
    "/Order/{id:int}",
    (int id, LinkGenerator link, HttpContext context) =>
    {
        var editUrl = link.GetUriByName(
            endpointName: "EditOrder",
            values: new { id },
            scheme: context.Request.Scheme,
            host: context.Request.Host
        );
        return Results.Ok(
            new
            {
                id = id,
                status = "PENDING",
                _links = new
                {
                    self = new { href = context.Request.Path },
                    edit = new { href = editUrl, Method = "PUT" },
                },
            }
        );
    }
);

app.MapPut(
        "/Order/{id:int}",
        (int id) =>
        {
            return Results.NoContent();
        }
    )
    .WithName(endpointName: "EditOrder");
app.Run();
