using Microsoft.AspNetCore.Http.HttpResults;
using ResponseGeneration.Data;
using ResponseGeneration.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

// the minimal api can return three types
// 1. string result
// 2. Anounums object e.g new {name , something}
// 3. IResult or TypedResult

// when to use the typeresult and when to use the IResult
// the typedresult is more complex to implement but it gives specification to the endpoit
// which make the unit testing and the documentation much easier
// the IResult is easier to implement but its harder in unit testing sice it required the type casting and Metadata not implicitly declared
// and also its less effective in documentation
app.MapGet(
    "/api/Products-le-ir",
    (ProductRepository pr) =>
    {
        var products = pr.Products();
        return products is null ? Results.NotFound() : Results.Ok(products);
    }
);

// typed results
app.MapGet(
    "/api/Products-le-tr",
    Results<Ok<List<Product>>, NotFound> (ProductRepository pr) =>
    {
        var products = pr.Products();
        return products is null ? TypedResults.NotFound() : TypedResults.Ok(products);
    }
);

// --------------------
// same thing put now with method refrencing lembda expression
static IResult GetProductsMr(ProductRepository pr)
{
    var products = pr.Products();
    return products is null ? Results.NotFound() : Results.Ok(products);
}
;
app.MapGet("/api/Products-mr-ir", GetProductsMr);

// typed results
static Results<Ok<List<Product>>, NotFound> GetProductstr(ProductRepository pr)
{
    var products = pr.Products();
    return products is null ? TypedResults.NotFound() : TypedResults.Ok(products);
}
;
app.MapGet("/api/Products-mr-tr", GetProductstr);

app.Run();
