using ParameterTransformer.Transformers;

/**
 * Parameter Transformers
 * ----------------------
 * Parameter transformers modify route parameter values
 * when generating or matching URLs.
 *
 * They are commonly used to convert text into a consistent
 * URL format, such as kebab-case or slug format.
 *
 * Parameter transformers improve URL readability and help
 * create SEO-friendly routes.
 *
 * A transformer is applied by adding :transformerName
 * after a route parameter.
 *
 * Syntax:
 *
 *   {parameter:transformer}
 *
 * Example:
 *
 *   [Route("[controller]")]
 *
 *   ProductCatalogController
 *
 *   URL Generated:
 *     /product-catalog
 *
 *   when using a slugify transformer.
 *
 * Common Uses:
 *
 * - Convert PascalCase to kebab-case.
 * - Generate SEO-friendly URLs.
 * - Enforce a consistent URL style.
 * - Automatically format controller or action names.
 *
 * Why Use Parameter Transformers?
 *
 * - Avoid manually formatting URLs.
 * - Keep routing conventions consistent.
 * - Improve URL readability.
 * - Separate URL formatting from application logic.
 *
 * Example:
 *
 *   {controller:slugify}
 *
 *   ProductsController
 *
 *   Generated URL:
 *     /products
 *
 *   ProductCatalogController
 *
 *   Generated URL:
 *     /product-catalog
 *
 * When to Use Parameter Transformers
 * ----------------------------------
 *
 * Use parameter transformers when you want URLs to follow
 * a specific naming convention without changing the actual
 * controller, action, or parameter names in code.
 *
 * Typical scenarios:
 *
 * - MVC and Razor Pages applications.
 * - Public APIs with user-friendly URLs.
 * - SEO optimization.
 * - Applications that require kebab-case URLs.
 *
 * When Not to Use Them
 * --------------------
 *
 * If URL formatting is not important or you want route
 * values to appear exactly as they are defined,
 * parameter transformers are unnecessary.
 *
 * Note:
 * Parameter transformers are for formatting route values.
 * They do not validate data like route constraints,
 * and they do not generate URLs by themselves like
 * LinkGenerator.
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap["slugify"] = typeof(SlugifyTransformers);
});
var app = builder.Build();

app.MapGet(
        "/books/{title:slugify}",
        (string title) =>
        {
            return Results.Ok(new { title });
        }
    )
    .WithName("BookByTitle");

app.MapGet(
    "/generator",
    (LinkGenerator link, HttpContext context) =>
    {
        var url = link.GetPathByName(
            "BookByTitle",
            new { title = "Clean Code A Handbook of Agile Software Craftsmanship" }
        );

        return Results.Ok(new { genertedUrl = url });
    }
);

app.Run();
