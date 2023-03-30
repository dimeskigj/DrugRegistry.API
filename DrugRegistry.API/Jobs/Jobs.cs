using Quartz;

namespace DrugRegistry.API.Jobs;

public static class Jobs
{
    private const string EverySundayAt2300 = "0 0 23 ? * SUN *";
    
    private static readonly IJobDetail? DrugScrapingJobDetail = JobBuilder
        .Create<DrugScrapingJob>()
        .WithIdentity(Constants.Quartz.DrugScrapingJobName)
        .Build();

    private static readonly ITrigger? DrugScrapingTrigger = TriggerBuilder
        .Create()
        .ForJob(DrugScrapingJobDetail)
        .WithIdentity(Constants.Quartz.DrugScrapingTriggerName)
        .StartNow()
        .WithCronSchedule(EverySundayAt2300)
        .Build();

    public static readonly Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> JobsDictionary =
        new()
        {
            { DrugScrapingJobDetail, new[] { DrugScrapingTrigger } }
        };
}