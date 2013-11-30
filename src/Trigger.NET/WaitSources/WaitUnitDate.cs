namespace Trigger.NET.WaitSources
{
    using System;
    using NativeWrappers;

    public class WaitUnitDate : IWait
    {
        private readonly DateTimeOffset dueDateTime;

        public WaitUnitDate(DateTimeOffset dueDateTime)
        {
            this.dueDateTime = dueDateTime;
        }

        public void Wait()
        {
            var waitHandle = Native.Current.AbsoluteTimer(this.dueDateTime);

            waitHandle.WaitOne();            
        }
    }
}