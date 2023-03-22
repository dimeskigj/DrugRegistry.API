using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Service.Interfaces;

public interface IDrugService
{
    Task<List<Drug>> GetAllDrugs();
    Task<Drug?> GetDrugById(Guid id);
    Task<Drug?> GetDrugByDecisionNumber(string decisionNumber);
    Task<Guid?> AddDrug(Drug drug);
    Task<PagedResult<Drug>> QueryDrugs(string query, int page, int size);
}