namespace AutofacIntegration
{
    using System;
    using Autofac;
    using Trigger.NET;
    using Trigger.NET.Autofac;
    using Trigger.NET.FluentAPI;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EnglishHello>().As<IHello>();

            builder.RegisterType<SampleJob>().AsSelf();

            var container = builder.Build();

            var scheduler = new Scheduler(containerFactory: new AutofacContainerFactory(container));

            scheduler.AddJob<SampleJob>(cfg => cfg.RunEvery(TimeSpan.FromSeconds(1)));

            Console.ReadLine();
        }
    }

    internal interface IHello
    {
        string Hello(string name);
    }

    internal class EnglishHello : IHello
    {
        public string Hello(string name)
        {
            return string.Format("Hello {0}", name);
        }
    }

    internal class SampleJob : IJob
    {
        private readonly IHello hello;

        public SampleJob(IHello hello)
        {
            this.hello = hello;
        }

        public void Execute(JobContext context)
        {
            Console.WriteLine(hello.Hello("World"));
        }
    }
}
