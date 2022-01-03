using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreezyBenchmark;
using BreezyBenchmark.Attributes;

namespace BreezyBenchmark.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Call BenchmarkRunner on an entire class
            // Methods need to be marked with the BreezyBenchmark attribute
            BreezyBenchmarkRunner.Run<Program>();

            // 2. Can test a specific block of code
            BreezyBenchmarkRunner.Run("Code Block Test", () =>
            {
                string str = "";
                str += "Test";
            }, true);

            // 3. Add actions to an Instance
            // Good for code comparisons. Different code implementations for the same result/outcome
            // Need to call BreezyBenchmarkRunner.Instance.Run to execute the tests. Once done, Instance is "reset"
            BreezyBenchmarkRunner.Instance.Add(() => { string str = ""; str += "Test"; }, "StringInter");
            BreezyBenchmarkRunner.Instance.Add(() => { var str = new System.Text.StringBuilder(); str.Append("Test"); }, "StringBuilder");
            BreezyBenchmarkRunner.Instance.Run(true);
        }

        [BreezyBenchmark]
        public void String()
        {
            string str = "Test";
        }

        [BreezyBenchmark]
        public void StringInterpolation()
        {
            string str = "";
            str += "Test";
        }

        [BreezyBenchmark]
        public void StringBuilder()
        {
            var str = new System.Text.StringBuilder();
            str.Append("Test");
        }

        [BreezyBenchmark]
        public void CreateString()
        {
            var str = new String(' ', 4);
            str = str.Insert(0, "Test");
        }
    }
}
