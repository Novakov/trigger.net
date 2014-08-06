namespace Trigger.NET.Configuration.Internals
{
    using System;
    using System.Collections.Generic;

    static internal class JobFactory
    {
        internal static Func<IScheduler, Guid> Create(Dictionary<string, string> attributes)
        {
            return s =>
            {
                var descriptor = ConfigurationParser.ParseAttributes(attributes);

                var jobSetup = new JobSetup()
                {
                    WaitSource = descriptor.Item2
                };

                return (Guid)typeof(IScheduler).GetMethod("AddJob")
                    .MakeGenericMethod(descriptor.Item1)
                    .Invoke(s, new object[] { jobSetup });
            };
        }
    }
}