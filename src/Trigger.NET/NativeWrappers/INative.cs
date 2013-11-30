namespace Trigger.NET.NativeWrappers
{
    using System;
    using System.Threading;

    interface INative
    {
        WaitHandle AbsoluteTimer(DateTimeOffset expireAt);
    }
}
