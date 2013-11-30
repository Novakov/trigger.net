namespace Trigger.NET.NativeWrappers
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Microsoft.Win32.SafeHandles;

    class Win32Native : INative
    {       
        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll")]
        private static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume);


        public WaitHandle AbsoluteTimer(DateTimeOffset expireAt)
        {
            var handle = CreateWaitableTimer(IntPtr.Zero, true, null);

            var dueTime = expireAt.ToUniversalTime().ToFileTime();

            SetWaitableTimer(handle, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, false);

            var safeHandle = new SafeWaitHandle(handle, true);

            return new TimerWaitHandle(safeHandle);
        }
    }
}