using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestfulApi.Models;

namespace RestfulApi.Data.Configurations;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.HasKey(pr => pr.Id);

        builder
            .Property(pr => pr.Reviewer)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(pr => pr.Stars).HasColumnType("INT").IsRequired(true);

        builder.HasData(ProductReviews());
    }

    private IEnumerable<ProductReview> ProductReviews()
    {
        return
        [
            new ProductReview
            {
                Id = Guid.Parse("ddd4e07a-4772-47f7-9cba-6bfc07c26bfe"),
                ProductId = Guid.Parse("2779ee47-10b0-4bd7-8342-404006aa1392"),
                Reviewer = "John Doe",
                Stars = 4,
            },
            new ProductReview
            {
                Id = Guid.Parse("c30d9647-1603-4948-8266-88a850547be0"),
                ProductId = Guid.Parse("2779ee47-10b0-4bd7-8342-404006aa1392"),
                Reviewer = "Sarah Peter",
                Stars = 3,
            },
        ];
    }
}
