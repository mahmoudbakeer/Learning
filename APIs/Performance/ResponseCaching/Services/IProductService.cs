

using CachingBasics.Requests;
using CachingBasics.Responses;

namespace CachingBasics.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync(int page = 1, int pageSize = 10);

    Task<ProductResponse?> GetProductByIdAsync(Guid productId);

    Task<ProductResponse> AddProductAsync(ProductRequest request);

    Task UpdateProductAsync(Guid productId, ProductRequest request);

    Task DeleteProductAsync(Guid id);
}
