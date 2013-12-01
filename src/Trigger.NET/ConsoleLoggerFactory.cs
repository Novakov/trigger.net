namespace Trigger.NET
{
    using System;

    public class ConsoleLoggerFactory : ILoggerFactory
    {
        private readonly LogSeverity minimumSeverity;

        public ConsoleLoggerFactory(LogSeverity minimumSeverity = LogSeverity.Info)
        {
            this.minimumSeverity = minimumSeverity;
        }

        public ILogger GetLogger(Type jobType, Guid jobId)
        {
            return new ConsoleLogger(this.minimumSeverity);
        }
    }
}