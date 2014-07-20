namespace Trigger.NET.Tests.Helpers
{
    using System;
    using FluentAPI;

    public static class SchedulerExtensions
    {
        public static Guid AddAction(this Scheduler @this, IWaitSource waitSource, Action<JobContext> job)
        {
            return @this.AddJob<LambdaJob>(cfg => cfg.UseWaitSource(waitSource).WithParameter(job));
        }

        private class LambdaJob : IJob
        {
            public void Execute(JobContext context)
            {
                ((Action<JobContext>)context.Parameter).Invoke(context);
            }
        }
    }
}
