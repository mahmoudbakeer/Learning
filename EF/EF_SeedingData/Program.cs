


using EF_SeedingData;
using EF_SeedingData.Data;
using EF_SeedingData.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

/* =========================================================================================
 * EF CORE DATA SEEDING STRATEGIES
 * =========================================================================================
 *
 * * 1. THE 'HasData' METHOD (MODEL SEEDING)
 * -----------------------------------------------------------------------------------------
 * WHAT IT IS: 
 * Defining data directly inside 'OnModelCreating' or IEntityTypeConfiguration. EF Core 
 * tracks this data and generates INSERT/UPDATE/DELETE statements in your migrations.
 * * HOW IT IS DONE:
 * builder.HasData(new Student { Id = 1, Name = "Mahmoud" });
 * // Note: You must explicitly provide the Primary Key (Id) even if it is auto-incremented.
 *
 * WHEN TO USE IT:
 * For static, read-only lookup data that rarely changes (e.g., Application Roles like 
 * "Admin"/"User", system configurations, fixed categories).
 * * WHEN TO AVOID IT:
 * Avoid for dynamic data (like random dates, generated passwords) because the data is 
 * locked into the migration snapshot. Avoid for massive datasets (thousands of rows) 
 * because it will bloat your migration files and make them unreadable.
 *
 * * 2. MANUAL 'InsertData' OR 'Sql' IN MIGRATIONS
 * -----------------------------------------------------------------------------------------
 * WHAT IT IS: 
 * Creating an empty migration (Add-Migration BulkSeed) and manually writing the insert 
 * commands inside the 'Up' method using migrationBuilder.
 * * HOW IT IS DONE (SAFELY):
 * protected override void Up(MigrationBuilder migrationBuilder)
 * {
 * // Using InsertData:
 * migrationBuilder.InsertData(table: "Students", columns: new[] { "Id", "Name" }, values: new object[] { 2, "Test" });
 * * // OR using raw SQL for massive files:
 * // migrationBuilder.Sql("INSERT INTO Students (Name) VALUES ('Bulk Student')");
 * }
 * * protected override void Down(MigrationBuilder migrationBuilder)
 * {
 * // Clarification: To avoid messing up the database during a rollback, you MUST 
 * // write the exact opposite command in the Down method.
 * migrationBuilder.DeleteData(table: "Students", keyColumn: "Id", keyValue: 2);
 * // OR: migrationBuilder.Sql("DELETE FROM Students WHERE Name = 'Bulk Student'");
 * }
 *
 * WHEN TO USE IT:
 * For massive bulk inserts (e.g., seeding 10,000 postal codes) where performance matters. 
 * Raw SQL handles bulk operations significantly faster than EF Core tracking.
 *
 * * 3. THE NORMAL '.Add()' IN MAIN/STARTUP (CUSTOM INITIALIZATION)
 * -----------------------------------------------------------------------------------------
 * WHAT IT IS: 
 * Writing standard C# logic in your Program.cs or a separate Seeder class to instantiate 
 * your DbContext, add objects, and call SaveChanges().
 * * HOW IT IS DONE:
 * using (var context = new AppDbContext())
 * {
 * // Note: ALWAYS check if the table is empty first to avoid duplicate inserts 
 * // every time the application runs.
 * if (!context.Set<Student>().Any()) 
 * {
 * context.Set<Student>().Add(new Student { Name = "Startup Student" });
 * context.SaveChanges();
 * }
 * }
 *
 * WHEN TO USE IT:
 * When you need Dependency Injection (e.g., using a PasswordHasher service for users).
 * When you need to read data dynamically from external sources at runtime (JSON files, 
 * external APIs). When you want to keep seeding logic completely separate from your 
 * migration history.
 * =========================================================================================
 */



using (var context = new AppDbContext())
{
    // create api, to make sure the database is created, this does not add anything to the migratinos]
    // use it usually for testing applications, The provider does not have migrations
    await context.Database.EnsureCreatedAsync();

    // this way also avoid seeding the data using the migration, and bypass the owned entitiy limitations 
    if (!await context.Set<Individual>().AnyAsync())
    {
        context.Set<Individual>().AddRange(SeedData.LoadIndividuals());
    }
    if (!await context.Set<Employee>().AnyAsync())
    {
        context.Set<Employee>().AddRange(SeedData.LoadCorporates());
    }
    if (!await context.Set<Course>().AnyAsync())
    {
        context.Set<Course>().AddRange(SeedData.LoadCourses());
    }
    if (!await context.Set<Office>().AnyAsync())
    {
        context.Set<Office>().AddRange(SeedData.LoadOffices());
    }
    if (!await context.Set<Schedule>().AnyAsync())
    {
        context.Set<Schedule>().AddRange(SeedData.LoadSchedules());
    }
    if (!await context.Set<Instructor>().AnyAsync())
    {
        context.Set<Instructor>().AddRange(SeedData.LoadInstructors());
    }
    if (!await context.Set<Section>().AnyAsync())
    {
        context.Set<Section>().AddRange(SeedData.LoadSections());
    }
    if (!await context.Set<Enrollment>().AnyAsync())
    {
        context.Set<Enrollment>().AddRange(SeedData.LoadEnrollments());
    }

    await context.SaveChangesAsync();


    // to drop the database in the same way and the same use of the EnsureCreatedAsync()
    // context.Database.EnsureDeletedAsync();
}