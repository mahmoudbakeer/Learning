using DataProtection.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace DataProtecting.Responses;

public class BidResponse
{
    public Guid? Id { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? BidDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Address { get; set; }

    public static BidResponse? FromModel(Bid bid, IDataProtector? dataProtector = null)
    {
        if (bid is null)
            throw new InvalidDataException();
        var newBid = new BidResponse
        {
            Id = bid.Id,
            Address = string.IsNullOrWhiteSpace(bid.Address) ? string.Empty : dataProtector?.Unprotect(bid.Address) ?? string.Empty,
            Amount = bid.Amount,
            BidDate = bid.BidDate,
            Email = string.IsNullOrWhiteSpace(bid.Email) ? string.Empty : dataProtector?.Unprotect(bid.Email) ?? string.Empty,
            FirstName = string.IsNullOrWhiteSpace(bid.FirstName) ? string.Empty : dataProtector?.Unprotect(bid.FirstName) ?? string.Empty,
            LastName = string.IsNullOrWhiteSpace(bid.LastName) ? string.Empty : dataProtector?.Unprotect(bid.LastName) ?? string.Empty,
            Telephone = string.IsNullOrWhiteSpace(bid.Telephone) ? string.Empty : dataProtector?.Unprotect(bid.Telephone) ?? string.Empty,
        };

        return newBid;
    }

    public static IEnumerable<BidResponse>? FromModels(IEnumerable<Bid> bids, IDataProtector? dataProtector = null)
    {
        IEnumerable<BidResponse>? outbids = [];
        foreach (var b in bids)
        {
            var outbid = FromModel(b, dataProtector);
            if (outbid is not null)
                outbids.Append(outbid);
        }
        return outbids;
    }
}
