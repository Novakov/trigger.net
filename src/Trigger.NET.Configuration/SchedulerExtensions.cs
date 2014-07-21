namespace Trigger.NET.Configuration
{
    public static class SchedulerExtensions
    {
        public static IScheduler ConfigureFromXml(this IScheduler @this, string path)
        {
            return @this.Configure<XmlConfigurationSource>(path);
        }

        public static IScheduler ConfigureFromJson(this IScheduler @this, string path)
        {
            return @this.Configure<JsonConfigurationSource>(path);
        }

        public static IScheduler Configure<TConfigurationSource>(this IScheduler @this, string source)
            where TConfigurationSource : IConfigurationSource, new()
        {
            var configurator = new TConfigurationSource();

            configurator.Configure(@this, source);

            return @this;
        }
    }
}
