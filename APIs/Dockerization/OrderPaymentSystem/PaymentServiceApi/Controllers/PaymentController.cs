using PaymentServiceApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        // Simulate processing delay
        await Task.Delay(Random.Shared.Next(100, 500));

        // Mock success/failure
        var success = Random.Shared.NextDouble() > 0.1;

        if (!success)
            return StatusCode(502, new { Message = "Payment processing failed." });

        return Ok(new
        {
            TransactionId = $"txn_{Guid.NewGuid().ToString("N")}"[..8],
            Success = true
        });
    }
}