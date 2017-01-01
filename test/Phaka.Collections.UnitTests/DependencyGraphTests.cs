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

using System.Linq;
using NUnit.Framework;

namespace Phaka.Collections.UnitTests
{
    [TestFixture]
    public class DependencyGraphTests
    {
        [Test]
        public void AddDependency()
        {
            // Arrange
            IDependencyGraph<string> target = new DependencyGraph<string>();

            // Act
            target.AddDependency("t1", "t2");

            // Assert
            Assert.AreEqual(2, target.Count);
            Assert.Contains("t2", target.GetDirectDependents("t1").ToArray());
            Assert.Contains("t1", target.GetAntecedents("t2").ToArray());
            Assert.Contains("t1", target.GetRootNodes().ToArray());
            Assert.Contains("t2", target.GetLeafNodes().ToArray());
        }

        [Test]
        public void GetSubgraph_Simple()
        {
            // Arrange
            IDependencyGraph<string> target = new DependencyGraph<string>();
            target.AddDependency("t1", "t2");
            target.AddDependency("t2", "t3");
            target.AddDependency("t3", "t4");
            target.AddDependency("t4", "t5");

            // Act
            var expected = target.GetSubgraph(s => s == "t3");

            // Assert
            var nodes = expected.GetNodes().ToList();
            Assert.AreEqual(3, expected.Count);
            Assert.Contains("t1", nodes);
            Assert.Contains("t2", nodes);
            Assert.Contains("t3", nodes);
            Assert.IsFalse(nodes.Contains("t4"));
            Assert.IsFalse(nodes.Contains("t5"));
        }

        [Test]
        public void GetSubgraph_TestCase2()
        {
            // Arrange
            IDependencyGraph<string> target = new DependencyGraph<string>();
            target.AddDependency("t1", "t2");
            target.AddDependency("t2", "t3");
            target.AddDependency("t1", "t4");
            target.AddDependency("t4", "t5");
            target.AddDependency("t3", "t5");

            // Act
            var expected = target.GetSubgraph(s => s == "t4");

            // Assert
            var nodes = expected.GetNodes().ToList();
            Assert.AreEqual(2, expected.Count);
            Assert.Contains("t1", nodes);
            Assert.Contains("t4", nodes);
            Assert.IsFalse(nodes.Contains("t2"));
            Assert.IsFalse(nodes.Contains("t3"));
            Assert.IsFalse(nodes.Contains("t5"));
        }


        [Test]
        public void GetSubgraph_TestCase3()
        {
            // Arrange
            IDependencyGraph<string> target = new DependencyGraph<string>();
            target.AddDependency("t1", "t2");
            target.AddDependency("t2", "t3");
            target.AddDependency("t1", "t4");
            target.AddDependency("t4", "t5");
            target.AddDependency("t3", "t5");

            target.AddDependency("s1", "s2");
            target.AddDependency("s2", "s3");
            target.AddDependency("s1", "s4");
            target.AddDependency("s4", "s5");
            target.AddDependency("s3", "s5");


            // Act
            var expected = target.GetSubgraph(s => s == "t4" || s == "s4");

            // Assert
            var nodes = expected.GetNodes().ToList();
            Assert.AreEqual(4, expected.Count);
            Assert.Contains("t1", nodes);
            Assert.Contains("t4", nodes);
            Assert.IsFalse(nodes.Contains("t2"));
            Assert.IsFalse(nodes.Contains("t3"));
            Assert.IsFalse(nodes.Contains("t5"));

            Assert.Contains("s1", nodes);
            Assert.Contains("s4", nodes);
            Assert.IsFalse(nodes.Contains("s2"));
            Assert.IsFalse(nodes.Contains("s3"));
            Assert.IsFalse(nodes.Contains("s5"));
        }
    }
}