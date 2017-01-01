using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Phaka.Collections;
using Phaka.Scheduling.Abstractions;

namespace Phaka.Scheduling.UnitTests
{
    [TestFixture]
    public class ActivitySchedulerTests
    {
        [Test]
        [TestCaseSource(typeof(ActivitySchedulerTestCases), nameof(ActivitySchedulerTestCases.TestCases))]
        public async Task<ICollection<IActivity>> ScheduleAsync(IDependencyGraph<IActivity> graph) 
        {
            // Arrange
            var target = new Scheduler();
            var list = new BlockingCollection<IActivity>();
            var random = new Random();

            // Act
            await target.ScheduleAsync(graph, async t =>
            {
                await Console.Out.WriteLineAsync(t.Key);
                list.Add(t);
                await Task.Delay(random.Next(2, 50));
            });
            return list.ToArray();
        }
    }
}