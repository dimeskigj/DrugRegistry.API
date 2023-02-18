using DrugRegistry.API.Domain;
using DrugRegistry.API.Endpoint.Interfaces;
using DrugRegistry.API.Service;
using DrugRegistry.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrugRegistry.API.Endpoint;

public class DrugEndpoint : IEndpoint
{
    public IServiceCollection RegisterServices(IServiceCollection collection)
    {
        collection.AddScoped<IDrugService, DrugService>();
        return collection;
    }

    public WebApplication MapEndpoints(WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
        app.MapGet("/drugs", async ([FromServices] IDrugService drugService) => await drugService.GetAllDrugs())
            .Produces<List<Drug>>()
            .WithName("Get all drugs")
            .WithTags("Drugs");
        app.MapGet("/test-add", async ([FromServices] IDrugService drugService) => await drugService.AddDrug(
                new Drug
                {
                    LatinName = "TestDrug" + Guid.NewGuid(),
                    DecisionDate = DateTime.Now.ToUniversalTime(),
                    LastUpdate = DateTime.Now.ToUniversalTime()
                }))
            .Produces<Guid>()
            .WithName("Add a template drug")
            .WithTags("Drugs");
        return app;
    }
}