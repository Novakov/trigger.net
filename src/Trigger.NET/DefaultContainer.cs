namespace Trigger.NET
{
    using System;

    public class DefaultContainer : IContainer
    {
        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public void Dispose()
        {
            // Nothing to do
        }
    }
}