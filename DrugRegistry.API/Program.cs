using DrugRegistry.API.Database;
using DrugRegistry.API.Extensions;
using DrugRegistry.API.Jobs;
using DrugRegistry.API.Scraping;
using DrugRegistry.API.Services;
using DrugRegistry.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("db");

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

app.MapEndpoints().Run();