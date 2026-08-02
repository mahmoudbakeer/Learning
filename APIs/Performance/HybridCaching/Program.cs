using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

/*
 * ==========================================================================================
 * ASP.NET CORE PERFORMANCE: HYBRID CACHING (.NET 9+)
 * ==========================================================================================
 *
 * 1. WHAT IT IS:
 * HybridCache is a modern, unified caching API that automatically wraps and orchestrates 
 * both IMemoryCache (L1) and IDistributedCache (L2). It routes requests optimally without 
 * requiring the developer to write manual fallback logic or JSON serialization.
 *
 * 2. WHY WE USE IT (The 3 Major Solves):
 * - NO BOILERPLATE: Removes the messy `if/else` checks between RAM, Redis, and SQL.
 * - NO SERIALIZATION TAX: Automatically handles JsonSerializer operations for the L2 cache.
 * - STAMPEDE PROTECTION: Natively prevents the "Thundering Herd" problem (see below).
 *
 * 3. CACHE STAMPEDE (THUNDERING HERD) PROTECTION:
 * If a highly trafficked cache key expires, 1,000 concurrent users might query it at the 
 * exact same millisecond. 
 * - Old Way: All 1,000 requests miss the cache and hit SQL simultaneously, crashing the DB.
 * - HybridCache: Locks the key. 1 request is allowed to hit SQL. The other 999 wait a few 
 *   milliseconds and are instantly handed the result the moment the 1st request finishes.
 *
 * 4. THE IMPLEMENTATION PATTERN (GetOrCreateAsync):
 * return await _hybridCache.GetOrCreateAsync(
 *     "key",
 *     async cancelToken => await _db.GetDataAsync(cancelToken),
 *     cancellationToken: ct
 * );
 *
 * ==========================================================================================
 * ⚠️ CRITICAL ARCHITECTURAL WARNINGS & GOTCHAS ⚠️
 * ==========================================================================================
 *
 * A. THE IMMUTABILITY TRAP (L1 Danger):
 * Because HybridCache checks L1 (RAM) first, it returns a direct pointer to the C# object 
 * in memory. IF YOU MODIFY THIS OBJECT (e.g., `cachedProduct.Price = 50;`), you instantly 
 * modify it for EVERY user on the server. You MUST treat cached objects as strictly read-only.
 *
 * B. THE SERIALIZATION LIMIT (L2 Danger):
 * Because HybridCache still writes to Redis (L2) in the background, your C# objects MUST 
 * be serializable. You cannot return Entity Framework objects that contain circular 
 * navigation properties (e.g., A Product that points to a Category that points to a Product), 
 * or the background serialization thread will crash.
 *
 * C. CACHE INVALIDATION:
 * Use `await _hybridCache.RemoveAsync("key")` on database writes. This will safely 
 * destroy the key in BOTH the local RAM and the distributed Redis cache simultaneously.
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
// this is the same service as redis so the one who register first wins. since they both use the same interface, so if you register the redis cache first, it will be used instead of the in-memory cache.
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("SqlServer");
    options.SchemaName = "dbo";
    options.TableName = "DistributedCache";
});
// hybrid caching is a combination of both in-memory and distributed caching. the in-memory cache is used to store the data in the memory of the application, while the distributed cache is used to store the data in a distributed cache like Redis or SQL Server. this way, if the application is restarted, the data will still be available in the distributed cache. and if the application is running on multiple instances, the data will be available to all instances.
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        LocalCacheExpiration = TimeSpan.FromSeconds(30),
        Expiration = TimeSpan.FromMinutes(10)
    };
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});


var app = builder.Build();

app.MapControllers();
app.Run();
