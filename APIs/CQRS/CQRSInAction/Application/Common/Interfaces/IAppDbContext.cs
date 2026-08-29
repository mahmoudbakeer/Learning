using CQRSInAction.Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace CQRSInAction.Application.Common.Interfaces;


public interface IAppDbContext
{
    DbSet<Todo> Todos { get; } // here you have to install the package EntityFrameWorkCore on the application, and that a bit violation of the clean architecture 
    // since the application should know nothing about the ef core or care how to be implemented and using this package is converting the design to be more coupled with ef core.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}