﻿using DrugRegistry.API.Domain;
using DrugRegistry.API.Endpoint.Interfaces;
using DrugRegistry.API.Service;
using DrugRegistry.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrugRegistry.API.Endpoint;

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
        app.MapGet("/drugs", async (IDrugService drugService) => await drugService.GetAllDrugs())
            .Produces<List<Drug>>()
            .WithName("Get all drugs")
            .WithTags("Drugs");

        app.MapGet("/test-add", async (IDrugService drugService) => await drugService.AddDrug(
                new Drug
                {
                    LatinName = "TestDrug" + Guid.NewGuid(),
                    DecisionDate = DateTime.Now.ToUniversalTime(),
                    LastUpdate = DateTime.Now.ToUniversalTime()
                }))
            .Produces<Guid>()
            .WithName("Add a template drug")
            .WithTags("Drugs");

        app.MapGet("/drugs/search", async (
                    IDrugService drugService,
                    [FromQuery] string? query,
                    [FromQuery] int? page,
                    [FromQuery] int? size) =>
                Results.Ok(await drugService.QueryDrugs(query ?? "", page ?? 0, size ?? 10)))
            .Produces<PagedResult<Drug>>()
            .WithName("Search drugs")
            .WithTags("Drugs");

        return app;
    }
}