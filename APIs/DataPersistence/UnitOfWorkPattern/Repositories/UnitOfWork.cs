using RestfulApi.Data;
using RestfulApi.Repositories.Interfaces;

namespace RestfulApi.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IProductRepository _ProductRepository;
    public IProductRepository Products => _ProductRepository ?? new ProductRepository(context);

    public void Dispose()
    {
        context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}
