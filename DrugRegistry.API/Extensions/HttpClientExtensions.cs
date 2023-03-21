namespace DrugRegistry.API.Extensions;

public static class HttpClientExtensions
{
    public static async Task<string> RequestHtml(this HttpClient client, string uri, HttpMethod method)
    {
        var message = new HttpRequestMessage(method, uri);
        var responseMessage = await client.SendAsync(message);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadAsStringAsync();
    }
}