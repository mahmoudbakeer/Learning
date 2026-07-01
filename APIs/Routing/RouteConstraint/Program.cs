using RouteConstraint.Constraints;

/**
 * Route Constraints
 * -----------------
 * Route constraints are rules applied to route parameters
 * to control which URL values are allowed to match an endpoint.
 *
 * They help ASP.NET Core distinguish between similar routes
 * and prevent invalid URLs from reaching the endpoint.
 *
 * Route constraints are added after the parameter name
 * using a colon (:).
 *
 * Syntax:
 *   {parameter:constraint}
 *
 * Examples:
 *
 *   {id:int}
 *   Accepts only integer values.
 *
 *   {price:decimal}
 *   Accepts only decimal numbers.
 *
 *   {name:alpha}
 *   Accepts only alphabetic characters.
 *
 *   {id:min(1)}
 *   Accepts values greater than or equal to 1.
 *
 *   {age:range(18,60)}
 *   Accepts values between 18 and 60.
 *
 * Multiple constraints can be combined:
 *
 *   {id:int:min(1)}
 *
 * If a URL does not satisfy the constraint,
 * the route is not matched and ASP.NET Core
 * continues searching for another matching endpoint.
 *
 * Route constraints are used for URL matching,
 * not for validating user input. Input validation
 * should be handled separately inside the application.
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("MonthValidation", typeof(MonthRouteConstraint));
});
var app = builder.Build();

app.MapGet("/int/{id:int}", (int id) => $"Integer: {id}");

app.MapGet("/bool/{active:bool}", (bool active) => $"Boolean: {active}");

app.MapGet("/datetime/{dob:datetime}", (DateTime dob) => $"DateTime: {dob}");

app.MapGet("/decimal/{price:decimal}", (decimal price) => $"Decimal: {price}");

app.MapGet("/double/{weight:double}", (double weight) => $"Double: {weight}");

app.MapGet("/float/{weight:float}", (float weight) => $"Float: {weight}");

app.MapGet("/guid/{id:guid}", (Guid id) => $"GUID: {id}");

app.MapGet("/long/{ticks:long}", (long ticks) => $"Long: {ticks}");

app.MapGet("/minlength/{username:minlength(12)}", (string username) => $"Username is {username}");

app.MapGet("/maxlength/{username:maxlength(12)}", (string username) => $"Username is {username}");

app.MapGet("/length/{username:length(12)}", (string username) => $"Username is {username}");
app.MapGet("/lengthrange/{username:length(4,12)}", (string username) => $"Username is {username}");
app.MapGet("/min/{age:min(8)}", (int age) => $"age is {age}");
app.MapGet("/max/{age:max(8)}", (int age) => $"age is {age}");
app.MapGet("/alpha/{name:alpha}", (string name) => $"name is {name}");

app.MapGet(
    "/Custom/{Month:MonthValidation}",
    (int Month) =>
    {
        return $"Month is {Month}";
    }
);
app.Run();
