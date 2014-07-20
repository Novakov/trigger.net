namespace Trigger.NET.Tests.Helpers
{
    using System.Threading;

    public class SetOnExecute : IJob
    {
        public void Execute(JobContext context)
        {
            ((EventWaitHandle) context.Parameter).Set();
        }
    }
}