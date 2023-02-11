using DrugRegistry.API.Models;

namespace DrugRegistry.API.Repository.Interfaces;

public interface IDrugRepository
{
    Task<List<Drug>> GetAllDrugs();
    Task<Drug> GetDrugById(Guid id);
}