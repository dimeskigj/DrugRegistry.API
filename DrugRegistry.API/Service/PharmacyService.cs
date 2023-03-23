using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Service.Interfaces;
using DrugRegistry.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrugRegistry.API.Service;

public class PharmacyService : BaseService, IPharmacyService
{
    public PharmacyService(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Pharmacy>> GetAllPharmacies() => await AppDbContext.Pharmacies.ToListAsync();

    public async Task<Pharmacy?> GetPharmacyById(Guid id) =>
        await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Pharmacy?> GetPharmacyByIdNumber(string idNumber) =>
        await AppDbContext.Pharmacies.FirstOrDefaultAsync(p => p.IdNumber == idNumber);


    public async Task<Guid?> AddPharmacy(Pharmacy pharmacy)
    {
        var res = await AppDbContext.AddAsync(pharmacy);
        await AppDbContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<PagedResult<Pharmacy>> GetDrugsByDistance(Location location, int page, int size)
    {
        var pharmacies = await GetAllPharmacies();
        var results = pharmacies.OrderBy(p =>
                p.Location is not null
                    ? GeoUtils.GetDistanceBetweenLocations(p.Location, location)
                    : double.MaxValue)
            .Skip(page * size)
            .Take(size);

        var total = await AppDbContext.Pharmacies.CountAsync();

        return new PagedResult<Pharmacy>(results, total, page, size);
    }
}