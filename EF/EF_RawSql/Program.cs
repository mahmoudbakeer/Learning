using EF_RawSql.Data;
using Microsoft.EntityFrameworkCore;

// now we will study how to deal with raw sql in the EF Core
// this is improtant to know since there is alot of the situations where linq cannot serve you to get the desired result
// means the linq cannot translate each sql statement
// so knowing the sql raw way in ef core and how to deal with it is improtant

// there is three methods to deal with raw sql statement in ef core
// 1. using the  FromSql() method EF Core 7.0.0
// 2. using the FrowmSqlInterpolated() this one is older
// 3. using the FromSqlRaw() and this is the oldest and i dont know if we should use today or not

using (var context = new AppDbContext())
{
    // first lets test the FromSql
    // this method expect an interpolated string means fromatable
    var courses = context.Courses.FromSql($"select * from Courses").ToList();

    Console.WriteLine("Courses using the FromSql : ");
    foreach (var item in courses)
    {
        Console.WriteLine($"The CourseName : {item.CourseName} - the price {item.Price}");
    }
    Console.WriteLine();
    Console.WriteLine();
    // now with FrowmSqlInterpolated() which idk the difference between it and the FromSql
    // same thing it expect the interpolated string
    var coursesv2 = context.Courses.FromSqlInterpolated($"select * from Courses").ToList();

    Console.WriteLine("Courses using the FromSqlInterpolated : ");
    foreach (var item in coursesv2)
    {
        Console.WriteLine($"The CourseName : {item.CourseName} - the price {item.Price}");
    }
    Console.WriteLine();
    Console.WriteLine();

    // now with the FromSqlRaw() i
    var coursesv3 = context.Courses.FromSqlRaw($"select * from Courses").ToList();

    Console.WriteLine("Courses using the FromSqlRaw : ");
    foreach (var item in coursesv3)
    {
        Console.WriteLine($"The CourseName : {item.CourseName} - the price {item.Price}");
    }
    Console.WriteLine();
    Console.WriteLine();

    // now we have procedure called sp_StudentsAndSchedulePerCourse
    // we can call it using any of the above extensions
    // lets decalre the required sqlparameter
    // one thing to remember always to call a view or stored procedure that return custom data,
    // you have to make entity in the code and add it to DBContext and also configure it as keyless, means HasNoKey()
    // you dont have to worry about sql injiction here since the ef core implicitly take the job to declare the sql parameter
    string coursename = "CS-50";
    var StoredProcedureQuery = context.sp_StudentsCountAndScedulePerCourses.FromSql(
        $"exec sp_StudentsCountAndScedulePerCourse {coursename}"
    );

    Console.WriteLine("The result of the stored procedure : ");
    foreach (var item in StoredProcedureQuery)
        Console.WriteLine(
            $"The CourseName : {item.CourseName} - ScheduleTitle : {item.Title} - StudentsCount : {item.StudentsCount}"
        );

    Console.WriteLine();
    Console.WriteLine();

    // now lets tru to call a view to our console
    // checkout the configurations to understande how did we bring the data
    var SchedulesView = context.SchedulesOverViews.ToList();
    Console.WriteLine("The View Result : ");
    foreach (var item in SchedulesView)
    {
        Console.WriteLine(
            $"CourseName : {item.CourseName} - SectionName : {item.SectionName} - InstructorName : {item.InstructorName} - Title : {item.Title} - Time : {item.StartTime} to {item.EndTime}"
        );
    }

    // now with the sql user-defined functions calling in the EF core,
    // first the Scalar value function
    // the configuration of the scalar done in the DbContext class check it out to know how we have done it
    // one thing to know is that calling the function should be done iside the linq  query
    var SectionsStudents = context
        .Sections.Select(section => new
        {
            SectionName = section.SectionName,
            StudentsCount = context.GetNumberOfStudentsPerSection(section.SectionName),
        })
        .Distinct()
        .ToList();
    Console.WriteLine("Use the user-defined Scalar Function: ");
    foreach (var item in SectionsStudents)
    {
        Console.WriteLine(
            $"The SectionName : {item.SectionName} - StudentsAttending : {item.StudentsCount}"
        );
    }

    Console.WriteLine();
    Console.WriteLine();
    // now lets call the TVFs and its actually like the any collection we can perform linq ops on item
    Console.WriteLine("The Result of the table valued function is : ");
    foreach (var item in context.Sections.ToList())
    {
        var StudentsPerSection = context.GetStudentsPerSectionShift(item.SectionName).ToList();
        foreach (var std in StudentsPerSection)
            Console.WriteLine(std);
    }
}
