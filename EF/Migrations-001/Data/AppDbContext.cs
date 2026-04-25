using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Migrations_001.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Migrations_001.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // the best practice is to make group call configuration
            // use the ApplyConfigurationFromAssembly()
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // this will seach on configurations in the assembly that implement the IEntityTypeConfiguraiton
        }

        // bad practice, always use the DI but since this is Learning project no worries
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // now lets connect
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // get the connection string
            string conStr = config.GetConnectionString("DefaultConnection");

            // pass the connection to the provider
            optionsBuilder.UseSqlServer(conStr);
        }
    }
}
