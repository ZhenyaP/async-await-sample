using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitSample.ConsoleApp.Entities;

namespace AsyncAwaitSample.ConsoleApp
{
    public class ConcurrencyPerfTestUtility
    {
        private const int _maxTasks = 100;
        private const int _sleepMs = 100;
        private const int _delayMs = 100;
        private readonly object _lock = new object();
        private readonly ConcurrentBag<int> _loggedThreads = new ConcurrentBag<int>();

        private TestResult GetTestResult(Stopwatch sw)
        {
            return new TestResult
            {
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                ThreadsCount = this._loggedThreads.Distinct().Count()
            };
        }

        public TestResult ExecuteParallelTest()
        {
            this._loggedThreads.Clear();

            var sw = new Stopwatch();
            sw.Restart();

            Parallel.For(0, _maxTasks, this.LongRunningTask);

            sw.Stop();

            return GetTestResult(sw);
        }

        public TestResult ExecuteTaskTest()
        {
            this._loggedThreads.Clear();

            var tasks = new List<Task>();

            var sw = new Stopwatch();
            sw.Restart();

            for (var i = 0; i < _maxTasks; i++)
            {
                var closure = i;
                tasks.Add(Task.Run(() => this.LongRunningTask(closure)));
            }
            Task.WaitAll(tasks.ToArray());

            sw.Stop();

            return GetTestResult(sw);
        }

        public TestResult ExecuteAwaitTestWithCapturedContext()
        {
            return ExecuteAwaitTest(true);
        }

        public TestResult ExecuteAwaitTestWithoutCapturedContext()
        {
            return ExecuteAwaitTest(false);
        }

        private TestResult ExecuteAwaitTest(bool continueOnCapturedContext)
        {
            this._loggedThreads.Clear();

            var tasks = new List<Task>();

            var sw = new Stopwatch();
            sw.Restart();

            for (var i = 0; i < _maxTasks; i++)
                tasks.Add(this.LongRunningAsync(continueOnCapturedContext));
            Task.WaitAll(tasks.ToArray());

            sw.Stop();

            return GetTestResult(sw);
        }

        private void LongRunningTask(int i)
        {
            this.LogCurrentThread();
            Thread.Sleep(_sleepMs);
        }

        private async Task LongRunningAsync(bool continueOnCapturedContext)
        {
            this.LogCurrentThread();
            await Task.Delay(_delayMs)
                .ConfigureAwait(continueOnCapturedContext);
        }

        private void LogCurrentThread()
        {
            this._loggedThreads.Add(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
