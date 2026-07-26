using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ControllerProjectManagement.Entities;
namespace ControllerProjectManagement.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.AssignedUserId).IsRequired();
        builder.Property(t => t.ProjectId).IsRequired();
        builder.Property(t => t.Status).IsRequired();


        // configure the relationship between ProjectTask and Project
        builder.HasOne(t => t.Project)
        .WithMany(p => p.Tasks)
        .HasForeignKey(t => t.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}