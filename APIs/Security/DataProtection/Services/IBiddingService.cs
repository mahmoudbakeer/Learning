using DataProtection.Requests;
using DataProtection.Responses;

namespace DataProtection.Services;

public interface IBiddingService
{
    Task<BidResponse> CreateBidAsync(CreateBidRequest request);
    Task<List<BidResponse>> GetAllBidsAsync();
    Task<BidResponse?> GetBidAsync(Guid id);
}
