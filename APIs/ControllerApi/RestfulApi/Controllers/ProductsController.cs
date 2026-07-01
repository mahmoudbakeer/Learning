using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Data;
using RestfulApi.Models;
using RestfulApi.Requests;
using RestfulApi.Responses;

namespace RestfulApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ProductRepository productRepository) : ControllerBase
{
    [HttpOptions]
    public IActionResult ProductsOptions()
    {
        Response.Headers.Append("Allow", "GET, HEAD, POST, PUT, PATCH, DELETE,OPTIONS");
        return NoContent();
    }

    [HttpHead("{productId:Guid}")]
    public IActionResult ProductHead(Guid productId)
    {
        return productRepository.ExistsById(productId) ? Ok() : NotFound();
    }

    [HttpGet("{productId:Guid}", Name = "GetProductById")]
    public ActionResult<ProductResponse> GetProductById(Guid productId, bool includeReviews = false)
    {
        var product = productRepository.GetProductById(productId);
        if (product is null)
            return NotFound();
        IEnumerable<ProductReview>? Reviews = null;
        if (includeReviews)
        {
            Reviews = productRepository.GetProductReviews(productId);
        }

        return ProductResponse.FromModel(product, Reviews);
    }

    [HttpGet]
    public IActionResult GetPaged(int page, int pageSize)
    {
        page = Math.Max(1, page);
        page = Math.Clamp(pageSize, 1, 100);

        int totalcount = productRepository.GetProductsCount();

        var products = productRepository.GetProductsPage(page, pageSize);
        var result = PagedResult<ProductResponse>.Create(
            ProductResponse.FromModel(products),
            totalcount,
            page,
            pageSize
        );

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<ProductResponse> CreateProduct(CreateProductRequest product)
    {
        if (product is null)
            throw new ArgumentNullException(
                nameof(product),
                "The product cannot be null or empty."
            );
        if (productRepository.ExistsByName(product.Name))
            return Conflict($"The prdouct with name '{product.Name}' is already exists.");
        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = product.Name,
            Price = product.Price,
        };

        productRepository.AddProduct(newProduct);

        return CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { ProductId = newProduct.Id },
            value: ProductResponse.FromModel(newProduct)
        );
    }

    [HttpPut("{productId:Guid}")]
    public IActionResult UpdateProduct(Guid productId, UpdateProductRequest product)
    {
        var RepoProduct = productRepository.GetProductById(productId);
        if (RepoProduct is null)
            return NotFound($"The product with id '{product}' does not exists.");

        RepoProduct.Name = product.Name;
        RepoProduct.Price = product.Price ?? 0;

        var successeded = productRepository.UpdateProduct(RepoProduct);

        if (!successeded)
            return StatusCode(500, "Failed to update");
        else
            return NoContent();
    }

    [HttpPatch("{productId:Guid}")]
    public IActionResult PatchProduct(
        Guid productId,
        JsonPatchDocument<UpdateProductRequest> productdoc
    )
    {
        if (productdoc is null)
            return BadRequest("not be null or empty");
        var product = productRepository.GetProductById(productId);
        if (product is null)
            return NotFound("Could not be found");

        var productrequest = new UpdateProductRequest
        {
            Name = product.Name,
            Price = product.Price,
        };

        productdoc.ApplyTo(productrequest);

        product.Name = productrequest.Name;
        product.Price = productrequest.Price ?? 0;

        var success = productRepository.UpdateProduct(product);

        if (!success)
            return StatusCode(500, "Failed to patch product");
        else
            return NoContent();
    }

    [HttpDelete("{productId:Guid}")]
    public IActionResult DeleteProduct(Guid productId)
    {
        if (!productRepository.ExistsById(productId))
            return NotFound($"The product with id '{productId}' does not exists.");
        var sucess = productRepository.DeleteProduct(productId);

        if (!sucess)
            return StatusCode(500, "Failed to delete product");
        else
            return NoContent();
    }

    [HttpGet("csv")]
    public IActionResult GetCsvFile()
    {
        var products = productRepository.GetProductsPage(1, 100);

        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Id,Name,Price");

        foreach (var p in products)
        {
            csvBuilder.AppendLine($"{p.Id},{p.Name},{p.Price}");
        }

        var encoded = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return File(encoded, "text/csv", "products-encoded.csv");
    }

    // here if you want you can use the RedirectPermanant() which will send 302 code moved permanantly
    // instead of sending the 301 Found, means moved temporarly
    // depend on your use case
    [HttpGet("temp-product")]
    public IActionResult GetTempProduct()
    {
        return Ok(new { result = "You are in the right path, chill" });
    }

    [HttpGet("legacy-product")]
    public IActionResult GetLegcyProduct()
    {
        return Redirect("/api/products/temp-product");
    }
}
