using System.Net.Mime;
using ControllerProjectManagement.Requests;
using ControllerProjectManagement.Responses;
using ControllerProjectManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControllerProjectManagement.Controllers;

[ApiController]
[Route("Api/Token")]
public class TokenController(JWTTokenProvider tokenProvider) : ControllerBase
{
    [HttpPost("Generate")]
    public ActionResult<TokenResponse> GenerateToken([FromBody] GenerateTokenRequest tokenRequest)
    {
        return Ok(tokenProvider.GenerateJwtToken(tokenRequest));
    }
    [HttpPost("refresh-token")]
    public ActionResult<TokenResponse> RefreshToken([FromBody] RefreshTokenRequest refreshToken)
    {
        var refreshTokenRecord = new
        {
            UserId = "79410514-0136-4442-be9b-01f097c57f7a",
            RefreshToken = "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1",
            Expires = DateTime.UtcNow.AddHours(12)
        };

        if (refreshTokenRecord is null ||
            refreshToken.RefreshToken != "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1" ||
            refreshTokenRecord.Expires < DateTime.UtcNow)
            return Problem(
                title: "Bad Request",
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Refresh token is invalid and/or has expired"
            );

        var user = new
        {
            Id = "79410514-0136-4442-be9b-01f097c57f7a",
            FirstName = "Primary",
            LastName = "Manager",
            Email = "pm@localhost",
            Permissions = new List<string> {
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
            Roles = new List<string> {
         "ProjectManager"
     }
        };

        var generateTokenRequest = new GenerateTokenRequest
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles,
            Permissions = user.Permissions
        };

        return Ok(tokenProvider.GenerateJwtToken(generateTokenRequest));
    }
}