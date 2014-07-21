namespace Trigger.NET.Configuration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Moq;
    using NUnit.Framework;
    using Trigger.NET.WaitSources;

    [TestFixture]
    public class JsonConfigurationTests
    {
        [Test]
        public void ShouldAddJobForValidJson()
        {
            // arrange
            var scheduler = new Mock<IScheduler>();
            var json = string.Format("[{{\"JobType\": \"{0}\", \"Interval\": \"00:00:01\"}}]", typeof(DummyJob).AssemblyQualifiedName);
            var source = new JsonConfigurationSource();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            // act
            source.ProcessJson(scheduler.Object, stream);

            // assert
            scheduler.Verify(x => x.AddJob<DummyJob>(It.Is<JobSetup>(y => (y.WaitSource as IntervalWaitSource).Interval == TimeSpan.FromSeconds(1))), Times.Once);
        }
    }
}
