namespace Trigger.NET
{
    using System;

    public class ConsoleLogger : ILogger
    {
        private readonly LogSeverity minimalSeverity;
      
        public ConsoleLogger(LogSeverity minimalSeverity)
        {
            this.minimalSeverity = minimalSeverity;
        }

        public void Log(LogSeverity severity, string message, params object[] args)
        {
            if (!Environment.UserInteractive) return;
            if ((int)severity < (int)this.minimalSeverity) return;

            var color = Console.ForegroundColor;

            switch (severity)
            {
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("{0}\t{1}\t{2}", severity, DateTime.Now, string.Format(message, args));
                    Console.ForegroundColor = color;
                    break;
                case LogSeverity.Info:
                case LogSeverity.Warn:
                    Console.WriteLine("{0}\t{1}\t{2}", severity, DateTime.Now, string.Format(message, args));
                    break;
                case LogSeverity.Error:
                case LogSeverity.Critical:
                default:
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