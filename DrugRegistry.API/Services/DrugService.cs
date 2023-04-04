using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Services.Interfaces;
using FuzzySharp;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using Microsoft.EntityFrameworkCore;

namespace DrugRegistry.API.Services;

public class DrugService : BaseDbService, IDrugService
{
    public DrugService(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Drug>> GetAllDrugs()
    {
        return await AppDbContext.Drugs.ToListAsync();
    }

    public async Task<Drug?> GetDrugById(Guid id)
    {
        return await AppDbContext.Drugs.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Drug?> GetDrugByDecisionNumberAndAtc(string decisionNumber, string atc)
    {
        return await AppDbContext.Drugs.FirstOrDefaultAsync(d => d.DecisionNumber == decisionNumber && d.Atc == atc);
    }

    public async Task<Drug?> GetDrugByUrl(Uri uri)
    {
        return await AppDbContext.Drugs.FirstOrDefaultAsync(d => d.Url == uri);
    }

    public async Task<Guid?> AddDrug(Drug drug)
    {
        var res = await AppDbContext.AddAsync(drug);
        await AppDbContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<PagedResult<Drug>> QueryDrugs(string query, int page, int size)
    {
        // TODO: Refactor this to not load the entire DB on the server if performance's an issue
        var filtered = (await AppDbContext.Drugs.ToListAsync())
            .Select(d => new
            {
                Drug = d,
                Process.ExtractOne(query,
                        new[]
                        {
                            d.GenericName ?? "",
                            d.LatinName ?? "",
                            d.Atc ?? "",
                            d.Ingredients ?? ""
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
            .Take(size);

        return new PagedResult<Drug>(results, total, page, size);
    }
}