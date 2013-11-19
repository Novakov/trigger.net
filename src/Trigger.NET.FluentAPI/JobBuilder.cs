namespace Trigger.NET.FluentAPI
{
    using System;

    public class JobBuilder<T> : IJobBuilder, ISpecifyWaitSource<T>
    {
        private readonly IWaitSource waitSource;

        internal JobBuilder(Scheduler scheduler)
        {
            this.Scheduler = scheduler;
        }

        internal JobBuilder(Scheduler scheduler, IWaitSource waitSource)
            : this(scheduler)
        {
            this.waitSource = waitSource;
        }

        public Scheduler Scheduler { get; protected set; }

        public Guid Done()
        {
            return this.Scheduler.AddJob<T>(this.waitSource);
        }
    }
}