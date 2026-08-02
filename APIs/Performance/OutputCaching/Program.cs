using System.Data;
using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
/*
 * ==========================================================================================
 * ASP.NET CORE PERFORMANCE: OUTPUT CACHING (.NET 7+)
 * ==========================================================================================
 *
 * 1. WHAT IT IS (Data Caching vs. Output Caching):
 * - Data Caching (IMemoryCache/HybridCache) caches raw C# objects inside your Services.
 * - Output Caching caches the ENTIRE HTTP RESPONSE (the final JSON/HTML bytes) at the 
 *   Middleware pipeline level. It intercepts requests before they even reach your Controllers.
 * 
 * 2. WHY WE USE IT:
 * It is the ultimate performance shield for your API. Because it bypasses the Controller, 
 * Dependency Injection, Database queries, and JSON Serialization completely, it provides 
 * the absolute maximum possible throughput for endpoints that rarely change.
 *
 * 3. MIDDLEWARE CONFIGURATION (Program.cs):
 * Middleware order is strictly enforced for Output Caching to work safely!
 * 
 * builder.Services.AddOutputCache(); 
 * // ... 
 * app.UseRouting();
 * app.UseOutputCache(); // MUST be exactly here (After Routing, Before Auth & Controllers)
 * app.UseAuthorization();
 *
 * 4. THE IMPLEMENTATION PATTERN (Attributes):
 * Simply apply the attribute to your Controller method or Minimal API endpoint:
 * 
 * [HttpGet]
 * [OutputCache(Duration = 60, Tags = new[] { "products_list" } , PolicyName = "Multiple")] // Bypasses the Controller logic entirely for 60 seconds and has this tag for cache invalidation on POST/PUT/DeletedRowInaccessibleException and policy name for the policy defined in Program.cs
 * public async Task<IActionResult> GetAllProducts() { ... }
 *
 * ==========================================================================================
 * ⚠️ EVENT-DRIVEN INVALIDATION (PREVENTING STALE DATA) ⚠️
 * ==========================================================================================
 * 
 * If a user modifies the database, the cached HTTP response becomes stale. You must use 
 * TAGS to link endpoints together so a Write operation can immediately destroy the Read cache.
 * 
 * STEP A (Tag the GET Endpoint): and only the GET and Head Endpoints should be tagged, not the POST/PUT/DELETE endpoints,
  because those endpoints are the ones that modify the data and should not be cached.
   The GET and HEAD endpoints are the ones that retrieve the data and should be cached.
 * [HttpGet]
 * [OutputCache(Duration = 3600, Tags = new[] { "products_list" })]
 * public async Task<IActionResult> GetAll() { ... }
 * 
 * STEP B (Evict the Tag on POST/PUT/DELETE):
 * Inject `IOutputCacheStore` into your modifying endpoint to wipe the tagged response:
 * 
 * [HttpPost]
 * public async Task<IActionResult> AddProduct(
 *     ProductRequest req, 
 *     [FromServices] IOutputCacheStore outputCache, 
 *     CancellationToken ct)
 * {
 *     await _service.AddProductAsync(req);
 *     await outputCache.EvictByTagAsync("products_list", ct); // Destroys the stale response!
 *     return Ok();
 * }
 * 
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => // for all the policies, you can set the default expiration time for all the policies, so you don't have to set it for each policy individually
    {

        builder.Tag("products_list"); // Tagging the GET endpoint to allow for cache invalidation on POST/PUT/DELETE
    });
    options.AddPolicy("Single", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(30));
        builder.SetVaryByRouteValue("ProductId");
    });
    options.AddPolicy("Multiple", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(30));
        builder.SetVaryByQuery("page");
        builder.SetVaryByQuery("pageSize");


    });

    // options.MaximumBodySize = 1024 * 1024 * 10; // 10 MB
    // options.SizeLimit = 1024 * 1024 * 100; // 100 MB
    // options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(30);
    // options.UseCaseSensitivePaths = false;
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();
app.UseOutputCache();
app.MapControllers();
app.Run();
