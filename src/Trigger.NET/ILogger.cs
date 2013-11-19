namespace Trigger.NET
{
    using System;

    public interface ILogger
    {
        void Log(LogSeverity severity, string message, params object[] args);

        void Log(LogSeverity severity, Exception exception);
    }
}