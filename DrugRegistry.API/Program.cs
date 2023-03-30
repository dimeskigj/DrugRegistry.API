using DrugRegistry.API.Database;
using DrugRegistry.API.Extensions;
using DrugRegistry.API.Jobs;
using DrugRegistry.API.Scraping;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("db");

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
    .AddScoped<DrugScraper>();

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