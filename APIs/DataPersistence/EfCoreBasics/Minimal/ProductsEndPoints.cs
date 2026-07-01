using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using RestfulApi.Data;
using RestfulApi.Models;
using RestfulApi.Requests;
using RestfulApi.Responses;

namespace RestfulApi.Controllers;

public static class ProductsEndPoints
{
    public static RouteGroupBuilder MapProductEndPoints(this IEndpointRouteBuilder app)
    {
        // Create a route group so all endpoints share the same base route.
        // Every endpoint below will be prefixed with /api/products.
        var appProducts = app.MapGroup("/api/products");

        // Minimal APIs do not provide a MapOptions() helper,
        // so OPTIONS requests are registered manually.
        appProducts.MapMethods("", ["OPTIONS"], ProductsOptions);

        // Minimal APIs do not provide a MapHead() helper,
        // so HEAD requests are registered manually.
        appProducts.MapMethods("{productId:Guid}", ["HEAD"], ProductHead);

        appProducts.MapGet("{productId:Guid}", GetProductById).WithName(nameof(GetProductById));

        appProducts.MapGet("", GetPaged);
        appProducts.MapPost("", CreateProduct);
        appProducts.MapPut("{productId:Guid}", UpdateProduct);
        appProducts.MapPatch("{productId:Guid}", PatchProductAsync);
        appProducts.MapPost("{productId:Guid}/Reviews", CreateProductReviews);
        appProducts.MapDelete("{productId:Guid}", DeleteProduct);

        appProducts.MapGet("csv", GetCsvFile);

        // Avoid starting with '/' inside a route group,
        // otherwise the route becomes absolute.
        appProducts.MapGet("Temporary", GetTempProduct);

        appProducts.MapGet("Legacy", GetLegcyProduct);

        return appProducts;
    }

    private static IResult ProductsOptions(HttpResponse response)
    {
        // Tell the client which HTTP methods are supported.
        response.Headers.Append("Allow", "GET, HEAD, POST, PUT, PATCH, DELETE, OPTIONS");

        return Results.NoContent();
    }

    private static IResult ProductHead(Guid productId, ProductRepository productRepository)
    {
        // HEAD requests are commonly used to check resource existence
        // without downloading the full response body.
        return productRepository.ExistsById(productId) ? Results.Ok() : Results.NotFound();
    }

    private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(
        Guid productId,
        ProductRepository productRepository,
        bool includeReviews = false
    )
    {
        var product = productRepository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound("Product not found.");

        IEnumerable<ProductReview>? reviews = null;

        // Load reviews only when requested.
        // This avoids unnecessary database work.
        if (includeReviews)
        {
            reviews = productRepository.GetProductReviews(productId);
        }

        return TypedResults.Ok(ProductResponse.FromModel(product, reviews));
    }

    private static IResult CreateProductReviews(
        Guid productId,
        CreateProductReviewRequest reviews,
        ProductRepository productRepository
    )
    {
        var product = productRepository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound($"Product with id '{productId}' not found.");

        var productReview = new ProductReview
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Reviewer = reviews.Reviewer,
            Stars = reviews.Stars,
        };

        productRepository.AddProductReview(productReview);

        return Results.Created(
            $"/api/products/{productReview.ProductId}/reviews",
            ProductReviewResponse.FromModel(productReview)
        );
    }

    private static IResult GetPaged(ProductRepository productRepository, int page, int pageSize)
    {
        // Prevent invalid page numbers.
        page = Math.Max(1, page);

        // Limit page size to protect the API
        // from very large requests.
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = productRepository.GetProductsCount();

        var products = productRepository.GetProductsPage(page, pageSize);

        var result = PagedResult<ProductResponse>.Create(
            ProductResponse.FromModel(products),
            totalCount,
            page,
            pageSize
        );

        return Results.Ok(result);
    }

    private static Results<CreatedAtRoute<ProductResponse>, Conflict<string>> CreateProduct(
        CreateProductRequest product,
        ProductRepository productRepository
    )
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");

        // Prevent duplicate products by name.
        if (productRepository.ExistsByName(product.Name))
        {
            return TypedResults.Conflict($"Product '{product.Name}' already exists.");
        }

        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = product.Name,
            Price = product.Price,
        };

        productRepository.AddProduct(newProduct);

        // CreatedAtRoute generates a Location header
        // pointing to the newly created resource.
        return TypedResults.CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { productId = newProduct.Id },
            value: ProductResponse.FromModel(newProduct)
        );
    }

    private static IResult UpdateProduct(
        Guid productId,
        UpdateProductRequest product,
        ProductRepository productRepository
    )
    {
        var repoProduct = productRepository.GetProductById(productId);

        if (repoProduct is null)
        {
            return Results.NotFound($"Product with id '{productId}' does not exist.");
        }

        // PUT replaces the resource state with
        // the values supplied by the client.
        repoProduct.Name = product.Name;
        repoProduct.Price = product.Price ?? 0;

        var succeeded = productRepository.UpdateProduct(repoProduct);

        return succeeded ? Results.NoContent() : Results.StatusCode(500);
    }

    private static async Task<IResult> PatchProductAsync(
        Guid productId,
        ProductRepository productRepository,
        HttpRequest request
    )
    {
        // Read the raw JSON Patch document.
        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync();

        var productDoc = JsonConvert.DeserializeObject<JsonPatchDocument<UpdateProductRequest>>(
            json
        );

        if (productDoc is null)
            return Results.BadRequest("Patch document cannot be null.");

        var product = productRepository.GetProductById(productId);

        if (product is null)
            return Results.NotFound("Product not found.");

        // Create a DTO initialized with current values.
        // Patch operations are applied to the DTO first
        // rather than directly modifying the entity.
        var productRequest = new UpdateProductRequest
        {
            Name = product.Name,
            Price = product.Price,
        };

        try
        {
            productDoc.ApplyTo(productRequest);
        }
        catch
        {
            return Results.BadRequest("Invalid patch operation.");
        }

        // Copy patched values back to the entity.
        product.Name = productRequest.Name;
        product.Price = productRequest.Price ?? 0;

        var success = productRepository.UpdateProduct(product);

        return success ? Results.NoContent() : Results.StatusCode(500);
    }

    private static IResult DeleteProduct(Guid productId, ProductRepository productRepository)
    {
        if (!productRepository.ExistsById(productId))
        {
            return Results.NotFound($"Product with id '{productId}' does not exist.");
        }

        var success = productRepository.DeleteProduct(productId);

        return success ? Results.NoContent() : Results.StatusCode(500);
    }

    private static IResult GetCsvFile(ProductRepository productRepository)
    {
        var products = productRepository.GetProductsPage(1, 100);

        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("Id,Name,Price");

        foreach (var product in products)
        {
            csvBuilder.AppendLine($"{product.Id},{product.Name},{product.Price}");
        }

        // Convert the CSV text into bytes
        // so it can be returned as a downloadable file.
        var encoded = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return Results.File(encoded, "text/csv", "products.csv");
    }

    private static IResult GetTempProduct()
    {
        return Results.Ok(new { Result = "You are in the right path." });
    }

    private static IResult GetLegcyProduct()
    {
        // Redirect the client to the new endpoint.
        // permanent: false => 302 Found
        // permanent: true  => 301 Moved Permanently
        return Results.Redirect("/api/products/Temporary");
    }
}
