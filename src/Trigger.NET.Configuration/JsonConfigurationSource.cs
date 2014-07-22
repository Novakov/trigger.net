namespace Trigger.NET.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using Trigger.NET.Configuration.Internals;

    public class JsonConfigurationSource : IConfigurationSource
    {
        public void Configure(IScheduler scheduler, Stream source)
        {
            var settings = new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true };
            var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>[]), settings);

            var obj = serializer.ReadObject(source) as Dictionary<string, string>[];

            foreach (var attributes in obj)
            {
                JobFactory.Create(attributes)(scheduler);
            }
        }
    }
}