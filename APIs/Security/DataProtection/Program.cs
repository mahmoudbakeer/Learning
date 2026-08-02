using DataProtection.Requests;
using DataProtection.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/bids", async (IBiddingService biddingService) =>
{
    return Results.Ok(await biddingService.GetAllBidsAsync());
});


app.MapGet("/api/bids/{bidId:guid}", async (Guid bidId, IBiddingService biddingService) =>
{
    var bid = await biddingService.GetBidAsync(bidId);
    if (bid == null)
        return Results.NotFound($"Bid with ID {bidId} not found");

    return Results.Ok(bid);
});

app.MapPost("/api/bids", async (CreateBidRequest request, IBiddingService biddingService) =>
{
    var bid = await biddingService.CreateBidAsync(request);
    return Results.Created($"/api/bids/{bid.Id}", bid);
});

app.Run();
