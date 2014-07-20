namespace Trigger.NET.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Threading;

    public class ManualTrigger : IWaitSource
    {
        private readonly AutoResetEvent @event;

        public ManualTrigger()
        {
            this.@event = new AutoResetEvent(false);
        }

        public IEnumerable<IWait> GetWaits()
        {
            while (true)
            {
                yield return new Wait(this.@event);
            }
        }

        public void TriggerNow()
        {
            this.@event.Set();
        }

        private class Wait : IWait
        {
            private readonly AutoResetEvent @event;

            public Wait(AutoResetEvent @event)
            {
                this.@event = @event;
            }

            void IWait.Wait()
            {
                this.@event.WaitOne();
            }
        }
    }
}