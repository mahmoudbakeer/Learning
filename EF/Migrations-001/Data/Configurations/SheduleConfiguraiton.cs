using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrations_001.Entities;
using Migrations_001.Entities.Enum;

namespace Migrations_001.Data.Configurations
{
    public class SheduleConfiguraiton : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            // configure the table name
            builder.ToTable("Schedules");

            // configure the Primary Key
            builder.HasKey(schedule => schedule.Id);
            builder.Property(schedule => schedule.Id)
                .ValueGeneratedNever()// do not generate value in migrations, i will pass it
                .IsRequired();

            //// configure the property
            //builder.Property(schedule => schedule.Title)
            //    .HasColumnType("VARCHAR")
            //    .HasMaxLength(255)
            //    .IsRequired();

            // now lets do conversion for the Title Property
            builder.Property(sc => sc.Title)
                .HasConversion(
                title => title.ToString(),
                title => (ScheduleEnum)Enum.Parse(typeof(ScheduleEnum), title)
               );
        }
    }
}
