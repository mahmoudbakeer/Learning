using System.Data;
using System.IO.Compression;
using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
/*
 * ==========================================================================================
 * ASP.NET CORE ARCHITECTURE: RESPONSE COMPRESSION MASTER GUIDE
 * ==========================================================================================
 * 
 * 1. WHAT IS RESPONSE COMPRESSION?
 * Response compression dynamically shrinks the size of your HTTP responses (JSON, HTML, 
 * CSS, JS) before sending them over the network to the client's browser.
 * 
 * Why we use it:
 * - Bandwidth Optimization: Drastically reduces payload size (often by 70-80% for JSON).
 * - Faster Load Times: Smaller payloads travel across the network faster.
 * - Cloud Cost Savings: Reduces outbound data transfer costs from your cloud provider.
 * 
 * ------------------------------------------------------------------------------------------
 * 2. THE ALGORITHMS (Providers)
 * 
 * A. Brotli (br)
 *    - The modern standard developed by Google.
 *    - Provides significantly better compression ratios than Gzip.
 *    - ASP.NET Core prioritizes Brotli automatically if the client browser supports it.
 * 
 * B. Gzip (gzip)
 *    - The legacy standard.
 *    - Used as a fallback for older clients, tools, or proxies that don't support Brotli.
 * 
 * Compression Levels:
 * - Fastest (Default): Compresses quickly to save CPU, but the payload isn't as small.
 * - Optimal: Squeezes the file as small as possible, but consumes more Server CPU.
 * - SmallestSize: Maximizes compression regardless of time.
 * 
 * ------------------------------------------------------------------------------------------
 * 3. HIGH-LEVEL IMPLEMENTATION
 * 
 * Step 1: Register the Service -> `builder.Services.AddResponseCompression(options => ...)`
 * Step 2: Configure Providers -> Add Brotli and Gzip and set their compression levels.
 * Step 3: Add the Middleware -> `app.UseResponseCompression()`. 
 *         **CRITICAL ORDERING**: Place this BEFORE any middleware that writes to the 
 *         response body (like `MapControllers` or `UseStaticFiles`).
 * 
 * ------------------------------------------------------------------------------------------
 * 4. CRITICAL PRODUCTION NOTES & SECURITY WARNINGS
 * 
 * -  DO NOT Compress Native Binary Files: 
 *   Never compress Images (PNG, JPG), PDFs, Videos, or Zip files. They are already 
 *   compressed. Trying to compress them again burns Server CPU and can actually make 
 *   the final file BIGGER.
 * 
 * -  The CPU vs. Bandwidth Trade-off:
 *   Compression is CPU-intensive. Do not compress tiny payloads (under 1KB). The CPU 
 *   cost of compressing it is worse than the microsecond of network time saved.
 * 
 * -  SECURITY WARNING (CRIME / BREACH Attacks) :
 *   By default, ASP.NET Core completely disables compression over HTTPS. Why? If you mix 
 *   compression + HTTPS + reflected user input + sensitive secrets (like Anti-Forgery 
 *   Tokens), hackers can mathematically deduce the secret by measuring the compressed 
 *   payload size byte-by-byte. 
 * 
 *   How to handle this: You can set `options.EnableForHttps = true` safely ONLY IF you 
 *   are building a standard JSON API where you aren't rendering secrets alongside user 
 *   input in HTML pages.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    // there is multiple levels of compression, we can choose the optimal one for our use case
    // such as CompressionLevel.Optimal, CompressionLevel.Fastest, CompressionLevel.NoCompression
    options.Level = CompressionLevel.Optimal;
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();
app.UseResponseCompression();
app.MapControllers();
app.Run();
