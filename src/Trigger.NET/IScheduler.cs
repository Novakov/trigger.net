namespace Trigger.NET
{
    using System;

    public interface IScheduler
    {
        Guid AddJob<TJob>(JobSetup jobSetup);
        void RemoveJob(Guid jobId);
    }
}