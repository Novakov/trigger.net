using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Trigger.NET
{
    public class Scheduler
    {
        private readonly Dictionary<Guid, Worker> jobs;

        public Scheduler()
        {
            this.jobs = new Dictionary<Guid, Worker>();
        }

        public Guid AddJob<TJob>(IWaitSource waitSource)
        {
            var id = Guid.NewGuid();

            var state = new Worker(typeof (TJob), waitSource);

            this.jobs[id] = state;

            state.Start();

            return id;
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
