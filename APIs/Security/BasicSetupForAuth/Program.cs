using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication().AddCookie();

/*
 * ==========================================================================================
 * ASP.NET CORE SECURITY: AUTHENTICATION, AUTHORIZATION & IDENTITY HIERARCHY
 * ==========================================================================================
 *
 * 1. AUTHENTICATION (AuthN) vs. AUTHORIZATION (AuthZ)
 * ------------------------------------------------------------------------------------------
 * - AUTHENTICATION (Who are you?): The process of verifying a user's identity.
 * It answers the question: "Are you who you say you are?"
 * (e.g., Validating a password, checking a JWT signature, or reading a secure cookie).
 * * - AUTHORIZATION (What can you do?): The process of checking permissions.
 * It answers the question: "Are you allowed to access this specific resource?"
 * (e.g., "You are logged in, but you don't have the 'Admin' role to delete this user.")
 *
 * - PIPELINE RULE: Authentication MUST happen before Authorization. You cannot check
 * what someone is allowed to do until you know who they are.
 * (app.UseAuthentication() always goes before app.UseAuthorization()).
 *
 *
 * 2. THE IDENTITY HIERARCHY (How ASP.NET Core models a user)
 * ------------------------------------------------------------------------------------------
 * At the center of ASP.NET Core security is a deeply nested hierarchy of objects
 * that represent the user making the current HTTP request.
 *
 * [ HttpContext.User ] -> The globally accessible property for the current HTTP request.
 * |
 * V
 * (is a) ClaimsPrincipal -> Represents the actual HUMAN interacting with the app.
 * |             A human can hold *multiple* forms of identification.
 * |
 * |-- ClaimsIdentity (e.g., "Local Cookie Login") -> The 1st ID Card.
 * |        |-- Claim (Type: "Name",  Value: "Mahmoud")
 * |        |-- Claim (Type: "Email", Value: "mahmoud@example.com")
 * |        |-- Claim (Type: "Role",  Value: "Manager")
 * |
 * |-- ClaimsIdentity (e.g., "Google OAuth") -> The 2nd ID Card.
 * |-- Claim (Type: "GoogleId", Value: "123456789")
 *
 *
 * TERMINOLOGY BREAKDOWN:
 * - Claim: A single fact or piece of information about the user (a Key-Value pair).
 * - ClaimsIdentity: A collection of claims issued by one specific authority (like a
 * Driver's License). *Note: An identity is only considered valid (IsAuthenticated = true)
 * if it was created with an AuthenticationScheme (e.g., "Cookies").*
 * - ClaimsPrincipal: The container for one or more ClaimsIdentities. This is the
 * object that is actually assigned to `HttpContext.User`.
 * * WHY MULTIPLE IDENTITIES?
 * A user might log in to your site locally with a password (Identity 1) but also
 * link their Corporate Microsoft account (Identity 2) to the exact same session.
 * The `ClaimsPrincipal` groups all these distinct ID cards together into one human.
 * ==========================================================================================
 */

// those all are the same of the above one
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
// builder.Services.AddAuthentication("Cookies").AddCookie();
// builder
//     .Services.AddAuthentication(op =>
//     {
//         op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     })
//     .AddCookie();
var app = builder.Build();

app.UseAuthentication(); // this comes before any middleware has to deal with the identity of the user

app.MapGet(
    "/login",
    static async (HttpContext context) =>
    {
        List<Claim> claims =
        [
            new Claim("Name", "mahmoud"),
            new Claim("Email", "Mahmoud@something"),
            new Claim("PassKey", "hello"),
        ];

        ClaimsIdentity identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        ClaimsPrincipal principals = new ClaimsPrincipal(identity);

        // why i just can't make this context.User = principals;
        // because the compilar will destroy the HttpContext context object after this endpoint so the data won't be saved or send as Set-Cooki header with the response
        // And when the use next time send request there will be no cookie to decrypt so The user will be Unauthorized

        await context.SignInAsync(principals);
    }
);
app.MapGet(
    "/user",
    (HttpContext context) =>
    {
        var principal = context.User;

        if (principal.Identity is { IsAuthenticated: true })
        {
            var claims = principal.Claims.Select(c => new { c.Type, c.Value });

            return Results.Ok(claims);
        }
        else
            return Results.Unauthorized();
    }
);
app.MapGet(
    "/logout",
    async (HttpContext context) =>
    {
        await context.SignOutAsync();
    }
);

app.Run();
