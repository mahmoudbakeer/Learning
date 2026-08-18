using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderServiceApi.Models;

namespace OrderServiceApi.Data.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerId).IsRequired();

        builder.Property(o => o.CreatedAt).IsRequired();

        builder.Property(o => o.PaidAt).IsRequired(false);
        builder.Property(o => o.AmountPaid).IsRequired().HasPrecision(18, 2);

        builder.Property(o => o.PaymentReference).IsRequired(false);

        builder.OwnsMany(
            o => o.Items,
            item =>
            {
                item.WithOwner().HasForeignKey(o => o.OrderId);
                item.HasKey(oi => new { oi.OrderId, oi.ProductId });
                item.Property(oi => oi.Quantity).IsRequired();
                item.Property(oi => oi.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                item.Ignore(oi => oi.Total);
            }
        );

        builder.Navigation(o => o.Items).AutoInclude();
    }
}
