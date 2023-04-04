namespace DrugRegistry.API;

public static class Constants
{
    public const string AppName = "DrugRegistry.API/0.1";

    private const string LekoviWeb = "https://lekovi.zdravstvo.gov.mk";
    private const string NominatimGeocodingApi = "https://nominatim.openstreetmap.org";
    public static readonly Uri LekoviWebUrl = new(LekoviWeb);
    public static readonly Uri NominatimGeocodingApiUrl = new(NominatimGeocodingApi);

    public static class Quartz
    {
        public const string DrugScrapingJobName = nameof(DrugScrapingJobName);
        public const string DrugScrapingTriggerName = nameof(DrugScrapingTriggerName);
        public const string PharmacyScrapingJobName = nameof(PharmacyScrapingJobName);
        public const string PharmacyScrapingTriggerName = nameof(PharmacyScrapingTriggerName);
    }
}