using Asp.Versioning;
using Asp.Versioning.Builder;
using MinimalVersioning.Data;
using MinimalVersioning.Responses.V1;

namespace MinimalVersioning.EndPoints.V1;

public static class ProductEndPoints
{
    public static RouteGroupBuilder MapProductEndPointsV1(
        this IEndpointRouteBuilder app,
        ApiVersionSet apiVersionSet
    )
    {
        var DefaultApi = app.MapGroup("/api/products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        var productApi = app.MapGroup("/api/v{version:ApiVersion}/products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));
        productApi
            .MapGet("/{Id:int}", GetProductById)
            .HasApiVersion(new ApiVersion(1))
            .WithName("GetProductByIdV1");

        DefaultApi
            .MapGet("/{Id:int}", GetProductById)
            .HasApiVersion(new ApiVersion(1))
            .WithName("GetProductById");
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
