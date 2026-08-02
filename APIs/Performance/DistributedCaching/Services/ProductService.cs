using System.Text.Json;
using CachingBasics.Data;
using CachingBasics.Entities;
using CachingBasics.Requests;
using CachingBasics.Responses;
using CachingBasics.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace M01.CachingInMemory.Services;

public class ProductService(AppDbContext context, IDistributedCache cache) : IProductService
{
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var cachekey = "products";
        var cacheData = await cache.GetStringAsync(cachekey);
        if (cacheData is not null)
            return JsonSerializer.Deserialize<List<ProductResponse>>(cacheData)!;
        var entities = await context.Products.ToListAsync();

        var productResponse = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
        };

        await cache.SetStringAsync(cachekey, JsonSerializer.Serialize(productResponse), options);
        return productResponse;
    }

    public async Task<ProductResponse?> GetProductByIdAsync(Guid productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        return product is null ? null : ProductResponse.FromModel(product);
    }

    public async Task<ProductResponse> AddProductAsync(ProductRequest request)
    {
        var product = new Product { Name = request.Name, Price = request.Price };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        await cache.RemoveAsync("products");

        return ProductResponse.FromModel(product);
    }

    public async Task UpdateProductAsync(Guid productId, ProductRequest request)
    {
        var existingProduct =
            await context.Products.FirstOrDefaultAsync(p => p.Id == productId)
            ?? throw new KeyNotFoundException("product not found");

        existingProduct.Name = request.Name;

        existingProduct.Price = request.Price;

        await context.SaveChangesAsync();
        await cache.RemoveAsync("products");
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product =
            await context.Products.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);

        await context.SaveChangesAsync();
        cache.Remove("products");
    }
}
