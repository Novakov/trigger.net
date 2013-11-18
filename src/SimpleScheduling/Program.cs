using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.NET;
using Trigger.NET.WaitSources;

namespace SimpleScheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();

            Console.WriteLine("Adding WriteDot job");

            var jobId = scheduler.AddJob<WriteDot>(new IntervalWaitSource(TimeSpan.FromSeconds(1)));

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
            Console.Write(".");
        }
    }
}
