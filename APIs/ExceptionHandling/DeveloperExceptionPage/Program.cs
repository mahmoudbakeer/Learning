using DeveloperExceptionPage.Endpoints;

/*
 * ==========================================================================================
 * ASP.NET CORE DEVELOPER EXCEPTION PAGE (MIDDLEWARE)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * A built-in diagnostic middleware that catches unhandled exceptions anywhere in the
 * HTTP pipeline and generates a highly detailed HTML error page. It displays the exact
 * line of C# code that crashed, the full stack trace, HTTP headers, routing details,
 * and query string parameters.
 *
 * WHY WE USE IT:
 * 1. Immediate Debugging: It provides massive amounts of context directly in the browser
 * so you don't have to go digging through terminal logs while building features locally.
 * 2. Visual Routing Context: It shows exactly which endpoint matched (or failed to match),
 * making it invaluable for debugging 404s and 500s.
 *
 * THE DEADLY TRAP (CRITICAL SECURITY WARNING):
 * NEVER, EVER leave this enabled in a Staging or Production environment.
 * If a malicious user forces your app to crash (e.g., by sending malformed JSON), this
 * page will literally print your raw C# source code, internal file paths, database query
 * structures, and potentially sensitive tokens in the browser. It is classified as a
 * Critical Information Disclosure vulnerability.
 *
 * WHEN TO USE IT:
 * - Exclusively when running on your local machine (the "Development" environment).
 *
 * WHEN NOT TO USE IT:
 * - Production, QA, or Staging environments. In these environments, you MUST use
 * `app.UseExceptionHandler()` instead, which catches the error securely and returns a
 * generic 500 error page or a clean JSON ProblemDetails response.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Note for .NET 6+: If you use `WebApplication.CreateBuilder(args)`, this middleware
 * is actually added automatically for you behind the scenes when in Development mode!
 * 2. Manual Implementation (If overriding):
 * if (app.Environment.IsDevelopment())
 * {
 * // ONLY here.
 * app.UseDeveloperExceptionPage();
 * }
 * else
 * {
 * // The safe fallback for actual users.
 * app.UseExceptionHandler("/error");
 * }
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
else
{
    app.UseExceptionHandler("/development-error");
}
app.MapControllers();
app.MapErrorEndpoints();
app.Run();
