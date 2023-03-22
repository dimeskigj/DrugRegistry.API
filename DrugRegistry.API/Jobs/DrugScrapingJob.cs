using DrugRegistry.API.Scraping;
using DrugRegistry.API.Service.Interfaces;
using Quartz;

namespace DrugRegistry.API.Jobs;

public class DrugScrapingJob : IJob
{
    private readonly IDrugService _drugService;
    private readonly DrugScraper _drugScraper;

    public DrugScrapingJob(IDrugService drugService, DrugScraper drugScraper)
    {
        _drugService = drugService;
        _drugScraper = drugScraper;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var pageCount = await _drugScraper.GetPageCount();
        for (var currentPage = 1; currentPage <= pageCount; currentPage++)
        {
            try
            {
                var pageResults = await _drugScraper.ScrapePage(currentPage);
                foreach (var drug in pageResults)
                {
                    if (await _drugService.GetDrugByDecisionNumber(drug.DecisionNumber ?? string.Empty) is null)
                    {
                        await _drugService.AddDrug(drug);
                    }
                }
            }
            catch
            {
                // retry in case of failure, the website is unstable we might need to try multiple times
                currentPage--;
                // await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}