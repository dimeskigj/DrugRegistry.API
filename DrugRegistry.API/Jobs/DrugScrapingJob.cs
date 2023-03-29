using DrugRegistry.API.Scraping;
using DrugRegistry.API.Service.Interfaces;
using Quartz;

namespace DrugRegistry.API.Jobs;

public class DrugScrapingJob : IJob
{
    private const int MaxAttempts = 5;
    private readonly IDrugService _drugService;
    private readonly DrugScraper _drugScraper;
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
        {
            try
            {
                var counter = 0;
                var pageResults = await _drugScraper.ScrapePage(currentPage);
                foreach (var drug in pageResults.Where(d => d.Url is not null))
                {
                    if (await _drugService.GetDrugByUrl(drug.Url!) is not null) continue;
                    await _drugService.AddDrug(drug);
                    counter++;
                }

                retryCount = 0;
                Console.WriteLine($"Wrote {counter} entries from page {currentPage}");
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
                    _logger.LogError("Couldn't scrape page #{currentPage} after multiple attempts.\n{StackTrace}",
                        currentPage, e.StackTrace);
                }
            }
        }
    }
}