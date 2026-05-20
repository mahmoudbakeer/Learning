using EF_Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_Transaction.Data.Configurations
{
    public class GLTransactionConfiguration : IEntityTypeConfiguration<GLTransaction>
    {
        public void Configure(
            Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<GLTransaction> builder
        )
        {
            builder.ToTable("GLTransactions");

            builder.HasKey(tran => tran.Id);

            builder
                .Property(trans => trans.Notes)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(trans => trans.CreatedAt).HasColumnType("DATETIME").IsRequired();
            builder.Property(trans => trans.Amount).HasColumnType("DECIMAL(18,2)").IsRequired();

            builder
                .HasOne(trans => trans.Account)
                .WithMany(account => account.Transactions)
                .HasForeignKey(trans => trans.AccountId)
                .IsRequired();
        }
    }
}
