namespace Trigger.NET.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Trigger.NET.Configuration.Internals;

    public class JsonConfigurationSource : IConfigurationSource
    {
        public void Configure(IScheduler scheduler, Stream source)
        {
            string json;

            using (var reader = new StreamReader(source, Encoding.UTF8))
            {
                json = reader.ReadToEnd();
            }

            var obj = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            foreach (var attributes in obj)
            {
                JobFactory.Create(attributes)(scheduler);
            }
        }
    }
}