using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Data;
using RestfulApi.Repositories;
using RestfulApi.Repositories.Interfaces;

/*
 * ==========================================================================================
 * THE REPOSITORY PATTERN
 * ==========================================================================================
 *
 * WHAT IT IS:
 * An architectural design pattern that acts as a middleman (abstraction layer) between
 * the application's business logic (API Endpoints/Controllers) and the data access
 * layer (Entity Framework Core / Database).
 *
 * WHY WE USE IT:
 * 1. Separation of Concerns: Endpoints only handle HTTP requests/responses. They don't
 * need to know how to write database queries.
 * 2. Testability: By depending on an interface instead of a direct database connection
 * (DbContext), we can easily mock or fake the data layer for unit testing our APIs.
 * 3. Centralized Logic: Complex queries (e.g., LINQ) are written in one place. If the
 * database schema changes, you only update the repository, not every controller.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Define an Interface: Create a contract (e.g., IProductRepository) outlining the
 * allowed data operations (GetAll, GetById, Add, etc.).
 * 2. Create the Implementation: Build a concrete class (e.g., ProductRepository) that
 * implements the interface. This class injects EF Core's ApplicationDbContext to
 * perform the actual database operations.
 * 3. Register in DI: In Program.cs, map the interface to the implementation using
 * builder.Services.AddScoped<IProductRepository, ProductRepository>();
 * 4. Inject and Use: Inject the interface into your Minimal API endpoints or MVC
 * Controllers to access data cleanly without directly coupling to Entity Framework.
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
