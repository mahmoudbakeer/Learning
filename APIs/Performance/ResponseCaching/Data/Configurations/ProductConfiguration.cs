using CachingBasics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CachingBasics.Data.Configurations;


public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired(true);
        builder.Property(x => x.Price).IsRequired();

        builder.HasData(GetProducts());
    }
    private IEnumerable<Product> GetProducts() => [
          new Product
        {
            Id = Guid.Parse("2779ee47-10b0-4bd7-8342-404006aa1392"),
            Name = "Soda",
            Price = 1.99m,
        },
        new Product
        {
            Id = Guid.Parse("27a65726-a07f-484c-9b0c-334611ec1c18"),
            Name = "Milk",
            Price = 3.49m,
        },
        new Product
        {
            Id = Guid.Parse("69a0b1fe-3c20-4a4a-bc57-13a8078d8e00"),
            Name = "Juice",
            Price = 4.75m,
        },
        new Product
        {
            Id = Guid.Parse("8fa9f2a4-1b8a-4e66-ae9b-1234567890ab"),
            Name = "Bread",
            Price = 2.49m,
        },
        new Product
        {
            Id = Guid.Parse("2f8b4f29-4d8f-49c1-86f2-234567890abc"),
            Name = "Butter",
            Price = 3.99m,
        },
        new Product
        {
            Id = Guid.Parse("5e76a48d-0e75-4e23-9bcd-34567890abcd"),
            Name = "Eggs",
            Price = 2.99m,
        },
        new Product
        {
            Id = Guid.Parse("7d2f3b91-3f2d-4f0a-92c1-4567890abcde"),
            Name = "Cheese",
            Price = 5.49m,
        },
        new Product
        {
            Id = Guid.Parse("9a4c7e3f-5b2d-4e9c-bcde-567890abcdef"),
            Name = "Chocolate",
            Price = 1.99m,
        },
        new Product
        {
            Id = Guid.Parse("3c9f3f00-2b1e-4a3b-a7f9-67890abcdef0"),
            Name = "Coffee",
            Price = 7.99m,
        },
        new Product
        {
            Id = Guid.Parse("d5e8f1a2-3c4d-4b5e-89ab-7890abcdef12"),
            Name = "Tea",
            Price = 4.99m,
        },
        new Product
        {
            Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-890abcdef123"),
            Name = "Water",
            Price = 0.99m,
        },
        new Product
        {
            Id = Guid.Parse("2b3c4d5e-6f7a-8b9c-0d1e-90abcdef1234"),
            Name = "Fruit Juice",
            Price = 3.99m,
        },
        new Product
        {
            Id = Guid.Parse("3c4d5e6f-7a8b-9c0d-1e2f-abcdef123456"),
            Name = "Yogurt",
            Price = 2.99m,
        },
        new Product
        {
            Id = Guid.Parse("4d5e6f7a-8b9c-0d1e-2f3a-bcdef1234567"),
            Name = "Cereal",
            Price = 4.49m,
        },
        new Product
        {
            Id = Guid.Parse("5e6f7a8b-9c0d-1e2f-3a4b-cdef12345678"),
            Name = "Rice",
            Price = 6.99m,
        },
        new Product
        {
            Id = Guid.Parse("6f7a8b9c-0d1e-2f3a-4b5c-def123456789"),
            Name = "Pasta",
            Price = 3.49m,
        },
        new Product
        {
            Id = Guid.Parse("7a8b9c0d-1e2f-3a4b-5c6d-ef1234567890"),
            Name = "Apple",
            Price = 0.79m,
        },
        new Product
        {
            Id = Guid.Parse("8b9c0d1e-2f3a-4b5c-6d7e-1234567890ab"),
            Name = "Banana",
            Price = 0.59m,
        },
        new Product
        {
            Id = Guid.Parse("9c0d1e2f-3a4b-5c6d-7e8f-234567890abc"),
            Name = "Orange",
            Price = 0.99m,
        },
        new Product
        {
            Id = Guid.Parse("abcdef12-3456-7890-abcd-ef1234567890"),
            Name = "Grapes",
            Price = 2.99m,
        }
    ];
}
