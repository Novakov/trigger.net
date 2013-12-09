namespace Trigger.NET
{
    public class DefaultContainerFactory : IContainerFactory
    {        
        public IContainer GetContainer()
        {
            return new DefaultContainer();
        }
    }
}