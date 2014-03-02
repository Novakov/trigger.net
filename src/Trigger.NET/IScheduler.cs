namespace Trigger.NET
{
    using System;

    public interface IScheduler
    {
        Func<IContainer> ContainerFactory { get; set; }

        Func<ILogger> LoggerFactory { get; set; }

        Guid AddJob<TJob>(IWaitSource waitSource);

        void RemoveJob(Guid jobId);
    }
}