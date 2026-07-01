var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

/**
 * Cookies
 * --------
 * A cookie is a small piece of data stored by the client's
 * browser and automatically sent with future requests to
 * the same server.
 *
 * Cookies allow an application to remember information
 * between requests.
 *
 * Common Uses:
 *
 * - User authentication.
 * - Session management.
 * - User preferences (theme, language).
 * - Shopping cart information.
 * - Remember Me functionality.
 *
 * How Cookies Work:
 *
 * 1. The server sends a cookie to the client.
 *
 * 2. The browser stores the cookie.
 *
 * 3. The browser automatically includes the cookie
 *    in subsequent requests.
 *
 * Reading Cookies:
 *
 *   Request.Cookies["Theme"]
 *
 * Writing Cookies:
 *
 *   Response.Cookies.Append(
 *       "Theme",
 *       "Dark");
 *
 * Deleting Cookies:
 *
 *   Response.Cookies.Delete("Theme");
 *
 * When to Use Cookies
 * -------------------
 *
 * Use cookies when information should persist
 * across multiple requests and be stored
 * on the client.
 *
 * Typical scenarios:
 *
 * - Authentication.
 * - Remembering user settings.
 * - Maintaining a shopping cart.
 * - Storing a selected language.
 *
 * When Not to Use Cookies
 * -----------------------
 *
 * Do not store:
 *
 * - Passwords.
 * - Sensitive personal information.
 * - Large amounts of data.
 *
 * Cookies have size limitations and can be
 * modified by the client.
 *
 * Note:
 * In ASP.NET Core APIs, cookies are often
 * accessed through Request.Cookies rather
 * than using model binding.
 *
 * Modern Web APIs commonly use JWT Bearer
 * tokens, while MVC and Razor Pages
 * applications frequently use cookies
 * for authentication.
 */
app.MapGet(
    "/minimal/prefrences",
    (HttpContext context) =>
    {
        return Results.Ok(
            new
            {
                theme = context.Request.Cookies["theme"],
                language = context.Request.Cookies["language"],
            }
        );
    }
);

app.Run();
