using EF_Interceptors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Interceptors.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(book => book.Id);
            builder.Property(book => book.Id).ValueGeneratedNever();
            builder
                .Property(book => book.BookName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();
            builder
                .HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId);
        }
    }
}
