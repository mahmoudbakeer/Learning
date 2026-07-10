namespace RestfulApi.Models;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public decimal AverageRating { get; set; }
    public List<ProductReview> ProductReviews { get; set; } = [];
}
