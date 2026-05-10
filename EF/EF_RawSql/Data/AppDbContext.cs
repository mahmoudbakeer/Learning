using EF_RawSql.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF_RawSql.Data;

public class AppDbContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Student> Students { get; set; }

    //public DbSet<Individual> Individuals { get; set; } // now we added them means the ef core will treat them TPH
    //public DbSet<Employee> Employees { get; set; } // now we added them means the ef core will treat them TP
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<sp_StudentsCountAndScedulePerCourse> sp_StudentsCountAndScedulePerCourses { get; set; }
    public DbSet<SchedulesOverView> SchedulesOverViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // the best practice is to make group call configuration
        // use the ApplyConfigurationFromAssembly()
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // this will seach on configurations in the assembly that implement the IEntityTypeConfiguraiton

        // now lets spicify the TVFs, means configure it
        modelBuilder
            .HasDbFunction(
                typeof(AppDbContext).GetMethod(nameof(AppDbContext.GetStudentsPerSectionShift))
            )
            .HasName("GetNumberOfStudentsPerSectionShifts");
    }

    // bad practice, always use the DI but since this is Learning project no worries
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // now lets connect
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        // get the connection string
        string conStr = config.GetConnectionString("DefaultConnection");

        // pass the connection to the provider
        // you can tell the EF Core that any data comes by default make it NoTrack or track all
        optionsBuilder
            .UseSqlServer(conStr)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }

    [DbFunction("GetNumberOfStudentsPerSection", "dbo")]
    public int GetNumberOfStudentsPerSection(string SectionName)
    {
        throw new NotImplementedException();
    }

    public IQueryable<StudentsPerSectionShift> GetStudentsPerSectionShift(string sectionname) =>
        FromExpression(() => GetStudentsPerSectionShift(sectionname));
}
