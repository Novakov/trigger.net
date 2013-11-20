namespace Trigger.NET.FluentAPI
{
    using System;

    public interface IJobConfiguration<TJob>
        where TJob : IJob
    {
        IJobConfiguration<TJob> UseWaitSource(IWaitSource waitSource);
        IJobConfiguration<TJob> Setup(Action<JobSetup> action);
    }
}