namespace Trigger.NET
{
    using System;
    using System.Collections.Generic;

    public class Scheduler : IScheduler
    {
        private Func<IContainer> containerFactory = () => new DefaultContainer();

        private Func<ILogger> loggerFactory = () => new DefaultLogger();

        private readonly Dictionary<Guid, Worker> jobs;

        public Scheduler()
        {
            this.jobs = new Dictionary<Guid, Worker>();
        }

        public Func<IContainer> ContainerFactory
        {
            get { return this.containerFactory; }
            set { this.containerFactory = value; }
        }

        public Func<ILogger> LoggerFactory
        {
            get { return this.loggerFactory; }
            set { this.loggerFactory = value; }
        }

        public Guid AddJob<TJob>(IWaitSource waitSource)
        {
            var id = Guid.NewGuid();

            var state = new Worker(typeof(TJob), waitSource, this.ContainerFactory, this.LoggerFactory);

            this.jobs[id] = state;

            state.Start();

            return id;
        }

        public virtual void RemoveJob(Guid jobId)
        {
            if (this.jobs.ContainsKey(jobId))
            {
                this.jobs[jobId].Stop();

                this.jobs.Remove(jobId);
            }
        }
    }
}
