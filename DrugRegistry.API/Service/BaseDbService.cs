using DrugRegistry.API.Database;

namespace DrugRegistry.API.Service;

public class BaseDbService
{
    protected readonly AppDbContext AppDbContext;

    protected BaseDbService(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }
}