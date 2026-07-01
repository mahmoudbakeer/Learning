using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Data;

/*
Benefits of Using Async Methods in .NET

1. Improves UI Responsiveness
   - Prevents desktop applications from freezing during long-running I/O operations.

2. Increases Web Application Scalability
   - Frees request threads while waiting for I/O, allowing the server to handle more concurrent requests.

3. Uses Threads More Efficiently
   - Async methods do not block threads while waiting for operations like database or network calls to complete.

4. Supports Non-Blocking I/O
   - Ideal for file access, database queries, HTTP requests, and other I/O-bound tasks.

5. Improves Throughput
   - More requests can be processed with the same number of threads, reducing thread pool exhaustion.

6. Produces Cleaner, More Readable Code
   - async/await makes asynchronous code easier to understand and maintain than callbacks or continuations.

7. Enables Concurrent Execution
   - Independent asynchronous operations can run simultaneously using Task.WhenAll().

8. Simplifies Exception Handling
   - Exceptions can be handled naturally using standard try/catch blocks.

9. Supports Cancellation
   - Many async APIs accept a CancellationToken, allowing operations to be canceled gracefully.

10. Optimizes Resource Utilization
    - Reduces blocked threads, memory usage, and unnecessary context switching.

Note:
- Async does NOT make an operation inherently faster.
- It improves responsiveness and scalability by avoiding blocked threads during I/O-bound operations.
- Use async primarily for I/O-bound work (database, file system, network, APIs), not for short CPU-bound computations.
*/

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlite("Data Source = app.db");
});
var app = builder.Build();
app.MapProductEndPoints();
app.Run();
