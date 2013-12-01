namespace Trigger.NET
{
    using System;

    public class JobSetup
    {
        public Guid JobId { get; set; }
        public IWaitSource WaitSource { get; set; }

        public JobSetup()
        {
            this.JobId = Guid.Empty;
        }
    }
}