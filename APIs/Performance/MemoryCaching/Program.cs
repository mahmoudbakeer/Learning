using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.EntityFrameworkCore;
/*
 * ==========================================================================================
 * ASP.NET CORE PERFORMANCE: IN-MEMORY CACHING (L1 CACHE)
 * ==========================================================================================
 *
 * 1. WHAT IT IS:
 * IMemoryCache stores C# objects directly inside the web server's active RAM. 
 * Because there is no network serialization required (like with Redis), it is the 
 * absolute fastest way to retrieve data in ASP.NET Core (nanosecond response times).
 *
 * 2. WHEN TO USE IT:
 * - Highly accessed, rarely changing data (e.g., Categories, Configurations, Product Lists).
 * - Single-server deployments. 
 * - (Note: If deploying to a Web Farm with multiple servers, Server A's memory cache 
 * will not match Server B's memory cache. You must use IDistributedCache/HybridCache).
 *
 * 3. THE IMPLEMENTATION PATTERN (GetOrCreateAsync):
 * Avoid the legacy `if(!cache.TryGetValue(...))` pattern. Always use `GetOrCreateAsync`.
 * It is cleaner, safer, and guarantees the cache entry is initialized properly.
 * Example:
 * return await _cache.GetOrCreateAsync("key", async entry => {
 * entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
 * return await _db.GetDataAsync(cancellationToken);
 * });
 *
 * 4. CACHE INVALIDATION (Preventing Stale Data):
 * Never let the cache dictate the truth if the database has changed.
 * On ANY Write operation (Create, Update, Delete) that affects the cached data, 
 * you MUST explicitly destroy the cache entry using: `_cache.Remove("key");`
 *
 * ==========================================================================================
 * ⚠️ CRITICAL ARCHITECTURAL WARNINGS & GOTCHAS ⚠️
 * ==========================================================================================
 *
 * A. THE MEMORY LEAK (Out Of Memory Exception):
 * By default, IMemoryCache has NO LIMIT. If you cache millions of rows, it will consume 
 * 100% of the server's RAM and crash the application.
 * SOLUTION: 
 * 1. In Program.cs: `builder.Services.AddMemoryCache(opt => opt.SizeLimit = 1024);`
 * 2. In your code: Set `entry.Size = 1;` every time you add an item.
 *
 * B. THE SLIDING EXPIRATION TRAP:
 * "Sliding Expiration" keeps data alive as long as it is requested frequently. 
 * DANGER: If a highly popular endpoint is requested every 5 seconds, and the sliding 
 * window is 10 seconds, the cache will NEVER expire, and the data will be permanently stale.
 * SOLUTION: If you use Sliding Expiration, you MUST pair it with Absolute Expiration to 
 * guarantee a maximum lifespan.
 *
 * C. THREAD POOL STARVATION:
 * When a Cache Miss occurs, the callback function usually hits the database.
 * You MUST pass a `CancellationToken` down into that database query. If the DB locks up,
 * the cache miss will hang forever, consuming server threads until the API crashes.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddMemoryCache(op => op.SizeLimit = 100);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.MapControllers();
app.Run();
