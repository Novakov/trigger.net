namespace Trigger.NET.FluentAPI
{
    using System;

    public interface IJobConfiguration<TJob> : IHideObjectMembers
        where TJob : IJob
    {
        IJobConfiguration<TJob> UseWaitSource(IWaitSource waitSource);
        IJobConfiguration<TJob> Setup(Action<JobSetup> action);
    }
}