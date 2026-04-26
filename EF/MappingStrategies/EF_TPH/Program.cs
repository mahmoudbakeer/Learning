


using Migrations_001.Data;
using Migrations_001.Entities;
using System.Reflection;

using (var context = new AppDbContext())
{
 //   **1.BASE DEFAULT MAPPING(UNMAPPED BASE CLASS)
 //*-----------------------------------------------------------------------------------------
 //*WHAT IT IS: 
 //*The base class exists only in C# to share code (DRY principle). EF Core does not know 
 //* about the inheritance. Each derived class gets its own completely separate table with 
 //* all base and derived columns duplicated.
 //* * WHEN TO USE IT:
 //*Use when the base class is just a utility(e.g., an 'AuditableEntity' with CreatedAt
 //* and UpdatedAt).Use when you will NEVER need to query all child types together in a 
 //* single polymorphic query (e.g., you will never query context.BaseClasses).
 //* * HOW IT IS DONE:
 //*Simply do not add the Base class as a DbSet, or decorate the base class with the
 //* [NotMapped] attribute.Configure each derived class normally.
 //***2.TPH(TABLE - PER - HIERARCHY)---> * *THE EF CORE DEFAULT **
 //* -----------------------------------------------------------------------------------------
 //* WHAT IT IS: 
 //*All classes in the inheritance hierarchy (Base and all Derived classes) are mapped to 
 //* one single, massive table in the database. A 'Discriminator' column is automatically 
 //* added to tell EF Core which type of entity each row represents.
 //* * WHEN TO USE IT:
 //*This is the recommended default for 90 % of cases.Use it when performance is your top
 //* priority(it requires zero JOINs for querying) and when derived classes do not have 
 //* an overwhelming number of unique properties that would cause too many NULL columns.
 //* * HOW IT IS DONE:
 //*Happens automatically when you configure inheritance in C#. 
 //* Best Practice Optimization:
 //*builder.HasDiscriminator<int>("EntityTypeId")
 //* .HasValue<Student>(1)
 //* .HasValue<Instructor>(2);




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