// this will create builder object which is responsible on configuring the app before it starts
// configurations such as :
// You configure:
// -- Services
// -- Logging
// -- Configuration
// -- Dependency Injection
// -- Middleware settings
// -- Environment
// -- Server options
using System.Text.Json;
using MyFirstWebApp.Entities;
using MyFirstWebApp.Entities.Repositories;

var builder = WebApplication.CreateBuilder(args);

// the configuration will be  here is this place after creating the builder object and before building it
var app = builder.Build(); // this will create the actual application

// this define endpoint, route is : '/' which is the root, and get method will be sent , and will call this endpoint to print the Hello World!
// app.MapGet("/", () => "Hello World!");

// lets print the http request elements on the web page
app.Run(
    async (HttpContext context) =>
    {
        if (context.Request.Path == "/")
        {
            if (context.Request.Method == "GET")
            {
                await context.Response.WriteAsync(
                    text: $"the mothod is : {context.Request.Method}\r\n"
                );
                await context.Response.WriteAsync(text: $"the Url is : {context.Request.Path}\r\n");
                await context.Response.WriteAsync(text: $"Headers\r\n");
                foreach (var key in context.Request.Headers.Keys)
                {
                    await context.Response.WriteAsync(
                        text: $"{key} : {context.Request.Headers[key]}\r\n"
                    );
                }
            }
        }
        else if (context.Request.Path.StartsWithSegments(other: "/Employee"))
        {
            if (context.Request.Method == "GET")
            {
                if (context.Request.Query.ContainsKey("id"))
                {
                    if (int.TryParse(context.Request.Query["id"], out int employeeId))
                    {
                        if (employeeId <= 0)
                            context.Response.StatusCode = 400;
                        else
                        {
                            var emp = EmployeeRepo
                                .GetEmployees()
                                .FirstOrDefault(e => e.Id == employeeId);
                            if (emp is null)
                            {
                                context.Response.StatusCode = 404;
                            }
                            else
                            {
                                context.Response.StatusCode = 200;
                                await context.Response.WriteAsync(
                                    $"{emp.Name} - {emp.Position} - {emp.Salary}$"
                                );
                            }
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 200;
                    var employees = EmployeeRepo.GetEmployees();
                    await context.Response.WriteAsync($"The Employees :\r\n");
                    foreach (var emp in employees)
                        await context.Response.WriteAsync(
                            $"{emp.Name} - {emp.Position} - {emp.Salary}$\r\n"
                
                }
            }
            else if (context.Request.Method == "POST")
            {
                // the context.Request.Body will return stream object we have to read this stream object
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var employee = JsonSerializer.Deserialize<Employee>(body);
                EmployeeRepo.AddEmployee(employee);
                context.Response.StatusCode = 201;
            }
            else if (context.Request.Method == "PUT")
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var employee = JsonSerializer.Deserialize<Employee>(body);
                var result = EmployeeRepo.UpdateEmployee(employee);
                if (result)
                {
                    await context.Response.WriteAsync($"Emloyee Updated Successfully.");
                    context.Response.StatusCode = 204;
                }
                else
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync($"Employee Not Found!");
                }
            }
            else if (context.Request.Method == "DELETE")
            {
                if (context.Request.Query.ContainsKey("id"))
                {
                    var id = context.Request.Query["id"];
                    if (int.TryParse(id, out int employeeId))
                    {
                        if (context.Request.Headers["Authorization"] == "Mahmoud")
                        {
                            var result = EmployeeRepo.DeleteEmployee(employeeId);
                            if (result)
                            {
                                context.Response.StatusCode = 200;
                                await context.Response.WriteAsync($"Emloyee Deleted Successfully.");
                            }
                            else
                            {
                                context.Response.StatusCode = 404;
                                await context.Response.WriteAsync($"Employee Not Found!");
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync($"You are not authorized to delete.");
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync($"Id is not valid");
                    }
                }
            }
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }
);

// run the applicatoin
app.Run();
