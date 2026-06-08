using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.AppName));

var app = builder.Build();

// this will take snapshot at the startup of the project and return the configuration from the snapshot
// means that any change on the file during the app running won't be reflected result
app.MapGet("/ioptions", (IOptions<AppSettings> options) =>
{
    return options.Value;
});

// this will be updated per request (per scope)
// if you changed anythings during the request won't be reflected
// it must be before the request to be reflected
app.MapGet("/ioptions-snapshot", (IOptionsSnapshot<AppSettings> options) =>
{
    return options.Value;
});

// this will be updated per change any change will be directly reflected
app.MapGet("/ioptions-monitor", (IOptionsMonitor<AppSettings> options) =>
{
    return options.CurrentValue;
});
app.Run();


public class AppSettings
{
    public const string AppName = "AppSettings";

    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsMale { get; set; }
    public TimeSpan Time { get; set; }
}
