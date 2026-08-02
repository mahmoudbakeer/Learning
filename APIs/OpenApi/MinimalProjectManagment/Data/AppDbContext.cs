using ControllerProjectManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControllerProjectManagement.Data;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    protected AppDbContext() { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}
