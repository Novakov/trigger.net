namespace Trigger.NET.FluentAPI
{
    public static class SchedulerExtensions
    {
        public static ISpecifyWaitSource<T> AddJob<T>(this Scheduler @this)
        {
            return (ISpecifyWaitSource<T>)new JobBuilder<T>(@this);
        }
    }
}
