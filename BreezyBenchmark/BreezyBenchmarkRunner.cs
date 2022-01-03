using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BreezyBenchmark.Attributes;

namespace BreezyBenchmark
{
    public static class BreezyBenchmarkRunner
    {
        private static BenchmarkContainer _instance;

        /// <summary>
        /// This is best used for comparison testing. Testing different code implementations for the same use/outcome
        /// Example: Looping through a list. 1 test could be using a for loop, and another test could use a foreach loop
        /// </summary>
        public static BenchmarkContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BenchmarkContainer();
                }

                return _instance;
            }
        }

        private static void Dispose()
        {
            _instance = null;
        }

        public static void Run(this BenchmarkContainer benchmarkContainer, bool writeToFile = false)
        {
            foreach (var benchmark in benchmarkContainer.Benchmarks)
            {
                if (benchmark.RunOnNewThread)
                {
                    RunCodeAsync(benchmark).ConfigureAwait(true);
                }
                else
                {
                    benchmark.Start();

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.GC.Collect();

                    benchmark.Execute();

                    benchmark.Stop();
                    benchmark.SetAllocatedMemory(CreateGetAllocatedBytesForCurrentThreadDelegate().Invoke());
                }
            }

            var summary = benchmarkContainer.Summary;

            // Write to the Output console in VS
            System.Diagnostics.Debug.WriteLine(summary);

            if (writeToFile)
            {
                WriteToFile(summary);
            }

            Dispose();
        }

        /// <summary>
        /// This implementation is for testing code blocks
        /// Example: 
        /// Epc.Iinvision.BenchmarkRunner.Run("Item Retrieval Test", () =>
        /// {
        ///     var p = this.Form.Context.Projects.Where(x => x.Items.Count > 0).Take(25).OrderByDescending(x => x.ID).ToList();
        ///     var items = p.SelectMany(x => x.Items).ToList();
        /// }, false);
        /// </summary>
        /// <param name="name">Name of the Test</param>
        /// <param name="numberOfTestsToRun"># of times to run the specified code</param>
        /// <param name="codeToRun">A code block or method call</param>
        /// <param name="writeToFile">If True, then writes results to file and opens</param>
        public static void Run(string name, int numberOfTestsToRun, Action action, bool writeToFile = false, bool runOnNewThread = true)
        {
            var benchmarkResult = new BenchmarkContainer(name);

            for (int i = 0; i < numberOfTestsToRun; i++)
            {
                var benchmark = new Benchmark(action, i + 1);

                try
                {
                    if (runOnNewThread)
                    {
                        RunCodeAsync(benchmark).ConfigureAwait(true);
                    }
                    else
                    {
                        benchmark.Start();

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();

                        benchmark.Execute();

                        benchmark.Stop();
                        benchmark.SetAllocatedMemory(CreateGetAllocatedBytesForCurrentThreadDelegate().Invoke());
                    }
                }
                catch (Exception ex)
                {
                    benchmark.SetException(ex.Message);
                }

                benchmarkResult.Add(benchmark);
            }

            var summary = benchmarkResult.Summary;

            // Write to the Output console in VS
            System.Diagnostics.Debug.WriteLine(summary);

            if (writeToFile)
            {
                WriteToFile(summary);
            }
        }

        private static async Task RunCodeAsync(Benchmark benchmark)
        {
            await RunInThread(benchmark);
        }

        private static Task RunInThread(Benchmark benchmark)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            var allocatedBytesDelegate = CreateGetAllocatedBytesForCurrentThreadDelegate();

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                try
                {
                    benchmark.Start();

                    benchmark.Execute();

                    benchmark.Stop();
                    benchmark.SetAllocatedMemory(allocatedBytesDelegate.Invoke());
                    taskCompletionSource.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    benchmark.SetException(ex.Message);
                    taskCompletionSource.TrySetException(ex);
                }
            });
            thread.Start();

            taskCompletionSource.Task.Wait();
            thread.Interrupt();

            return taskCompletionSource.Task;
        }

        private static Func<long> CreateGetAllocatedBytesForCurrentThreadDelegate()
        {
            // this method is not a part of .NET Standard so we need to use reflection
            var method = typeof(GC).GetTypeInfo().GetMethod("GetAllocatedBytesForCurrentThread", BindingFlags.Public | BindingFlags.Static);

            // we create delegate to avoid boxing, IMPORTANT!
            return method != null ? (Func<long>)method.CreateDelegate(typeof(Func<long>)) : null;
        }

        /// <summary>
        /// This implementation is for testing code blocks
        /// Example: 
        /// Epc.Iinvision.BenchmarkRunner.Run("Item Retrieval Test", () =>
        /// {
        ///     var p = this.Form.Context.Projects.Where(x => x.Items.Count > 0).Take(25).OrderByDescending(x => x.ID).ToList();
        ///     var items = p.SelectMany(x => x.Items).ToList();
        /// }, false);
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="writeToFile"></param>
        public static void Run(string name, Action action, bool writeToFile = false, bool runOnNewThread = true)
        {
            BreezyBenchmarkRunner.Run(name, 1, action, writeToFile, runOnNewThread);
        }

        /// <summary>
        /// This test will create a web request and call the specified URL. You can specify the # of tests to run
        /// Example: Epc.Iinvision.BenchmarkRunner.Run("Testing Web Request Benchmark", 2, $"https://localhost:44345/StockTransfer/Exists?authToken={token}&stockTransferID={97042}", false);
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfTestsToRun"></param>
        /// <param name="url"></param>
        /// <param name="writeToFile"></param>
        public static void Run(string name, int numberOfTestsToRun, string url, bool writeToFile = false, bool runOnNewThread = true)
        {
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
            BreezyBenchmarkRunner.Run(name, numberOfTestsToRun, () => webRequest.GetResponse(), writeToFile, runOnNewThread);
        }

        /// <summary>
        /// This test will create a web request and call the specified URL.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="writeToFile"></param>
        public static void Run(string name, string url, bool writeToFile = false, bool runOnNewThread = true)
        {
            BreezyBenchmarkRunner.Run(name, 1, url, writeToFile, runOnNewThread);
        }

        /// <summary>
        /// This implementation can be called on an entire class
        /// User can specify methods by including the EpcBenchmark attribute on the method
        /// Example: BenchmarkRunner.Run<Epc.Iinvision.BatchAction>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyRunTaggedMethods"></param>
        /// <param name="writeToFile"></param>
        public static void Run<T>(bool onlyRunTaggedMethods = true, bool writeToFile = false)
            where T : class
        {
            var classType = typeof(T);
            var classObj = Activator.CreateInstance(classType);

            // Get all public methods first
            var methods = classType.GetMethods();

            // Users can specify methods they want to run using the EpcBenchmark attribute. If onlyRunTaggedMethods is set to true we will only run those methods
            if (onlyRunTaggedMethods)
            {
                methods = methods.Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(BreezyBenchmarkAttribute))).ToArray();
            }

            if (methods.Any())
            {
                var benchmarkResult = new BenchmarkContainer($"Testing methods in {classType.Name}");
                foreach (var method in methods)
                {
                    BreezyBenchmarkAttribute benchmarkAttribute = (BreezyBenchmarkAttribute)System.Attribute.GetCustomAttribute(method, typeof(BreezyBenchmarkAttribute));
                    bool runOnNewThread = benchmarkAttribute != null ? benchmarkAttribute.RunOnNewThread : true;
                    BreezyBenchmarkParamsAttribute paramAttribute = (BreezyBenchmarkParamsAttribute)System.Attribute.GetCustomAttribute(method, typeof(BreezyBenchmarkParamsAttribute));
                    var paramValues = paramAttribute != null ? paramAttribute.Values : new object[0];

                    var benchmark = new Benchmark(() => { method.Invoke(classObj, paramValues); }, method.Name);

                    try
                    {
                        if (runOnNewThread)
                        {
                            RunCodeAsync(benchmark).ConfigureAwait(true);
                        }
                        else
                        {
                            benchmark.Start();

                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            System.GC.Collect();

                            benchmark.Execute();

                            benchmark.Stop();
                            benchmark.SetAllocatedMemory(CreateGetAllocatedBytesForCurrentThreadDelegate().Invoke());
                        }
                    }
                    catch (Exception ex)
                    {
                        benchmark.SetException(ex.Message);
                    }

                    benchmarkResult.Add(benchmark);
                }

                var summary = benchmarkResult.Summary;

                // Write to the Output console in VS
                System.Diagnostics.Debug.WriteLine(summary);

                if (writeToFile)
                {
                    WriteToFile(summary);
                }
            }
        }

        /// <summary>
        /// Writes the performance summary to a txt file and opens it
        /// </summary>
        /// <param name="summary"></param>
        private static void WriteToFile(string summary)
        {
            var path = System.IO.Path.GetTempPath();
            var filename = $"BenchmarkResults_{DateTime.UtcNow.ToFullDateTimeString()}.txt".Replace("/", "-").Replace(" ", "_").Replace(":", "-");
            var tempFilename = System.IO.Path.Combine(path, filename);

            System.IO.File.WriteAllText(tempFilename, summary);

            System.Diagnostics.Process.Start(tempFilename);
        }
    }
}
