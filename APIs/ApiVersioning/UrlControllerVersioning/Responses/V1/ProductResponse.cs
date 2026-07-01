using UrlControllerVersioning.Models;

namespace UrlControllerVersioning.Responses.V1;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    private ProductResponse() { }

    public static ProductResponse FromModel(Product product)
    {
        if (product is null)
            throw new ArgumentNullException();
        var newPr = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
        };

        return newPr;
    }

    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        return products.Select(FromModel);
    }
}
