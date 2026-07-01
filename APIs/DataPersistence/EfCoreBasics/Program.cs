using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlite("Data Source = app.db");
});
var app = builder.Build();
app.MapProductEndPoints();
app.Run();
