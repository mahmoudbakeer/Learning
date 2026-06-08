using MiddleWaresPipleline.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// here is allowed too add configuration and services before building the builder
builder.Services.AddTransient<ExceptionHandelerMiddleware>();

var app = builder.Build();

// Demonstrating how middleware components are registered in the request pipeline.
//
// - app.Use(): Adds a middleware component. It can either pass the request to the next
//   middleware (Regular Middleware) or halt the pipeline by not calling next() (Terminal Middleware).
// - app.Run(): Exclusively used to create a terminal middleware (ends the pipeline).
// - app.Map(): Branches the pipeline based on the request's URL path.
// - app.MapWhen(): Branches the pipeline based on complex conditions evaluated against the HttpContext.

// let create a custom middleware class
// there are three steps to do so :
// 1. implement the interface IMiddleware and its method in the class you have created
// 2. register the Custom class middleware into the Services container in builder
// 3. call it

app.UseMiddleware<ExceptionHandelerMiddleware>();

// First Middleware
app.Use(
    async (HttpContext context, RequestDelegate next) =>
    {
        // Headers must be modified before the response body starts being written (e.g., before WriteAsync).
        // This modification can happen in ANY middleware, not just the first one, as long as the response hasn't started.

        // Check if the response hasn't started yet.
        if (!context.Response.HasStarted)
        {
            context.Response.Headers["MyAddition"] = "Hello my addition";
        }

        await context.Response.WriteAsync($"middleware #1 before passing to next\r\n");
        await next(context);
        await context.Response.WriteAsync($"middleware #1 after passing to next\r\n");
    }
);

// app.Map() creates a new pipeline branch specifically for requests matching the "/Employee" route.
// The important thing is that Map() itself is the branching middleware.
// Once it decides the request belongs to the branch, the rest of the main pipeline is no longer part of the execution path.
// Uncomment to test:
// app.Map(
//     "/Employee",
//     (appBuilder) =>
//     {
//         // 'appBuilder' functions similarly to the main 'app' object for this specific branch.
//         appBuilder.Use(
//             async (HttpContext context, RequestDelegate next) =>
//             {
//                 await context.Response.WriteAsync($"middleware #4 before passing to next\r\n");
//                 await next(context);
//                 await context.Response.WriteAsync($"middleware #4 after passing to next\r\n");
//             }
//         );
//         appBuilder.Use(
//             async (HttpContext context, RequestDelegate next) =>
//             {
//                 await context.Response.WriteAsync($"middleware #5 before passing to next\r\n");
//                 await next(context);
//                 await context.Response.WriteAsync($"middleware #5 after passing to next\r\n");
//             }
//         );
//     }
// );

// app.MapWhen() branches the pipeline based on a predicate.
// Note: This branch does NOT rejoin the main pipeline.
// Uncomment to test:
// app.MapWhen(
//     (HttpContext context) =>
//     {
//         // Predicate: Returns true to route the request into this branch, or false to skip it.
//         return context.Request.Path.StartsWithSegments("/Employee")
//             && context.Request.Query.ContainsKey("id");
//     },
//     (appBuilder) =>
//     {
//         appBuilder.Use(
//             async (HttpContext context, RequestDelegate next) =>
//             {
//                 await context.Response.WriteAsync($"middleware #6 before passing to next\r\n");
//                 await next(context);
//                 await context.Response.WriteAsync($"middleware #6 after passing to next\r\n");
//             }
//         );
//         appBuilder.Use(
//             async (HttpContext context, RequestDelegate next) =>
//             {
//                 await context.Response.WriteAsync($"middleware #7 before passing to next\r\n");
//                 await next(context);
//                 await context.Response.WriteAsync($"middleware #7 after passing to next\r\n");
//             }
//         );
//     }
// );

// app.UseWhen() branches the pipeline based on a predicate, similar to MapWhen().
// Key Difference: It REJOINS the main pipeline after the branch completes its execution.
app.UseWhen(
    (HttpContext context) =>
    {
        // Predicate: Returns true to route the request into this branch.
        return context.Request.Path.StartsWithSegments("/Employee")
            && context.Request.Query.ContainsKey("id");
    },
    configuration: (appBuilder) =>
    {
        appBuilder.Use(
            async (HttpContext context, RequestDelegate next) =>
            {
                await context.Response.WriteAsync($"middleware #8 before passing to next\r\n");
                await next(context);
                await context.Response.WriteAsync($"middleware #8 after passing to next\r\n");
            }
        );
        appBuilder.Use(
            async (HttpContext context, RequestDelegate next) =>
            {
                await context.Response.WriteAsync($"middleware #9 before passing to next\r\n");
                await next(context);
                await context.Response.WriteAsync($"middleware #9 after passing to next\r\n");
            }
        );
    }
);

// Second Middleware
app.Use(
    async (HttpContext context, RequestDelegate next) =>
    {
        throw new ApplicationException();
        await context.Response.WriteAsync($"middleware #2 before passing to next\r\n");
        await next(context);
        await context.Response.WriteAsync($"middleware #2 after passing to next\r\n");
    }
);

// Example of a Terminal Middleware using app.Run().
// Uncommenting this will prevent the third middleware from executing.
// app.Run(
//     async (context) =>
//     {
//         await context.Response.WriteAsync(
//             $"middleware #2 terminal middleware using app.Run() \r\n"
//         );
//     }
// );

// Third Middleware
app.Use(
    async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync($"middleware #3 before passing to next\r\n");

        // To make this a terminal middleware, simply omit the 'await next(context);' call.
        await next(context);

        await context.Response.WriteAsync($"middleware #3 after passing to next\r\n");
    }
);

app.Run();
