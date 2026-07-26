using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;


namespace ControllerProjectManagement.OpenApi.Transofrmers;

/* ============================================================================
 *  THE SECURITY TRANSFORMER
 * ============================================================================
 * This class serves a dual purpose. It implements TWO interfaces because 
 * OpenAPI security requires two distinct steps:
 * 
 * Step 1: IOpenApiDocumentTransformer
 * Define what a "Security Scheme" is at the global document level. This tells 
 * the UI, "Hey, this API supports JWT Bearer tokens. Here is how they work."
 * 
 * Step 2: IOpenApiOperationTransformer
 * Evaluate every single HTTP endpoint (Operation). If the endpoint requires 
 * authorization, attach the security scheme to it. This is what renders the 
 * little padlock icon next to the endpoints in Swagger UI/Scalar.
 * ============================================================================ */
internal sealed class BearerSecurityTransformers : IOpenApiDocumentTransformer, IOpenApiOperationTransformer
{
    // Best Practice: Using the built-in JwtBearerDefaults.AuthenticationScheme ("Bearer") 
    // prevents typos and ensures your OpenAPI spec matches your actual Auth setup perfectly.
    private const string schemeId = JwtBearerDefaults.AuthenticationScheme;

    // ------------------------------------------------------------------------
    // STEP 1: DOCUMENT LEVEL TRANSFORMATION (Defines the token mechanism)
    // ------------------------------------------------------------------------
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // The Components object holds reusable definitions (schemas, parameters, security schemes).
        // The '??=' operator ensures we only create a new instance if it doesn't already exist.
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        // We add our Bearer configuration to the dictionary of security schemes.
        document.Components.SecuritySchemes[schemeId] = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,      // Specifies this is standard HTTP authentication
            Scheme = "Bearer",                   // The HTTP auth scheme name (must be "Bearer")
            BearerFormat = "JWT",                // A hint to the client that the token format is JSON Web Token
            In = ParameterLocation.Header,       // Tells the UI to inject the token into the HTTP Headers
            Description = "Enter JWT Bearer Token.", // Text that appears in the UI input box
            Name = "Authorization",              // The exact name of the HTTP header to generate

            // The Reference object is crucial. It gives this scheme a unique ID ("Bearer").
            // When we want to lock down an endpoint in Step 2, we will point to this exact ID.
            Reference = new OpenApiReference()
            {
                Type = ReferenceType.SecurityScheme,
                Id = schemeId
            }
        };

        return Task.CompletedTask;
    }

    // ------------------------------------------------------------------------
    // STEP 2: OPERATION LEVEL TRANSFORMATION (Locks down specific endpoints)
    // ------------------------------------------------------------------------
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // We inspect the metadata of the current controller method.
        // By checking for 'IAuthorizeData' instead of '[Authorize]', this safely catches 
        // the default [Authorize] attribute AND any custom authorization attributes you might write.
        if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
        {
            // If the endpoint is protected, ensure the Security requirements list is initialized.
            // Using the collection expression '[]' is modern C# 12 syntax for 'new List<OpenApiSecurityRequirement>()'.
            operation.Security ??= [];

            // We create a "pointer" (Reference) back to the scheme we defined in Step 1.
            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference(),
            };

            key.Reference.Type = ReferenceType.SecurityScheme;
            key.Reference.Id = schemeId;

            // We build the actual requirement mapping. 
            // The empty array '[]' means no specific OAuth2 scopes are required, just a valid token.
            var requirements = new OpenApiSecurityRequirement()
            {
              {key , []}
            };

            // Finally, attach the requirement to the endpoint. This applies the UI padlock.
            operation.Security.Add(requirements);
        }

        return Task.CompletedTask;
    }
}