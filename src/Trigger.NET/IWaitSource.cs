using System.Collections.Generic;

namespace Trigger.NET
{
    public interface IWaitSource
    {
        IEnumerable<IWait> GetWaits();
    }

    public interface IWait
    {
        void Wait();
    }
}