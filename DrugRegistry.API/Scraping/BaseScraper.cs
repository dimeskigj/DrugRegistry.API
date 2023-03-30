using HtmlAgilityPack;

namespace DrugRegistry.API.Scraping;

public abstract class BaseScraper
{
    protected readonly HttpClient Client;

    protected BaseScraper(IHttpClientFactory httpClientFactory)
    {
        Client = httpClientFactory.CreateClient();
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