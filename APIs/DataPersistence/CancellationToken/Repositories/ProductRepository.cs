using Microsoft.EntityFrameworkCore;
using RestfulApi.Data;
using RestfulApi.Models;
using RestfulApi.Repositories.Interfaces;

namespace RestfulApi.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<int> ProductCountsAsync(CancellationToken ct = default) =>
        await context.Products.CountAsync();

    public async Task<List<Product>> GetProductsPageAsync(
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default
    )
    {
        var products = await context
            .Products.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId, ct);

        if (product is null)
            return null;

        return product;
    }

    public async Task<IEnumerable<ProductReview>> GetProductReviewsAsync(
        Guid productId,
        CancellationToken ct = default
    )
    {
        var results = await context
            .ProductReviews.Where(r => r.ProductId == productId)
            .ToListAsync(ct);
        return results;
    }

    public async Task<ProductReview?> GetReviewAsync(
        Guid productId,
        Guid reviewId,
        CancellationToken ct = default
    )
    {
        var results = await context.ProductReviews.FirstOrDefaultAsync(
            r => r.ProductId == productId && r.Id == reviewId,
            ct
        );
        return results;
    }

    public async Task<bool> AddProductAsync(Product product, CancellationToken ct = default)
    {
        await context.Products.AddAsync(product, ct);

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> AddProductReviewAsync(
        ProductReview review,
        CancellationToken ct = default
    )
    {
        if (!await context.Products.AnyAsync(p => p.Id == review.ProductId, ct))
            return false;

        await context.ProductReviews.AddAsync(review, ct);

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateProductAsync(
        Product updatedProduct,
        CancellationToken ct = default
    )
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(
            p => p.Id == updatedProduct.Id,
            ct
        );

        if (existingProduct == null)
            return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id, CancellationToken ct = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

        if (product == null)
            return false;

        context.Products.Remove(product);

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Products.AnyAsync(p => p.Id == id, ct);

    public async Task<bool> ExistsByNameAsync(string? name, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        return await context.Products.AnyAsync(
            p => EF.Functions.Like(p.Name!.ToLower(), name.ToLower()),
            ct
        );
    }
}
