using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestWithJWTSecurity.Requests;
using RestWithJwtSecurity.Responses;

namespace RestWithJWTSecurity.Services;

/*
 * ==========================================================================================
 * JWT TOKEN PROVIDER (AUTHENTICATION SERVICE)
 * ==========================================================================================
 * This service is responsible for generating cryptographically signed JSON Web Tokens (JWT)
 * for users upon successful login. It acts as the "Token Factory", assembling the claims,
 * applying the server's digital signature, and outputting the Base64 string payload.
 * ==========================================================================================
 */
public class JwtTokenProvider(IConfiguration configure)
{
    public TokenResponse GenerateJwtToken(GenerateTokenRequest tokenRequest)
    {
        var settings = configure.GetSection("JWTSettings");
        var issuer = settings["Issuer"];
        var audience = settings["Audience"];

        DateTime expires = DateTime.UtcNow.AddMinutes(
            int.Parse(settings["TokenExpirationInMinutes"]!)
        );

        // 1. ASSEMBLE THE CLAIMS (The Payload)
        // We use JwtRegisteredClaimNames for standard properties to keep the token size small.
        List<Claim> claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, tokenRequest.Id),
            new(JwtRegisteredClaimNames.Email, tokenRequest.Email),
            new(JwtRegisteredClaimNames.GivenName, tokenRequest.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, tokenRequest.LastName),
        };

        // We specifically use ClaimTypes.Role because ASP.NET Core's [Authorize(Roles="")]
        // middleware hardcodes its search to look for this exact Microsoft schema string.
        foreach (var role in tokenRequest.Roles)
            claims.Add(new(ClaimTypes.Role, role));

        foreach (var perm in tokenRequest.Permissions)
            claims.Add(new("Permission", perm));

        // 2. CONFIGURE THE BLUEPRINT (SecurityTokenDescriptor)
        // This object holds all the metadata and security rules for the token we want to build.
        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Audience = audience,
            Issuer = issuer,
            Expires = expires,

            // 3. THE DIGITAL SIGNATURE (SigningCredentials)
            // We convert our secret string into a SymmetricSecurityKey (raw bytes), and
            // pair it with the HmacSha256 hashing algorithm to mathematically seal the token.
            SigningCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings["SecretKey"]!)),
                algorithm: SecurityAlgorithms.HmacSha256Signature
            ),
        };

        // 4. THE FACTORY ENGINE (JwtSecurityTokenHandler)
        // The handler is the engine that executes the cryptography and builds the token.
        var tokenhandler = new JwtSecurityTokenHandler();

        // Generates a rich C# object in memory representing the token.
        var token = tokenhandler.CreateToken(descriptor);

        return new TokenResponse()
        {
            Expires = expires,

            // 5. SERIALIZATION (WriteToken)
            // We must convert the C# object into a Base64-encoded string ("eyJhbGci...")
            // so it can be transmitted safely over the HTTP protocol to the client.
            AccessToken = tokenhandler.WriteToken(token),

            // Note: Refresh tokens are usually stored in a database to allow the user
            // to get a new AccessToken without logging in again.
            RefreshToken = "ThisIsTheRefireshTokenForTestingPurposesOnly",
        };
    }
}
