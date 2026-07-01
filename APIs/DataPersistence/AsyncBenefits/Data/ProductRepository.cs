using Microsoft.EntityFrameworkCore;
using RestfulApi.Models;

namespace RestfulApi.Data;

public class ProductRepository(AppDbContext context)
{
    public async Task<int> ProductCountsAsync() => await context.Products.CountAsync();

    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10)
    {
        var products = await context
            .Products.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return null;

        return product;
    }

    public async Task<IEnumerable<ProductReview>> GetProductReviewsAsync(Guid productId)
    {
        var results = await context
            .ProductReviews.Where(r => r.ProductId == productId)
            .ToListAsync();
        return results;
    }

    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId)
    {
        var results = await context.ProductReviews.FirstOrDefaultAsync(r =>
            r.ProductId == productId && r.Id == reviewId
        );
        return results;
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        await context.Products.AddAsync(product);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddProductReviewAsync(ProductReview review)
    {
        if (!await context.Products.AnyAsync(p => p.Id == review.ProductId))
            return false;

        await context.ProductReviews.AddAsync(review);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateProductAsync(Product updatedProduct)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p =>
            p.Id == updatedProduct.Id
        );

        if (existingProduct == null)
            return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return false;

        context.Products.Remove(product);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id) =>
        await context.Products.AnyAsync(p => p.Id == id);

    public async Task<bool> ExistsByNameAsync(string? name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        return await context.Products.AnyAsync(p =>
            EF.Functions.Like(p.Name!.ToLower(), name.ToLower())
        );
    }
}
