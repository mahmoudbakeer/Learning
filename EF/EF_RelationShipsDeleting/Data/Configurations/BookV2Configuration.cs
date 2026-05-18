using EF_RelationShipsDeleting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_RelationShipsDeleting.Data
{
    public class BookV2Configuration : IEntityTypeConfiguration<BookV2>
    {
        public void Configure(EntityTypeBuilder<BookV2> builder)
        {
            builder.ToTable("BookV2");
            builder.HasKey(book => book.Id);
            builder.Property(book => book.Id).ValueGeneratedNever();
            builder
                .Property(book => book.BookName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            // the relationship
            builder
                .HasOne(book => book.AuthorV2)
                .WithMany(author => author.BookV2s)
                .HasForeignKey(book => book.AuthorV2Id)
                .IsRequired(false);
        }
    }
}
