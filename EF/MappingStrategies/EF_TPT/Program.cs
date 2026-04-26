


using Migrations_001.Data;
using Migrations_001.Entities;
using System.Reflection;

using (var context = new AppDbContext())
{
    /* * * 3. TPT (TABLE-PER-TYPE)
 * -----------------------------------------------------------------------------------------
 * WHAT IT IS: 
 * Strict database normalization. Every class (both Base and Derived) gets its own exact 
 * table. The derived tables only contain their specific columns and are linked back to 
 * the base table using a Foreign Key (which also acts as the Primary Key).
 * * WHEN TO USE IT:
 * Use ONLY when your Database Administrator (DBA) strictly forbids NULL columns, or 
 * when derived classes have a massive amount of unique properties. Avoid if possible, 
 * as querying a derived type requires slow SQL JOIN operations.
 * * HOW IT IS DONE:
 * builder.Entity<Student>().ToTable("Students");
 * builder.Entity<Instructor>().ToTable("Instructors");
 * // EF Core figures out the Foreign Key relationships automatically.
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

    context.SaveChanges();

    Console.WriteLine("Individuals : ");
    foreach(var student in context.Set<Student>().OfType<Individual>())
        Console.WriteLine($"name : {student.FirstName} - University : {student.University}");

    Console.WriteLine("Employees : ");
    foreach(var student in context.Set<Student>().OfType<Employee>())
        Console.WriteLine($"name : {student.FirstName} - Company : {student.Company}");
}