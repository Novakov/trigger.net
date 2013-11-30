namespace Trigger.NET.NativeWrappers
{
    using System;

    static class Native
    {
        public static readonly INative Current;

        static Native()
        {
            Current = null;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Current = new Win32Native();
            }            
        }
    }
}
