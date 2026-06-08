var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// this method will create a terminal middleware means its the final middleware in the pipeline
app.Run(
    async (HttpContext context) =>
    { // this is extension method and is different than the last line, that's a instance method of WebApplication
        await context.Response.WriteAsync("This is the starting middleware MW#1");
        // you cant modify any of the headers or response settings after sending the first response from any middleware in the pipeline
        // context.Response.ContentLength = 20;// this will produce an error
    }
);

app.Run();
