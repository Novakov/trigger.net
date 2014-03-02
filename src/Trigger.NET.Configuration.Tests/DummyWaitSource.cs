namespace Trigger.NET.Configuration.Tests
{
    using System;
    using System.Collections.Generic;

    internal class DummyWaitSource : IWaitSource
    {
        public string Test { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime From { get; set; }

        public DummyWaitSource(string test, TimeSpan interval, DateTime from)
        {
            Test = test;
            Interval = interval;
            From = @from;
        }

        public IEnumerable<IWait> GetWaits()
        {
            yield break;
        }
    }
}