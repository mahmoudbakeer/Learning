using DataProtecting.Requests;
using DataProtecting.Responses;
namespace DataProtecting.Services;

public interface IBiddingService
{
    Task<BidResponse?> CreateBidAsync(CreateBidRequest bid);
    Task<List<BidResponse?>> GetAllBidsAsync();
    Task<BidResponse?> GetBidAsync(Guid bidId);
}