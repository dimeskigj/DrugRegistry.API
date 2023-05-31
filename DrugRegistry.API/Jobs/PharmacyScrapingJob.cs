using DrugRegistry.API.Scraping;
using DrugRegistry.API.Services.Interfaces;
using Quartz;

namespace DrugRegistry.API.Jobs;

// ReSharper disable once ClassNeverInstantiated.Global
public class PharmacyScrapingJob : IJob
{
    private const int MaxAttempts = 5;
    private readonly ILogger<PharmacyScrapingJob> _logger;
    private readonly PharmacyScraper _pharmacyScraper;
    private readonly IPharmacyService _pharmacyService;

    public PharmacyScrapingJob(IPharmacyService pharmacyService, PharmacyScraper pharmacyScraper,
        ILogger<PharmacyScrapingJob> logger)
    {
        _pharmacyService = pharmacyService;
        _pharmacyScraper = pharmacyScraper;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var pageCount = await _pharmacyScraper.GetPageCount();
        var retryCount = 0;
        for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            try
            {
                var createCounter = 0;
                var updateCounter = 0;
                var pageResults = await _pharmacyScraper.ScrapePage(currentPage);
                foreach (var pharmacy in pageResults.Where(p => p.Name is not null && p.Address is not null))
                {
                    var pharmacyWithSameNameAndAddress =
                        await _pharmacyService.GetPharmacyByNameAndAddress(pharmacy.Name!, pharmacy.Address!);
                    if (pharmacyWithSameNameAndAddress is null)
                    {
                        await _pharmacyService.AddPharmacy(pharmacy);
                        createCounter++;
                    }
                    else
                    {
                        pharmacy.Id = pharmacyWithSameNameAndAddress.Id;
                        await _pharmacyService.UpdatePharmacy(pharmacy);
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
                    _logger.LogError("Couldn't scrape pharamcy page #{currentPage} after multiple attempts.\n{StackTrace}",
                        currentPage, e.StackTrace);
                }
            }
    }
}