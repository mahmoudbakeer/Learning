using LoggerCategory.Services;
/*
 * ==========================================================================================
 * ASP.NET CORE ARCHITECTURE: LOGGING LEVELS (A PRAGMATIC GUIDE)
 * ==========================================================================================
 * * 1. THE 6 LOGGING LEVELS (From least severe to most severe)
 * * [0] Trace       - The absolute most detailed logs. May contain sensitive data (passwords, 
 * full HTTP bodies). Almost NEVER enabled in production.
 * [1] Debug       - Used during development to track exact variable states or loop counters.
 * [2] Information - Tracks the "Happy Path" or major milestones of the application.
 * [3] Warning     - Something unexpected happened, but the app recovered and did not crash.
 * [4] Error       - The current operation/HTTP request failed completely.
 * [5] Critical    - System-level failure. The app is crashing or requires waking up an 
 * engineer at 3:00 AM.
 * * ------------------------------------------------------------------------------------------
 * 2. REAL-WORLD EXAMPLES (When to use what)
 * * Trace       => _logger.LogTrace("User typed 'A' in the password field"); //  Never do this
 * Debug       => _logger.LogDebug("Entering the calculate tax loop. iteration {i}", i);
 * Information => _logger.LogInformation("Order {OrderId} successfully paid.", order.Id);
 * Warning     => _logger.LogWarning("Payment API took 4000ms to respond for Order {OrderId}", id);
 * Error       => _logger.LogError(ex, "Failed to save Order {OrderId} to database.", id);
 * Critical    => _logger.LogCritical(ex, "FATAL: Cannot connect to the main SQL Database.");
 * * ------------------------------------------------------------------------------------------
 * 3. WHAT DO I ACTUALLY DO WITH THESE?
 * * As a developer, do I need to use all 6 levels? 
 * NO. Keep it simple. Follow the "Rule of Three":
 * *  Use LogInformation:
 * For things the business cares about. "User registered", "Order shipped", "Item deleted".
 * Don't log every single method call, just the major milestones.
 * *  Use LogWarning:
 * For things that are suspicious but didn't break the app. 
 * Examples: "User failed login 3 times", "404 Not Found requested", "Retrying failed DB call".
 * *  Use LogError:
 * Anytime you write a `catch (Exception ex)` block, put a `LogError` inside it. 
 * Always pass the exception object as the FIRST parameter so the stack trace is saved!
 * Example: `_logger.LogError(ex, "Error processing payment");`
 * * What about Trace, Debug, and Critical?
 * - The ASP.NET Core framework itself writes thousands of Trace/Debug logs for you. You 
 * rarely need to write them yourself.
 * - Critical is usually reserved for Infrastructure/DevOps level events (like running out 
 * of hard drive space).
 * * ------------------------------------------------------------------------------------------
 * 4. CONFIGURATION NOTE (appsettings.json)
 * You control what gets printed/saved in `appsettings.json`.
 * If you set the Default level to "Warning", then Information, Debug, and Trace logs 
 * are completely ignored to save CPU and Hard Drive space!
 * * "Logging": {
 * "LogLevel": {
 * "Default": "Information",       // Show Info, Warn, Error, Critical
 * "Microsoft.AspNetCore": "Warning" // Hide Microsoft's internal Info logs
 * }
 * }
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<ProcessService>();
var app = builder.Build();
app.MapControllers();
app.Run();
