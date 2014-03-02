namespace Trigger.NET.Configuration
{
    using System.IO;
    using System.Xml.Linq;
    using Trigger.NET.Configuration.Internals;

    public static class SchedulerExtensions
    {
        public static void ConfigureFromXml(this IScheduler @this, string path)
        {
            using (var tr = File.OpenText(path))
            {
                var document = XDocument.Load(tr);

                foreach (var addDef in XmlJobParser.Parse(document))
                {
                    addDef(@this);
                }
            }
        }

        public static void ConfigureFromJson(this IScheduler @this, string path)
        {
            
        }
    }
}
