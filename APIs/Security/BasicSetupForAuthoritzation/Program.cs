/*
 * ==========================================================================================
 * ASP.NET CORE SECURITY FUNDAMENTALS: AUTHORIZATION
 * ==========================================================================================
 *
 * WHAT IS AUTHORIZATION (AuthZ)?
 * While Authentication asks "Who are you?" (verifying your identity), Authorization asks
 * "What are you allowed to do?". It is the process of checking a user's permissions against
 * a specific resource or endpoint to see if they are granted access.
 *
 * WHEN TO USE IT:
 * You use Authorization on any endpoint that contains sensitive data, performs state-changing
 * actions (POST, PUT, DELETE), or represents business features that are restricted to paying
 * customers, administrators, or specific roles. It ALWAYS runs after Authentication.
 *
 * THE 4 AUTHORIZATION STRATEGIES:
 * 1. Role-Based (Legacy): Checks if a user belongs to a group (e.g., "Admin"). It is easy
 * to use but becomes brittle as business rules get more complex.
 * 2. Claim-Based: Checks if a user possesses a specific fact or attribute (e.g., checking
 * if they have a "PassKey" claim, or a "DateOfBirth" claim).
 * 3. Policy-Based (Modern Standard): The recommended approach. You define a named Policy
 * in Program.cs (e.g., "CanDeleteUsers") that groups multiple roles and claims together.
 * Endpoints just ask for the Policy by name, decoupling business rules from endpoints.
 * 4. Resource-Based: Used when permissions depend on the data itself (e.g., "You can only
 * edit this document if you are the author"). Handled dynamically via IAuthorizationService.
 * ==========================================================================================
 */

using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTER AUTHENTICATION
// Note: We are configuring the Cookie options here to fix the "404 Not Found" issue!
builder
    .Services.AddAuthentication()
    .AddCookie(options =>
    {
        // FIX FOR THE 404 ISSUE:
        // We explicitly tell the Cookie Handler where OUR custom login page is.
        // If we don't set this, Microsoft defaults to "/Account/Login".
        options.LoginPath = "/login";

        // We can also override where users go if they lack permissions (403 Forbidden)
        options.AccessDeniedPath = "/access-denied";
    });

// 2. REGISTER AUTHORIZATION (Policies)
builder.Services.AddAuthorization(op =>
{
    // Registering all the needed services and custom policies for authorization
    op.AddPolicy(
        "PassKeyAndAdmin-policy",
        policy =>
        {
            policy.RequireClaim("PassKey", "hello");
            policy.RequireRole(ClaimTypes.Role, "Admin");
        }
    );
});

var app = builder.Build();

// 3. THE SECURITY MIDDLEWARE PIPELINE
// Authentication MUST come before Authorization.
// First we identify the user, then we check their access.
app.UseAuthentication();
app.UseAuthorization();

// ==========================================================================================
// LOGIN ENDPOINT (Building the ClaimsPrincipal)
// ==========================================================================================
app.MapGet(
    "/login",
    static async (HttpContext context) =>
    {
        List<Claim> claims =
        [
            new Claim("Name", "mahmoud"),
            //  NOTE: Using ClaimTypes.Role is strictly required here for Role-Based
            // Authorization to work. The framework looks specifically for the long URL schema
            // string, not the short word "Role".
            new Claim(ClaimTypes.Role, "Supervisor"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("Email", "Mahmoud@something"),
            new Claim("PassKey", "hello"),
        ];

        ClaimsIdentity identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        ClaimsPrincipal principals = new ClaimsPrincipal(identity);

        await context.SignInAsync(principals);
        return Results.Ok("Logged in successfully!");
    }
);

// ==========================================================================================
// AUTHORIZATION EXAMPLES
// ==========================================================================================

// -- 1. BASIC AUTHORIZATION (Must simply be logged in)
/* * QUESTION: Why does this method return 404 Not Found instead of Unauthorized (401)?
 * * ANSWER: The Cookie Authentication Handler is designed for HTML websites. When a user
 * tries to access [Authorize] but is not logged in, the handler intercepts the 401 error
 * and changes it to a 302 Redirect to send them to the Login page.
 * By default, it redirects to "/Account/Login". Because you originally didn't have
 * an endpoint at "/Account/Login", the routing engine returned a 404 Not Found!
 * (Note: We fixed this by setting options.LoginPath = "/login" at the top of this file!)
 */
app.MapGet(
    "/user",
    [Authorize]
    (HttpContext context) =>
    {
        var principal = context.User;
        var claims = principal.Claims.Select(c => new { c.Type, c.Value });
        return Results.Ok(claims);
    }
);

// Same as above, but using the Fluent API approach
app.MapGet(
        "/secure",
        (HttpContext context) =>
        {
            var principal = context.User;
            var claims = principal.Claims.Select(c => new { c.Type, c.Value });
            return Results.Ok(claims);
        }
    )
    .RequireAuthorization();

// -- 2. ROLE-BASED AUTHORIZATION (Checking group membership)
// With Attribute (Comma separation implies an "OR" relationship: Supervisor OR Admin)
app.MapGet(
    "/SupervisorAndAdmin-only",
    [Authorize(Roles = "Supervisor,Admin")]
    () =>
    {
        return "Supervisor and Admin access page";
    }
);

// With Fluent API (Must be Admin)
app.MapGet(
        "/Admin-only",
        () =>
        {
            return "Admin access page";
        }
    )
    .RequireAuthorization(policy => policy.RequireRole("Admin"));

// -- 3. CLAIM-BASED AUTHORIZATION (Checking specific user facts)
app.MapGet(
        "/Passkey-claim",
        () =>
        {
            return "Passkey Hello only.";
        }
    )
    .RequireAuthorization(policy => policy.RequireClaim("PassKey", "hello"));

// -- 4. POLICY-BASED AUTHORIZATION (The Modern Standard)
// When you have complex configurations, map them to a Policy so you don't clutter your endpoints.
app.MapGet("/PassKeyAndAdmin-policy", () => "PassKeyAndAdmin-policy access page.")
    .RequireAuthorization("PassKeyAndAdmin-policy");

// ==========================================================================================
// LOGOUT & FALLBACK ROUTES
// ==========================================================================================

// Logout sends a request with a header to tell the browser to delete the cookie
app.MapGet(
    "/logout",
    async (HttpContext context) =>
    {
        await context.SignOutAsync();
        return Results.Ok("Logged out securely.");
    }
);

/*
 * QUESTION: Is account/login the formal route for the login ?
 * * ANSWER: YES. "/Account/Login" is a hardcoded default created by Microsoft years ago
 * for MVC applications. If you do not explicitly configure `options.LoginPath`, the
 * framework will always try to redirect unauthorized users here. Now that we configured
 * it properly at the top of the file, this dummy route is no longer strictly needed!
 */
app.MapGet("account/login", () => "Legacy Login Page Fallback");

app.Run();
