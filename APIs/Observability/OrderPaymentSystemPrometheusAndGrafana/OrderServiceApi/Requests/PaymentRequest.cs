using OrderServiceApi.Enums;

namespace OrderServiceApi.Requests;

public class PaymentRequest
{
    public PaymentMethod PaymentMethod { get; set; }
    public string? CardNumber { get; set; }
    public string? CardHolderName { get; set; }
}