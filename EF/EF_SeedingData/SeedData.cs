using EF_SeedingData.Entities;
using EF_SeedingData.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SeedingData
{
    public static class SeedData
    {

        // Method to load data for Offices
        public static List<Office> LoadOffices() => new()
        {
            new Office { Id = 1, OfficeName = "Off_05", OfficeLocation = "Building A" },
            new Office { Id = 2, OfficeName = "Off_12", OfficeLocation = "Building B" },
            new Office { Id = 3, OfficeName = "Off_32", OfficeLocation = "Administration" },
            new Office { Id = 4, OfficeName = "Off_44", OfficeLocation = "IT Department" },
            new Office { Id = 5, OfficeName = "Off_43", OfficeLocation = "IT Department" }
        };

        // Method to load data for Courses
        public static List<Course> LoadCourses() => new()
        {
           new Course { Id = 1, CourseName = "Mathematics", Price = 1000.00m },
           new Course { Id = 2, CourseName = "Physics", Price = 2000.00m },
           new Course { Id = 3, CourseName = "Chemistry", Price = 1500.00m },
           new Course { Id = 4, CourseName = "Biology", Price = 1200.00m },
           new Course { Id = 5, CourseName = "CS-50", Price = 3000.00m }
        };

        // Method to load data for Schedules
        public static List<Schedule> LoadSchedules() => new()
        {
          new Schedule { Id = 1, Title = ScheduleEnum.Daily, SUN = true, MON = true, TUE = true, WED = true, THU = true, FRI = false, SAT = false },
          new Schedule { Id = 2, Title = ScheduleEnum.DayAfterDay, SUN = true, MON = false, TUE = true, WED = false, THU = true, FRI = false, SAT = false },
          new Schedule { Id = 3, Title = ScheduleEnum.TwoDaysAWeek, SUN = false, MON = true, TUE = false, WED = true, THU = false, FRI = false, SAT = false },
          new Schedule { Id = 4, Title = ScheduleEnum.Weekend, SUN = false, MON = false, TUE = false, WED = false, THU = false, FRI = true, SAT = true },
          new Schedule { Id = 5, Title = ScheduleEnum.Compact, SUN = true, MON = true, TUE = true, WED = true, THU = true, FRI = true, SAT = true }
        };

        // Method to load data for Instructors
        public static List<Instructor> LoadInstructors() => new()
        {
            new Instructor { Id = 1, FirstName = "Ahmed", LastName = "Abdullah", OfficeId = 1 },
            new Instructor { Id = 2, FirstName = "Yasmeen", LastName = "Mohammed", OfficeId = 2 },
            new Instructor { Id = 3, FirstName = "Khalid", LastName = "Hassan", OfficeId = 3 },
            new Instructor { Id = 4, FirstName = "Nadia", LastName = "Ali", OfficeId = 4 },
            new Instructor { Id = 5, FirstName = "Omar", LastName = "Ibrahim", OfficeId = 5 }
        };

        // Method to load data for Sections
        public static List<Section> LoadSections() => new()
        {
            new Section { Id = 1, SectionName = "S_MA1", CourseId = 1, InstructorId = 1, ScheduleId = 1, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("08:00:00"), EndTime = TimeSpan.Parse("10:00:00") } },
            new Section { Id = 2, SectionName = "S_MA2", CourseId = 1, InstructorId = 2, ScheduleId = 3, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("14:00:00"), EndTime = TimeSpan.Parse("18:00:00") } },
            new Section { Id = 3, SectionName = "S_PH1", CourseId = 2, InstructorId = 1, ScheduleId = 4, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("15:00:00") } },
            new Section { Id = 4, SectionName = "S_PH2", CourseId = 2, InstructorId = 3, ScheduleId = 1, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("12:00:00") } },
            new Section { Id = 5, SectionName = "S_CH1", CourseId = 3, InstructorId = 2, ScheduleId = 1, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("16:00:00"), EndTime = TimeSpan.Parse("18:00:00") } },
            new Section { Id = 6, SectionName = "S_CH2", CourseId = 3, InstructorId = 3, ScheduleId = 2, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("08:00:00"), EndTime = TimeSpan.Parse("10:00:00") } },
            new Section { Id = 7, SectionName = "S_BI1", CourseId = 4, InstructorId = 4, ScheduleId = 3, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("11:00:00"), EndTime = TimeSpan.Parse("14:00:00") } },
            new Section { Id = 8, SectionName = "S_BI2", CourseId = 4, InstructorId = 5, ScheduleId = 4, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("14:00:00") } },
            new Section { Id = 9, SectionName = "S_CS1", CourseId = 5, InstructorId = 4, ScheduleId = 4, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("16:00:00"), EndTime = TimeSpan.Parse("18:00:00") } },
            new Section { Id = 10, SectionName = "S_CS2", CourseId = 5, InstructorId = 5, ScheduleId = 3, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("12:00:00"), EndTime = TimeSpan.Parse("15:00:00") } },
            new Section { Id = 11, SectionName = "S_CS3", CourseId = 5, InstructorId = 4, ScheduleId = 5, TimeSlot = new TimeSlot { StartTime = TimeSpan.Parse("09:00:00"), EndTime = TimeSpan.Parse("11:00:00") } }
        };


        // Method to load data for Corporates
        public static List<Employee> LoadCorporates() => new()
        {
            new Employee { Id = 2, FirstName = "Noor", LastName = "Saleh", Company = "ABC", Title = "Developer" },
            new Employee { Id = 4, FirstName = "Huda", LastName = "Ahmed", Company = "ABC", Title = "QA" },
            new Employee { Id = 7, FirstName = "Yousef", LastName = "Farid", Company = "EFG", Title = "Developer" },
            new Employee { Id = 8, FirstName = "Layla", LastName = "Mustafa", Company = "EFG", Title = "QA" }
        };

        // Method to load data for Individuals
        public static List<Individual> LoadIndividuals() => new()
        {
            new Individual { Id = 1, FirstName = "Fatima", LastName = "Ali",University = "XYZ", YearOfGraduation = 2024, IsIntern = false },
            new Individual { Id = 3, FirstName = "Omar", LastName = "Youssef", University = "POQ", YearOfGraduation = 2023, IsIntern = true },
            new Individual { Id = 5, FirstName = "Amira", LastName = "Tariq", University = "POQ", YearOfGraduation = 2025, IsIntern = false },
            new Individual { Id = 6, FirstName = "Zainab", LastName = "Ismail", University = "POQ", YearOfGraduation = 2023, IsIntern = true },
            new Individual { Id = 9, FirstName = "Mohammed", LastName = "Adel", University = "XYZ", YearOfGraduation = 2024, IsIntern = false },
            new Individual { Id = 10, FirstName = "Samira", LastName = "Nabil", University = "XYZ", YearOfGraduation = 2024, IsIntern = false }
        };


        // Method to load data for Enrollments
        public static List<Enrollment> LoadEnrollments() => new()
        {
            new Enrollment { SectionId = 6, StudentId = 1 },
            new Enrollment { SectionId = 6, StudentId = 2 },
            new Enrollment { SectionId = 7, StudentId = 3 },
            new Enrollment { SectionId = 7, StudentId = 4 },
            new Enrollment { SectionId = 8, StudentId = 5 },
            new Enrollment { SectionId = 8, StudentId = 6 },
            new Enrollment { SectionId = 9, StudentId = 7 },
            new Enrollment { SectionId = 9, StudentId = 8 },
            new Enrollment { SectionId = 10, StudentId = 9 },
            new Enrollment { SectionId = 10, StudentId = 10 }
        };
    }
}
