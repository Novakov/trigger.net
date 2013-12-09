namespace SimpleSchedulingWithFluentAPI
{
    using System;
    using Trigger.NET;
    using Trigger.NET.Cron;
    using Trigger.NET.FluentAPI;

    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();            

            Console.WriteLine("Adding WriteDot job");

            var writeDotId = scheduler.AddJob<WriteDot>(cfg => cfg.RunEvery(TimeSpan.FromSeconds(1)));                                

            Console.WriteLine("Adding WriteComma job");

            var writeCommaId = scheduler.AddJob<WriteComma>(cfg => cfg.RunWithSequence(DateTimeOffset.Now, x => x.AddMinutes(5)));

            scheduler.AddJob<CronJob>(cfg => cfg.UseCron("*/4 * * * *"));

            Console.ReadLine();

            Console.WriteLine("Removing WriteDot job");

            scheduler.RemoveJob(writeDotId);

            Console.WriteLine("Removing WriteComma job");

            scheduler.RemoveJob(writeCommaId);

            Console.ReadLine();
        }
    }

    public class WriteDot : IJob
    {
        public void Execute()
        {
            Console.Write(".");
        }
    }

    public class WriteComma : IJob
    {
        public void Execute()
        {
            Console.WriteLine("{0}: ,", DateTimeOffset.Now);
        }
    }

    public class CronJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("\nCron job: {0}", DateTimeOffset.Now);
        }
    }
}
