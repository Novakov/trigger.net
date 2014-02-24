namespace Trigger.NET.Configuration.Internals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Trigger.NET.WaitSources;

    public class ConfigurationParser
    {
        public static Tuple<Type, IWaitSource> ParseAttributes(Dictionary<string, string> attributes)
        {
            var attrs = new Dictionary<string, string>(attributes, StringComparer.InvariantCultureIgnoreCase);

            if (!attrs.ContainsKey("JobType"))
            {
                throw new Exception("Job Type unspecified!");
            }

            var type = Type.GetType(attrs["JobType"]);
            if (type == null)
            {
                throw new Exception("Cannot find specified type!");
            }

            // TODO: Check why IsAssignableFrom does not work (is faster)
            if (!type.GetInterfaces().Contains(typeof (IJob)))
            {
                throw new Exception("Specified job type does not implement IJob interface!");
            }

            var waitSourceType = typeof (IntervalWaitSource);
            if (attrs.ContainsKey("WaitSourceType"))
            {
                waitSourceType = Type.GetType(attrs["WaitSourceType"]);
                if (waitSourceType == null)
                {
                    throw new Exception("Cannot find specified Wait Source Type!");
                }

                if (!waitSourceType.GetInterfaces().Contains(typeof (IWaitSource)))
                {
                    throw new Exception("Specified wait source type does not implement IWaitSource interface!");
                }
            }

            var ctor = waitSourceType.GetConstructors().FirstOrDefault();
            var parameters = ctor.GetParameters();
            var ctorParams = new List<object>();

            foreach (var parameter in parameters)
            {
                string name = parameter.Name;
                if (!attrs.ContainsKey(name))
                {
                    if (parameter.IsOptional)
                    {
                        continue;
                    }

                    throw new Exception(string.Format("Cannot instantiate wait source! Required parameter `{0}` is missing!", parameter.Name));
                }

                var value = TypeDescriptor.GetConverter(parameter.ParameterType).ConvertFromString(attrs[name]);
                if (value == null)
                {
                    throw new Exception(
                        string.Format("Parameter conversion error! Parameter {0} cannot be converted from '{1}' to type {2}",
                            parameter.Name, attrs[name], parameter.ParameterType.Name));
                }

                ctorParams.Add(value);
            }

            var waitSource = ctor.Invoke(ctorParams.ToArray());

            return new Tuple<Type, IWaitSource>(type, waitSource as IWaitSource);
        }
    }
}