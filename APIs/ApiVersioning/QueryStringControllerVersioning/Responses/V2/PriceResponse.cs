namespace UrlControllerVersioning.Responses.V2;

using UrlControllerVersioning.Models;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PriceResponse Price { get; set; }

    private ProductResponse() { }

    public static ProductResponse FromModel(Product product)
    {
        if (product is null)
            throw new ArgumentNullException();
        var newPr = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = new PriceResponse { Currency = "USD", Amount = product.Price },
        };

        return newPr;
    }

    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        return products.Select(FromModel);
    }
}
