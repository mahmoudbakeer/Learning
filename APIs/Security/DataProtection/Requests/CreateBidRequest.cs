namespace DataProtection.Requests;

public class CreateBidRequest
{
    public decimal Amount { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Location { get; set; }
    public string? Address { get; set; }
}