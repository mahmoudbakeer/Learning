using DataProtection.Data.Configurations;
using DataProtection.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataProtection.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options),
IDataProtectionKeyContext
{
    public DbSet<Bid> Bids => Set<Bid>();

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BidConfiguration).Assembly);
    }
}