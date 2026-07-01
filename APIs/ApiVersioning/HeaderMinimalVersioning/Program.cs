using Asp.Versioning;
using MinimalVersioning.Data;
using MinimalVersioning.EndPoints.V1;
using MinimalVersioning.EndPoints.V2;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<ProductRepository>();
        builder.Services.AddApiVersioning(op =>
        {
            op.DefaultApiVersion = new ApiVersion(1, 0);
            op.AssumeDefaultVersionWhenUnspecified = true;
            op.ReportApiVersions = true;
            op.ApiVersionReader = new HeaderApiVersionReader("Api-Version");
        });
        var app = builder.Build();
        //ApiVersionSet is a concept used to define and group a collection of API versions for Minimal APIs using the modern Asp.Versioning.Http package.
        //It tells the framework which specific API versions are assigned to a route group or endpoint,
        //preventing you from having to type attributes over every single method
        var apiversionset = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();
        app.MapProductEndPointsV2(apiversionset);
        app.MapProductEndPointsV1(apiversionset);
        app.Run();
    }
}
