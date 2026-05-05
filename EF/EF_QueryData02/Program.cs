using System;
using System.Linq;
using EF_QueryData02.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            /* =========================================================================================
            *  AVOIDING CARTESIAN EXPLOSION (DEEP PROJECTION)
            * =========================================================================================
            * WHAT IT IS: When you use multiple .Include() statements on 1-to-Many relationships
            * (e.g., Course -> Sections -> Enrollments), SQL Server generates a massive JOIN query.
            * This multiplies the rows returned (Cartesian Product), consuming massive RAM and network
            * bandwidth, even though the actual data is small.
            *
            * THE SOLUTION:
            * 1. Projection (.Select): Fetch exactly what you need. (Implemented below).
            * 2. Split Queries: Using .AsSplitQuery() after context.Courses to force EF Core to
            *    send multiple smaller, faster SQL queries instead of one massive JOIN.
            * =========================================================================================
            */
            var coursesQuery = context
                .Courses.AsNoTracking()
                .Select(course => new
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    Sections = course.Sections.Select(sect => new
                    {
                        SectionId = sect.Id,
                        SectionName = sect.SectionName,
                        TimeSlot = sect.TimeSlot,
                    }),
                })
                .ToList();
            Console.WriteLine("Courses : \n");
            foreach (var course in coursesQuery)
            {
                Console.Write($"\tCourseName : {course.CourseName} - CourseId : {course.CourseId}");
                Console.WriteLine("\tSections : ");
                foreach (var section in course.Sections)
                {
                    {
                        Console.Write(
                            $"\t\tSectionName : {section.SectionName} - SectionId : {section.SectionId} - TimeSlot : {section.TimeSlot.StartTime} to {section.TimeSlot.EndTime}"
                        );
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }

            /* =========================================================================================
             * 8. SPLIT QUERIES (AS-SPLIT-QUERY)
             * =========================================================================================
             * WHAT IT IS: Fixes the Cartesian Explosion problem without needing to write a custom
             * .Select() projection. It forces EF Core to send multiple smaller SQL queries to the DB
             * instead of one massive JOIN query.
             *
             * HOW IT LINKS THEM (IDENTITY RESOLUTION / MEMORY FIX-UP):
             * EF Core DOES NOT use SQL to link the separated queries. Instead:
             * 1. Query 1 brings the parent records (Courses) into your server's RAM.
             * 2. Query 2 brings the child records (Sections) into RAM.
             * 3. EF Core acts as the "assembler" in memory. It looks at the Foreign Keys of the
             *    children and physically injects them into the correct parent objects' lists in C#.
             *
             * THE CONS (WHEN TO AVOID IT):
             * 1. Data Inconsistency (No Atomicity): Because EF Core sends multiple queries one after
             *    the other, another user might update the database *between* your first and second
             *    query. This can result in you fetching mismatched or inconsistent data.
             * 2. Network Latency (Round-trips): Sending 5 separate queries means making 5 separate
             *    network trips to the database server. If your database is hosted far away
             *    (high ping), the wait time for these multiple round-trips will be worse than the
             *    Cartesian Explosion.
             *
             * GLOBAL CONFIG: You can set this globally in AppDbContext.OnConfiguring:
             * optionsBuilder.UseSqlServer("...").UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
             *
             * OVERRIDE: If set globally, you can force a specific query back to a single JOIN using:
             * context.Courses.Include(c => c.Sections).AsSingleQuery();
             * =========================================================================================
             */
            var ResultOfSplitQuery = context
                .Courses.AsNoTracking() // Added for performance
                .Include(course => course.Sections)
                .AsSplitQuery() // Explicitly split the query
                .ToList(); // Execute and bring to memory

            Console.WriteLine("Let's see the result of the split query:\n");

            foreach (var item in ResultOfSplitQuery)
            {
                Console.WriteLine($"The courseName : {item.CourseName}, Id : {item.Id}");

                foreach (var sec in item.Sections)
                {
                    Console.WriteLine(
                        $"\tThe SectionName : {sec.SectionName} - TimeSlot : {sec.TimeSlot.StartTime} to {sec.TimeSlot.EndTime}"
                    );
                }
                Console.WriteLine();
            }

            // now with joins
            // inner joins
            var InnerMethodSyn = context
                .Courses.AsNoTracking()
                .Join(
                    context.Sections.AsNoTracking(),
                    c => c.Id,
                    sec => sec.CourseId,
                    (course, sec) =>
                        new
                        {
                            CourseName = course.CourseName,
                            SectionName = sec.SectionName,
                            TimeSlot = sec.TimeSlot,
                        }
                );

            // or we can do it using the query syntax
            var InnderQuerySyn =
                from c in context.Courses.AsNoTracking()
                join sec in context.Sections on c.Id equals sec.CourseId
                select new
                {
                    CourseName = c.CourseName,
                    SectionName = sec.SectionName,
                    TimeSlot = sec.TimeSlot,
                };
            // lets print it
            Console.WriteLine();
            Console.WriteLine("The result of inner join :");
            foreach (var item in InnderQuerySyn)
            {
                Console.WriteLine(
                    $"CourseName : {item.CourseName} - SectionName : {item.SectionName} - TimeSlot : {item.TimeSlot.StartTime} to {item.TimeSlot.EndTime}"
                );
            }
            Console.WriteLine();

            var LeftJoinQuerySyn =
                from c in context.Offices.AsNoTracking()
                join sec in context.Instructors on c.Id equals sec.OfficeId into OfficeInstructor
                from cv in OfficeInstructor.DefaultIfEmpty()
                select new
                {
                    OfficeName = c.OfficeName,
                    OfficeLocation = c.OfficeLocation,
                    InstructorName = cv == null ? "Not Occupied" : cv.LastName + " " + cv.FirstName,
                };

            // with method syntax
            var LeftJoinMethodSyn = context
                .Offices.AsNoTracking()
                .GroupJoin(
                    context.Instructors.AsNoTracking(),
                    office => office.Id,
                    instructor => instructor.OfficeId,
                    (office, instructor) => new { office, instructor }
                )
                .SelectMany(
                    officeInstructor => officeInstructor.instructor.DefaultIfEmpty(),
                    (OI, inst) =>
                        new
                        {
                            OfficeName = OI.office.OfficeName,
                            OfficeLocation = OI.office.OfficeLocation,
                            InstructorName = inst == null
                                ? "Not Occupied"
                                : inst.LastName + " " + inst.FirstName,
                        }
                );

            // lets print it
            Console.WriteLine();
            Console.WriteLine("The result of Left join :");
            foreach (var item in LeftJoinMethodSyn)
            {
                Console.WriteLine(
                    $"OfficeName : {item.OfficeName} - OfficeLocation : {item.OfficeLocation} - InstructorName : {item.InstructorName}"
                );
            }
            Console.WriteLine();

            // note there is no need to write code for cross join
            // the cross join mean that each row of the first table will be combined with each row in the opposite table
            // this will produce the Cartesian product , means 100 row in first table and 200 row in the second , will produce 20000 row

            // now with the select many exerxises
            // we want to get all the students names studing in CourseName 'CS-50'
            var CsStudnets = context
                .Courses.AsNoTracking()
                .Where(course => course.CourseName == "CS-50")
                .SelectMany(course => course.Sections)
                .SelectMany(
                    cs => cs.Enrollments,
                    (section, enrollment) =>
                        new
                        {
                            StudentName = enrollment.Student.FirstName
                                + " "
                                + enrollment.Student.LastName,
                            SectionName = section.SectionName,
                        }
                );
            Console.WriteLine("SelectMany, Students studing the CS-50 : ");
            foreach (var item in CsStudnets)
            {
                Console.WriteLine(
                    $"SectionName : {item.SectionName} - StudentName : {item.StudentName}"
                );
            }
            Console.WriteLine();

            var InstructorSection = context
                .Sections.AsNoTracking() // Always use for read-only queries!
                .GroupBy(
                    sect => sect.Instructor, // The Key we are grouping by
                    (Inst, sectionsGroup) =>
                        new
                        {
                            InstructorName = Inst.FirstName + " " + Inst.LastName,

                            //  Extract ONLY the SectionNames as a list of strings
                            SectionNames = sectionsGroup.Select(s => s.SectionName).ToList(),
                        }
                )
                .ToList(); // Execute the query

            Console.WriteLine("GroupBy, Instructor teaching in sections:");
            foreach (var item in InstructorSection)
            {
                Console.WriteLine($"\tInstructorName : {item.InstructorName}");

                // Now String.Join works beautifully because it's joining a list of pure strings!
                Console.WriteLine($"\tSections : {string.Join(", ", item.SectionNames)}\n");
            }
            Console.WriteLine();

            // now with the pagination
            // instead of getting the whole data as one single query, we can use the pagination
            // means at each time we want more we get specific number of rows
            // that significantly reduces the pressure on the server and enhance the performance
            // please make sure to turnoff this when you want to runt the code
            Console.Clear();
            var QueryResult = context
                .Sections.Include(sec => sec.Instructor)
                .Include(sec => sec.Schedule)
                .Include(sec => sec.Course);

            // now with the pagination
            var pageSize = 2;
            var totalPages = (int)Math.Ceiling((double)QueryResult.Count() / pageSize);
            var pageNumber = 1;

            Console.WriteLine("The Pages : ");
            while (pageNumber <= totalPages)
            {
                var tempPage = QueryResult.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                foreach (var item in tempPage)
                {
                    Console.WriteLine("=========================================================");
                    Console.WriteLine($"SectionName : {item.SectionName}");
                    Console.WriteLine(
                        $"The instructorName : {item.Instructor.FirstName} - {item.Instructor.LastName}"
                    );
                    Console.WriteLine($"The ScheduleTitle : {item.Schedule.Title.ToString()}");
                    Console.WriteLine($"The CourseName : {item.Course.CourseName}");
                    Console.WriteLine("=========================================================");
                }
                Console.WriteLine($"The Page Number : {pageNumber}");
                Console.WriteLine();
                Console.WriteLine();
                ++pageNumber;
                Console.ReadKey();
            }
        }
    }
}
