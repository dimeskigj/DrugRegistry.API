using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrugRegistry.API.Service;

public class DrugService : IDrugService
{
    private readonly AppDbContext _appDbContext;

    public DrugService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Drug>> GetAllDrugs() => await _appDbContext.Drugs.ToListAsync();
    public async Task<Drug?> GetDrugById(Guid id) => await Task.FromResult<Drug?>(null);

    public async Task<Guid?> AddDrug(Drug drug)
    {
        var res = await _appDbContext.AddAsync(drug);
        await _appDbContext.SaveChangesAsync();
        return res.Entity.Id;
    }
}