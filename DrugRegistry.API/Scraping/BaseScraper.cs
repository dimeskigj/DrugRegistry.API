using DrugRegistry.API.Extensions;
using HtmlAgilityPack;

namespace DrugRegistry.API.Scraping;

public abstract class BaseScraper
{
    protected readonly HttpClient Client;

    protected BaseScraper(IHttpClientFactory httpClientFactory)
    {
        Client = httpClientFactory.CreateClient();
    }

    protected async Task<int> GetPageCount(string url)
    {
        var document = LoadHtmlDocument(await Client.RequestHtml(url, HttpMethod.Post));
        var count = document.DocumentNode
            .Descendants("a")
            .Where(el => el.Id.StartsWith("pager_") && int.TryParse(el.InnerText, out _))
            .Select(el => int.Parse(el.InnerText)).MaxBy(number => number);
        return count;
    }

    protected static HtmlDocument LoadHtmlDocument(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc;
    }

    protected static string ExtractDeepText(HtmlNode? node)
    {
        if (node is null) throw new ArgumentException("Node can't be null");
        return node.HasChildNodes ? ExtractDeepText(node.FirstChild) : node.InnerText.Trim();
    }
}