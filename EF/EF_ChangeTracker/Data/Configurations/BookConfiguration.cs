using EF_ChangeTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_ChangeTracker.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.HasKey(book => book.Id);
            builder.Property(book => book.Id).ValueGeneratedNever().IsRequired();
            builder
                .Property(book => book.BookName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(LoadBooks());
        }

        private static IEnumerable<Book> LoadBooks()
        {
            return new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    AuthorId = 1,
                    BookName = "Domain Driven Design Book for Dummies",
                },
                new Book
                {
                    Id = 2,
                    AuthorId = 1,
                    BookName = "Hands of practice for system design",
                },
                new Book
                {
                    Id = 3,
                    AuthorId = 2,
                    BookName = "Sql Server All what you need",
                },
                new Book
                {
                    Id = 4,
                    AuthorId = 2,
                    BookName = "Transactional Queries For Dummies",
                },
            };
        }
    }
}
