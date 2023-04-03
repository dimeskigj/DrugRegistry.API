using DrugRegistry.API.Database;
using DrugRegistry.API.Extensions;
using DrugRegistry.API.Jobs;
using DrugRegistry.API.Scraping;
using DrugRegistry.API.Service;
using DrugRegistry.API.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var dbConnectionString =
    environment == Environments.Development
        ? builder.Configuration.GetConnectionString("db")
        : Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

Console.WriteLine($"dbConnectionString = {dbConnectionString}");

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContextFactory<AppDbContext>(
        options => options.UseNpgsql(dbConnectionString)
    )
    .RegisterServices()
    .AddHttpClient()
    .AddQuartz(q => q.UseMicrosoftDependencyInjectionJobFactory())
    .AddQuartzHostedService(opt => opt.WaitForJobsToComplete = false)
    .AddScoped<IGeocodingService, GeocodingService>()
    .AddScoped<DrugScraper>()
    .AddScoped<PharmacyScraper>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();
await scheduler.ScheduleJobs(Jobs.JobsDictionary, true);

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints().Run();