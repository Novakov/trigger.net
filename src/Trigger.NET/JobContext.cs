namespace Trigger.NET
{
    using System;

    public class JobContext
    {
        public Guid JobId { get; private set; }
        public object Parameter { get; private set; }

        public JobContext(Guid jobId, object parameter)
        {
            this.JobId = jobId;
            this.Parameter = parameter;
        }
    }
}