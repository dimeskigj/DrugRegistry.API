using HtmlAgilityPack;

namespace DrugRegistry.API.Utils;

public static class HtmlUtils
{
    public static string ExtractDeepText(this HtmlNode? node)
    {
        while (true)
        {
            if (node is null) throw new ArgumentException("Node can't be null");
            if (!node.HasChildNodes) return node.InnerText.Trim();
            node = node.FirstChild;
        }
    }
}