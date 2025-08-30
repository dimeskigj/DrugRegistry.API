using DrugRegistry.API.Domain;
using DrugRegistry.API.Services.Interfaces;

namespace DrugRegistry.API.Services;

public class EmptyGeocodingService : IGeocodingService
{
    public Task<Location?> GeocodePlace(string query, int millisecondsDelay = 0) => Task.FromResult<Location?>(null);
}