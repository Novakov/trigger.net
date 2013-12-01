namespace Trigger.NET
{
    using System;

    public interface ILoggerFactory
    {
        ILogger GetLogger(Type jobType, Guid jobId);
    }
}
