namespace OrderServiceApi.Responses;

public class PaymentResponse
{
    public string? TransactionId { get; set; }
    public bool Success { get; set; }
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
}