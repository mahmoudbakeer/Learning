namespace RestfulApi.Repositories.Interfaces;

using RestfulApi.Models;

public interface IProductRepository
{
    void AddProduct(Product product);
    Task AddProductReviewAsync(ProductReview review);
    Task DeleteProductAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> ExistsByNameAsync(string? name);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<ProductReview>> GetProductReviewsAsync(Guid productId);
    Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10);
    Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId);
    Task<int> ProductCountsAsync();
    Task UpdateProductAsync(Product updatedProduct);
}
