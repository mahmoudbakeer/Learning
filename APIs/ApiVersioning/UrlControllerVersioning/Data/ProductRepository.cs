namespace UrlControllerVersioning.Data;

using UrlControllerVersioning.Models;

public class ProductRepository
{
    public IEnumerable<Product> GetProducts()
    {
        return
        [
            new Product
            {
                Name = "Mahmoud",
                Id = 1,
                Price = 19.99m,
            },
            new Product
            {
                Name = "Milk",
                Id = 2,
                Price = 29.99m,
            },
        ];
    }

    public Product GetProductById(int id)
    {
        var product = GetProducts().SingleOrDefault(p => p.Id == id);

        return product;
    }
}
