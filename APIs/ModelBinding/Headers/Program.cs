using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

// whenever you want to recieve the date from header you have to explicitly add the [FromHeader] Attribute for it
// and as convension use X-HeaderName to distinguish it
app.MapGet(
    "/Products-minimal",
    ([FromHeader(Name = "X-Api-Version")] string ApiVersion) => $"The api version is {ApiVersion}"
);

app.Run();
