using DrugRegistry.API.Scraping;
using DrugRegistry.API.Services.Interfaces;
using Quartz;

namespace DrugRegistry.API.Jobs;

// ReSharper disable once ClassNeverInstantiated.Global
public class DrugScrapingJob(IDrugService drugService, DrugScraper drugScraper, ILogger<DrugScrapingJob> logger)
    : IJob
{
    private const int MaxAttempts = 5;

    public async Task Execute(IJobExecutionContext context)
    {
        var pageCount = await drugScraper.GetPageCount();
        var retryCount = 0;
        for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            try
            {
                var createCounter = 0;
                var updateCounter = 0;
                var pageResults = await drugScraper.ScrapePage(currentPage);
                foreach (var drug in pageResults.Where(d => d.Url is not null))
                {
                    var existingDrug = await drugService.GetDrugByUrl(drug.Url!);
                    if (existingDrug is null)
                    {
                        await drugService.AddDrug(drug);
                        createCounter++;
                    }
                    else
                    {
                        await drugService.UpdateDrug(drug, existingDrug.Id);
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
                    logger.LogError("Couldn't scrape drug page #{currentPage} after multiple attempts.\n{StackTrace}",
                        currentPage, e.StackTrace);
                }
            }
    }
}