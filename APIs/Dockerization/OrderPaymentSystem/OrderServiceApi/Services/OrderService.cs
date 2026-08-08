using System.Globalization;
using OrderServiceApi.Models;
using OrderServiceApi.Repositories;
using OrderServiceApi.Requests;
using OrderServiceApi.Responses;

namespace OrderServiceApi.Services;

public class OrderService(IOrderRepository repository, HttpClient paymentHttpClient) : IOrderService
{
    public async Task<OrderResponse?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await repository.GetByIdAsync(orderId, cancellationToken);

        return order is not null ? OrderResponse.FromModel(order) : null;
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var items = request.Items.Select(i =>
            new OrderItem(i.ProductId, i.Quantity, i.UnitPrice)).ToList();

        var order = new Order(request.CustomerId, items);

        await repository.AddAsync(order, cancellationToken);

        return OrderResponse.FromModel(order);
    }


    public async Task PayAsync(Guid orderId, PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var order = await repository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order {orderId} not found");

        if (order.PaidAt.HasValue)
            throw new InvalidOperationException("Order has already been paid.");

        var payload = new Dictionary<string, string?>
        {
            { "OrderId", orderId.ToString() },
            { "Amount", order.TotalAmount.ToString(CultureInfo.InvariantCulture) },
            { "Currency", "USD" },
            { "PaymentMethod", request.PaymentMethod.ToString() },
            { "CardNumber", request.CardNumber },
            { "CardHolderName", request.CardHolderName },
        };

        var response = await paymentHttpClient.PostAsJsonAsync("Payment/process", payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Payment failed with status: {(int)response.StatusCode}, body: {body}");
        }

        var paymentResult = await response.Content.ReadFromJsonAsync<PaymentResponse>(cancellationToken);

        if (paymentResult is null)
        {
            var raw = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Deserialization failed. Raw response: {raw}");
        }

        if (!paymentResult.Success)
            throw new InvalidOperationException("Payment was declined");

        order.PaidAt = DateTime.UtcNow;
        order.PaymentReference = paymentResult.TransactionId;

        await repository.UpdateAsync(order, cancellationToken);
    }




    public async Task CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await repository.GetByIdAsync(id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Order {id} not found");

        if (order.PaidAt.HasValue)
        {
            throw new InvalidOperationException("paid invoice can not be cancelled");
        }

        await repository.RemoveAsync(order, cancellationToken);
    }
}
