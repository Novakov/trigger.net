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

            Console.WriteLine("Adding parameterized jobs");
            var paramJob1 = scheduler.AddJob<ParameterizedJob>(cfg => cfg
                .RunEvery(TimeSpan.FromSeconds(1))
                .WithParameter("First parameter value"));

            var paramJob2 = scheduler.AddJob<ParameterizedJob>(cfg => cfg
                .RunEvery(TimeSpan.FromSeconds(3))
                .WithParameter("Second parameter value"));

            Console.ReadLine();

            Console.WriteLine("Removing WriteDot job");

            scheduler.RemoveJob(writeDotId);

            Console.WriteLine("Removing WriteComma job");

            scheduler.RemoveJob(writeCommaId);

            Console.WriteLine("Removing second parametrized job");
            scheduler.RemoveJob(paramJob2);

            Console.WriteLine("Removing first parametrized job");
            scheduler.RemoveJob(paramJob1);

            Console.ReadLine();
        }
    }

    public class WriteDot : IJob
    {
        public void Execute(JobContext context)
        {
            Console.Write(".");
        }
    }

    public class WriteComma : IJob
    {
        public void Execute(JobContext context)
        {
            Console.WriteLine("{0}: ,", DateTimeOffset.Now);
        }
    }

    public class CronJob : IJob
    {
        public void Execute(JobContext context)
        {
            Console.WriteLine("\nCron job: {0}", DateTimeOffset.Now);
        }
    }

    public class ParameterizedJob : IJob
    {
        public void Execute(JobContext context)
        {
            Console.WriteLine("Param: {0}", context.Parameter);
        }
    }
}
