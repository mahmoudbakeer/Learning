using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EF_SeedingData.Entities;

namespace EF_SeedingData.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            // configure the table name
            builder.ToTable("Sections");

            // configure the Primary Key
            builder.HasKey(section => section.Id);
            builder.Property(section => section.Id)
                .ValueGeneratedNever()// do not generate value in migrations, i will pass it
                .IsRequired(); 

            // configure the property
            builder.Property(sectinon => sectinon.SectionName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            // now lets configure the owned property
            builder.OwnsOne(section => section.TimeSlot, ts =>
            {
                ts.Property(ts => ts.StartTime).HasColumnType("time").HasColumnName("StartTime");
                ts.Property(ts => ts.EndTime).HasColumnType("time").HasColumnName("EndTime");
            });
            // Configures a one-to-many relationship between Course and Section.
            // Course is the principal, Section is the dependent.
            // IsRequired(true) ensures a section must belong to a course.
            builder.HasOne(section => section.Course)
                .WithMany(course => course.Sections)
                .HasForeignKey(section => section.CourseId)
                .IsRequired(true);

            // classify the new relationship with the schedule instead of many to many
            builder.HasOne(sec => sec.Schedule)
                .WithMany(sch => sch.Sections)
                .HasForeignKey(sec => sec.ScheduleId)
                .IsRequired();

            // Configures a one-to-many relationship between Instructor and Section.
            // IsRequired(false) means a section can exist without an assigned instructor.
            builder.HasOne(section => section.Instructor)
                .WithMany(instructor => instructor.Sections)
                .HasForeignKey(section => section.InstructorId)
                .IsRequired(false);

        }
    }
}
