using Microsoft.AspNetCore.Mvc;
using OrderServiceApi.Requests;
using OrderServiceApi.Services;

namespace OrderServiceApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { orderId = response.Id }, response);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById(Guid orderId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(orderId, cancellationToken);
        return response is not null ? Ok(response) : NotFound();
    }

    [HttpPost("{orderId}/payment")]
    public async Task<IActionResult> Pay(
        Guid orderId,
        PaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        await service.PayAsync(orderId, request, cancellationToken);
        return Created();
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Cancel(Guid orderId, CancellationToken cancellationToken)
    {
        await service.CancelAsync(orderId, cancellationToken);
        return NoContent();
    }
}
