using EF_RelationShipsDeleting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_RelationShipsDeleting.Data
{
    public class AuthorV2Configuration : IEntityTypeConfiguration<AuthorV2>
    {
        public void Configure(EntityTypeBuilder<AuthorV2> builder)
        {
            builder.ToTable("AuthorV2");
            builder.HasKey(author => author.Id);
            builder.Property(author => author.Id).ValueGeneratedNever();
            builder
                .Property(author => author.AuthorName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired(false);
        }
    }
}
