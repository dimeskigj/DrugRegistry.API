using DrugRegistry.API.Scraping;
using DrugRegistry.API.Services.Interfaces;
using Quartz;

namespace DrugRegistry.API.Jobs;

// ReSharper disable once ClassNeverInstantiated.Global
public class DrugScrapingJob : IJob
{
    private const int MaxAttempts = 5;
    private readonly DrugScraper _drugScraper;
    private readonly IDrugService _drugService;
    private readonly ILogger<DrugScrapingJob> _logger;

    public DrugScrapingJob(IDrugService drugService, DrugScraper drugScraper, ILogger<DrugScrapingJob> logger)
    {
        _drugService = drugService;
        _drugScraper = drugScraper;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var pageCount = await _drugScraper.GetPageCount();
        var retryCount = 0;
        for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            try
            {
                var createCounter = 0;
                var updateCounter = 0;
                var pageResults = await _drugScraper.ScrapePage(currentPage);
                foreach (var drug in pageResults.Where(d => d.Url is not null))
                {
                    var existingDrug = await _drugService.GetDrugByUrl(drug.Url!);
                    if (existingDrug is null)
                    {
                        await _drugService.AddDrug(drug);
                        createCounter++;
                    }
                    else
                    {
                        drug.Id = existingDrug.Id;
                        await _drugService.UpdateDrug(drug);
                        updateCounter++;
                    }
                }

                retryCount = 0;
                Console.WriteLine($"Wrote {createCounter} drug entries from page {currentPage}");
                Console.WriteLine($"Updated {updateCounter} drug entries from page {currentPage}");
            }
            catch (Exception e)
            {
                // retry in case of failure, the website is unstable we might need to try multiple times
                if (retryCount++ < MaxAttempts)
                {
                    currentPage--;
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                else
                {
                    _logger.LogError("Couldn't scrape drug page #{currentPage} after multiple attempts.\n{StackTrace}",
                        currentPage, e.StackTrace);
                }
            }
    }
}