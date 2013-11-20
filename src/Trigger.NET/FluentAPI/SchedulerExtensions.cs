namespace Trigger.NET.FluentAPI
{
    using System;

    public static class SchedulerExtensions
    {
        public static Guid AddJob<T>(this Scheduler @this, Action<IJobConfiguration<T>> configure) 
            where T : IJob
        {
            var cfg = new JobConfiguration<T>();

            configure(cfg);

            return cfg.SetupJob(@this);
        }
    }
}
