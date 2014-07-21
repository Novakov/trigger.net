namespace Trigger.NET.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using Trigger.NET.Configuration.Internals;

    public class JsonConfigurationSource : IConfigurationSource
    {
        public void Configure(IScheduler scheduler, string source)
        {
            using (var tr = File.OpenRead(source))
            {
                ProcessJson(scheduler, tr);
            }
        }

        public void ProcessJson(IScheduler scheduler, Stream tr)
        {
            var settings = new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true };
            var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>[]), settings);

            var obj = serializer.ReadObject(tr) as Dictionary<string, string>[];

            foreach (var attributes in obj)
            {
                JobFactory.Create(attributes)(scheduler);
            }
        }
    }
}