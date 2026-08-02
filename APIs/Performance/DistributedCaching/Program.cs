using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.EntityFrameworkCore;

/*
 * ==========================================================================================
 * ASP.NET CORE PERFORMANCE: DISTRIBUTED CACHING (L2 CACHE)
 * ==========================================================================================
 *
 * 1. WHAT IT IS:
 * An implementation of `IDistributedCache` that stores data on an external, centralized
 * server (typically Redis) rather than in the local web server's RAM.
 *
 * 2. WHY WE NEED IT (The Web Farm Problem):
 * In a multi-server environment behind a load balancer, if Server A caches data in local
 * memory, Server B cannot see it. By centralizing the cache in Redis, all API instances
 * share the exact same state, preventing duplicate database queries across the farm.
 * It also survives application restarts (unlike IMemoryCache, which wipes on reboot).
 *
 * 3. THE SERIALIZATION TAX (Crucial Difference):
 * Because the data leaves the C# process and travels over the network, you cannot store
 * raw C# objects. IDistributedCache strictly requires `byte[]` or `string`.
 * You MUST use `JsonSerializer.Serialize()` when writing to the cache, and
 * `JsonSerializer.Deserialize<T>()` when reading from the cache.
 *
 * 4. CACHE INVALIDATION:
 * Just like L1 caching, if a record is updated in the database, the old JSON in Redis
 * becomes Stale Data. You must call `await _cache.RemoveAsync("key")` on all Write/Update
 * operations to ensure consistency.
 *
 * ==========================================================================================
 * ⚠️ HYBRID CACHE NOTE (.NET 9) ⚠️
 * ==========================================================================================
 * Writing manual Serialization logic and checking `GetStringAsync` is becoming obsolete.
 * In .NET 9, Microsoft introduced `HybridCache`, which automatically wraps both `IMemoryCache`
 * and `IDistributedCache`. It handles the JSON serialization natively and prevents
 * Cache Stampedes automatically via thread locking. Always prefer HybridCache in modern apps.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache(); // this one because the docker did not work, so I used the in-memory cache instead of the distributed cache to just check the code 
// when you run the docker, you can use the distributed cache instead of the in-memory cache
// just remove the AddDistributedMemoryCache() and uncomment the code below to use the distributed cache with Redis
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetConnectionString("Redis");
//     options.InstanceName = "Distributed";
// });
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.MapControllers();
app.Run();
