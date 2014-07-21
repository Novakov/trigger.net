namespace Trigger.NET.Configuration.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public static class XmlJobParser
    {
        public static Func<IScheduler, Guid> Parse(XElement xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            if (!xml.Name.LocalName.Equals("Job"))
            {
                // TODO: Better exception/message
                throw new Exception("Bad node! ;)");
            }

            var attributes = xml.Attributes().ToDictionary(x => x.Name.LocalName, x => x.Value);

            return JobFactory.Create(attributes);
        }

        public static IEnumerable<Func<IScheduler, Guid>> Parse(XDocument xml)
        {
            if (xml == null || xml.Root == null)
            {
                throw new ArgumentNullException("xml");
            }

            if (xml.Root.Name.LocalName.Equals("Job"))
            {
                return new[] {Parse(xml.Root)};
            }

            return xml.Descendants("Job").Select(Parse);
        }
    }
}
