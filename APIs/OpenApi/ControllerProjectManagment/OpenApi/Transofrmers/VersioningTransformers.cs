using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace ControllerProjectManagement.OpenApi.Transofrmers;

/* ============================================================================
 * THE VERSIONING TRANSFORMER
 * ============================================================================
 * When you have multiple API versions (v1, v2), you need to generate multiple 
 * OpenAPI documents. In Program.cs, you typically register them like this:
 * 
 * builder.Services.AddOpenApi("v1", options => options.AddDocumentTransformer<VersioningTransformers>());
 * builder.Services.AddOpenApi("v2", options => options.AddDocumentTransformer<VersioningTransformers>());
 * 
 * This transformer runs once per document. Instead of hardcoding "v1" or "v2", 
 * it dynamically reads the document name and sets the title and version accordingly. 
 * This allows you to use one single transformer class for every API version you ever create.
 * ============================================================================ */
internal sealed class VersioningTransformers : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // 1. context.DocumentName holds the string you passed into AddOpenApi (e.g., "v1" or "v2").
        var version = context.DocumentName;

        // 2. We assign that dynamic version string to the OpenAPI document's official version metadata.
        document.Info.Version = version;

        // 3. We dynamically generate the title. In the UI, this will show up as 
        // "Project Api v1" or "Project Api v2" at the very top of the page.
        document.Info.Title = $"Project Api {version}";

        return Task.CompletedTask;
    }
}