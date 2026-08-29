using CQRSInAction.Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRSInAction.Data.Configurations;


public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("Todos");


        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
        .IsRequired(true);

    }
}