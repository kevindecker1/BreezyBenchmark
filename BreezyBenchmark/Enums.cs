using System;
using System.Collections.Generic;
using System.Text;

namespace BreezyBenchmark
{
    public enum BenchmarkHeaderLength
    {
        Method = 18,
        TestNo = 3,
        ElapsedTime = 14,
        FormattedElapsedTime = 19,
        AllocatedMemory = 13,
        Exception = 1000 // Just needs to be a large number
    }

    public enum BenchmarkHeaderPosition
    {
        Method = 0,
        TestNo = 24,
        ElapsedTime = 33,
        FormattedElapsedTime = 53,
        AllocatedMemory = 78,
        Exception = 97
    }
}
