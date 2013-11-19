namespace Trigger.NET
{
    using System;

    public class NoLogger : ILogger
    {
        public void Log(LogSeverity severity, string message, params object[] args)
        {
        }

        public void Log(LogSeverity severity, Exception exception)
        {
        }
    }
}