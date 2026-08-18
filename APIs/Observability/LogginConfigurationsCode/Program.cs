using LoggerCategory.Services;
using Microsoft.Extensions.Logging.Console;
/*
 * ==========================================================================================
 * ASP.NET CORE ARCHITECTURE: PROGRAMMATIC LOGGING CONFIGURATION
 * ==========================================================================================
 * * 1. WHY CONFIGURE LOGGING IN CODE?
 * While Log Levels (Information, Warning) belong in `appsettings.json` so you can change 
 * them without recompiling, the actual setup of *Providers* and *Complex Filters* must 
 * happen in C# code inside `Program.cs`.
 * * Use cases for code configuration:
 * - Adding third-party providers (like Serilog, Application Insights, AWS CloudWatch).
 * - Removing the default Microsoft loggers to improve performance.
 * - Applying dynamic logic (e.g., "If environment is X, add Provider Y").
 * * ------------------------------------------------------------------------------------------
 * 2. HIGH-LEVEL IMPLEMENTATION (Program.cs)
 * * var builder = WebApplication.CreateBuilder(args);
 * * // Access the Logging builder
 * builder.Logging.ClearProviders(); //  GOOD MOVE: Remove default Console/Debug loggers
 * * // Add back only what you explicitly want
 * builder.Logging.AddConsole();
 * builder.Logging.AddDebug();
 * * // Hardcoding a minimum level (Overrides appsettings.json! Use with caution)
 * builder.Logging.SetMinimumLevel(LogLevel.Warning);
 * * // Adding strict code-based filters
 * builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
 * builder.Logging.AddFilter("MyEcommerceApp.Controllers", LogLevel.Debug);
 * * ------------------------------------------------------------------------------------------
 * 3. THE "PROVIDERS VS. LEVELS" RULE
 * * THE BAD WAY:
 * Hardcoding `SetMinimumLevel(LogLevel.Information)` in C#. If a production bug happens, 
 * you have to change the code, recompile, and redeploy just to see Debug logs.
 * * THE RIGHT WAY:
 * Do exactly TWO things in C# code:
 * 1. Call `ClearProviders()`.
 * 2. Add your target provider (e.g., `builder.Host.UseSerilog(...)` or `.AddConsole()`).
 * Leave ALL filtering and LogLevel definitions inside `appsettings.json` so they can be 
 * hot-reloaded dynamically on a live server.
 * * ------------------------------------------------------------------------------------------
 * 4. THE SERILOG EXCEPTION
 * When using a heavy-duty framework like Serilog, you bypass `builder.Logging` entirely.
 * Serilog takes over the entire host. 
 * * builder.Host.UseSerilog((context, loggerConfig) => {
 * // Read the levels from appsettings.json
 * loggerConfig.ReadFrom.Configuration(context.Configuration) 
 * .WriteTo.Console()
 * .WriteTo.File("log.txt");
 * });
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Warning); //  BAD: Hardcoding a minimum level in code
builder.Logging.ClearProviders(); //  GOOD: Remove default Console/Debug loggers
builder.Logging.AddConsole(); //  GOOD: Add back only what you explicitly want
builder.Logging.AddDebug(); //  GOOD: Add back only what you explicitly want
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning); //  GOOD: Add strict code-based filters
builder.Logging.AddFilter("LoggerCategory.Services.ProcessService", LogLevel.Debug); // GOOD: Add strict code-based filters
builder.Logging.AddFilter<ConsoleLoggerProvider>((category, level) =>
{
    if (category is not null && category.StartsWith("Microsoft"))
        return level >= LogLevel.Information;
    if (category is not null && category.StartsWith("LoggerCategory.Services"))
        return level >= LogLevel.Debug;
    return level >= LogLevel.Error;
});
builder.Services.AddControllers();
builder.Services.AddScoped<ProcessService>();
var app = builder.Build();
app.MapControllers();
app.Run();
