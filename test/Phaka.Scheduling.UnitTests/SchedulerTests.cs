// The MIT License (MIT)
// 
// Copyright (c) 2016 Werner Strydom
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NUnit.Framework;
using Phaka.Collections;

namespace Phaka.Scheduling.UnitTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SchedulerTests
    {
        [Test]
        [TestCaseSource(typeof(SchedulerTestCases), nameof(SchedulerTestCases.TestCases))]
        public async Task<string[]> ScheduleAsync(IDependencyGraph<string> graph)
        {
            // Arrange
            var target = new Scheduler();
            var list = new BlockingCollection<string>();
            var random = new Random();

            // Act
            await target.ScheduleAsync(graph, async t =>
            {
                await Console.Out.WriteLineAsync(t);
                list.Add(t);

                if (t == "t4")
                    await Task.Delay(random.Next(50, 100));
                else
                    await Task.Delay(random.Next(2, 50));
            });

            return list.ToArray();
        }
    }
}