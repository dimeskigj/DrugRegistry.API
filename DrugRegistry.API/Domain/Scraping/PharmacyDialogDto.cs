using Newtonsoft.Json;

namespace DrugRegistry.API.Domain.Scraping;

public class PharmacyDialogDto
{
    [JsonProperty("zones")]
    public Dictionary<string, string> Zones { get; set; }
}