using System;
using System.Collections.Generic;
using System.Text;

namespace BreezyBenchmark.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BreezyBenchmarkParamsAttribute : System.Attribute
    {
        public object[] Values { get; set; }

        public BreezyBenchmarkParamsAttribute() => Values = new object[0];

        public BreezyBenchmarkParamsAttribute(params object[] values)
        {
            Values = values ?? new object[] { null };
        }
    }
}
