using Newtonsoft.Json;

namespace DrugRegistry.API.Service;

public class BaseHttpService
{
    protected readonly HttpClient Client;

    protected BaseHttpService(IHttpClientFactory httpClientFactory)
    {
        Client = httpClientFactory.CreateClient();
    }

    protected async Task<T?> Get<T>(Uri uri)
    {
        var resp = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
        resp.EnsureSuccessStatusCode();
        var content = await resp.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }
}