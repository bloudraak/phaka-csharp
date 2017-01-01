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
    }
}