namespace ResponseGeneration.Models;

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Stars { get; set; }
    public string? Reviewer { get; set; }
}
