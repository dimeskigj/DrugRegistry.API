using DrugRegistry.API.Domain;
using DrugRegistry.API.Endpoints.Interfaces;
using DrugRegistry.API.Services;
using DrugRegistry.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrugRegistry.API.Endpoints;

// ReSharper disable once UnusedType.Global
public class DrugEndpoint : IEndpoint
{
    public IServiceCollection RegisterServices(IServiceCollection collection)
    {
        collection.AddScoped<IDrugService, DrugService>();
        return collection;
    }

    public WebApplication MapEndpoints(WebApplication app)
    {
        app.MapGet("/api/drugs/search", async (
                    IDrugService drugService,
                    [FromQuery] string query,
                    [FromQuery] int? page,
                    [FromQuery] int? size) =>
                Results.Ok(await drugService.QueryDrugs(query, page ?? 0, size ?? 10)))
            .Produces<PagedResult<Drug>>()
            .WithName("Search drugs")
            .WithTags("Drugs");

        return app;
    }
}