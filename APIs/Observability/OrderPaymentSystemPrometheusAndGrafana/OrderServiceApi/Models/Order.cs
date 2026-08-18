namespace OrderServiceApi.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
    public decimal TotalAmount => Items.Sum(i => i.Total);
    public decimal AmountPaid { get; set; }
    public List<OrderItem> Items { get; set; } = [];

    public Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        if (items is null || !items.Any())
            throw new ArgumentNullException("The order must h`ave at least one item.");
        else
        {
            CustomerId = customerId;
            Items = items.ToList();
            CreatedAt = DateTime.UtcNow;
        }
    }

    private Order() { }
}

