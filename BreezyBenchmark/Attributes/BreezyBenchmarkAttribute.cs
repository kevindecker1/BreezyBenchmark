using System;
using System.Collections.Generic;
using System.Text;

namespace BreezyBenchmark.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BreezyBenchmarkAttribute : System.Attribute
    {
        /// <summary>
        /// Defaults to True. Set to False if code relies on HttpContext
        /// By running on a new thread, we can get more accurate results for memory allocation
        /// </summary>
        public bool RunOnNewThread { get; set; }

        public BreezyBenchmarkAttribute(bool runOnNewThread = true)
        {
            this.RunOnNewThread = runOnNewThread;
        }
    }
}
