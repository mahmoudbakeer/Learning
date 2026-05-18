using EF_Interceptors.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EF_Interceptors.Data.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> interceptionResult
        )
        {
            // here we see if the entity is Book then Implement SoftDelete on it
            if (eventData.Context is null)
                return interceptionResult;
            foreach (var item in eventData.Context.ChangeTracker.Entries())
            {
                if (item is not { State: EntityState.Deleted, Entity: ISoftDelete entity })
                    continue;
                item.State = EntityState.Modified;
                entity.Delete();
            }
            return interceptionResult;
        }
    }
}
