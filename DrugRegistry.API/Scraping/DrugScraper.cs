using DrugRegistry.API.Domain;
using DrugRegistry.API.Extensions;
using DrugRegistry.API.Utils;
using HtmlAgilityPack;

namespace DrugRegistry.API.Scraping;

public class DrugScraper : BaseScraper
{
    private readonly ILogger<DrugScraper> _logger;

    public DrugScraper(IHttpClientFactory httpClientFactory, ILogger<DrugScraper> logger) : base(httpClientFactory)
    {
        _logger = logger;
    }

    public Task<int> GetPageCount()
    {
        return base.GetPageCount($"{Constants.LekoviWebUrl}/drugsregister.grid.pager/1/grid_0?t:ac=overview");
    }

    public async Task<IEnumerable<Drug>> ScrapePage(int pageNumber = 1)
    {
        var uri = $"{Constants.LekoviWebUrl}/drugsregister.grid.pager/{pageNumber}/grid_0?t:ac=overview";
        var document = LoadHtmlDocument(await Client.RequestHtml(uri, HttpMethod.Post));

        var tableBody = document.DocumentNode
            .Descendants("table")
            .First(el => el.HasClass("table"))
            .Descendants("tbody")
            .First();

        var results = tableBody
            .Descendants("tr")
            .Select(ParseRow)
            .Select(ScrapeDetails)
            .Select(t => t.Result)
            .ToList();

        return results;
    }

    private static Drug ParseRow(HtmlNode row)
    {
        var latinName = row.Descendants().First(el => el.HasClass("latinName")).ExtractDeepText();
        var genericName = row.Descendants().First(el => el.HasClass("genericNameMultiple")).ExtractDeepText();
        var strength = row.Descendants().First(el => el.HasClass("strength")).ExtractDeepText();
        var packaging = row.Descendants().First(el => el.HasClass("drugPackage")).ExtractDeepText();
        var pharmaceuticalForm = row.Descendants().First(el => el.HasClass("pharmacyForm")).ExtractDeepText();
        var issuance = row.Descendants().First(el => el.HasClass("modeOfIssuance")).ExtractDeepText();
        var issuingType = ParseIssuingType(issuance);
        var manufacturer = row.Descendants().First(el => el.HasClass("manufacturersNames")).ExtractDeepText();
        var approvalCarrier = row.Descendants().First(el => el.HasClass("approvalCarrier")).ExtractDeepText();
        var solutionNumber = row.Descendants().First(el => el.HasClass("solutionNumber")).ExtractDeepText();
        var solutionDate = row.Descendants().First(el => el.HasClass("solutionDate")).ExtractDeepText();
        var validityDate = row.Descendants().First(el => el.HasClass("validityDate")).ExtractDeepText();
        var wholesalePrice = row.Descendants().First(el => el.HasClass("wholesalePrice")).ExtractDeepText();
        var retailPrice = row.Descendants().First(el => el.HasClass("retailPrice")).ExtractDeepText();
        var url = row.Descendants().First(el => el.HasClass("latinName")).Descendants("a").First()
            .GetAttributeValue("href", string.Empty);

        var result = new Drug
        {
            LatinName = latinName, GenericName = genericName, Strength = strength, Packaging = packaging,
            PharmaceuticalForm = pharmaceuticalForm, IssuingType = issuingType, Manufacturer = manufacturer,
            ApprovalCarrier = approvalCarrier, DecisionNumber = solutionNumber,
            PriceWithVat = double.Parse(retailPrice),
            PriceWithoutVat = double.Parse(wholesalePrice),
            Url = new Uri(Constants.LekoviWebUrl, url)
        };

        if (DateTime.TryParse(solutionDate, out var d1)) result.DecisionDate = d1;
        if (DateTime.TryParse(validityDate, out var d2)) result.ValidityDate = d2;
        result.LastUpdate = DateTime.UtcNow.Date;

        return result;
    }

    private async Task<Drug> ScrapeDetails(Drug drug)
    {
        try
        {
            var document = LoadHtmlDocument(await Client.RequestHtml(drug.Url!, HttpMethod.Post));
            
            var atc = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => el.ExtractDeepText().Trim() == "АТЦ")?
                .Descendants()?
                .Select(el => el.ExtractDeepText())
                .LastOrDefault()?
                .Trim();
            
            var ingredients = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => el.ExtractDeepText().Trim() == "Состав")?
                .Descendants()?
                .Select(el => el.ExtractDeepText())
                .LastOrDefault()?
                .Trim();
            
            var manualUrl = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => el.ExtractDeepText().Trim() == "Упатство за употреба:", null)?
                .Descendants("div")?
                .LastOrDefault()?
                .Descendants("a")?
                .FirstOrDefault()?
                .GetAttributeValue("href", null);
            
            var reportUrl = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => el.ExtractDeepText().Trim() == "Збирен извештај:", null)?
                .Descendants("div")?
                .LastOrDefault()?
                .Descendants("a")?
                .FirstOrDefault()?
                .GetAttributeValue("href", null);

            drug.Atc = atc;
            drug.Ingredients = ingredients;
            if (manualUrl is not null) drug.ManualUrl = new Uri(Constants.LekoviWebUrl, manualUrl);
            if (reportUrl is not null) drug.ReportUrl = new Uri(Constants.LekoviWebUrl, reportUrl);

            return drug;
        }
        catch (Exception e)
        {
            _logger.LogError("Couldn't Scrape details for {Url}.\n{StackTrace}", drug.Url, e.StackTrace);
            throw;
        }
    }

    private static IssuingType ParseIssuingType(string type)
    {
        return type switch
        {
            "Rp" => IssuingType.PrescriptionOnly,
            "H" => IssuingType.HospitalOnly,
            _ => IssuingType.OverTheCounter
        };
    }
}