﻿using DrugRegistry.API.Domain;
using DrugRegistry.API.Extensions;
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
        var latinName = ExtractDeepText(row.Descendants().First(el => el.HasClass("latinName")));
        var genericName = ExtractDeepText(row.Descendants().First(el => el.HasClass("genericNameMultiple")));
        var strength = ExtractDeepText(row.Descendants().First(el => el.HasClass("strength")));
        var packaging = ExtractDeepText(row.Descendants().First(el => el.HasClass("drugPackage")));
        var pharmaceuticalForm = ExtractDeepText(row.Descendants().First(el => el.HasClass("pharmacyForm")));
        var issuance = ExtractDeepText(row.Descendants().First(el => el.HasClass("modeOfIssuance")));
        var issuingType = ParseIssuingType(issuance);
        var manufacturer = ExtractDeepText(row.Descendants().First(el => el.HasClass("manufacturersNames")));
        var approvalCarrier = ExtractDeepText(row.Descendants().First(el => el.HasClass("approvalCarrier")));
        var solutionNumber = ExtractDeepText(row.Descendants().First(el => el.HasClass("solutionNumber")));
        var solutionDate = ExtractDeepText(row.Descendants().First(el => el.HasClass("solutionDate")));
        var validityDate = ExtractDeepText(row.Descendants().First(el => el.HasClass("validityDate")));
        var wholesalePrice = ExtractDeepText(row.Descendants().First(el => el.HasClass("wholesalePrice")));
        var retailPrice = ExtractDeepText(row.Descendants().First(el => el.HasClass("retailPrice")));
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
                .FirstOrDefault(el => ExtractDeepText(el).Trim() == "АТЦ")?
                .Descendants()?
                .Select(ExtractDeepText)?
                .LastOrDefault()?
                .Trim();
            var ingredients = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => ExtractDeepText(el).Trim() == "Состав")?
                .Descendants()?
                .Select(ExtractDeepText)
                .LastOrDefault()?
                .Trim();
            var manualUrl = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => ExtractDeepText(el).Trim() == "Упатство за употреба:", null)?
                .Descendants("div")?
                .LastOrDefault()?
                .Descendants("a")?
                .FirstOrDefault()?
                .GetAttributeValue("href", null);
            var reportUrl = document.DocumentNode
                .Descendants()?
                .Where(el => el.HasClass("row-fluid"))
                .FirstOrDefault(el => ExtractDeepText(el).Trim() == "Збирен извештај:", null)?
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