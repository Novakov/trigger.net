namespace Trigger.NET.FluentAPI
{
    using System;

    public class JobConfiguration<TJob> : IJobConfiguration<TJob>
        where TJob : IJob
    {
        public IWaitSource WaitSource { get; private set; }

        public Guid SetupJob(Scheduler scheduler)
        {
            return scheduler.AddJob<TJob>(WaitSource);
        }

        public IJobConfiguration<TJob> UseWaitSource(IWaitSource waitSource)
        {
            this.WaitSource = waitSource;
            return this;
        }
    }
}