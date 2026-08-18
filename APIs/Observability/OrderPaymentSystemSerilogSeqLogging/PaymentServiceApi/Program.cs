using System.Text.Json.Serialization;
using Serilog;
/*
 * ==========================================================================================
 * ASP.NET CORE ARCHITECTURE: SERILOG & SEQ (THE OBSERVABILITY DREAM TEAM)
 * ==========================================================================================
 * * 1. WHAT IS SERILOG? (The Engine)
 * Serilog is a third-party logging provider for .NET. Its primary superpower is 
 * "Structured Logging." Instead of writing logs as flat text strings, Serilog serializes 
 * your log messages and their parameters into rich JSON objects.
 * * Why use Serilog?
 * - Sinks: It can route logs anywhere (Console, Files, Cloud Providers, Databases) easily.
 * - Enrichers: It automatically injects metadata into EVERY log (e.g., Server Name, 
 * Environment, ThreadId) without you writing extra code in your controllers.
 * * ------------------------------------------------------------------------------------------
 * 2. WHAT IS SEQ? (The Dashboard & Search Engine)
 * Seq is a centralized log server and web dashboard designed specifically for structured 
 * data. You run it on a server (or in a Docker container), and your application sends 
 * its logs to Seq over HTTP.
 * * Why use Seq?
 * - No More Text Files: Reading gigabytes of flat text files on a production server to 
 * find a bug is a nightmare. 
 * - SQL-Like Querying: Because Seq receives JSON, you can query your logs like a database. 
 * You can search: `OrderId == '1234' and @Level == 'Error'` and instantly find the exact 
 * error across millions of logs in milliseconds.
 * - Dashboards & Alerts: You can build graphs (e.g., "Show me 500 errors per minute") 
 * and tell Seq to email/Slack you if the error rate spikes.
 * * ------------------------------------------------------------------------------------------
 * 3. WHY USE THEM TOGETHER? (The Perfect Synergy)
 * Serilog is the perfect PRODUCER of JSON logs. Seq is the perfect CONSUMER of JSON logs.
 * * If you have a Microservices architecture (e.g., OrderService, PaymentService), you 
 * point BOTH services' Serilog configuration to the SAME Seq server. When a request 
 * jumps from Order to Payment, you can track the entire flow in one single dashboard.
 * * ------------------------------------------------------------------------------------------
 * 4. HIGH-LEVEL IMPLEMENTATION & CONFIGURATION
 * * Step 1: Install NuGet Packages
 * - Serilog.AspNetCore
 * - Serilog.Sinks.Seq
 * * Step 2: Configure `appsettings.json`
 * (Senior Tip: Configure Serilog in JSON, not C#, so you can change the Seq URL or 
 * log levels without recompiling the app).
 * * "Serilog": {
 * "MinimumLevel": {
 * "Default": "Information",
 * "Override": {
 * "Microsoft.AspNetCore": "Warning" // Silence framework noise
 * }
 * },
 * "WriteTo": [
 * { "Name": "Console" }, // Still write to terminal for local debugging
 * {
 * "Name": "Seq",
 * "Args": { 
 * "serverUrl": "http://localhost:5341", // The URL of your Seq server
 * "apiKey": "OptionalSecretKeyHere"
 * }
 * }
 * ],
 * "Enrich": [ "FromLogContext", "WithMachineName", "WithEnvironmentName" ]
 * }
 * * Step 3: Wire it up in `Program.cs`
 * Replace the default ASP.NET Core logger with Serilog at the very top of your file.
 * * // Program.cs
 * var builder = WebApplication.CreateBuilder(args);
 * * // Tell the host to use Serilog and read from appsettings.json
 * builder.Host.UseSerilog((context, configuration) => 
 * {
 * configuration.ReadFrom.Configuration(context.Configuration);
 * });
 * * // Note: Add this AFTER app.UseRouting() so Serilog captures the HTTP request data
 * app.UseSerilogRequestLogging(); 
 * * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerconfiguration) => { loggerconfiguration.ReadFrom.Configuration(builder.Configuration); });
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
