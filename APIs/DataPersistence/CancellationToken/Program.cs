using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Data;
using RestfulApi.Repositories;
using RestfulApi.Repositories.Interfaces;

/*
 * ==========================================================================================
 * CANCELLATION TOKENS
 * ==========================================================================================
 *
 * WHAT IT IS:
 * A lightweight struct (System.Threading.CancellationToken) used to signal that an
 * ongoing asynchronous operation should be aborted. In ASP.NET Core, it is tied directly
 * to the HTTP request lifecycle. If the client disconnects, the framework signals this token.
 *
 * WHY WE USE IT:
 * 1. Resource Management: If a user closes their browser or navigates away before a request
 * finishes, continuing to process a heavy database query or external API call wastes server CPU.
 * 2. Scalability: Aborting dead requests immediately frees up web server threads so they
 * can be used to serve other active users.
 * 3. Cost Savings: Prevents unnecessary compute time and database utilization, which can
 * save money in cloud environments (like Azure or AWS).
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Inject into Endpoint: Simply add `CancellationToken cancellationToken` as a parameter
 * to your Minimal API lambda or MVC Controller action. ASP.NET Core automatically provides it.
 * 2. Pass it Down the Chain: Pass this token through your API to your Services, and then
 * down to your Repository interfaces.
 * 3. Give it to EF Core / HttpClient: Pass the token into the final async methods
 * (e.g., await _context.Products.ToListAsync(cancellationToken)).
 * 4. Let the Framework Handle It: If the client disconnects, the token is triggered.
 * EF Core automatically aborts the SQL query and throws a TaskCanceledException,
 * which ASP.NET Core silently catches and cleans up gracefully.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlite("Data Source = app.db");
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();
app.MapProductEndPoints();
app.Run();
