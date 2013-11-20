namespace SimpleSchedulingWithFluentAPI
{
    using System;
    using Trigger.NET;
    using Trigger.NET.FluentAPI;

    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();

            scheduler.LoggerFactory = () => new DefaultLogger(minimalSeverity: LogSeverity.Info);

            Console.WriteLine("Adding WriteDot job");

            var jobId = scheduler.AddJob<WriteDot>()
                                 .RunEvery(TimeSpan.FromSeconds(1))
                                 .Done();

            Console.ReadLine();

            Console.WriteLine("Removing WriteDot job");

            scheduler.RemoveJob(jobId);

            Console.ReadLine();
        }
    }

    public class WriteDot : IJob
    {
        public void Execute()
        {
            Console.WriteLine(".");
        }
    }
}
