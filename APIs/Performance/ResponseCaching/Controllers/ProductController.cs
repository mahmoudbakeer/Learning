using System.Security.Cryptography;
using CachingBasics.Requests;
using CachingBasics.Responses;
using CachingBasics.Services;
using Microsoft.AspNetCore.Mvc;

namespace CachingBasics.Controllers;

[ApiController]
[Route("api/Products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    // Removed VaryByHeader = "If-None-Match"
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "page", "pageSize" })]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Get(int page = 1, int pageSize = 10)
    {
        var response = await productService.GetProductsAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{productId:Guid}", Name = nameof(GetById))]
    [ResponseCache(Duration = 60)] // Removed VaryByHeader
    public async Task<ActionResult<ProductResponse>> GetById(Guid productId)
    {
        var response = await productService.GetProductByIdAsync(productId);

        if (response is null)
            return NotFound($"Product with Id '{productId}' not found");

        // 1. Calculate the ETag for the current data in the database
        var currentETag = GetETag(response);

        // 2. Check if the Client sent an ETag in the request headers
        if (Request.Headers.TryGetValue("If-None-Match", out var clientETag))
        {
            // 3. If their ETag matches our current ETag, send 304!
            if (clientETag == currentETag)
            {
                return StatusCode(304); // 304 Not Modified (Empty Body)
            }
        }

        // 4. If they don't match, attach the new ETag to the response and send 200 OK!
        Response.Headers.ETag = currentETag;
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductRequest request)
    {
        var response = await productService.AddProductAsync(request);
        return CreatedAtRoute(nameof(GetById), new { productId = response.Id }, response);
    }

    [HttpPut("{productId:Guid}")]
    public async Task<IActionResult> Put(Guid productId, [FromBody] ProductRequest request)
    {
        await productService.UpdateProductAsync(productId, request);
        return NoContent();
    }

    [HttpDelete("{productId:Guid}")]
    public async Task<IActionResult> Delete(Guid productId)
    {
        await productService.DeleteProductAsync(productId);
        return NoContent();
    }

    private string GetETag(ProductResponse product)
    {
        var raw = $"{product.Id}-{product.Name}-{product.Price}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(raw);
        var hash = SHA256.HashData(bytes);
        return $"\"{Convert.ToBase64String(hash)}\""; // The quotes are required by HTTP specs!
    }
}