using System.ComponentModel.DataAnnotations;

namespace DeveloperExceptionPage.Endpoints;

public static class ErrorEndpoints
{
    public static RouteGroupBuilder MapErrorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal");

        group.MapGet(
            "/server-error",
            () =>
            {
                System.IO.File.ReadAllText(@"C:\Settings\UploadSettings.json"); // not exist
                Results.Created();
            }
        );
        group.MapPost("/bad-request", () => Results.BadRequest());
        group.MapPost("/not-found", () => Results.NotFound());
        group.MapPost("/unauthorized", () => Results.Unauthorized());
        group.MapPost("/conflict", () => Results.Conflict());
        group.MapPost(
            "/business-rule-error",
            () =>
            {
                throw new ValidationException("A discontinued product cannot be put on promotion.");
            }
        );

        return group;
    }
}
