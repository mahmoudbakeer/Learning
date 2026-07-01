using RestfulApi.Controllers;
using RestfulApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();
app.MapProductEndPoints();
app.Run();
