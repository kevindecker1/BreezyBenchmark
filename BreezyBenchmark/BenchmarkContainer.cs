using System;
using System.Collections.Generic;
using System.Text;

namespace BreezyBenchmark
{
    public sealed class BenchmarkContainer
    {
        private string _name { get; set; }
        private List<Benchmark> _benchmarks { get; set; }
        public List<Benchmark> Benchmarks
        {
            get
            {
                if (_benchmarks == null)
                {
                    return _benchmarks = new List<Benchmark>();
                }

                return _benchmarks;
            }
        }

        public string Summary
        {
            get
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("");

                if (_name.HasValue())
                {
                    sb.AppendLine($"{_name}");
                }

                sb.AppendLine("");
                sb.AppendLine(_generateHeaderLine());
                sb.AppendLine($"{new string('-', 106)}");

                foreach (var benchmark in _benchmarks)
                {
                    sb.AppendLine(benchmark.Summary);
                }

                return sb.ToString();
            }
        }

        public BenchmarkContainer(string name = null)
        {
            _name = name;
            _benchmarks = new List<Benchmark>();
        }

        public void Add(Benchmark benchmark)
        {
            if (_benchmarks == null)
            {
                _benchmarks = new List<Benchmark>();
            }

            _benchmarks.Add(benchmark);
        }

        public void Add(Action action, string name, bool runOnNewThread = true)
        {
            var benchmark = new Benchmark(action, name, runOnNewThread);
            this.Add(benchmark);
        }

        public void Add(string url, string name, bool runOnNewThread = true)
        {
            var benchmark = new Benchmark
            (
                () =>
                {
                    System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
                    webRequest.GetResponse();
                },
                name,
                runOnNewThread
            );

            this.Add(benchmark);
        }

        private string _generateHeaderLine()
        {
            return $"Method Name           | Test # | Elapsed Time      | Formatted Elapsed Time | Allocated Memory | Exception";
        }
    }
}
