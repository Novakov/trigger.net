namespace Trigger.NET.Configuration.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using Trigger.NET.Configuration.Internals;
    using Trigger.NET.WaitSources;

    [TestFixture]
    public class JobParserTests
    {
        [Test, TestCaseSource("IntervalTestCases")]
        public void CanHandleIntervalJobDefinition(TimeSpan time)
        {
            //arrange
            var attrs = new Dictionary<string, string>()
            {
                {"JobType", typeof(DummyJob).AssemblyQualifiedName},
                {"Interval", time.ToString()}
            };

            //act
            var job = ConfigurationParser.ParseAttributes(attrs);

            //assert
            Assert.AreEqual(typeof(DummyJob), job.Item1);
            Assert.IsInstanceOf<IntervalWaitSource>(job.Item2);

            var ws = job.Item2 as IntervalWaitSource;

            Assert.AreEqual(ws.Interval, time);
        }

        private static readonly object[] IntervalTestCases =
        {
            new object[] {TimeSpan.FromSeconds(1)},
            new object[] {TimeSpan.FromSeconds(5)},
            new object[] {TimeSpan.FromSeconds(120)},
            new object[] {TimeSpan.FromSeconds(0.5)},
            new object[] {TimeSpan.FromDays(1)},
            new object[] {TimeSpan.FromHours(333)},
        };

        [Test]
        public void CanHandleOtherWaitSources()
        {
            //arrange
            var attrs = new Dictionary<string, string>()
            {
                {"JobType", typeof(DummyJob).AssemblyQualifiedName},
                {"WaitSourceType", typeof(DummyWaitSource).AssemblyQualifiedName},
                {"Test", "TestValue"},
                {"Interval", TimeSpan.FromDays(1).ToString()},
                {"from", new DateTime(2014, 1, 1, 12, 20, 0).ToString()}
            };

            //act
            var job = ConfigurationParser.ParseAttributes(attrs);

            //assert
            Assert.AreEqual(typeof(DummyJob).FullName, job.Item1.FullName);
            Assert.IsInstanceOf<DummyWaitSource>(job.Item2);

            var ws = job.Item2 as DummyWaitSource;

            Assert.AreEqual(ws.Interval, TimeSpan.FromDays(1));
            Assert.AreEqual(ws.From, new DateTime(2014, 1, 1, 12, 20, 0));
            Assert.AreEqual(ws.Test, "TestValue");
        }

        [Test, ExpectedException(typeof(Exception))]
        public void ForMissingArugmentThrowsException()
        {
            //arrange
            var attrs = new Dictionary<string, string>()
            {
                {"JobType", typeof(DummyJob).AssemblyQualifiedName},
                {"WaitSourceType", typeof(DummyWaitSource).AssemblyQualifiedName},
                {"Test", "TestValue"},
                //{"Interval", TimeSpan.FromDays(1).ToString()}, <-- Missing parameter
                {"from", new DateTime(2014, 1, 1, 12, 20, 0).ToString()}
            };

            //act
            ConfigurationParser.ParseAttributes(attrs);

            //assert
            Assert.Fail("Should throw an exception");
        }

        [Test, ExpectedException(typeof(Exception))]
        public void ForNotIJobShouldThrowException()
        {
            //arrange
            var attrs = new Dictionary<string, string>()
            {
                {"JobType", typeof(string).AssemblyQualifiedName},
            };

            //act
            ConfigurationParser.ParseAttributes(attrs);

            //assert
            Assert.Fail("Should throw an exception!");
        }
    }
}
