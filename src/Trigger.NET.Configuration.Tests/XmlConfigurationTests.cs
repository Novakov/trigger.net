namespace Trigger.NET.Configuration.Tests
{
    using System;
    using System.Xml.Linq;
    using Moq;
    using NUnit.Framework;
    using Trigger.NET.Configuration.Internals;
    using Trigger.NET.WaitSources;

    [TestFixture]
    public class XmlConfigurationTests
    {
        public Mock<Scheduler> Scheduler { get; set; }

        [SetUp]
        public void Setup()
        {
            this.Scheduler = new Mock<Scheduler>();
        }

        [Test]
        public void ForValidXmlShouldAddJob()
        {
            // arrange
            var xmlString = string.Format("<Job JobType=\"{0}\" Interval=\"00:00:01\"/>", typeof(DummyJob).AssemblyQualifiedName);
            var xml = XElement.Parse(xmlString);

            // act
            var func = XmlJobParser.Parse(xml);
            func(this.Scheduler.Object);

            // assert
            this.Scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<IntervalWaitSource>(y => y.Interval == TimeSpan.FromSeconds(1))));
        }
    }
}
