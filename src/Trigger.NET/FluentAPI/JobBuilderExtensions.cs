namespace Trigger.NET.FluentAPI
{
    using System;
    using Trigger.NET.WaitSources;

    public static class JobConfigurationWaitSource
    {
        public static IJobConfiguration<T> RunEvery<T>(this IJobConfiguration<T> @this, TimeSpan interval) 
            where T : IJob
        {
            return @this.UseWaitSource(new IntervalWaitSource(interval));
        }

        public static IJobConfiguration<T> RunWithSequence<T>(this IJobConfiguration<T> @this, DateTimeOffset startingFrom, Func<DateTimeOffset, DateTimeOffset> thenAt) 
            where T : IJob
        {
            return @this.UseWaitSource(new WaitUntilNextDateWaitSource(startingFrom, thenAt));
        }
    }
}