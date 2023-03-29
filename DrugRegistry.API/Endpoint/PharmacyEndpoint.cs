using DrugRegistry.API.Endpoint.Interfaces;
using DrugRegistry.API.Service;
using DrugRegistry.API.Service.Interfaces;

namespace DrugRegistry.API.Endpoint;

// ReSharper disable once UnusedType.Global
public class PharmacyEndpoint : IEndpoint
{
    public IServiceCollection RegisterServices(IServiceCollection collection)
    {
        collection.AddScoped<IPharmacyService, PharmacyDbService>();
        return collection;
    }

    public WebApplication MapEndpoints(WebApplication app) => app;
}