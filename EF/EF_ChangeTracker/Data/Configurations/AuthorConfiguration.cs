using EF_ChangeTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_ChangeTracker.Configurations
{
    class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Author");
            builder.HasKey(author => author.Id);
            builder.Property(author => author.Id).ValueGeneratedNever().IsRequired();
            builder
                .Property(author => author.AuthorName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasData(LoadAuthors());
        }

        private static IEnumerable<Author> LoadAuthors()
        {
            return new List<Author>()
            {
                new Author { Id = 1, AuthorName = "Hadi" },
                new Author { Id = 2, AuthorName = "Mahmoud" },
            };
        }
    }
}
