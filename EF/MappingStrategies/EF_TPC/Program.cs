


using Migrations_001.Data;
using Migrations_001.Entities;
using System.Reflection;

using (var context = new AppDbContext())
{
        /* * * 4. TPC (TABLE-PER-CONCRETE-TYPE) ---> ** NATIVE IN EF CORE 7+ **
     * -----------------------------------------------------------------------------------------
     * WHAT IT IS: 
     * Only concrete (derived) classes get tables. The base class does NOT get a table. 
     * Each derived table contains ALL columns (both its own and the ones inherited from 
     * the base class). No JOINs, no Discriminators, no NULLs.
     * * WHEN TO USE IT:
     * Use when you have large tables and you usually query specific derived types 
     * (e.g., you query Students alone, Instructors alone). It provides excellent read 
     * performance for specific types without the NULL clutter of TPH. 
     * Warning: Querying the base type (context.Quiz) is slow because it uses UNION ALL.
     * * HOW IT IS DONE:
     * // Apply this single line to the BASE class configuration:
     * builder.UseTpcMappingStrategy(); 
     * // EF Core will automatically create sequences to prevent Primary Key overlap.
     * =========================================================================================
     */


    //var student1 = new Individual
    //{
    //    Id = 1,
    //    FirstName = "Mahmoud",
    //    LastName = "Bakir",
    //    YearOfGraduation = 2027,
    //    IsIntern = false,
    //    University = "Sharda"
    //};
    //var student2 = new Employee
    //{
    //    Id = 2,
    //    FirstName = "Samer",
    //    LastName = "Hamed",
    //    Title = "Developer",
    //    Company = "ISPC",
    //    YearsOfExperience = 1
    //};


    //context.Students.Add(student1);
    //context.Students.Add(student2);

    //context.SaveChanges();

    Console.WriteLine("Individuals : ");
    foreach(var student in context.Set<Student>().OfType<Individual>())
        Console.WriteLine($"name : {student.FirstName} - University : {student.University}");

    Console.WriteLine("Employees : ");
    foreach(var student in context.Set<Student>().OfType<Employee>())
        Console.WriteLine($"name : {student.FirstName} - Company : {student.Company}");
}