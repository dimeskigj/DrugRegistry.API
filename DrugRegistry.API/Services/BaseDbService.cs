using DrugRegistry.API.Database;

namespace DrugRegistry.API.Services;

public class BaseDbService
{
    protected readonly AppDbContext AppDbContext;

    protected BaseDbService(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }
}