using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
var app = builder.Build();

app.MapControllers();

// most common use of the Body binding with the post or put verbs
app.MapPost(
    "/Products-minimal",
    ([FromBody] ProductRequest productRequest) => new { productRequest.Name, productRequest.Price }
);
app.Run();

public class ProductRequest
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}
