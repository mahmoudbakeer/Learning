using EF_Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_Transaction.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(
            Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Account> builder
        )
        {
            builder.ToTable("Accounts");

            builder.HasKey(account => account.Id);
            builder.Property(account => account.Id).ValueGeneratedNever();

            builder
                .Property(account => account.ClientName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(account => account.Balance)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();
        }
    }
}
