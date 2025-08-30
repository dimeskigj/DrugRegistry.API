using DrugRegistry.API.Scraping;
using DrugRegistry.API.Services.Interfaces;
using Quartz;

namespace DrugRegistry.API.Jobs;

// ReSharper disable once ClassNeverInstantiated.Global
public class PharmacyScrapingJob(
    IPharmacyService pharmacyService,
    PharmacyScraper pharmacyScraper,
    ILogger<PharmacyScrapingJob> logger)
    : IJob
{
    private const int MaxAttempts = 5;

    public async Task Execute(IJobExecutionContext context)
    {
        var pageCount = await pharmacyScraper.GetPageCount();
        var retryCount = 0;
        for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            try
            {
                var createCounter = 0;
                var updateCounter = 0;
                var pageResults = await pharmacyScraper.ScrapePage(currentPage);
                foreach (var pharmacy in pageResults.Where(p => p.Name is not null && p.Address is not null))
                {
                    var pharmacyWithSameNameAndAddress =
                        await pharmacyService.GetPharmacyByNameAndAddress(pharmacy.Name!, pharmacy.Address!);
                    if (pharmacyWithSameNameAndAddress is null)
                    {
                        await pharmacyService.AddPharmacy(pharmacy);
                        createCounter++;
                    }
                    else
                    {
                        await pharmacyService.UpdatePharmacy(pharmacy, pharmacyWithSameNameAndAddress.Id);
                        updateCounter++;
                    }
                }

                retryCount = 0;
                Console.WriteLine($"Wrote {createCounter} pharmacy entries from page {currentPage}");
                Console.WriteLine($"Updated {updateCounter} pharmacy entries from page {currentPage}");
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
                    logger.LogError(
                        "Couldn't scrape pharamcy page #{currentPage} after multiple attempts.\n{StackTrace}",
                        currentPage, e.StackTrace);
                }
            }
    }
}