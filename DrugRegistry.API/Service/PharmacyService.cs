﻿using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Service.Interfaces;
using DrugRegistry.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrugRegistry.API.Service;

public class PharmacyDbService : BaseDbService, IPharmacyService
{
    public PharmacyDbService(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Pharmacy>> GetAllPharmacies() => await AppDbContext.Pharmacies.ToListAsync();

    public async Task<Pharmacy?> GetPharmacyById(Guid id) =>
        await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Pharmacy?> GetPharmacyByIdNumber(string idNumber) =>
        await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.IdNumber == idNumber);

    public async Task<Pharmacy?> GetPharmacyByUrl(Uri uri) =>
        await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.Url == uri);


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

    public async Task<IEnumerable<string>> GetMunicipalitiesOrderedByFrequency() =>
        (await AppDbContext.Pharmacies
            .GroupBy(p => p.Municipality)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Where(m => m != null)
            .ToListAsync())!;

    public async Task<IEnumerable<string>> GetPlacesOrderedByFrequencyForMunicipality(string municipality) =>
        (await AppDbContext.Pharmacies
            .Where(p => p.Municipality == municipality)
            .GroupBy(p => p.Place)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Where(m => m != null)
            .ToListAsync())!;
}