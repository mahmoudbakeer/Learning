using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrations_001.Entities;

namespace Migrations_001.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedNever();

            builder.Property(s => s.FirstName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(s => s.LastName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            // name the descriminator, and the values
            builder.HasDiscriminator<string>("StudentType")
                .HasValue<Individual>("INDV")
                .HasValue<Employee>("EMPL");

            builder.Property("StudentType")
                .HasMaxLength(4);
        }
    }
}