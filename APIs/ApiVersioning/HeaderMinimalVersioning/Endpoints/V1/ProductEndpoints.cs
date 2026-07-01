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

        DefaultApi.MapGet("/{Id:int}", GetProductById).WithName("GetProductByIdV1");
        return DefaultApi;
    }

    public static IResult GetProductById(int Id, ProductRepository pr)
    {
        var product = pr.GetProductById(Id);

        Console.WriteLine(product == null ? "Product is null" : product.Name);

        return product is null
            ? Results.NotFound()
            : Results.Ok(ProductResponse.FromModel(product));
    }
}
