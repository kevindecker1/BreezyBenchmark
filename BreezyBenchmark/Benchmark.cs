using System;
using System.Collections.Generic;
using System.Text;

namespace BreezyBenchmark
{
    public sealed class Benchmark
    {
        private TimeSpan _elapsedTime { get; set; }
        private long _allocatedMemory { get; set; }
        private string _methodName { get; set; }
        private int? _testNumber { get; set; }
        private string _exception { get; set; }

        private Action _action { get; set; }
        private bool _runOnNewThread { get; set; }
        public bool RunOnNewThread
        {
            get
            {
                return _runOnNewThread;
            }
        }

        private System.Diagnostics.Stopwatch _stopWatch { get; set; }

        public Benchmark()
        {
            _stopWatch = new System.Diagnostics.Stopwatch();
        }

        public Benchmark(Action action, int testNumber)
            : this()
        {
            _action = action;
            _testNumber = testNumber;
        }

        public Benchmark(Action action, string methodName, bool runOnNewThread = true)
            : this()
        {
            _runOnNewThread = runOnNewThread;
            _action = action;
            _methodName = methodName;
        }

        public Benchmark(Action action)
            : this()
        {
            _action = action;
        }

        public void Start()
        {
            _stopWatch.Start();
        }

        public void Stop()
        {
            _stopWatch.Stop();
            _elapsedTime = _stopWatch.Elapsed;
            _stopWatch.Reset();
        }

        public void SetException(string exception)
        {
            _exception = exception;
        }

        public void SetAllocatedMemory(long allocatedMemory)
        {
            _allocatedMemory = allocatedMemory;
        }

        public string Summary
        {
            get
            {
                return _generateSummaryLine();
            }
        }

        public void Execute()
        {
            if (_action != null)
            {
                _action();
            }
        }

        private string _generateSummaryLine()
        {
            var line = new string(' ', 200);
            line = line.Insert(BenchmarkHeaderPosition.Method.ToInt32(), _getValue(_methodName, BenchmarkHeaderLength.Method));
            line = line.Insert(BenchmarkHeaderPosition.TestNo.ToInt32(), _getValue(_testNumber, BenchmarkHeaderLength.TestNo));
            line = line.Insert(BenchmarkHeaderPosition.ElapsedTime.ToInt32(), _getValue(_elapsedTime, BenchmarkHeaderLength.ElapsedTime));
            line = line.Insert(BenchmarkHeaderPosition.FormattedElapsedTime.ToInt32(), _getValue(_elapsedTime.ToDisplayString(), BenchmarkHeaderLength.FormattedElapsedTime));
            line = line.Insert(BenchmarkHeaderPosition.AllocatedMemory.ToInt32(), _getValue(_allocatedMemory, BenchmarkHeaderLength.AllocatedMemory));
            line = line.Insert(BenchmarkHeaderPosition.Exception.ToInt32(), _getValue(_exception, BenchmarkHeaderLength.Exception));
            return line;
        }

        private string _getValue(object value, BenchmarkHeaderLength headerLength)
        {
            var length = headerLength.ToInt32();
            var convertedValue = value.ToSafeString();

            if (convertedValue.Length == 0 && headerLength == BenchmarkHeaderLength.TestNo)
            {
                convertedValue = "1";
            }

            if (convertedValue.Length > length)
            {
                var tempValue = convertedValue;
                convertedValue = tempValue.Substring(0, length) + "...";
            }

            return convertedValue;
        }
    }
}
