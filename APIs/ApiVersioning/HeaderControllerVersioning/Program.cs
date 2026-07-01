using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using UrlControllerVersioning.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddControllers();

builder.Services.AddApiVersioning(op =>
{
    op.DefaultApiVersion = new ApiVersion(1, 0);
    op.AssumeDefaultVersionWhenUnspecified = true;
    op.ReportApiVersions = true;
    op.ApiVersionReader = new HeaderApiVersionReader("api-version");
});
var app = builder.Build();

app.MapControllers();

app.Run();
