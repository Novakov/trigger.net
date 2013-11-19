namespace Trigger.NET
{
    using System;

    public interface IContainer : IDisposable
    {
        object Resolve(Type type);
    }
}