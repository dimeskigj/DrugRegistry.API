using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Service.Interfaces;

public interface IGeocodingService
{
    Task<Location?> GeocodePlace(string query, int millisecondsDelay = 0);
}