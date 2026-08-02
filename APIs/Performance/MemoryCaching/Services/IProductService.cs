

using CachingBasics.Requests;
using CachingBasics.Responses;

namespace CachingBasics.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync();

    Task<ProductResponse?> GetProductByIdAsync(Guid productId);

    Task<ProductResponse> AddProductAsync(ProductRequest request);

    Task UpdateProductAsync(Guid productId, ProductRequest request);

    Task DeleteProductAsync(Guid id);
}
