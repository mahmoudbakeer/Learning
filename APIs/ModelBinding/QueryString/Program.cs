using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.MapGet("/Products-minimal", (int id) => $"Product with id {id} exist.");

// using model with different name than the name of route parameter
app.MapGet(
    "/Products-minimal-1/",
    ([FromQuery(Name = "id")] int identifier) =>
    {
        return $"Product with id {identifier} exist.";
    }
);

// get array from query string
app.MapGet(
    "/bools-minimal-array/",
    (bool[] bools) =>
    {
        return Results.Ok(bools);
    }
);

// bind a complex object from the query string
app.MapGet(
    "/DateRange-minimal-Complex/",
    ([AsParameters] DateRangeQuery daterange) =>
    {
        return Results.Ok(daterange);
    }
);

// ========================================================================
// THE DIFFERENCE BETWEEN [AsParameters] AND IParsable<T>
// ========================================================================
//
// 1. [AsParameters]
//    - HOW IT WORKS: It acts as a "wrapper". ASP.NET Core looks at the properties of your class
//      and tries to find matching keys in the request (Query, Route, Headers).
//    - HOW THE URL LOOKS: /DateRange-minimal-Complex/?query=sales&FromDate=2023-01-01&ToDate=2023-12-31
//    - WHEN TO USE IT: Use it when the client is sending standard, separated key-value pairs
//      and you simply want to group them into one C# object to keep your method signature clean.
//
// 2. IParsable<T> (or static TryParse)
//    - HOW IT WORKS: It expects the ENTIRE object to be represented as a SINGLE string in the HTTP request.
//      You write the custom logic to take that single string and convert it into your object.
//    - HOW THE URL LOOKS: /DateRangeQuery-minimal-ComplexQuery/?daterange=2023-01-01,2023-12-31
//      (Notice how both dates are combined into one single comma-separated string).
//    - WHEN TO USE IT: Use it when you want to pass complex data inside a SINGLE route parameter
//      (e.g., /reports/{2023-01-01,2023-12-31}) or a single custom-formatted query/header string.
// ========================================================================
app.MapGet(
    "/DateRangeQuery-minimal-ComplexQuery/",
    // Because DateRangeComplexQuery implements IParsable, ASP.NET Core looks for a single string
    // named "daterange" in the request, and passes that string into your TryParse method.
    (DateRangeComplexQuery daterange) =>
    {
        return Results.Ok(daterange);
    }
);

app.Run();

public class DateRangeQuery
{
    public string query { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}

public class DateRangeComplexQuery : IParsable<DateRangeComplexQuery>
{
    public DateOnly FromDate { get; init; }
    public DateOnly ToDate { get; init; }

    public static DateRangeComplexQuery Parse(string value, IFormatProvider? provider)
    {
        if (!TryParse(value, provider, out var result))
        {
            throw new ArgumentException("could not parse this object.");
        }
        return result;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? value,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out DateRangeComplexQuery result
    )
    {
        var segments = value?.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );

        if (
            segments?.Length == 2
            && DateOnly.TryParse(segments[0], out DateOnly fromdate)
            && DateOnly.TryParse(segments[1], out DateOnly todate)
        )
        {
            result = new DateRangeComplexQuery { FromDate = fromdate, ToDate = todate };
            return true;
        }

        result = new DateRangeComplexQuery { FromDate = default, ToDate = default };
        return false;
    }
}
