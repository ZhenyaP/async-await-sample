using System;
using System.Collections.Generic;
using System.Linq;
using AsyncAwaitSample.ConsoleApp.Entities;

namespace AsyncAwaitSample.ConsoleApp
{
    class Program
    {
        private static readonly ConcurrencyPerfTestUtility ConcurrencyPerfTestUtility = new ConcurrencyPerfTestUtility();

        static void Main(string[] args)
        {
            RunTest("await with captured context",
                ConcurrencyPerfTestUtility.ExecuteAwaitTestWithCapturedContext);
            RunTest("await without captured context",
                ConcurrencyPerfTestUtility.ExecuteAwaitTestWithoutCapturedContext);
            RunTest("Parallel.For",
                ConcurrencyPerfTestUtility.ExecuteParallelTest);
            RunTest("Task.Run",
                ConcurrencyPerfTestUtility.ExecuteTaskTest);

            Console.ReadKey(true);
        }

        static void RunTest(string testName, Func<TestResult> testMethod)
        {
            const int testLoops = 10;

            var elapsedTime = new List<long>();
            var threadCount = new List<int>();

            for (var i = 0; i < testLoops; i++)
            {
                var result = testMethod();
                elapsedTime.Add(result.ElapsedMilliseconds);
                threadCount.Add(result.ThreadsCount);
            }

            Console.WriteLine("{0} average time: {1} ms", testName, elapsedTime.Average());
            Console.WriteLine("Thread count: min {0} – max {1}",
                threadCount.Min(),
                threadCount.Max());
        }
    }
}
