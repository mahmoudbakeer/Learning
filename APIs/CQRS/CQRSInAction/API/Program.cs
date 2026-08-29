
using CQRSInAction.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CQRSInAction.Application;
using FluentValidation;
using MediatR;
using CQRSInAction.Application.Behaviors;
using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source = app.db"));
builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(IAssemblyMarker).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
builder.Services.AddScoped<IAppDbContext, AppDbContext>();
var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();

app.Run();
