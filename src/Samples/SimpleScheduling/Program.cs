namespace SimpleScheduling
{
    using System;
    using Trigger.NET;
    using Trigger.NET.WaitSources;

    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();

            Console.WriteLine("Adding WriteDot job");

            var jobId = scheduler.AddJob<WriteDot>(new JobSetup() {WaitSource = new IntervalWaitSource(TimeSpan.FromSeconds(1))});

            Console.ReadLine();

            Console.WriteLine("Removing WriteDot job");

            scheduler.RemoveJob(jobId);

            Console.ReadLine();
        }
    }

    public class WriteDot : IJob
    {
        public void Execute(JobContext context)
        {
            Console.WriteLine(".");
        }
    }
}
