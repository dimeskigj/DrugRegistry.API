using DrugRegistry.API.Database;
using DrugRegistry.API.Domain;
using DrugRegistry.API.Service;
using DrugRegistry.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("db");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<AppDbContext>(
    options => options.UseNpgsql(dbConnectionString)
);
builder.Services.AddScoped<IDrugService, DrugService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.Run();