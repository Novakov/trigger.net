namespace Trigger.NET.NativeWrappers
{
    using System;
    using System.Threading;

    public interface INative
    {
        WaitHandle AbsoluteTimer(DateTimeOffset expireAt);
    }
}
