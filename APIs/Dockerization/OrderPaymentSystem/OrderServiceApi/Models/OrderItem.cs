namespace OrderServiceApi.Models;

public class OrderItem
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => Quantity * UnitPrice;

    private OrderItem() { } // FOR EF

    public OrderItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("The quantity must be > 0.");
        if (unitPrice <= 0)
            throw new ArgumentException("The price must be > 0.");
        else
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
