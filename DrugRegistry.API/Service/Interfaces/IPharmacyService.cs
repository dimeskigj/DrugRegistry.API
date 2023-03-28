using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Service.Interfaces;

public interface IPharmacyService
{
    Task<List<Pharmacy>> GetAllPharmacies();
    Task<Pharmacy?> GetPharmacyById(Guid id);
    Task<Pharmacy?> GetPharmacyByIdNumber(string idNumber);
    Task<Guid?> AddPharmacy(Pharmacy pharmacy);
    Task<PagedResult<Pharmacy>> GetDrugsByDistance(Location location, int page, int size);
    Task<IEnumerable<string>> GetMunicipalities();
}