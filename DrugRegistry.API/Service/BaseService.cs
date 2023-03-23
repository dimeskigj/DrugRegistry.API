using DrugRegistry.API.Database;

namespace DrugRegistry.API.Service;

public class BaseService
{
    protected readonly AppDbContext AppDbContext;

    public BaseService(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }
}