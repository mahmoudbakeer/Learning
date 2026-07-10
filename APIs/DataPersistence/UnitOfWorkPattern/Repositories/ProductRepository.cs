using Microsoft.EntityFrameworkCore;
using RestfulApi.Data;
using RestfulApi.Models;
using RestfulApi.Repositories.Interfaces;

namespace RestfulApi.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
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

    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public async Task AddProductReviewAsync(ProductReview review)
    {
        var product = context.Products.FirstOrDefault(pr => pr.Id == review.ProductId);
        if (product is null)
            throw new InvalidOperationException();
        context.ProductReviews.Add(review);
        var reviewscount = await context
            .ProductReviews.Where(r => r.ProductId == product.Id)
            .CountAsync();
        var currentrating = await context
            .ProductReviews.Where(r => r.ProductId == product.Id)
            .AverageAsync(r => (decimal?)r.Stars);

        product.AverageRating = Math.Round(
            (((currentrating ?? 0m) * reviewscount) + review.Stars) / (reviewscount + 1),
            1,
            MidpointRounding.AwayFromZero
        );
    }

    public async Task UpdateProductAsync(Product updatedProduct)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p =>
            p.Id == updatedProduct.Id
        );

        if (existingProduct == null)
            throw new ArgumentNullException();

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new ArgumentNullException();

        context.Products.Remove(product);
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
