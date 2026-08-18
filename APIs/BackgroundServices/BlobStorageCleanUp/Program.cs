using BackgroundServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<BlobStorageCleanUpBackgroundServices>();
var app = builder.Build();

app.Run();
