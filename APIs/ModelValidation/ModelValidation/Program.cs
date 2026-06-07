using Microsoft.AspNetCore.Mvc;
using ModelValidation;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/register", (Registeration registeration) =>
    {
        return $"registeration successfully done for the user {registeration.Email}.";
    }).WithParameterValidation();

    endpoints.MapPost("/register", ([FromBody]
        Registeration registeration) =>
    {
        return $"registeration successfully done for the user {registeration.Email}.";
    }).WithParameterValidation();
});

app.Run();
