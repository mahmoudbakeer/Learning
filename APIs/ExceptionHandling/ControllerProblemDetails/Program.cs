/*
 * ==========================================================================================
 * ASP.NET CORE GLOBAL ERROR HANDLING SUITE
 * ==========================================================================================
 *
 * 1. THE SERVICE: builder.Services.AddProblemDetails()
 * - WHAT IT IS: Registers the internal services required to format errors into the
 * RFC 9457 standard (Problem Details).
 * - WHEN TO USE: Always in modern APIs (.NET 7+). It tells the framework, "Whenever you
 * need to generate an automatic error (like a 400 for bad JSON, or a 404 for a bad URL),
 * format it as a standard Problem Details JSON object."
 *
 * 2. THE CRASH CATCHERS: UseDeveloperExceptionPage() vs UseExceptionHandler()
 * - WHAT THEY DO: They sit at the very start of the pipeline and wrap the ENTIRE app in
 * a giant `try-catch` block. They ONLY trigger if an unhandled Exception is thrown.
 * - UseDeveloperExceptionPage: Generates the massive HTML page with raw C# source code
 * and stack traces. Use ONLY in Development. (Note: .NET 6+ adds this automatically in dev).
 * - UseExceptionHandler: The secure version for Production. Catches the crash, logs it,
 * and either routes the user to an ErrorController or runs your custom `IExceptionHandler`.
 *
 * 3. THE EMPTY RESPONSE FILLER: UseStatusCodePages()
 * - WHAT IT IS: This is the missing link most developers forget! It does NOT catch crashes.
 * Instead, it catches empty, body-less HTTP responses (400-599).
 * - WHY WE NEED IT: If a user hits a URL that doesn't exist, the Routing middleware simply
 * returns a naked `404 Not Found` with no JSON body. If your controller does `return NotFound();`
 * with no arguments, it returns an empty body.
 * - THE MAGIC: If you put UseStatusCodePages() in the pipeline AND you registered
 * AddProblemDetails() earlier, it intercepts those empty 404/401/403 responses and
 * injects a beautiful RFC 9457 Problem Details JSON body before sending it to the client!
 *
 * ==========================================================================================
 * THE PIPELINE ORDER (CRITICAL)
 * ==========================================================================================
 * Error handling middleware MUST go at the very top of the HTTP pipeline (immediately
 * after `app.Build()`). Middleware can only catch errors from middleware that runs AFTER it.
 *
 * var app = builder.Build();
 *
 * // 1. Catch fatal crashes first (so it catches errors in StatusCodePages or downstream)
 * if (app.Environment.IsDevelopment()) {
 * app.UseDeveloperExceptionPage();
 * } else {
 * app.UseExceptionHandler();
 * }
 *
 * // 2. Catch empty error status codes (like 404s from Routing or 401s from Auth)
 * app.UseStatusCodePages();
 *
 * // 3. Normal Pipeline follows...
 * app.UseRouting();
 * app.UseAuthentication();
 * app.UseAuthorization();
 * app.MapControllers();
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/development-error");
}
app.UseStatusCodePages();
app.UseRouting();
app.MapControllers();
app.Run();
