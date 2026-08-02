
using CachingBasics.Data;
using CachingBasics.Entities;
using CachingBasics.Requests;
using CachingBasics.Responses;
using CachingBasics.Services;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace M01.CachingInMemory.Services;

public class ProductService(AppDbContext context, IMemoryCache cache) : IProductService
{

    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        return await cache.GetOrCreate("products", async en =>
        {
            // when it reach here that means the cache has no value for the key to return, must get it from DB.
            en.Size = 1;
            en.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

            var entities = await context.Products.ToListAsync();

            var productResponse = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
            return productResponse;
        })!;

    }



    public async Task<ProductResponse?> GetProductByIdAsync(Guid productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        return product is null ? null : ProductResponse.FromModel(product);
    }

    public async Task<ProductResponse> AddProductAsync(ProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        cache.Remove("products");

        return ProductResponse.FromModel(product);
    }

    public async Task UpdateProductAsync(Guid productId, ProductRequest request)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                                ?? throw new KeyNotFoundException("product not found");

        existingProduct.Name = request.Name;

        existingProduct.Price = request.Price;

        await context.SaveChangesAsync();
        cache.Remove("products");


    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id)
                      ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);

        await context.SaveChangesAsync();
        cache.Remove("products");


    }
}
