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
}
