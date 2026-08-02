using System.Data;
using System.Threading.RateLimiting;
using CachingBasics.Data;
using CachingBasics.Services;
using M01.CachingInMemory.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
/*
 * ==========================================================================================
 * RATE LIMITING 
 * ==========================================================================================
 * 
 * 1. WHAT IS RATE LIMITING?
 * Rate limiting is a defensive mechanism that controls the amount of traffic an API can 
 * receive within a specific timeframe or at a specific moment. 
 * 
 * Why we use it (The 4 Pillars of Protection):
 * - Security: Stops brute-force attacks (e.g., guessing passwords).
 * - Stability: Prevents connection pool exhaustion and server crashes (DoS protection).
 * - Fairness: Ensures one spamming user doesn't consume 100% of the server's CPU.
 * - Cost Control: Prevents massive bills if your API calls paid third-party services.
 * 
 * ------------------------------------------------------------------------------------------
 * 2. HIGH-LEVEL IMPLEMENTATION
 * 
 * Step 1: Register the Service (`builder.Services.AddRateLimiter()`) and define named policies.
 * Step 2: Add the Middleware (`app.UseRateLimiter()`). **CRITICAL**: This must go after 
 *         routing (`UseRouting`) but before authorization (`UseAuthorization`).
 * Step 3: Apply to Controllers using `[EnableRateLimiting("PolicyName")]`.
 * 
 * ------------------------------------------------------------------------------------------
 * 3. PRODUCTION NOTES & PARAMETERS
 * 
 * - QueueLimit = 0: If a user exceeds the limit, they are instantly rejected.
 * - QueueLimit > 0: If a user exceeds the limit, the server puts them "on hold" in a line 
 *   until a slot opens up.
 * - 429 Too Many Requests: The standard HTTP status code for hitting a rate limit.
 * - Partitioning (Advanced): The policies below are GLOBAL (shared by everyone). In a real 
 *   production app, you wrap these in `RateLimitPartition.Get...` to create a separate 
 *   counter for each user's IP Address or JWT ID.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // ========================================================================
    // 1. FIXED WINDOW LIMITER
    // ========================================================================
    // Definition: Allows a strict number of requests in a fixed time window. 
    // Once the limit is reached, further requests are rejected until the next window starts.
    // Flaw: Vulnerable to "edge spikes" (e.g., max requests sent at 0:59 and 1:00).
    // Use Case: Preventing brute-force attacks on a /login endpoint (e.g., 5 attempts per minute).
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // ========================================================================
    // 2. SLIDING WINDOW LIMITER
    // ========================================================================
    // Definition: Smooths out the Fixed Window by dividing the time into segments. 
    // It prevents edge spikes by checking traffic over a rolling timeframe rather than a hard reset.
    // Use Case: Standard, general-purpose API throttling for public endpoints to ensure fair use.
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.PermitLimit = 2; // Max requests in the full 10-second window
        opt.Window = TimeSpan.FromSeconds(10);
        opt.SegmentsPerWindow = 2; // Divides the 10s window into two 5s segments for smoother rolling
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // ========================================================================
    // 3. TOKEN BUCKET LIMITER
    // ========================================================================
    // Definition: A bucket holds a maximum capacity of tokens. Requests consume tokens. 
    // Tokens are steadily replenished over time. 
    // Use Case: APIs that require steady traffic but need to allow sudden "bursts" of activity.
    // For example, an SMS service where a user usually sends 1 text an hour, but occasionally 
    // needs to send a burst of 10 texts at once.
    options.AddTokenBucketLimiter("token", opt =>
    {
        opt.TokenLimit = 2; // Maximum bucket capacity (the absolute max burst allowed)
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10); // Bucket is refilled every 10 seconds
        opt.TokensPerPeriod = 1; // Adds 1 token per cycle
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // ========================================================================
    // 4. CONCURRENCY LIMITER
    // ========================================================================
    // Definition: Time does not matter here. It strictly limits how many requests 
    // can execute at the exact same millisecond. 
    // Use Case: Extremely heavy CPU/Memory operations. For example, a /generate-pdf endpoint 
    // or a massive database report. You limit it so only 2 users can run the report simultaneously, 
    // queuing the rest so the server doesn't crash from memory exhaustion.
    options.AddConcurrencyLimiter("concurrency", opt =>
    {
        opt.PermitLimit = 2; // Only 2 requests can process simultaneously
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2; // Up to 2 requests can wait in line
    });



    options.AddPolicy("UserId", context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.User.Identity?.Name ?? "anynoumous",
                factory: partition => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 2,
                    Window = TimeSpan.FromSeconds(10),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 2
                });

    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.UseResponseCaching();

// CRITICAL FIX: You MUST add the Rate Limiter middleware, otherwise the rules above are ignored!
app.UseRateLimiter();

app.MapControllers();
app.Run();
