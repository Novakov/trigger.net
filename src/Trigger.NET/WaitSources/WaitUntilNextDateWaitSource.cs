namespace Trigger.NET.WaitSources
{
    using System;
    using System.Collections.Generic;

    public class WaitUntilNextDateWaitSource : DateTimeSequenceWaitSource
    {
        private readonly DateTimeOffset startFrom;
        private readonly Func<DateTimeOffset, DateTimeOffset> next;

        public WaitUntilNextDateWaitSource(DateTimeOffset startFrom, Func<DateTimeOffset, DateTimeOffset> next)
        {
            this.startFrom = startFrom;
            this.next = next;
        }

        protected override IEnumerable<DateTimeOffset> RunTimes()
        {
            var date = this.startFrom;
            
            while (true)
            {
                yield return date;

                date = next(date);
            }
        }
    }
}
