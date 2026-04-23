using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrations_001.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrations_001.Data.Configurations
{
    public class CourseConfiguraiton : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // configure the table name
            builder.ToTable("Courses");

            // configure the Primary Key
            builder.HasKey(course => course.Id);
            builder.Property(course => course.Id)
                .ValueGeneratedNever()// do not generate value in migrations, i will pass it

                .IsRequired(); 
            // configure the property
            builder.Property(course => course.CourseName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            // configure the property
            builder.Property(course => course.Price)
                .HasPrecision(15, 2)
                .IsRequired(); // the decimal precision

            builder.HasData(LoadCourses()); // this will insert the data at the migration time 
        }
        private static List<Course> LoadCourses()
        {
            return new List<Course>
            {
                new Course { Id = 1, CourseName = "Mathmatics", Price = 1000m},
                new Course { Id = 2, CourseName = "Physics", Price = 2000m},
                new Course { Id = 3, CourseName = "Chemistry", Price = 1500m},
                new Course { Id = 4, CourseName = "Biology", Price = 1200m},
                new Course { Id = 5, CourseName = "CS-50", Price = 3000m},
            };
        }
    }
}
