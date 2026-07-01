using RestfulApi.Models;

namespace RestfulApi.Responses;

public class ProductResponse
{
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public IEnumerable<ProductReviewResponse>? Reviews { get; set; } = default;

    private ProductResponse() { }

    public static ProductResponse FromModel(
        Product? product,
        IEnumerable<ProductReview>? reviews = null
    )
    {
        if (product is null)
            throw new ArgumentNullException(
                nameof(product),
                "Cannot create ProductResponse from null Product"
            );

        var productresponse = new ProductResponse
        {
            ProductId = product.Id,
            Name = product?.Name,
            Price = product.Price,
        };
        if (reviews != null)
            productresponse.Reviews = ProductReviewResponse.FromModel(reviews);
        return productresponse;
    }

    public static IEnumerable<ProductResponse> FromModel(IEnumerable<Product> products)
    {
        if (products is null || !products.Any())
            throw new ArgumentNullException(
                nameof(products),
                "Cannot convert null IEnumerable<Product> to IEnumerable<ProductResponse>."
            );

        return products.Select(p => FromModel(p));
    }
}
