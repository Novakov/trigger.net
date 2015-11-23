namespace Trigger.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Scheduler : IScheduler, IDisposable
    {
        private readonly IContainerFactory containerFactory;
        private readonly ILoggerFactory loggerFactory;

        private readonly Dictionary<Guid, Worker> jobs;
        private bool _disposed;

        public Scheduler(ILoggerFactory loggerFactory = null, IContainerFactory containerFactory = null)
        {
            this.jobs = new Dictionary<Guid, Worker>();

            this.loggerFactory = loggerFactory ?? new ConsoleLoggerFactory();

            this.containerFactory = containerFactory ?? new DefaultContainerFactory();
        }

        public Guid AddJob<TJob>(JobSetup jobSetup)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("Scheduler", "Cannot schedule new jobs.");
            }

            if (jobSetup.JobId == Guid.Empty)
            {
                jobSetup.JobId = Guid.NewGuid();
            }

            var state = new Worker(typeof(TJob), jobSetup, this.containerFactory, this.loggerFactory);

            this.jobs[jobSetup.JobId] = state;

            state.Start();

            return jobSetup.JobId;
        }

        public virtual void RemoveJob(Guid jobId)
        {
            if (this.jobs.ContainsKey(jobId))
            {
                this.jobs[jobId].Stop();

                this.jobs.Remove(jobId);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.jobs.Keys.ToList().ForEach(this.RemoveJob);
            }

            this._disposed = true;
        }
    }
}
