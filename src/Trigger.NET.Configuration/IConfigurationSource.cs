namespace Trigger.NET.Configuration
{
    using System.IO;

    public interface IConfigurationSource
    {
        void Configure(IScheduler scheduler, Stream source);
    }
}