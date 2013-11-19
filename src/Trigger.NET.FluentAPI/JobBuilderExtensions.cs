namespace Trigger.NET.FluentAPI
{
    using System;
    using Trigger.NET.WaitSources;

    public static class JobBuilderExtensions
    {
        public static IJobBuilder RunEvery<T>(this ISpecifyWaitSource<T> @this, TimeSpan ts)
        {
            return (IJobBuilder) new JobBuilder<T>(@this.Scheduler, new IntervalWaitSource(ts));
        }
    }
}