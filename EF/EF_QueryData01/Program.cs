using System;
using System.Linq;
using EF_QueryData01.Data;
using EF_QueryData01.Entities;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            /* =========================================================================================
             * 1. DEFERRED VS. IMMEDIATE EXECUTION
             * =========================================================================================
             * Deferred Execution (IQueryable): Methods like .Where(), .Select(), and .OrderBy() DO NOT
             * execute against the database. They simply build a SQL expression tree in memory. You can
             * chain as many of these as you want without hitting the DB.
             * * Immediate Execution: Methods that return a single value or a concrete collection
             * (like .ToList(), .Single(), .First(), .Count(), .Any()) force EF Core to instantly
             * translate the expression tree into SQL and execute it against the database.
             */

            // Immediate execution examples:
            var section = context.Sections.Single(section => section.Id == 1);
            Console.WriteLine(
                $"Single __ The section has id {section.Id} - name : {section.SectionName}"
            );

            section = context.Sections.First(section => section.Id == 1);
            Console.WriteLine(
                $"First __ The section has id {section.Id} - name : {section.SectionName}"
            );

            /* =========================================================================================
             * 2. CLIENT VS. SERVER EVALUATION
             * =========================================================================================
             * Server-side: EF Core translates the C# LINQ into SQL and SQL Server does the heavy lifting.
             */
            var SelectQuery = context
                .Sections.Where(section => section.Id == 1)
                .Select(section => new { Name = section.SectionName });

            Console.WriteLine("\nThe Server-side query will look like this : ");
            Console.WriteLine(SelectQuery.ToQueryString());

            /* Client-side: If you use a custom C# function (like CalculateThePeriod), EF Core cannot
             * translate it to SQL. It will fetch the raw data from the DB first, and then run your
             * C# function in the application's memory (Client-side).
             * This can cause massive performance hits if it downloads thousands of rows
             * just to calculate something in memory.
             */
            var newSelectQuery = context
                .Sections.Where(section => section.Id == 1)
                .Select(static section => new
                {
                    Name = section.SectionName,
                    Hours = CalculateThePeriod(
                        section.TimeSlot.StartTime,
                        section.TimeSlot.EndTime
                    ),
                });

            Console.WriteLine(
                "\nThe Client-side query will look like this (notice the missing calculation in SQL): "
            );
            Console.WriteLine(newSelectQuery.ToQueryString());

            /* =========================================================================================
             * 3. TRACKING VS. NO-TRACKING
             * =========================================================================================
             */
            Console.WriteLine($"\nBefore changes : sectionName : {section.SectionName}");
            string OldSectionName = section.SectionName;
            section.SectionName = "NewSectionName"; // Tracked by default
            context.SaveChanges();

            section = context.Sections.First(section => section.Id == 1);
            Console.WriteLine($"After changes : sectionName : {section.SectionName}");

            section.SectionName = OldSectionName;
            context.SaveChanges();

            // AsNoTracking() makes the query read-only. It is much faster and consumes less memory.
            section = context.Sections.AsNoTracking().First(section => section.Id == 1);
            Console.WriteLine($"Before changes (NoTracking) : sectionName : {section.SectionName}");
            section.SectionName = "NewSectionName"; // EF Core ignores this change
            context.SaveChanges(); // Nothing happens
            Console.WriteLine(
                $"After changes (NoTracking) : sectionName : {section.SectionName}\n"
            );

            /* =========================================================================================
             * 4. EAGER LOADING (.Include & .ThenInclude)
             * =========================================================================================
             * WHAT IT IS: Loading the main entity and all its related data in a single, massive SQL
             * query using JOINs.
             * WHEN TO USE IT: Use it when you are 100% sure you need the related data immediately
             * (e.g., displaying a Section and its Student list on a webpage). It prevents the "N+1"
             * query problem by hitting the database only once.
             */
            var QuerySection = context
                .Sections.Where(section => section.Id == 6)
                .Include(section => section.Enrollments) // JOIN with Enrollments table
                    .ThenInclude(Enrollments => Enrollments.Student) // JOIN with Students table
                .Single();

            Console.WriteLine(
                $"The Students of this section {QuerySection.SectionName} (Eager Loading): "
            );
            foreach (var item in QuerySection.Enrollments)
                Console.WriteLine(
                    $"Enrollment ID : {item.SectionId},{item.StudentId} - StudentNAME : {item.Student.FirstName} {item.Student.LastName}"
                );
            Console.WriteLine();

            /* =========================================================================================
             * 5. EXPLICIT LOADING (context.Entry)
             * =========================================================================================
             * WHAT IT IS: Loading the main entity first, and then explicitly fetching the related data
             * later in a separate SQL query. By using .Query(), you can also filter the related data
             * before loading it (e.g., only loading students who passed).
             * WHEN TO USE IT: Use it when related data is massive, and you only need it under certain
             * conditions (e.g., an "if" statement). It saves memory by not loading everything upfront.
             */
            var targetSection = context.Sections.Where(section => section.Id == 6).Single();

            var queryEnrollments = context
                .Entry(targetSection)
                .Collection(section => section.Enrollments)
                .Query()
                .Include(enrollment => enrollment.Student)
                .ToList();

            Console.WriteLine(
                $"The Students of this section {targetSection.SectionName} (Explicit Loading): "
            );
            foreach (var item in queryEnrollments)
            {
                Console.WriteLine(
                    $"Enrollment ID : {item.SectionId},{item.StudentId} - StudentNAME : {item.Student.FirstName} {item.Student.LastName}"
                );
            }
            Console.WriteLine();

            /* =========================================================================================
             * 6. SELECT LOADING (PROJECTION / DTOs)
             * =========================================================================================
             * WHAT IT IS: Instead of loading entire tracked entities using .Include(), you project
             * exactly the columns you need into a custom anonymous object or DTO (Data Transfer Object).
             * * WHEN TO USE IT: Use this for READ-ONLY queries (e.g., returning data to a webpage or API)
             * where you do not intend to call SaveChanges() later.
             * * WHY IT IS AWESOME: Maximum performance. It generates highly optimized SQL that only
             * fetches the specific columns requested (ignoring all other columns in the table).
             * It completely bypasses the EF Core tracking system, saving memory.
             */
            var projectedSection = context
                .Sections.Where(section => section.Id == 6)
                .Select(section => new
                {
                    // 1. Grab only the specific column from the main table
                    SectionName = section.SectionName,

                    // 2. Dig directly into the navigation property without using .Include()
                    // and grab only the specific columns from the related tables
                    StudentsList = section
                        .Enrollments.Select(enrollment => new
                        {
                            StudentId = enrollment.StudentId,
                            FullName = enrollment.Student.FirstName
                                + " "
                                + enrollment.Student.LastName,
                        })
                        .ToList(),
                })
                .Single();

            Console.WriteLine(
                $"The Students of this section {projectedSection.SectionName} (Select Projection): "
            );
            foreach (var student in projectedSection.StudentsList)
            {
                Console.WriteLine(
                    $"Student ID : {student.StudentId} - Full Name : {student.FullName}"
                );
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public static int CalculateThePeriod(TimeSpan StartTime, TimeSpan EndTime)
    {
        return StartTime.Hours - EndTime.Hours;
    }
}

