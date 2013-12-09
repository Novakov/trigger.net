namespace Trigger.NET
{
    using System;
    using System.Collections.Generic;

    public class Scheduler
    {
        private readonly IContainerFactory containerFactory;
        private readonly ILoggerFactory loggerFactory;

        private readonly Dictionary<Guid, Worker> jobs;

        public Scheduler(ILoggerFactory loggerFactory = null, IContainerFactory containerFactory = null)
        {
            this.jobs = new Dictionary<Guid, Worker>();

            this.loggerFactory = loggerFactory ?? new ConsoleLoggerFactory();

            this.containerFactory = containerFactory ?? new DefaultContainerFactory();
        }

        public Guid AddJob<TJob>(JobSetup jobSetup)
        {
            if (jobSetup.JobId == Guid.Empty)
            {
                jobSetup.JobId = Guid.NewGuid();
            }

            var state = new Worker(typeof(TJob), jobSetup, this.containerFactory, this.loggerFactory);

            this.jobs[jobSetup.JobId] = state;

            state.Start();

            return jobSetup.JobId;
        }

        public void RemoveJob(Guid jobId)
        {
            if (this.jobs.ContainsKey(jobId))
            {
                this.jobs[jobId].Stop();

                this.jobs.Remove(jobId);
            }
        }
    }
}
