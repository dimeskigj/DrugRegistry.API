using DrugRegistry.API.Models;
using DrugRegistry.API.Repository.Interfaces;

namespace DrugRegistry.API.Repository;

public class InMemoryDrugRepository : IDrugRepository
{
    private List<Drug> _drugList = new()
    {
        new Drug("decision number", "atc", "TESTDRUG1", "Test Drug 1", IssuingType.HospitalOnly, "Test Ingredients",
            "Bottle",
            "100mg", "Pills", "google.com", "google.com", "google.com", DateTime.Now, "Test Carroer",
            "Test Manufacturer", 100, 100),

        new Drug("decision number", "atc", "TESTDRUG2", "Test Drug 2", IssuingType.PrescriptionOnly,
            "Test Ingredients",
            "Bottle",
            "100mg", "Pills", "google.com", "google.com", "google.com", DateTime.Now, "Test Carroer",
            "Test Manufacturer", 100, 100),

        new Drug("decision number", "atc", "TESTDRUG3", "Test Drug 3", IssuingType.OverTheCounter,
            "Test Ingredients",
            "Bottle",
            "100mg", "Pills", "google.com", "google.com", "google.com", DateTime.Now, "Test Carroer",
            "Test Manufacturer", 100, 100),
    };

    public Task<List<Drug>> GetAllDrugs()
    {
        return new Task<List<Drug>>(() => _drugList);
    }

    public Task<Drug> GetDrugById(Guid id)
    {
        return new Task<Drug>(() => _drugList[new Random().Next(0, _drugList.Count)]);
    }
}