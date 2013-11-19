namespace Trigger.NET.FluentAPI
{
    public interface ISpecifyWaitSource<T>
    {
        Scheduler Scheduler { get; }
    }
}