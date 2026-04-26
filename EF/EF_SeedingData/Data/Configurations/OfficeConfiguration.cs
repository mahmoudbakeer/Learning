using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EF_SeedingData.Entities;
using System.Collections.Generic;

namespace EF_SeedingData.Data.Configurations
{
    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.ToTable("Offices");

            builder.HasKey(office => office.Id);

            // builder.Property(office => office.Id).ValueGeneratedNever().IsRequired();
            builder.Property(office => office.Id)
                .ValueGeneratedNever(); // Primary keys are implicitly required.

            builder.Property(office => office.OfficeName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            // Best Practice: Use IsUnique() for non-primary key properties that must not have duplicate values.
            // This ensures no two offices can have the exact same OfficeName in the database.
            builder.HasIndex(office => office.OfficeName)
                .IsUnique();

            builder.Property(office => office.OfficeLocation)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            // builder.HasIndex(office => office.Id).IsUnique();
            // Explanation: A primary key automatically creates a unique index in the database.
            // Explicitly configuring a unique index on the Id property is redundant and unnecessary.

        }
    }
}