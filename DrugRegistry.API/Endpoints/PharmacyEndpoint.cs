using DrugRegistry.API.Domain;
using DrugRegistry.API.Endpoints.Interfaces;
using DrugRegistry.API.Services;
using DrugRegistry.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrugRegistry.API.Endpoints;

// ReSharper disable once UnusedType.Global
public class PharmacyEndpoint : IEndpoint
{
    public IServiceCollection RegisterServices(IServiceCollection collection)
    {
        collection.AddScoped<IPharmacyService, PharmacyService>();
        return collection;
    }

    public WebApplication MapEndpoints(WebApplication app)
    {
        app.MapGet("/pharmacies/byLocation", async (
                    IPharmacyService pharmacyService,
                    [FromQuery] double lon,
                    [FromQuery] double lat,
                    [FromQuery] int? page,
                    [FromQuery] int? size,
                    [FromQuery] string? municipality,
                    [FromQuery] string? place) =>
                Results.Ok(await pharmacyService.GetPharmaciesByDistance(
                    new Location { Longitude = lon, Latitude = lat },
                    page ?? 0, size ?? 10,
                    municipality, place)))
            .Produces<PagedResult<Drug>>()
            .WithName("Query pharmacies by location")
            .WithTags("Pharmacies");

        app.MapGet("/pharmacies/search", async (
                    IPharmacyService pharmacyService,
                    [FromQuery] string query,
                    [FromQuery] int? page,
                    [FromQuery] int? size,
                    [FromQuery] string? municipality,
                    [FromQuery] string? place) =>
                Results.Ok(await pharmacyService.GetPharmaciesByQuery(query,
                    page ?? 0, size ?? 10,
                    municipality, place)))
            .Produces<PagedResult<Drug>>()
            .WithName("Query pharmacies by name and address")
            .WithTags("Pharmacies");

        app.MapGet("/pharmacies/municipalitiesByFrequency", async (
                IPharmacyService pharmacyService) => Results.Ok(
                await pharmacyService.GetMunicipalitiesOrderedByFrequency()
            ))
            .Produces<PagedResult<Drug>>()
            .WithName("Query places by frequency")
            .WithTags("Pharmacies");

        app.MapGet("/pharmacies/placesByFrequency", async (
                    IPharmacyService pharmacyService,
                    [FromQuery] string municipality) =>
                Results.Ok(
                    await pharmacyService.GetPlacesOrderedByFrequencyForMunicipality(municipality)
                ))
            .Produces<PagedResult<Drug>>()
            .WithName("Query municipalities by frequency")
            .WithTags("Pharmacies");

        return app;
    }
}