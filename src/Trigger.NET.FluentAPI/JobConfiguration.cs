namespace Trigger.NET.FluentAPI
{
    using System;
    using System.Collections.Generic;

    public class JobConfiguration<TJob> : IJobConfiguration<TJob>
        where TJob : IJob
    {
        private readonly JobSetup setup;

        public JobConfiguration()
        {
            this.setup = new JobSetup();
        }

        public Guid SetupJob(Scheduler scheduler)
        {
            return scheduler.AddJob<TJob>(this.setup);
        }

        public IJobConfiguration<TJob> UseWaitSource(IWaitSource waitSource)
        {
            this.setup.WaitSource = waitSource;
            return this;
        }

        public IJobConfiguration<TJob> Setup(Action<JobSetup> action)
        {
            action(this.setup);

            return this;
        }
    }
}