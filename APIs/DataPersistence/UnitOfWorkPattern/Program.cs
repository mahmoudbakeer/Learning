using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Data;
using RestfulApi.Repositories;
using RestfulApi.Repositories.Interfaces;

/*
 * ==========================================================================================
 * THE UNIT OF WORK PATTERN (AND THE DBCONTEXT DEBATE)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * An architectural pattern that groups one or more database operations into a single
 * transaction. It acts as a single entry point for your Repositories, ensuring they all
 * share the exact same database connection (DbContext).
 *
 * WHY WE USE IT:
 * 1. Atomicity (All or Nothing): If you need to add a Product AND a ProductReview in one
 * request, it ensures both succeed together. If one fails, neither is saved.
 * 2. Shared Context: Prevents the dangerous bug of injecting different DbContext instances
 * into different repositories, which causes Entity Framework tracking errors.
 * 3. Fewer Database Calls: Instead of calling `SaveChangesAsync()` inside every repository,
 * you do all your work in memory and call `CompleteAsync()` just once at the end.
 *
 * THE BIG DEBATE: JUST USE DBCONTEXT?
 * By definition, EF Core's DbContext IS ALREADY a Unit of Work, and DbSet<T> is already
 * a Repository. If you save multiple entities via DbContext, EF Core automatically wraps
 * them in a single SQL transaction anyway. Because of this, wrapping DbContext in a custom
 * IUnitOfWork is often considered unnecessary "wrapper" boilerplate.
 *
 * THEREFORE, DEFAULT TO INJECTING DBCONTEXT DIRECTLY... *UNLESS*:
 * 1. Strict Domain-Driven Design (DDD) / Clean Architecture: Your Core domain is absolutely
 * not allowed to have a NuGet dependency on Entity Framework. You MUST use custom
 * interfaces to bridge the gap.
 * 2. Complex Queries: You need to hide a massive, 30-line LINQ query behind a clean
 * repository method to keep your API endpoints readable.
 * 3. Multi-Source Transactions: A single request needs to save to SQL Server (EF Core) AND
 * upload a file to AWS S3 AND publish a message to RabbitMQ. A custom Unit of Work
 * is required to orchestrate all three technologies together.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS (IF BUILDING CUSTOM):
 * 1. Define Interface: Create `IUnitOfWork` exposing your repository interfaces as properties
 * and a single `Task<int> CompleteAsync(CancellationToken ct)` method.
 * 2. Create Implementation: Build a concrete `UnitOfWork` class that injects your DbContext
 * and passes that single instance down to your repository implementations.
 * 3. Register in DI: Map it using builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
 * 4. Refactor Repositories: Remove `SaveChangesAsync()` from individual repositories.
 * 5. Use in API: Inject `IUnitOfWork` into your endpoints, do your operations, and call
 * `await uow.CompleteAsync()` to commit everything to the database.
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlite("Data Source = app.db");
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();
app.MapProductEndPoints();
app.Run();
