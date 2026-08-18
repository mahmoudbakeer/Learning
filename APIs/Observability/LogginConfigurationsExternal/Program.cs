using LoggerCategory.Services;
/*
 * ==========================================================================================
 * ASP.NET CORE ARCHITECTURE: LOGGER CONFIGURATION 
 * ==========================================================================================
 * * 1. WHERE IS LOGGING CONFIGURED?
 * While you *can* configure logging in C# code,  Developers almost exclusively 
 * configure it inside `appsettings.json` (and `appsettings.Development.json`). 
 * * Why? Because changing `appsettings.json` allows you to change the logging rules 
 * instantly without recompiling the code or redeploying the application.
 * * ------------------------------------------------------------------------------------------
 * 2. THE ANATOMY OF APPSETTINGS.JSON
 * * "Logging": {
 * "LogLevel": {
 * "Default": "Information",                  // 1. The Global Rule
 * "Microsoft.AspNetCore": "Warning",         // 2. The Namespace Rule
 * "MyEcommerceApp.Controllers": "Debug"      // 3. Your Custom Rule
 * }
 * }
 * * How the engine reads this:
 * 1. Default: By default, only print logs that are "Information" or higher (Warn, Error).
 * Ignore all Trace and Debug logs to save CPU and disk space.
 * * 2. Namespace Filtering: The ASP.NET Core framework itself writes thousands of internal 
 * "Information" logs (e.g., "Routing to endpoint X"). This creates massive noise. 
 * By setting "Microsoft.AspNetCore" to "Warning", we tell the framework: 
 * "Shut up unless something actually goes wrong."
 * * 3. Specific Overrides: You can target your exact C# namespaces! If you have a bug in 
 * your Controllers, you can set *only* that namespace to "Debug" to see the exact 
 * variable states, without turning on Debug for the whole app.
 * * ------------------------------------------------------------------------------------------
 * 3. ENVIRONMENT SPECIFIC CONFIGURATION
 * You should have different rules depending on where the app is running.
 * * A. appsettings.Development.json (Local Machine)
 * - Default: "Information"
 * - Microsoft: "Information" (You want to see the routing and EF Core SQL queries here).
 * * B. appsettings.Production.json (Live Server)
 * - Default: "Warning" (Only log things that are suspicious or broken).
 * - Microsoft: "Error" (Complete silence from the framework unless it crashes).
 * * ------------------------------------------------------------------------------------------
 * 4. ARCHITECT TIP: "HOT RELOADING"
 * ASP.NET Core monitors `appsettings.json` for file changes continuously. 
 * * If a critical bug happens in Production at 2:00 AM, you do NOT need to redeploy the app. 
 * You can simply open the `appsettings.json` file on the server, change "Default" to 
 * "Debug", and hit Save. The application will instantly start outputting Debug logs on 
 * the next HTTP request without restarting! Once you find the bug, change it back to 
 * "Warning" and hit Save again.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<ProcessService>();
var app = builder.Build();
app.MapControllers();
app.Run();
