using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddControllers()
    .AddJsonOptions(op =>
    {
        op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var app = builder.Build();
app.MapControllers();

app.Run();
