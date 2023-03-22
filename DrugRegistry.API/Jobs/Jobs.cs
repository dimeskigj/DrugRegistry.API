using Quartz;

namespace DrugRegistry.API.Jobs;

public static class Jobs
{
    private static readonly IJobDetail? DrugScrapingJobDetail = JobBuilder
        .Create<DrugScrapingJob>()
        .WithIdentity(Constants.Quartz.DrugScrapingJobName)
        .Build();

    private static readonly ITrigger? DrugScrapingTrigger = TriggerBuilder
        .Create()
        .ForJob(DrugScrapingJobDetail)
        .WithIdentity(Constants.Quartz.DrugScrapingTriggerName)
        .StartNow()
        // TODO: Update this to a cron after testing
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).WithRepeatCount(0))
        .Build();

    public static readonly Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> JobsDictionary =
        new()
        {
            { DrugScrapingJobDetail, new[] { DrugScrapingTrigger } }
        };
}