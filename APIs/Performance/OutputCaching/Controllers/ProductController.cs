using System.Runtime.InteropServices;
using CachingBasics.Requests;
using CachingBasics.Responses;
using CachingBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CachingBasics.Controllers;

[ApiController]
[Route("api/Products")]
public class ProductsController(IProductService productService, IOutputCacheStore outputCache) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "Multiple")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Get(int page = 1, int pageSize = 10)
    {
        System.Console.WriteLine("Controller Visited - GET ALL");
        var response = await productService.GetProductsAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{productId:Guid}", Name = nameof(GetById))]
    [OutputCache(PolicyName = "Single")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid productId)
    {
        System.Console.WriteLine("Controller Visited - GET SINGLE");
        var response = await productService.GetProductByIdAsync(productId);

        if (response is null)
            return NotFound($"Product with Id '{productId}' not found");

        return Ok(response);
    }

    [HttpPost]
    // REMOVED [OutputCache] attribute here!
    public async Task<IActionResult> Post([FromBody] ProductRequest request)
    {
        var response = await productService.AddProductAsync(request);

        // Evict ALL related caches to prevent stale data
        await ClearProductCachesAsync(HttpContext.RequestAborted);

        return CreatedAtRoute(nameof(GetById), new { productId = response.Id }, response);
    }

    [HttpPut("{productId:Guid}")]
    public async Task<IActionResult> Put(Guid productId, [FromBody] ProductRequest request)
    {
        await productService.UpdateProductAsync(productId, request);

        // Evict ALL related caches
        await ClearProductCachesAsync(HttpContext.RequestAborted);

        return NoContent();
    }

    [HttpDelete("{productId:Guid}")]
    public async Task<IActionResult> Delete(Guid productId)
    {
        await productService.DeleteProductAsync(productId);

        // Evict ALL related caches
        await ClearProductCachesAsync(HttpContext.RequestAborted);

        return NoContent();
    }

    /// <summary>
    /// Helper method to keep our cache invalidation DRY and consistent.
    /// Wipes both the specific item cache AND the global list cache.
    /// </summary>
    private async Task ClearProductCachesAsync(CancellationToken ct)
    {
        await outputCache.EvictByTagAsync("products_list", ct);
        await outputCache.EvictByTagAsync("product", ct);
    }
}