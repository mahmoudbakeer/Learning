using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Interceptors.Data.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");
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
