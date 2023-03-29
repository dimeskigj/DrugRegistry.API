using DrugRegistry.API.Domain;
using DrugRegistry.API.Domain.Scraping;
using DrugRegistry.API.Extensions;
using Newtonsoft.Json;

namespace DrugRegistry.API.Scraping;

public class PharmacyScraper : BaseScraper
{
    private readonly ILogger<PharmacyScraper> _logger;


    public PharmacyScraper(IHttpClientFactory httpClientFactory, ILogger<PharmacyScraper> logger) : base(
        httpClientFactory)
    {
        _logger = logger;
    }

    public Task<int> GetPageCount() => base.GetPageCount($"{Constants.LekoviWebUrl}/pharmacies");

    public async Task<IEnumerable<Pharmacy>> ScrapePage(int pageNumber = 1)
    {
        try
        {
            var uri = $"{Constants.LekoviWebUrl}/pharmacies.grid.pager/{pageNumber}/grid_0";
            var document = LoadHtmlDocument(await Client.RequestHtml(uri, HttpMethod.Post));

            var tableBody = document.DocumentNode
                .Descendants("table")
                .First(el => el.HasClass("table"))
                .Descendants("tbody")
                .First();

            var tableRows = tableBody.Descendants("tr");

            var nameTds = tableRows.Select(r => r.Descendants("td").Skip(1).FirstOrDefault());

            var detailUrls = nameTds.FilterNull()
                .Select(td => td.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", null))
                .Select(url => new Uri(new Uri(Constants.LekoviWebUrl), url));

            var pharmacies =
                detailUrls.FilterNull()
                    .Select(ScrapeDetails)
                    .Select(t => t.Result)
                    .ToList();

            return pharmacies;
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to scrape page {pageNumber}.\n{StackTrace}", pageNumber, e.StackTrace);
            throw;
        }
    }

    private async Task<Pharmacy> ScrapeDetails(Uri url)
    {
        try
        {
            var content = new StringContent(string.Empty);
            content.Headers.Add("X-Requested-With", "XMLHttpRequest");
            var response = await Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var dialogData =
                JsonConvert.DeserializeObject<PharmacyDialogDto>(await response.Content.ReadAsStringAsync());
            var html = dialogData?.Zones.Values.FirstOrDefault();

            var document = LoadHtmlDocument(html ?? string.Empty);
            var tableRows = document.DocumentNode.Descendants("tr").ToList();

            var title = document.DocumentNode.Descendants("h3").First(d => d.HasClass("modal-title")).InnerText;
            var address = tableRows.First(row => ExtractDeepText(row) == "Адреса").ChildNodes.Skip(1).First().InnerText;
            var municipality = tableRows.First(row => ExtractDeepText(row) == "Општина").ChildNodes.Skip(1).First().InnerText;
            var place = tableRows.First(row => ExtractDeepText(row) == "Населено место").ChildNodes.Skip(1).First().InnerText;
            var idNumber = tableRows.First(row => ExtractDeepText(row) == "Матичен број").ChildNodes.Skip(1).First().InnerText;
            var taxNumber = tableRows.First(row => ExtractDeepText(row) == "Даночен број").ChildNodes.Skip(1).First().InnerText;
            var code = tableRows.First(row => ExtractDeepText(row) == "Шифра").ChildNodes.Skip(1).First().InnerText;
            var phone = tableRows.First(row => ExtractDeepText(row) == "Tелефон").ChildNodes.Skip(1).First().InnerText;
            var decision = tableRows.First(row => ExtractDeepText(row) == "Решение").ChildNodes.Skip(1).First().InnerText;
            var email = tableRows.First(row => ExtractDeepText(row) == "E-mail").ChildNodes.Skip(1).First().InnerText;
            var pharmacists = tableRows.First(row => ExtractDeepText(row) == "Фармацевти").ChildNodes.Skip(1).First().InnerText;
            var technicians = tableRows.First(row => ExtractDeepText(row) == "Tехничари").ChildNodes.Skip(1).First().InnerText;
            var type = tableRows.First(row => ExtractDeepText(row) == "Тип").ChildNodes.Skip(1).First().InnerText;
            var central = tableRows.First(row => ExtractDeepText(row) == "Централна").ChildNodes.Skip(1).First().InnerText;
            var active = tableRows.First(row => ExtractDeepText(row) == "Активен").ChildNodes.Skip(1).First().InnerText;
            var comment = tableRows.First(row => ExtractDeepText(row) == "Коментар").ChildNodes.Skip(1).First().InnerText;

            var result = new Pharmacy
            {
                Name = title.Trim(),
                Address = address.Trim(),
                Municipality = municipality.Trim(),
                Place = place.Trim(),
                IdNumber = idNumber.Trim(),
                TaxNumber = taxNumber.Trim(),
                Code = code.Trim(),
                PhoneNumber = phone.Trim(),
                Decision = decision.Trim(),
                Email = email.Trim(),
                Pharmacists = pharmacists.Trim(),
                Technicians = technicians.Trim(),
                Comment = comment.Trim(),
                Url = url
            };
            
            result.Central = central.Trim() == "Да";
            result.Active = active.Trim() == "Да";
            result.PharmacyType = PharmacyTypeExtensions.ParseType(type.Trim());
            
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to scrape details for pharmacy with url: {url}.\n{StackTrace}", url, e.StackTrace);
            throw;
        }
    }
}