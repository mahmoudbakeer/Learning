using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrations_001.Entities;
using System.Collections.Generic;

namespace Migrations_001.Data.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructors");

            builder.HasKey(instructor => instructor.Id);

            builder.Property(instructor => instructor.Id)
                .ValueGeneratedNever();

            builder.Property(instructor => instructor.FirstName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255).IsRequired();

            // Configures the one-to-one relationship from the dependent side.
            // This is the correct approach to easily specify if the relationship is optional or required.
            builder.HasOne(instructor => instructor.Office)
                .WithOne(office => office.Instructor)
                .HasForeignKey<Instructor>(instructor => instructor.OfficeId)
                .IsRequired(false);

            builder.Property(instructor => instructor.LastName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255).IsRequired();

        }
    }
}