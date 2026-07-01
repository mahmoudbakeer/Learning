using Asp.Versioning;
using Asp.Versioning.Builder;
using MinimalVersioning.Data;
using MinimalVersioning.Responses.V2;

namespace MinimalVersioning.EndPoints.V2;

public static class ProductEndPoints
{
    public static RouteGroupBuilder MapProductEndPointsV2(
        this IEndpointRouteBuilder app,
        ApiVersionSet apiVersionSet
    )
    {
        var productApi = app.MapGroup("/api/products").WithApiVersionSet(apiVersionSet);
        productApi
            .MapGet("/{Id:int}", GetProductById)
            .HasApiVersion(new ApiVersion(2, 0))
            .WithName("GetProductByIdV2");

        return productApi;
    }

    public static IResult GetProductById(int Id, ProductRepository pr)
    {
        Console.WriteLine($"Endpoint hit! Id = {Id}");

        var product = pr.GetProductById(Id);

        Console.WriteLine(product == null ? "Product is null" : product.Name);

        return product is null
            ? Results.NotFound()
            : Results.Ok(ProductResponse.FromModel(product));
    }
}
