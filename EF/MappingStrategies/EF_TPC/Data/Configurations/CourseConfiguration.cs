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

        }
    }
}
