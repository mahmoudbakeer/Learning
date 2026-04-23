using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrations_001.Entities;

namespace Migrations_001.Data.Configurations
{
    public class InstructoreConfiguraiton : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            // configure the table name
            builder.ToTable("Instructors");

            // configure the Primary Key
            builder.HasKey(instructor => instructor.Id);
            builder.Property(instructor => instructor.Id)
                .ValueGeneratedNever(); // do not generate value in migrations, i will pass it

            // configure the property
            builder.Property(instructor => instructor.FirstName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255).IsRequired();


            // configure the property
            builder.Property(instructor => instructor.LastName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255).IsRequired();


            builder.HasData(LoadInstructors()); // this will insert the data at the migration time 
        }
        private static List<Instructor> LoadInstructors()
        {
            return new List<Instructor>
            {
                new Instructor { Id = 1, FirstName = "Ahmed", LastName = "Abdullah"},
                new Instructor { Id = 2, FirstName = "Yasmeen", LastName = "Yasmeen"},
                new Instructor { Id = 3, FirstName = "Khalid", LastName = "Hassan"},
                new Instructor { Id = 4, FirstName = "Nadia", LastName = "Ali"},
                new Instructor { Id = 5, FirstName = "Ahmed", LastName = "Abdullah"},
            };
        }
    }
}
