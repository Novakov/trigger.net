namespace Trigger.NET.Configuration.Tests
{
    using Moq;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Xml.Linq;
    using Trigger.NET.Configuration.Internals;
    using Trigger.NET.WaitSources;

    [TestFixture]
    public class XmlConfigurationTests
    {
        public Mock<IScheduler> Scheduler { get; set; }

        [SetUp]
        public void Setup()
        {
            this.Scheduler = new Mock<IScheduler>();
        }

        [Test]
        public void ForValidXmlShouldAddJob()
        {
            // arrange
            var xmlString = string.Format("<Job JobType=\"{0}\" Interval=\"00:00:01\"/>", typeof(DummyJob).AssemblyQualifiedName);
            var xml = XElement.Parse(xmlString);

            // act
            XmlJobParser.Parse(xml)(this.Scheduler.Object);

            // assert
            this.Scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<JobSetup>(y => (y.WaitSource as IntervalWaitSource).Interval == TimeSpan.FromSeconds(1))), Times.Once);
        }

        [Test]
        public void ForCoupleJobDefinitionsShouldConfigureCoupleJobs()
        {
            // arrange
            var xmlString = string.Format("<Jobs><Job JobType=\"{0}\" Interval=\"00:00:01\"/><Job JobType=\"{0}\" Interval=\"12:00:01\"/></Jobs>", typeof (DummyJob).AssemblyQualifiedName);
            var xml = XDocument.Parse(xmlString);

            // act
            foreach(var f in XmlJobParser.Parse(xml)) f(this.Scheduler.Object);

            // assert
            this.Scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<JobSetup>(y => (y.WaitSource as IntervalWaitSource).Interval == TimeSpan.FromSeconds(1))), Times.Once);
            this.Scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<JobSetup>(y => (y.WaitSource as IntervalWaitSource).Interval == TimeSpan.Parse("12:00:01"))), Times.Once);
        }

        [Test]
        public void ShouldConfigureFromFile()
        {
            // arrange
            var xmlString = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Jobs><Job JobType=\"{0}\" Interval=\"00:00:01\"/><Job JobType=\"{0}\" Interval=\"12:00:01\"/></Jobs>", typeof(DummyJob).AssemblyQualifiedName);
            var path = Path.GetTempFileName();

            File.WriteAllText(path, xmlString);

            // act
            this.Scheduler.Object.ConfigureFromXml(path);

            // assert
            this.Scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<JobSetup>(y => (y.WaitSource as IntervalWaitSource).Interval == TimeSpan.FromSeconds(1))), Times.Once);
            this.Scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<JobSetup>(y => (y.WaitSource as IntervalWaitSource).Interval == TimeSpan.Parse("12:00:01"))), Times.Once);
        }
    }
}
