namespace Trigger.NET
{
    using System;

    public class JobSetup
    {
        public Guid JobId { get; set; }
        public IWaitSource WaitSource { get; set; }
        public object Parameter { get; set; }

        public JobSetup()
        {
            this.JobId = Guid.Empty;
        }

        public JobContext BuildContext()
        {
            return new JobContext(this.JobId, this.Parameter);
        }
    }
}