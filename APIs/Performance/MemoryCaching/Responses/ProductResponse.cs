using CachingBasics.Entities;

namespace CachingBasics.Responses;


public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public static ProductResponse FromModel(Product product)
    {
        if (product is null)
        {
            throw new InvalidDataException();
        }
        else
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
    }
}