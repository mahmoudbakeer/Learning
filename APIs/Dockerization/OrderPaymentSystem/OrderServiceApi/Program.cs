using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Data;
using OrderServiceApi.Repositories;
using OrderServiceApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddControllers()
    .AddJsonOptions(op =>
    {
        op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        op.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=OrderServiceApi.db")
);
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PaymentService:BaseUrl"]!);
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}
app.MapControllers();

app.Run();
