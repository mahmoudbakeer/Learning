using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ControllerProjectManagement.Requests;
using ControllerProjectManagement.Responses;
using Microsoft.IdentityModel.Tokens;

namespace ControllerProjectManagement.Services;


public class JWTTokenProvider(IConfiguration configuration)
{
    public TokenResponse GenerateJwtToken(GenerateTokenRequest tokenRequest)
    {
        var settings = configuration.GetSection("JWTSettings");

        var audience = settings["Audience"];
        var issuer = settings["Issuer"];
        var secretKey = settings["SecretKey"];
        var expires = DateTime.MaxValue; // just to make sure the token will work in the future for test.


        var claims = new List<Claim>()
        {
          new Claim(JwtRegisteredClaimNames.Sub , tokenRequest.Id),
          new Claim(JwtRegisteredClaimNames.Email , tokenRequest.Email),
          new Claim(JwtRegisteredClaimNames.GivenName , tokenRequest.FirstName),
          new Claim(JwtRegisteredClaimNames.FamilyName , tokenRequest.LastName)
        };

        foreach (var role in tokenRequest.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        foreach (var perm in tokenRequest.Permissions)
        {
            claims.Add(new Claim("Permission", perm));
        }

        // descriptor 
        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = issuer,
            Audience = audience,
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                algorithm: SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(descriptor);


        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(token),
            RefreshToken = "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1",
            Expires = expires
        };

    }
}