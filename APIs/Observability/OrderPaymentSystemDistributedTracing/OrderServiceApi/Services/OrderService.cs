using System.Globalization;
using OrderServiceApi.Models;
using OrderServiceApi.Repositories;
using OrderServiceApi.Requests;
using OrderServiceApi.Responses;

namespace OrderServiceApi.Services;

public class OrderService(IOrderRepository repository, HttpClient paymentHttpClient, ILogger<OrderService> logger) : IOrderService
{
    public async Task<OrderResponse?> GetByIdAsync(Guid OrderId, CancellationToken cancellationToken = default)
    {
        var order = await repository.GetByIdAsync(OrderId, cancellationToken);

        if (order is null)
            logger.LogWarning("Order Not Found, OrderId : {OrderId}.", OrderId);
        return order is not null ? OrderResponse.FromModel(order) : null;
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var items = request.Items.Select(i =>
            new OrderItem(i.ProductId, i.Quantity, i.UnitPrice)).ToList();

        var order = new Order(request.CustomerId, items);

        await repository.AddAsync(order, cancellationToken);
        logger.LogInformation("Order Created, OrderId : {OrderId}.", order.Id);
        return OrderResponse.FromModel(order);
    }


    public async Task PayAsync(Guid OrderId, PaymentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await repository.GetByIdAsync(OrderId, cancellationToken);
            if (order is null)
            {
                logger.LogWarning("Order Not Found, OrderId : {OrderId}.", OrderId);
                throw new KeyNotFoundException($"Order {OrderId} not found");
            }
            if (order.PaidAt.HasValue)
            {
                //in logging don't record any sensitive data
                logger.LogWarning("Order has been paid before, OrderId : {OrderId}, PaymentReference : {PaymentReference}", OrderId, order.PaymentReference);
                throw new InvalidOperationException("Order has already been paid.");
            }
            var payload = new Dictionary<string, string?>
        {
            { "OrderId", OrderId.ToString() },
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
                logger.LogError("Payment Failed,OrderId : {OrderId}, StatusCode : {StatusCode}", OrderId, (int)response.StatusCode);
                throw new InvalidOperationException($"Payment failed with status: {(int)response.StatusCode}, body: {body}");
            }

            var paymentResult = await response.Content.ReadFromJsonAsync<PaymentResponse>(cancellationToken);

            if (paymentResult is null)
            {
                var raw = await response.Content.ReadAsStringAsync();
                logger.LogError("Process of payment failed by Api Gateway, OrderId : {OrderId}, Raw : {raw}", OrderId, raw);
                throw new InvalidOperationException($"Deserialization failed. Raw response: {raw}");
            }

            if (!paymentResult.Success)
            {
                logger.LogError("Payment Got Declined from the source, OrderId : {OrderId}", OrderId);
                throw new InvalidOperationException("Payment was declined");
            }
            order.PaidAt = DateTime.UtcNow;
            order.PaymentReference = paymentResult.TransactionId;
            logger.LogInformation("Payment have been processed successfully, OrderId : {OrderId}", OrderId);
            await repository.UpdateAsync(order, cancellationToken);
        }
        catch (Exception ex)
        {
            // this is just for learning purpose so don't make critical logging except there is something critical has happened.
            logger.LogCritical(ex, "Unknown Error have occurred during processing the payment, OrderId : {OrderId}", OrderId);
            throw;
        }
    }




    public async Task CancelAsync(Guid OrderId, CancellationToken cancellationToken = default)
    {
        var order = await repository.GetByIdAsync(OrderId, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order Not Found, OrderId : {OrderId}.", OrderId);
            throw new KeyNotFoundException($"Order {OrderId} not found");
        }


        if (order.PaidAt.HasValue)
        {
            throw new InvalidOperationException("paid invoice can not be cancelled");
        }

        await repository.RemoveAsync(order, cancellationToken);
    }
}
