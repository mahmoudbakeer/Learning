using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class osp_StudentsCountAndScedulePerCourseConfiguration
    : IEntityTypeConfiguration<sp_StudentsCountAndScedulePerCourse>
{
    public void Configure(EntityTypeBuilder<sp_StudentsCountAndScedulePerCourse> builder)
    {
        // make it as keyless entity
        builder.HasNoKey();
    }
}
