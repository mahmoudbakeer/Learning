using LoggerCategory.Services;
/*
 * ==========================================================================================
 * ASP.NET CORE ARCHITECTURE: LOGGING & OBSERVABILITY
 * ==========================================================================================
 * * 1. WHAT IS LOGGING AND WHY DO WE USE IT?
 * When your application is running in Production, you cannot attach a debugger to see 
 * what is happening. Logging is your application's "flight data recorder."
 * * Why we use it:
 * - Troubleshooting: When a user gets a 500 Server Error, logs tell you exactly which 
 * line of code threw the exception.
 * - Auditing/Security: Tracking when critical actions happen (e.g., "User X logged in").
 * - Performance Monitoring: Seeing how long specific database queries or API calls take.
 * - Observability: Integrating with tools like Serilog, Seq, or Datadog to create 
 * dashboards and set up automated alerts if errors spike.
 * * ------------------------------------------------------------------------------------------
 * 2. WHAT IS ILogger<T>?
 * `ILogger<TCategory>` is the built-in Microsoft interface used to write log messages.
 * * Why the `<T>`? (The Category)
 * The `<T>` is usually the class you are injecting the logger into (e.g., `ILogger<ProductService>`).
 * This automatically tags every log message with the name of that class. When you are 
 * searching through millions of logs, this allows you to filter messages specifically 
 * coming from the `ProductService`.
 * 3. HOW TO INJECT IT (The Implementation)
 * ASP.NET Core registers the logging system automatically in `WebApplication.CreateBuilder()`.
 * You simply request it in your constructor via Dependency Injection.
 * * public class ProductService
 * {
 * private readonly ILogger<ProductService> _logger;
 * * // Constructor Injection
 * public ProductService(ILogger<ProductService> logger)
 * {
 * _logger = logger;
 * }
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<ProcessService>();
var app = builder.Build();
app.MapControllers();
app.Run();
