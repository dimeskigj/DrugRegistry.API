using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Service.Interfaces;
using FuzzySharp;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
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

    public async Task<PagedResult<Drug>> QueryDrugs(string query, int page, int size)
    {
        var filtered = (await _appDbContext.Drugs.ToListAsync())
            .Select(d => new
            {
                Drug = d,
                Process.ExtractOne(query,
                        new[]
                        {
                            d.GenericName ?? "",
                            d.LatinName ?? ""
                        },
                        s => s,
                        ScorerCache.Get<PartialRatioScorer>())
                    .Score
            })
            .Where(d => d.Score > 75)
            .OrderByDescending(d => d.Score)
            .Select(d => d.Drug)
            .ToList();
        
        var total = filtered.Count;
        
        var results = filtered
            .Skip(page * size)
            .Take(size)
            .ToList();

        return new PagedResult<Drug>(results, total, page, size);
    }
}