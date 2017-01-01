using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Phaka.Collections;

namespace Phaka.Scheduling.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class SchedulerTestCases
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return TestCase1();
                yield return TestCase2();
                yield return TestCase3();
            }
        }

        private static TestCaseData TestCase1()
        {
            var graph = new DependencyGraph<string>();
            graph.AddDependency("t4", "t5");
            graph.AddDependency("t3", "t4");
            graph.AddDependency("t2", "t3");
            graph.AddDependency("t1", "t2");

            var expected = new[] {"t1", "t2", "t3", "t4", "t5"};

            var testCaseData = new TestCaseData(graph);
            testCaseData.SetDescription("Serial Nodes");
            testCaseData.SetName("TestCase1");
            return testCaseData.Returns(expected);
        }

        private static TestCaseData TestCase2()
        {
            var graph = new DependencyGraph<string>();

            graph.AddDependency("t4", "t5");
            graph.AddDependency("t3", "t5");
            graph.AddDependency("t1", "t4");
            graph.AddDependency("t2", "t3");
            graph.AddDependency("t1", "t2");

            var expected = new[] {"t1", "t2", "t4", "t3", "t5"};

            var testCaseData = new TestCaseData(graph);
            testCaseData.SetName("TestCase2");
            testCaseData.SetDescription("Interdependent Nodes");
            return testCaseData.Returns(expected);
        }


        private static TestCaseData TestCase3()
        {
            var size = 1000;

            var graph = new DependencyGraph<string>();
            var items = new List<string>();
            string previous = null;
            for (var i = 1; i < size; i++)
            {
                var name = $"t{i:D5}";
                items.Add(name);
                if (previous != null)
                    graph.AddDependency(previous, name);
                else
                    graph.Add(name);
                previous = name;
            }

            var expected = items.OrderBy(v => v).ToArray();

            var testCaseData = new TestCaseData(graph);
            testCaseData.SetName("TestCase3");
            testCaseData.SetDescription("Large Discrete Vertexes");
            return testCaseData.Returns(expected);
        }
    }
}