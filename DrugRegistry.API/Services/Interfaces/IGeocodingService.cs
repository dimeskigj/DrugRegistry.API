using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Services.Interfaces;

public interface IGeocodingService
{
    Task<Location?> GeocodePlace(string query, int millisecondsDelay = 0);
}