internal class Program
{
    private static void Main(string[] args)
    {
        // ---------------------------------------------------------
        // Creates the WebApplication builder.
        //
        // This object is responsible for:
        // - Loading configuration
        // - Registering services (Dependency Injection)
        // - Setting up logging
        // - Preparing the web host
        // ---------------------------------------------------------
        var builder = WebApplication.CreateBuilder(args);

        // ---------------------------------------------------------
        // Builds the actual application object.
        //
        // After Build():
        // - Middleware pipeline can be configured
        // - Endpoints can be mapped
        // ---------------------------------------------------------
        var app = builder.Build();

        // ---------------------------------------------------------
        // STATIC FILES MIDDLEWARE
        // ---------------------------------------------------------
        //
        // Enables serving static files directly to the browser.
        //
        // Examples of static files:
        // - HTML
        // - CSS
        // - JavaScript
        // - Images
        // - txt files
        //
        // By default, ASP.NET Core serves files from:
        //
        //      wwwroot/
        //
        // Example:
        //
        //      wwwroot/css/site.css
        //
        // becomes accessible from:
        //
        //      /css/site.css
        //
        // IMPORTANT:
        // wwwroot itself DOES NOT appear in the URL.
        //
        // Example:
        //
        // Physical Path:
        //      wwwroot/files/test.txt
        //
        // URL Route:
        //      /files/test.txt
        //
        // ---------------------------------------------------------
        // Why should it be BEFORE UseRouting() ?
        // ---------------------------------------------------------
        //
        // Because static files do not need:
        //
        // - Endpoint routing
        // - Model binding
        // - Controllers
        // - Minimal API matching
        //
        // So if the file exists:
        //
        // Request:
        //      GET /css/site.css
        //
        // UseStaticFiles() immediately returns the file
        // and STOPS the pipeline early.
        //
        // This improves performance significantly.
        //
        // If the file does NOT exist:
        // the request continues to the next middleware.
        // ---------------------------------------------------------
        app.UseStaticFiles();

        // ---------------------------------------------------------
        // ROUTING MIDDLEWARE
        // ---------------------------------------------------------
        //
        // Responsible for matching incoming URLs
        // to endpoints.
        //
        // Example:
        //
        //      /Employee
        //
        // gets matched to:
        //
        //      endpoints.MapGet("/Employee", ...)
        //
        // Routing only decides:
        // "Which endpoint should handle this request?"
        // ---------------------------------------------------------
        app.UseRouting();

        // ---------------------------------------------------------
        // ENDPOINTS
        // ---------------------------------------------------------
        //
        // Here we define the application's endpoints.
        //
        // Each endpoint is:
        // - HTTP Method
        // - Route
        // - Handler
        //
        // Examples:
        // - GET
        // - POST
        // - PUT
        // - DELETE
        // ---------------------------------------------------------
        app.UseEndpoints(
            (endpoints) =>
            {
                // -------------------------------------------------
                // GET /Employee
                // -------------------------------------------------
                //
                // This endpoint handles HTTP GET requests.
                //
                // Example:
                //
                //      GET /Employee
                //
                // Response:
                //
                //      Get Employees
                // -------------------------------------------------
                endpoints.MapGet(
                    "/Employee",
                    async (context) =>
                    {
                        await context.Response.WriteAsync($"Get Employees");
                    }
                );

                // -------------------------------------------------
                // POST /Employee
                // -------------------------------------------------
                //
                // Handles HTTP POST requests.
                //
                // Usually used for:
                // - Creating resources
                // - Sending form data
                // - Creating employees
                // -------------------------------------------------
                endpoints.MapPost(
                    "/Employee",
                    async (context) =>
                    {
                        await context.Response.WriteAsync($"Post Employees");
                    }
                );

                // -------------------------------------------------
                // PUT /Employee
                // -------------------------------------------------
                //
                // Handles HTTP PUT requests.
                //
                // Usually used for:
                // - Updating existing resources
                // -------------------------------------------------
                endpoints.MapPut(
                    "/Employee",
                    async (context) =>
                    {
                        await context.Response.WriteAsync($"Put Employees");
                    }
                );

                // -------------------------------------------------
                // ROUTE PARAMETERS EXPLANATION
                // -------------------------------------------------
                //
                // {id}
                //      Required parameter
                //
                // {position=Developer}
                //      Default value
                //
                // If position is not provided:
                //      position = "Developer"
                //
                // {name?}
                //      Optional parameter
                //
                // {id:int}
                //      Route constraint
                //
                // Means:
                // id MUST be integer.
                //
                // If route does not match constraint:
                // ASP.NET Core returns:
                //
                //      404 Not Found
                //
                // NOT because the endpoint threw an exception,
                // but because routing failed to match.
                // -------------------------------------------------

                // -------------------------------------------------
                // DELETE /Employee/{id:int}/{position=Developer}
                // -------------------------------------------------
                //
                // Examples:
                //
                //      DELETE /Employee/10
                //
                // Result:
                //      id = 10
                //      position = Developer
                //
                // -------------------------------------------------
                //
                //      DELETE /Employee/10/Manager
                //
                // Result:
                //      id = 10
                //      position = Manager
                //
                // -------------------------------------------------
                //
                // INVALID:
                //
                //      DELETE /Employee/abc
                //
                // Because:
                //      abc is not int
                //
                // Result:
                //      404 Not Found
                // -------------------------------------------------
                endpoints.MapDelete(
                    "/Employee/{id:int}/{position=Developer}",
                    async (context) =>
                    {
                        // -----------------------------------------
                        // RouteValues contains values extracted
                        // from the URL route.
                        //
                        // Example:
                        //
                        // URL:
                        //      /Employee/10/Manager
                        //
                        // RouteValues:
                        //      id = 10
                        //      position = Manager
                        // -----------------------------------------
                        await context.Response.WriteAsync(
                            $"Delete Employees : {context.Request.RouteValues["position"]}, {context.Request.RouteValues["id"]}"
                        );
                    }
                );
            }
        );

        // ---------------------------------------------------------
        // Starts the web server and begins listening for requests.
        // ---------------------------------------------------------
        app.Run();
    }
}
