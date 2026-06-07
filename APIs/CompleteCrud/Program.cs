using System.Text.Json;
using CompleteCrud.Entities;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseRouting();

app.UseEndpoints(
    (configure) =>
    {
        // ========================================================================
        // ASP.NET Core Minimal APIs Model Binding Default Priorities:
        // When no attribute is specified, ASP.NET Core infers the source in this order:
        // 1. [FromServices] - Checks the Dependency Injection container first.
        // 2. [FromRoute] - Checks route parameters matching the variable name.
        // 3. [FromQuery] - Checks the query string for matching keys.
        // ========================================================================

        // configure.MapGet(
        //     "/Students",
        //     async (context) =>
        //     {
        //         await context.Response.WriteAsync($"The Employees :\r\n ");
        //         var employees = StudentRepo.GetStudents();
        //         foreach (var item in employees)
        //         {
        //             await context.Response.WriteAsync(
        //                 $"StudentName : {item.FirstName} {item.LastName}\r\n "
        //             );
        //         }
        //     }
        // );

        // configure.MapGet(
        //     // Model binding is the automatic process where ASP.NET Core extracts values
        //     // from the HTTP request and converts them to .NET types, eliminating manual extraction.
        //     "/Students/{id:int}",
        //     // [FromRoute] explicitly tells the binder to look in the URL route values.
        //     // The 'Name' property maps the route parameter "id" to the method parameter.
        //     ([FromRoute(Name = "id")] int id) =>
        //     {
        //         var student = StudentRepo.GetStudent(id);
        //         return student;
        //     }
        // );

        // configure.MapGet(
        //     "/Students",
        //     // [FromHeader] explicitly extracts the value from the HTTP request headers.
        //     ([FromHeader(Name = "id")] int id) =>
        //     {
        //         var student = StudentRepo.GetStudent(id);
        //         return student;
        //     }
        // );

        // configure.MapGet(
        //     "/Students/{id:int}",
        //     // When an endpoint requires multiple parameters from different HTTP sources (Route, Query, etc.),
        //     // it is best practice to encapsulate them in a struct/class and use the [AsParameters] attribute.
        //     ([AsParameters] StudentParamters studnetParameter) =>
        //     {
        //         var student = StudentRepo.GetStudent(studnetParameter.id);
        //
        //         student.FirstName = studnetParameter.firstname;
        //         student.LastName = studnetParameter.lastname;
        //         return student;
        //     }
        // );

        // configure.MapGet(
        //     "/Students",
        //     // We can bind an array of parameters directly from the Query String.
        //     // Example request: /Students?id=1&id=2&id=3
        //     ([FromQuery(Name = "id")] int[] ids) =>
        //     {
        //         var students = StudentRepo.GetStudents().Where(st => ids.Contains(st.Id));
        //         return students;
        //     }
        // );

        configure.MapGet(
            "/Students",
            // We can also bind an array of parameters from the HTTP Headers.
            ([FromHeader(Name = "id")] int[] ids) =>
            {
                var students = StudentRepo.GetStudents().Where(st => ids.Contains(st.Id));
                return students;
            }
        );

        configure.MapGet(
            "/People",
            // Custom model binding: ASP.NET Core will look for a static 'BindAsync' method
            // inside the 'Person' class to determine how to construct this object from the HttpContext.
            (Person? P) =>
            {
                return $"name is {P.Name} , id is {P.Id}";
            }
        );

        configure.MapPut(
            "/Students",
            // Manual request body extraction and deserialization.
            // Note: In modern Minimal APIs, you rarely do this manually; model binding handles it automatically.
            async (context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var student = JsonSerializer.Deserialize<Student>(body);

                if (StudentRepo.UpdateStudent(student))
                    await context.Response.WriteAsync($"Updated Sucessfully.");
                else
                    await context.Response.WriteAsync($"Failed to updated.");
            }
        );

        configure.MapPost(
            "/Students",
            // Implicit Model Binding: Because 'Student' is a complex type, ASP.NET Core
            // automatically assumes it should be bound from the request body (JSON) as [FromBody].
            (Student student) =>
            {
                if (
                    student is null
                    || student.Id < 0
                    || StudentRepo.GetStudents().Contains(student)
                )
                {
                    return "the provided data cannot be added.";
                }
                else
                {
                    StudentRepo.AddStudent(student);
                    return "Added successfully";
                }
            }
        );

        configure.MapDelete(
            "/Students/{id:int}",
            // Manual route value extraction.
            // Passing 'int id' as a parameter to the delegate is the preferred, automatic way.
            async (context) =>
            {
                int id = Convert.ToInt32(context.Request.RouteValues["id"].ToString());
                if (StudentRepo.DeleteStudent(id))
                    await context.Response.WriteAsync($"Delete Sucessfully.");
                else
                    await context.Response.WriteAsync($"Failed to Delete.");
            }
        );
    }
);
app.Run();
