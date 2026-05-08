using System.Collections.Generic;
using EF_RawSql.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SchedulesOverViewConfiguration : IEntityTypeConfiguration<SchedulesOverView>
{
    public void Configure(EntityTypeBuilder<SchedulesOverView> builder)
    {
        builder.HasNoKey().ToView("SchedulesOverView");
    }
}
