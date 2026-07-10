namespace RestfulApi.Repositories.Interfaces;

using RestfulApi.Models;

public interface IProductRepository
{
    Task<bool> AddProductAsync(Product product, CancellationToken ct = default);
    Task<bool> AddProductReviewAsync(ProductReview review, CancellationToken ct = default);
    Task<bool> DeleteProductAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string? name, CancellationToken ct = default);
    Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct = default);
    Task<IEnumerable<ProductReview>> GetProductReviewsAsync(
        Guid productId,
        CancellationToken ct = default
    );
    Task<List<Product>> GetProductsPageAsync(
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default
    );
    Task<ProductReview?> GetReviewAsync(
        Guid productId,
        Guid reviewId,
        CancellationToken ct = default
    );
    Task<int> ProductCountsAsync(CancellationToken ct = default);
    Task<bool> UpdateProductAsync(Product updatedProduct, CancellationToken ct = default);
}
