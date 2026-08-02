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
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Get(int page = 1, int pageSize = 10)
    {
        var response = await productService.GetProductsAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{productId:Guid}", Name = nameof(GetById))]
    public async Task<ActionResult<ProductResponse>> GetById(Guid productId)
    {
        var response = await productService.GetProductByIdAsync(productId);

        if (response is null)
            return NotFound($"Product with Id '{productId}' not found");


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


}