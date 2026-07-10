using Microsoft.AspNetCore.Mvc;
using RestfulApi.Filters;

namespace RestfulApi.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpGet("{DocNumber:int}")]
    public IActionResult GetProductsDoc(int DocNumber)
    {
        var filename = "Something.pdf";

        var path = Path.Combine("C:\\", filename);

        if (!System.IO.File.Exists(path))
        {
            throw new FileNotFoundException("File Not Found", filename);
        }

        return PhysicalFile(path, "application/pdf", filename);
    }
}
