namespace Trigger.NET.Autofac
{
    using global::Autofac;

    public class AutofacContainerFactory : IContainerFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        public AutofacContainerFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public Trigger.NET.IContainer GetContainer()
        {
            return new AutofacContainer(this.lifetimeScope.BeginLifetimeScope());
        }
    }
}