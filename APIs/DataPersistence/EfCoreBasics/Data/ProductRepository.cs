using Microsoft.EntityFrameworkCore;
using RestfulApi.Models;

namespace RestfulApi.Data;

public class ProductRepository(AppDbContext context)
{
    public int ProductCounts() => context.Products.Count();

    public int GetProductsCount() => context.Products.Count();

    public List<Product> GetProductsPage(int page = 1, int pageSize = 10)
    {
        var products = context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return products;
    }

    public Product? GetProductById(Guid productId)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == productId);

        if (product is null)
            return null;

        return product;
    }

    public IEnumerable<ProductReview> GetProductReviews(Guid productId)
    {
        return context.ProductReviews.Where(r => r.ProductId == productId);
    }

    public ProductReview? GetReview(Guid productId, Guid reviewId)
    {
        return context.ProductReviews.FirstOrDefault(r =>
            r.ProductId == productId && r.Id == reviewId
        );
    }

    public bool AddProduct(Product product)
    {
        context.Products.Add(product);

        return context.SaveChanges() > 0;
    }

    public bool AddProductReview(ProductReview review)
    {
        if (!context.Products.Any(p => p.Id == review.ProductId))
            return false;

        context.ProductReviews.Add(review);

        return context.SaveChanges() > 0;
    }

    public bool UpdateProduct(Product updatedProduct)
    {
        var existingProduct = context.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);

        if (existingProduct == null)
            return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return context.SaveChanges() > 0;
    }

    public bool DeleteProduct(Guid id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return false;

        context.Products.Remove(product);

        return context.SaveChanges() > 0;
    }

    public bool ExistsById(Guid id) => context.Products.Any(p => p.Id == id);

    public bool ExistsByName(string? name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        return context.Products.Any(p => EF.Functions.Like(p.Name!.ToLower(), name.ToLower()));
    }
}
