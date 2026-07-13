/*
 * ==========================================================================================
 * ASP.NET CORE SECURITY: JWT BEARER AUTHENTICATION
 * ==========================================================================================
 *
 * WHAT IT IS:
 * JSON Web Tokens (JWT) are an open standard (RFC 7519) for transmitting secure claims
 * between a client and a server. A JWT consists of a Header, Payload (Claims), and a
 * Signature. It is sent by the client in the `Authorization: Bearer <token>` HTTP header.
 *
 * WHY WE USE IT (OVER COOKIES):
 * 1. Cross-Platform: Native mobile apps (iOS/Android) and SPAs (React/Angular) handle
 * headers much easier than cross-domain browser cookies.
 * 2. Statelessness: The server does not need to store active sessions in a database or
 * memory. The token itself contains all the state, and the server relies purely on
 * cryptography to verify it.
 * 3. Microservices: Token generation (Auth Server) and token consumption (Resource API)
 * can be separated into completely different physical servers.
 *
 * HOW IT WORKS IN ASP.NET CORE:
 * 1. The `UseAuthentication` middleware intercepts the incoming request.
 * 2. The `JwtBearerHandler` finds the `Authorization` header and extracts the token string.
 * 3. It cryptographically hashes the Header & Payload using the server's Secret Key.
 * If the resulting hash matches the Signature attached to the token, it proves the token
 * was not tampered with.
 * 4. It unpacks the JSON payload, translates it into a `ClaimsPrincipal`, and attaches it
 * to `HttpContext.User`.
 *
 * CRITICAL SECURITY WARNING:
 * JWT payloads are Base64 Encoded, NOT encrypted. Never put passwords, credit card numbers,
//  * or sensitive PII inside a JWT payload. Anyone who holds the token string can read the data.
 * ==========================================================================================
 */
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RestWithJWTSecurity.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<JwtTokenProvider>();

// 1. REGISTER AUTHENTICATION SCHEMES
builder
    .Services.AddAuthentication(options =>
    {
        // Tells ASP.NET to look for the "Authorization: Bearer <token>" header on incoming requests
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        // Tells ASP.NET to return a raw 401 Unauthorized instead of redirecting to a login page
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // 2. CONFIGURE THE JWT HANDLER
    .AddJwtBearer(options =>
    {
        var configs = builder.Configuration.GetSection("JWTSettings");

        // 3. THE VALIDATION RULEBOOK
        // Since we don't query a database for sessions, we use these mathematical rules
        // to prove the token is legitimate and hasn't been tampered with.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configs["Issuer"], // Must match the server that created it

            ValidateAudience = true,
            ValidAudience = configs["Audience"], // Must be meant for this specific API

            ValidateLifetime = true, // Rejects expired tokens automatically

            ValidateIssuerSigningKey = true,
            // The SymmetricSecurityKey transforms your string password into a byte array
            // that the HMAC-SHA256 algorithm uses to verify the cryptographic signature.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configs["SecretKey"]!)
            ),
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();

// Intercepts the request, finds the JWT, runs the Validation rules, and populates HttpContext.User
app.UseAuthentication();

// Checks HttpContext.User against the endpoint's rules (Roles, Claims, Policies)
app.UseAuthorization();
app.MapControllers();

app.Run();
