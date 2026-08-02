using DataProtecting.Data;
using DataProtecting.Requests;
using DataProtecting.Responses;
using DataProtection.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DataProtecting.Services
{
    public class BiddingService(AppDbContext context, IDataProtectionProvider dataprotectionprovider) : IBiddingService
    {

        private readonly IDataProtector _dataProtector = dataprotectionprovider.CreateProtector("BidProtection");
        public async Task<BidResponse?> CreateBidAsync(CreateBidRequest bid)
        {
            var newBid = new Bid
            {
                Id = Guid.NewGuid(),
                Address = _dataProtector.Protect(bid.Address ?? string.Empty),
                Amount = bid.Amount,
                FirstName = _dataProtector.Protect(bid.FirstName ?? string.Empty),
                LastName = _dataProtector.Protect(bid.LastName ?? string.Empty),
                Email = _dataProtector.Protect(bid.Email ?? string.Empty),
                Telephone = _dataProtector.Protect(bid.Telephone ?? string.Empty),
                BidDate = DateTime.UtcNow
            };

            context.Bids.Add(newBid);
            await context.SaveChangesAsync();
            return BidResponse.FromModel(newBid, _dataProtector);
        }

        public async Task<List<BidResponse?>> GetAllBidsAsync()
        {
            var bids = await context.Bids.ToListAsync();
            return bids.Select(b => BidResponse.FromModel(b, _dataProtector)).ToList();
        }
        public async Task<BidResponse?> GetBidAsync(Guid bidId)
        {
            var bid = await context.Bids.FindAsync(bidId);
            return bid == null ? null : BidResponse.FromModel(bid, _dataProtector);
        }
    }
}