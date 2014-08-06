namespace Trigger.NET.Configuration
{
    using System.IO;

    public static class SchedulerExtensions
    {
        public static IScheduler ConfigureFromXml(this IScheduler @this, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return @this.ConfigureFromXml(stream);
            }
        }

        public static IScheduler ConfigureFromXml(this IScheduler @this, Stream stream)
        {
            return @this.Configure<XmlConfigurationSource>(stream);
        }
        
        public static IScheduler ConfigureFromJson(this IScheduler @this, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return @this.ConfigureFromXml(stream);
            }
        }
        
        public static IScheduler ConfigureFromJson(this IScheduler @this, Stream stream)
        {
            return @this.Configure<JsonConfigurationSource>(stream);
        }

        public static IScheduler Configure<TConfigurationSource>(this IScheduler @this, Stream source)
            where TConfigurationSource : IConfigurationSource, new()
        {
            var configurator = new TConfigurationSource();

            configurator.Configure(@this, source);

            return @this;
        }
    }
}
