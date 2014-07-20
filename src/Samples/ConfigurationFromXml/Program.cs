namespace ConfigurationFromXml
{
    using System;
    using Trigger.NET;
    using Trigger.NET.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();

            scheduler.ConfigureFromXml(".\\scheduler.config");

            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
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
