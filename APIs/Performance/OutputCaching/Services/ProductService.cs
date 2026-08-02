using System.Text.Json;
using CachingBasics.Data;
using CachingBasics.Entities;
using CachingBasics.Requests;
using CachingBasics.Responses;
using CachingBasics.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;

namespace M01.CachingInMemory.Services;

public class ProductService(AppDbContext context) : IProductService
{
    public async Task<List<ProductResponse>> GetProductsAsync(int page = 1, int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var entities = await context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var productResponse = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
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
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product =
            await context.Products.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);

        await context.SaveChangesAsync();
    }
}
