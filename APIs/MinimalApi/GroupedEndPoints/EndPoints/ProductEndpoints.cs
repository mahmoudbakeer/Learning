using Microsoft.AspNetCore.Routing;

namespace GroupedEndpoints.EndPoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productapi = app.MapGroup("/api/products");

        productapi.MapGet(
            "/",
            () =>
            {
                return Results.Ok();
            }
        );

        productapi.MapGet(
            "/{id:int}",
            (int id) =>
            {
                return Results.Ok();
            }
        );
        productapi.MapPost(
            "/{id:int}",
            (int id) =>
            {
                return Results.Created();
            }
        );

        productapi.MapDelete(
            "/{id:int}",
            (int id) =>
            {
                return Results.NoContent();
            }
        );
        productapi.MapPut(
            "/",
            () =>
            {
                return Results.NoContent();
            }
        );
        return productapi;
    }
}
