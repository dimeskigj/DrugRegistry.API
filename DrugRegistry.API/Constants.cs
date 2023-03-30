namespace DrugRegistry.API;

public static class Constants
{
    public const string LekoviWebUrl = "https://lekovi.zdravstvo.gov.mk";

    public static class Quartz
    {
        public const string DrugScrapingJobName = nameof(DrugScrapingJobName);
        public const string DrugScrapingTriggerName = nameof(DrugScrapingTriggerName);
    }
}