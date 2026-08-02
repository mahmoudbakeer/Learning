using System.Data;
using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
/*
 * ==========================================================================================
 * ASP.NET CORE PERFORMANCE: RESPONSE CACHING & ETAGS
 * ==========================================================================================
 *
 * 1. WHAT IT IS (Output vs. Response Caching):
 * - Output Caching (Server-Side): Saves the response in the Server's RAM. 
 * - Response Caching (Client-Side): Uses HTTP Headers to tell the User's Browser or a 
 *   middleman CDN (like Cloudflare) to save the data on THEIR hard drive.
 *
 * 2. HTTP CACHE-CONTROL HEADERS (The [ResponseCache] Attribute):
 * The attribute directly manipulates the headers sent to the client:
 * 
 * - Duration = 60           => "Cache-Control: max-age=60" (Keep for 60 seconds).
 * - Location = Any          => "Cache-Control: public" (CDNs and Browsers can store it).
 * - Location = Client       => "Cache-Control: private" (ONLY the browser can store it).
 * - Location = None         => "Cache-Control: no-cache" (Must revalidate before using).
 * - NoStore = true          => "Cache-Control: no-store" (NEVER save this to disk).
 * - VaryByHeader = "Accept" => Caches different JSON based on what the client header sent.
 * - VaryByQueryKeys         => Caches different JSON for ?page=1 vs ?page=2. 
 *                              (Requires app.UseResponseCaching() in Program.cs).
 *
 * 3. ENTERPRISE PATTERN (Global Cache Profiles):
 * Don't repeat attributes on every controller. Define profiles in Program.cs:
 * 
 * builder.Services.AddControllers(options => {
 *     options.CacheProfiles.Add("PublicData", new CacheProfile { 
 *         Duration = 3600, Location = ResponseCacheLocation.Any 
 *     });
 * });
 * 
 * Usage: [ResponseCache(CacheProfileName = "PublicData")]
 *
 * ==========================================================================================
 *  ETAGS & CONDITIONAL GETS (Bandwidth Optimization)
 * ==========================================================================================
 * 
 * An ETag is a unique fingerprint (like a SHA256 Hash) of the data state.
 * 
 * THE 304 FLOW:
 * 1. Client asks for data. Server returns 200 OK + 5MB JSON + [ETag: "hash-123"].
 * 2. Client asks again later, sending header: [If-None-Match: "hash-123"].
 * 3. Server computes the current DB hash. Does it still match "hash-123"?
 *    - YES: Server returns 304 Not Modified (EMPTY BODY). Saves 5MB of network bandwidth!
 *    - NO : Server returns 200 OK + NEW 5MB JSON + [ETag: "hash-999"].
 *
 * NOTE: ASP.NET Core does not generate ETags automatically. You must manually compute 
 * the hash, check the Request.Headers["If-None-Match"], and return StatusCode(304).
 *
 * ==========================================================================================
 *  CRITICAL SECURITY WARNING 
 * ==========================================================================================
 * 
 * NEVER use `ResponseCacheLocation.Any` (Public) on endpoints that return authenticated 
 * or sensitive user data (like a shopping cart or bank balance). A public CDN will cache 
 * User A's private data and might accidentally serve it to User B. 
 * 
 * For sensitive data, ALWAYS use `ResponseCacheLocation.Client` (Private) or `NoStore = true`.
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddResponseCaching();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();
app.UseResponseCaching();
app.MapControllers();
app.Run();
