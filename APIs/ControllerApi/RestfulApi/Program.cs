using RestfulApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddControllers().AddNewtonsoftJson();
var app = builder.Build();
app.MapControllers();
app.Run();
