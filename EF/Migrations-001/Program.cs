using Microsoft.EntityFrameworkCore;
using Migrations_001.Data;
using Migrations_001.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Migrations_001
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Old query limited to direct relationships:
                // var sections = context.Set<SectionSchedule>()
                //     .Include(sc => sc.Section)
                //     .Include(sc => sc.Schedule)
                //     .AsNoTracking()
                //     .ToList();

                // Best Practice: Use ThenInclude() to eagerly load nested relationships.
                // This ensures all required navigation properties are retrieved in one efficient SQL JOIN query.
                var sections = context.Sections
                    .Include(sc => sc.Schedule)
                    .Include(sc => sc.Instructor)
                    .Include(sc => sc.Course)
                    .ToList();

                // Expand the console table headers to fit the new data columns.
                Console.WriteLine(new string('-', 125));
                Console.WriteLine(string.Format("{0,-5} | {1,-15} | {2,-10} | {3,-18} | {4,-15} | {5,-15} | {6}",
                    "ID", "Course", "Section", "Instructor", "Schedule", "Time", "Active Days"));
                Console.WriteLine(new string('-', 125));

                foreach (var sc in sections)
                {
                    string timeRange = $"{sc.TimeSlot.StartTime:hh\\:mm} - {sc.TimeSlot.EndTime:hh\\:mm}";
                    string activeDays = GetActiveDays(sc.Schedule);

                    // Handles the optional relationship for Instructor safely.
                    string instructorName = sc.Instructor != null
                        ? $"{sc.Instructor.FirstName} {sc.Instructor.LastName}"
                        : "No Instructor";

                    string courseName = sc.Course?.CourseName ?? "N/A";
                    string sectionName = sc.SectionName ?? "N/A";
                    string scheduleTitle = sc.Schedule.Title.ToString() ?? "N/A";

                    Console.WriteLine(string.Format("{0,-5} | {1,-15} | {2,-10} | {3,-18} | {4,-15} | {5,-15} | {6}",
                        sc.Id,
                        courseName,
                        sectionName,
                        instructorName,
                        scheduleTitle,
                        timeRange,
                        activeDays));
                }

                Console.WriteLine(new string('-', 125));
            }
        }

        private static string GetActiveDays(Schedule schedule)
        {
            if (schedule == null) return "N/A";

            var days = new List<string>();

            if (schedule.SUN) days.Add("Sun");
            if (schedule.MON) days.Add("Mon");
            if (schedule.TUE) days.Add("Tue");
            if (schedule.WED) days.Add("Wed");
            if (schedule.THU) days.Add("Thu");
            if (schedule.FRI) days.Add("Fri");
            if (schedule.SAT) days.Add("Sat");

            return string.Join(", ", days);
        }
    }
}