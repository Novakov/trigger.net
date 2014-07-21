namespace Trigger.NET.Configuration
{
    using System.IO;
    using System.Xml.Linq;
    using Trigger.NET.Configuration.Internals;

    public class XmlConfigurationSource : IConfigurationSource
    {
        public void Configure(IScheduler scheduler, string source)
        {
            using (var tr = File.OpenText(source))
            {
                var document = XDocument.Load(tr);

                foreach (var addDef in XmlJobParser.Parse(document))
                {
                    addDef(scheduler);
                }
            }
        }
    }
}