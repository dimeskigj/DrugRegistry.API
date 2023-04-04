using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Services.Interfaces;
using DrugRegistry.API.Utils;
using FuzzySharp;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using Microsoft.EntityFrameworkCore;

namespace DrugRegistry.API.Services;

public class PharmacyService : BaseDbService, IPharmacyService
{
    public PharmacyService(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Pharmacy>> GetAllPharmacies()
    {
        return await AppDbContext.Pharmacies.ToListAsync();
    }

    public async Task<Pharmacy?> GetPharmacyById(Guid id)
    {
        return await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pharmacy?> GetPharmacyByIdNumber(string idNumber)
    {
        return await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.IdNumber == idNumber);
    }

    public async Task<Pharmacy?> GetPharmacyByUrl(Uri uri)
    {
        return await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.Url == uri);
    }


    public async Task<Guid?> AddPharmacy(Pharmacy pharmacy)
    {
        var res = await AppDbContext.AddAsync(pharmacy);
        await AppDbContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<PagedResult<Pharmacy>> GetPharmaciesByDistance(Location location, int page, int size,
        string? municipality, string? place)
    {
        var pharmacies = await AppDbContext.Pharmacies
            .Include(p => p.Location)
            .Where(p => municipality == null || municipality == p.Municipality)
            .Where(p => place == null || place == p.Place)
            .ToListAsync();
        var results = pharmacies.OrderBy(p =>
                p.Location is not null
                    ? GeoUtils.GetDistanceBetweenLocations(p.Location, location)
                    : double.MaxValue)
            .Skip(page * size)
            .Take(size);

        var total = await AppDbContext.Pharmacies.CountAsync();

        return new PagedResult<Pharmacy>(results, total, page, size);
    }

    public async Task<PagedResult<Pharmacy>> GetPharmaciesByQuery(string query, int page, int size,
        string? municipality, string? place)
    {
        var pharmacies = await AppDbContext.Pharmacies
            .Include(p => p.Location)
            .Where(p => municipality == null || municipality == p.Municipality)
            .Where(p => place == null || place == p.Place)
            .ToListAsync();
        
        var results = pharmacies
            .Select(p => new
            {
                Pharmacy = p,
                Process.ExtractOne(query,
                        new[]
                        {
                            p.Name ?? string.Empty,
                            p.Address ?? string.Empty
                        },
                        s => s,
                        ScorerCache.Get<TokenSetScorer>())
                    .Score
            })
            .Where(d => d.Score > 75)
            .OrderByDescending(d => d.Score)
            .Select(d => d.Pharmacy)
            .ToList();

        var total = results.Count;

        results = results
            .Skip(page * size)
            .Take(size)
            .ToList();

        return new PagedResult<Pharmacy>(results, total, page, size);
    }

    public async Task<IEnumerable<string>> GetMunicipalitiesOrderedByFrequency()
    {
        return (await AppDbContext.Pharmacies
            .GroupBy(p => p.Municipality)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Where(m => m != null)
            .ToListAsync())!;
    }

    public async Task<IEnumerable<string>> GetPlacesOrderedByFrequencyForMunicipality(string municipality)
    {
        return (await AppDbContext.Pharmacies
            .Where(p => p.Municipality == municipality)
            .GroupBy(p => p.Place)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Where(m => m != null)
            .ToListAsync())!;
    }
}