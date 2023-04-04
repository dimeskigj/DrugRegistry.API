using DrugRegistry.API.Endpoints.Interfaces;

namespace DrugRegistry.API.Extensions;

public static class ProgramExtensions
{
    private static readonly IEnumerable<IEndpoint> Endpoints = DiscoverEndpoints();

    public static IServiceCollection RegisterServices(this IServiceCollection collection)
    {
        foreach (var endpoint in Endpoints) endpoint.RegisterServices(collection);

        return collection;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        foreach (var endpoint in Endpoints) endpoint.MapEndpoints(app);
        return app;
    }

    private static IEnumerable<IEndpoint> DiscoverEndpoints()
    {
        return typeof(IEndpoint).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();
    }
}