namespace Trigger.NET
{
    using System;

    public class DefaultLogger : ILogger
    {
        public void Log(LogSeverity severity, string message, params object[] args)
        {
            if (!Environment.UserInteractive) return;

            switch (severity)
            {
                case LogSeverity.Debug:
#if !DEBUG
                    break;
#endif
                case LogSeverity.Info:
                case LogSeverity.Warn:
                    Console.WriteLine("{0}\t{1}\t{2}", severity, DateTime.Now, string.Format(message, args));
                    break;
                case LogSeverity.Error:
                case LogSeverity.Critical:
                default:
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("{0}\t{1}\t{2}", severity, DateTime.Now, string.Format(message, args));
                    Console.ForegroundColor = color;
                    break;
            }
        }

        public void Log(LogSeverity severity, Exception exception)
        {
            this.Log(severity, exception.ToString());
        }
    }
}