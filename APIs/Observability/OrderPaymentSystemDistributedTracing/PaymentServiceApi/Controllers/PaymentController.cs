using System.Transactions;
using PaymentServiceApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController(ILogger<PaymentController> logger) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        try
        {
            if (request is null || request.OrderId == Guid.Empty || request.Amount < 0)
            {
                logger.LogWarning("Payment request is invalid, OrderId : {OrderId}, Amount : {Amount}", request?.OrderId, request?.Amount);
                return BadRequest("Invalid Payment Request.");
            }
            // Simulate processing delay
            await Task.Delay(Random.Shared.Next(100, 500));

            // Mock success/failure
            var success = Random.Shared.NextDouble() > 0.1;

            if (!success)
            {
                logger.LogError("Payment processing failed, OrderId : {OrderId}.", request.OrderId);
                return StatusCode(502, new { Message = "Payment processing failed." });
            }
            var pay = new
            {
                TransactionId = $"txn_{Guid.NewGuid().ToString("N")}"[..8],
                Success = true
            };
            logger.LogInformation("Payment have been processed successfully, TransactionId :{TransactionId}, OrderId : {OrderId}", pay.TransactionId, request?.OrderId);
            return Ok(pay);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Payment have failed for Unknown error, OrderId : {OrderId}", request.OrderId);
            throw;
        }
    }
}