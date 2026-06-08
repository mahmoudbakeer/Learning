var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// -----------------------------------------------------------------------------
// Handles unhandled exceptions thrown by any middleware or endpoint that follows.
// Must be one of the first middleware so it can catch exceptions from the entire
// pipeline and return a consistent error response instead of crashing the request.
// -----------------------------------------------------------------------------
app.UseExceptionHandler();

// -----------------------------------------------------------------------------
// Adds the Strict-Transport-Security (HSTS) header.
// Tells browsers to always use HTTPS for future requests.
// Must run early so the security header is included in responses.
// Typically enabled only in Production.
// -----------------------------------------------------------------------------
app.UseHsts();

// -----------------------------------------------------------------------------
// Redirects all HTTP requests to HTTPS.
// Should execute before authentication, authorization, routing, and business
// logic to ensure all communication is encrypted.
// -----------------------------------------------------------------------------
app.UseHttpsRedirection();

// -----------------------------------------------------------------------------
// Serves static files (css, js, images, fonts, etc.) directly.
// Placed early so requests for static resources can be handled immediately
// without going through routing, authentication, or other middleware.
// -----------------------------------------------------------------------------
app.UseStaticFiles();

// -----------------------------------------------------------------------------
// Matches the incoming request to an endpoint and stores route information
// in the HttpContext.
// Must execute before CORS, Authentication, Authorization, and endpoint
// execution because they may need route metadata.
// -----------------------------------------------------------------------------
app.UseRouting();

// -----------------------------------------------------------------------------
// Applies Cross-Origin Resource Sharing (CORS) policies.
// Determines whether requests from other origins are allowed.
// Usually placed after routing and before authentication/authorization so
// CORS can evaluate endpoint metadata and handle preflight requests correctly.
// -----------------------------------------------------------------------------
app.UseCors();

// -----------------------------------------------------------------------------
// Authenticates the user and creates the ClaimsPrincipal (HttpContext.User).
// This middleware identifies WHO is making the request.
// Must execute before Authorization because authorization requires an
// authenticated user to evaluate permissions and roles.
// -----------------------------------------------------------------------------
app.UseAuthentication();

// -----------------------------------------------------------------------------
// Verifies whether the authenticated user is allowed to access the resource.
// This middleware checks roles, policies, and permissions.
// Must run after Authentication because it needs the authenticated user.
// -----------------------------------------------------------------------------
app.UseAuthorization();

// -----------------------------------------------------------------------------
// Custom middleware example.
// Place it according to its requirements:
// - Before Authentication if it doesn't need user information.
// - After Authentication if it needs HttpContext.User.
// - After Authorization if it needs authorization results.
// -----------------------------------------------------------------------------
app.Use(
    async (HttpContext context, RequestDelegate next) =>
    {
        await next(context);
    }
);

// -----------------------------------------------------------------------------
// Executes the matched endpoint (Controllers, Minimal APIs, Razor Pages, etc.).
// This is usually the end of the request pipeline where the actual business
// logic runs and generates the response.
// -----------------------------------------------------------------------------
app.MapControllers();

app.Run();
