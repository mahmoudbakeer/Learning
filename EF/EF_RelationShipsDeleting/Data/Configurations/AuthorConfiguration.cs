using EF_RelationShipsDeleting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_RelationShipsDeleting.Data
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Author");
            builder.HasKey(author => author.Id);
            builder.Property(author => author.Id).ValueGeneratedNever();
            builder
                .Property(author => author.AuthorName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
