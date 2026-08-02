namespace DataProtection.Entities;

public class Bid
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Address { get; set; }
}