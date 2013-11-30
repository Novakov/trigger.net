namespace Trigger.NET.WaitSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    public abstract class DateTimeSequenceWaitSource : IWaitSource
    {
        public IEnumerable<IWait> GetWaits()
        {
            return this.RunTimes().Select(runTime => new WaitUnitDate(runTime));
        }

        protected abstract IEnumerable<DateTimeOffset> RunTimes();
    }
}
