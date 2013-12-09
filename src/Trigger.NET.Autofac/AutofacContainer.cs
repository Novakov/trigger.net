namespace Trigger.NET.Autofac
{
    using System;
    using global::Autofac;

    public class AutofacContainer : Trigger.NET.IContainer
    {
        private ILifetimeScope lifetimeScope;

        public AutofacContainer(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public void Dispose()
        {
            if (this.lifetimeScope != null)
            {
                this.lifetimeScope.Dispose();
                this.lifetimeScope = null;
            }
        }

        public object Resolve(Type type)
        {
            return this.lifetimeScope.Resolve(type);
        }
    }
}
