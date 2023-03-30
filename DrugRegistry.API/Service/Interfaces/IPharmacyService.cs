﻿using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Service.Interfaces;

public interface IPharmacyService
{
    Task<List<Pharmacy>> GetAllPharmacies();
    Task<Pharmacy?> GetPharmacyById(Guid id);
    Task<Pharmacy?> GetPharmacyByIdNumber(string idNumber);
    Task<Pharmacy?> GetPharmacyByUrl(Uri uri);
    Task<Guid?> AddPharmacy(Pharmacy pharmacy);

    Task<PagedResult<Pharmacy>> GetDrugsByDistance(Location location, int page, int size, string? municipality,
        string? place);

    Task<IEnumerable<string>> GetMunicipalities();
}