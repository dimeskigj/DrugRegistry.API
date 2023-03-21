﻿using HtmlAgilityPack;

namespace DrugRegistry.API.Scraping;

public class BaseScraper
{
    protected readonly HttpClient Client = new();

    protected static HtmlDocument LoadHtmlDocument(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc;
    }
}