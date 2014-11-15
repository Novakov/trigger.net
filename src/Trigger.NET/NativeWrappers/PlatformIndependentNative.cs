namespace Trigger.NET.NativeWrappers
{
    using System;
    using System.Threading;
    using System.Timers;
    using Timer = System.Timers.Timer;

    public class PlatformIndependentNative : INative
    {
        public WaitHandle AbsoluteTimer(DateTimeOffset expireAt)
        {
            return new AbsoluteTimerWaitHandle(expireAt);
        }
    }

    public class AbsoluteTimerWaitHandle : WaitHandle
    {
        private readonly DateTimeOffset _dueTime;
        private readonly Timer _timer;
        private readonly EventWaitHandle _eventWaitHandle;

        public AbsoluteTimerWaitHandle(DateTimeOffset dueTime)
        {
            _dueTime = dueTime;
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            SafeWaitHandle = _eventWaitHandle.SafeWaitHandle;

            var dueSpan = (_dueTime - DateTimeOffset.Now);
            var period = new TimeSpan(dueSpan.Ticks / 10);

            if (dueSpan < TimeSpan.Zero)
            {
                _eventWaitHandle.Set();
            }
            else
            {
                _timer = new Timer(period.TotalMilliseconds)
                {
                    AutoReset = false,
                };

                _timer.Elapsed += TimerOnElapsed;

                _timer.Start();
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (DateTimeOffset.Now >= this._dueTime)
            {
                _eventWaitHandle.Set();
            }
            else
            {
                var dueSpan = (_dueTime - DateTimeOffset.Now);
                var period = new TimeSpan(dueSpan.Ticks / 10);
                
                if (period < TimeSpan.FromSeconds(1))
                {
                    _timer.Interval = dueSpan.TotalMilliseconds;
                }
                else
                {
                    _timer.Interval = period.TotalMilliseconds;
                }

                _timer.Start();
            }
        }

        protected override void Dispose(bool explicitDisposing)
        {
            this._timer.Dispose();
            this._eventWaitHandle.Dispose();

            base.Dispose(explicitDisposing);
        }
    }
}
