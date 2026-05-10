using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StudentsPerSectionShiftConfiguration
    : IEntityTypeConfiguration<StudentsPerSectionShift>
{
    public void Configure(EntityTypeBuilder<StudentsPerSectionShift> builder)
    {
        // make it as keyless entity
        builder.HasNoKey();
    }
}
