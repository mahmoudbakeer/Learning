var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/Get-by-Name", (IConfiguration config) =>
{
    return config["ServerName"];
});

app.MapGet("/Get-by-Path", (IConfiguration config) =>
{
    return config["ConnectionStrings:DefaultString"];
});

app.MapGet("/Get-by-Value", (IConfiguration config) =>
{
    return config.GetConnectionString("DefaultString");
});



app.MapGet("/Get-Section", (IConfiguration config) =>
{
    return config.GetSection(appsettings.ConfigName).Get<appsettings>();
});

app.MapGet("/Get-Section-Bind", (IConfiguration config) =>
{
    appsettings appsettings = new ();
    config.GetSection(appsettings.ConfigName).Bind(appsettings);
    return appsettings;
});
app.Run();


public class appsettings
{
    public const string ConfigName = "OptionsSettings";

    public string Name { get; set; }
    public int Age { get; set; }
    public TimeSpan Time { get; set; }
    public bool isMale { get; set; }
}
