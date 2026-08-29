using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace CQRSInAction.Infrastructure.Data;


public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AppDbContext()
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}