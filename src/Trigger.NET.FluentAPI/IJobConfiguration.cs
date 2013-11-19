namespace Trigger.NET.FluentAPI
{
    public interface IJobConfiguration<TJob>
        where TJob : IJob
    {
        IJobConfiguration<TJob> UseWaitSource(IWaitSource waitSource);
    }
}