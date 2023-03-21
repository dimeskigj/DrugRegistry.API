using DrugRegistry.API.Database;
using DrugRegistry.API.Extensions;using DrugRegistry.API.Scraping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("db");

// Add services to the container.

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContextFactory<AppDbContext>(
        options => options.UseNpgsql(dbConnectionString)
    )
    .RegisterServices();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

new DrugScraper().GetPageCount();

app.UseHttpsRedirection();

app.MapEndpoints().Run();