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
using System.Collections;
using NUnit.Framework;
using Phaka.Collections;
using Phaka.Scheduling.Abstractions;

namespace Phaka.Scheduling.UnitTests
{
    public class ActivitySchedulerTestCases
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return TestCase1();
                yield return TestCase2();
            }
        }

        private static TestCaseData TestCase1()
        {
            var t = MakeActivities(5);

            var graph = new DependencyGraph<IActivity>();
            graph.AddDependency(t[3], t[4]);
            graph.AddDependency(t[2], t[3]);
            graph.AddDependency(t[1], t[2]);
            graph.AddDependency(t[0], t[1]);

            var expected = new[] {t[0], t[1], t[2], t[3], t[4] };

            var testCaseData = new TestCaseData(graph);
            testCaseData.SetDescription("Serial Nodes");
            testCaseData.SetName("TestCase1");
            return testCaseData.Returns(expected);
        }

        private static IActivity[] MakeActivities(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var t = new IActivity[count];
            for (var i = 1; i <= t.Length; i++)
            {
                var key = $"t{i:D5}";
                t[i-1] = new Activity(key, token => Console.Out.WriteLineAsync(key));
            }
            return t;
        }

        private static TestCaseData TestCase2()
        {
            var t = MakeActivities(5);

            var graph = new DependencyGraph<IActivity>();
            graph.AddDependency(t[3], t[4]);
            graph.AddDependency(t[2], t[4]);
            graph.AddDependency(t[1], t[2]);
            graph.AddDependency(t[0], t[1]);
            graph.AddDependency(t[0], t[3]);

            var expected = new[] {t[0], t[1], t[3], t[2], t[4]};

            var testCaseData = new TestCaseData(graph);
            testCaseData.SetName("TestCase2");
            testCaseData.SetDescription("Interdependent Nodes");
            return testCaseData.Returns(expected);
        }
    }
}