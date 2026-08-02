using System.Runtime.Intrinsics;
using System.Runtime.Serialization;
using ControllerProjectManagement.Data;
using ControllerProjectManagement.DependencyInjection;
using MinimalProjectManagement.Endpoints;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Project API V1");
        options.SwaggerEndpoint("/openapi/v2.json", "Project API V2");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();
    });
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppDbContextInitializer.InitializeAsync(context);
}
app.MapProjectEndpoints();
app.Run();
