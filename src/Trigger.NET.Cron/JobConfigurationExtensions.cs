namespace Trigger.NET.Cron
{
    using System;
    using System.Runtime.InteropServices;
    using FluentAPI;
    using NCrontab;

    public static class JobConfigurationExtensions
    {
        public static IJobConfiguration<T> UseCron<T>(this IJobConfiguration<T> @this, string cronExpression, DateTimeOffset? startingFrom = null)
            where T : IJob
        {
            var expression = CrontabSchedule.Parse(cronExpression);

            startingFrom = CalculateNextDate(expression, startingFrom ?? DateTimeOffset.Now);

            return @this.RunWithSequence(startingFrom.Value, x => CalculateNextDate(expression, x));
        }

        private static DateTimeOffset CalculateNextDate(CrontabSchedule expression, DateTimeOffset previousRun)
        {
            var nextRun = expression.GetNextOccurrence(previousRun.LocalDateTime);
            return DateTime.SpecifyKind(nextRun, DateTimeKind.Local);
        }
    }
}
