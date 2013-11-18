using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trigger.NET.WaitSources
{
    public class IntervalWaitSource : IWaitSource
    {
        public TimeSpan Interval { get; private set; }

        public IntervalWaitSource(TimeSpan interval)
        {
            Interval = interval;
        }

        public IEnumerable<IWait> GetWaits()
        {
            while (true)
            {
                yield return new SleepWait(this.Interval);
            }
        }
    }

    public class SleepWait : IWait
    {
        private readonly TimeSpan interval;

        public SleepWait(TimeSpan interval)
        {
            this.interval = interval;
        }

        public void Wait()
        {
            Thread.Sleep(this.interval);            
        }
    }
}
