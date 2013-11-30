namespace Trigger.NET.NativeWrappers
{
    using System.Threading;
    using Microsoft.Win32.SafeHandles;

    internal class TimerWaitHandle : WaitHandle
    {
        public TimerWaitHandle(SafeWaitHandle safeHandle)
        {
            this.SafeWaitHandle = safeHandle;
        }
    }
}