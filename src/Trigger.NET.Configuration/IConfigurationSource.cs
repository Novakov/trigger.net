namespace Trigger.NET.Configuration
{
    public interface IConfigurationSource
    {
        void Configure(IScheduler scheduler, string source);
    }
}