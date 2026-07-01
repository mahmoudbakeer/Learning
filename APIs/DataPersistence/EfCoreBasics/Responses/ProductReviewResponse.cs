using RestfulApi.Models;

namespace RestfulApi.Responses;

public class ProductReviewResponse
{
    public Guid ReviewId { get; set; }
    public Guid ProductId { get; set; }
    public int Stars { get; set; }
    public string? Reviewer { get; set; }

    private ProductReviewResponse() { }

    public static ProductReviewResponse FromModel(ProductReview? productReview)
    {
        if (productReview is null)
            throw new ArgumentNullException(nameof(productReview), "The ProductReview is null.");

        return new ProductReviewResponse
        {
            ReviewId = productReview.Id,
            ProductId = productReview.ProductId,
            Stars = productReview.Stars,
            Reviewer = productReview?.Reviewer,
        };
    }

    public static IEnumerable<ProductReviewResponse> FromModel(IEnumerable<ProductReview>? reviews)
    {
        if (reviews is null || !reviews.Any())
            throw new ArgumentNullException(
                nameof(reviews),
                "The list of reviews is null or empty."
            );

        return reviews.Select(r => FromModel(r));
    }
}
