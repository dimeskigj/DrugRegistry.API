using DrugRegistry.API.Domain;
using DrugRegistry.API.Extensions;

namespace DrugRegistry.API.Scraping;

public class DrugScraper : BaseScraper
{
    public async Task<int> GetPageCount()
    {
        const string uri = $"{Constants.LekoviWebUrl}/drugsregister.grid.pager/1/grid_0?t:ac=overview";
        var document = LoadHtmlDocument(await Client.RequestHtml(uri, HttpMethod.Post));
        var count = document.DocumentNode
            .Descendants("a")
            .Where(el => el.Id.StartsWith("pager_") && int.TryParse(el.InnerText, out _))
            .Select(el => int.Parse(el.InnerText)).MaxBy(number => number);
        return count;
    }

    public async Task<IEnumerable<Drug>> ScrapePage(int pageNumber = 1)
    {
        var uri = $"{Constants.LekoviWebUrl}/drugsregister.grid.pager/{pageNumber}/grid_0?t:ac=overview";
        var document = LoadHtmlDocument(await Client.RequestHtml(uri, HttpMethod.Post));
        
        // TODO: Finish parsing
        return new List<Drug>();
    }
}