using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestWithJWTSecurity.Requests;
using RestWithJWTSecurity.Services;

namespace RestWithJWTSecurity.Controllers;

[ApiController]
[Route("api/token")]
public class TokenController(JwtTokenProvider tokenProvider) : ControllerBase
{
    [HttpPost("generate")]
    public IActionResult GenerateToken(GenerateTokenRequest request)
    {
        return Ok(tokenProvider.GenerateJwtToken(request));
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken(RefreshTokenRequest request)
    {

        var tokenrecord = new
        {
            UserId = "79410514-0136-4442-be9b-01f097c57f7a",
            Token = "ThisIsTheRefireshTokenForTestingPurposesOnly",
            Expiration = DateTime.UtcNow.AddHours(22)
        };

        if (tokenrecord is null || tokenrecord.Expiration < DateTime.UtcNow || tokenrecord.Token != request.RefreshToken)
        {
            return Problem
            (
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Invalid Refresh Token",
                detail: "The provided refresh token is invalid or has expired."
            );
        }
        var user = new
        {
            Id = "79410514-0136-4442-be9b-01f097c57f7a",
            FirstName = "Primary",
            LastName = "Manager",
            Email = "pm@localhost",
            Permissions = new List<string>
            {
                "project:create",
                "project:read",
                "project:update",
                "project:delete",
                "project:assign_member",
                "project:manage_budget",
                "task:create",
                "task:read",
                "task:update",
                "task:delete",
                "task:assign_user",
                "task:update_status"
            },
            Roles = new List<string>
            {
                "ProjectManager"
            }
        };
        var generatetokenrequest = new GenerateTokenRequest
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Permissions = user.Permissions,
            Roles = user.Roles
        };
        return Ok(tokenProvider.GenerateJwtToken(generatetokenrequest));
    }
}
