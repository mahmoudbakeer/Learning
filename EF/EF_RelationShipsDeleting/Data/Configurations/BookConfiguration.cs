using EF_RelationShipsDeleting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_RelationShipsDeleting.Data
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");
            builder.HasKey(book => book.Id);
            builder.Property(book => book.Id).ValueGeneratedNever();
            builder
                .Property(book => book.BookName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(book => book.Price).HasColumnType("DECIMAL").IsRequired();
            // the relationship
            builder
                .HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId)
                .IsRequired();
            // remove it to return to cascade
        }
    }
}
