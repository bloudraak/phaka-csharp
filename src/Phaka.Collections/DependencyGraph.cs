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
using System.Collections.Generic;
using System.Linq;

namespace Phaka.Collections
{
    public class DependencyGraph<T> : IDependencyGraph<T>
    {
        public DependencyGraph()
        {
            AdjacencyList = new ConcurrentDictionary<T, Tuple<ISet<T>, ISet<T>>>();
        }

        private ConcurrentDictionary<T, Tuple<ISet<T>, ISet<T>>> AdjacencyList { get; }

        public void AddDependency(T antecedent, T dependent)
        {
            Add(antecedent);
            Add(dependent);

            AdjacencyList[antecedent].Item1.Add(dependent);
            AdjacencyList[dependent].Item2.Add(antecedent);
        }

        public void Add(T item)
        {
            AdjacencyList.GetOrAdd(item, new Tuple<ISet<T>, ISet<T>>(new HashSet<T>(), new HashSet<T>()));
        }

        public int Count => AdjacencyList.Count;

        public IEnumerable<T> GetDirectDependents(T item)
        {
            Tuple<ISet<T>, ISet<T>> tuple;
            if (AdjacencyList.TryGetValue(item, out tuple))
                return tuple.Item1;

            throw new InvalidOperationException($"The node `{item}` doesn't exist");
        }

        public IEnumerable<T> GetAntecedents(T item)
        {
            Tuple<ISet<T>, ISet<T>> tuple;
            if (AdjacencyList.TryGetValue(item, out tuple))
                return tuple.Item2;

            throw new InvalidOperationException($"The node `{item}` doesn't exist");
        }

        public IEnumerable<T> GetRootNodes()
        {
            return AdjacencyList.Where(p => p.Value.Item2.Count == 0).Select(p => p.Key);
        }

        public IEnumerable<T> GetLeafNodes()
        {
            return AdjacencyList.Where(p => p.Value.Item1.Count == 0).Select(p => p.Key);
        }

        public IEnumerable<T> GetNodes()
        {
            return AdjacencyList.Keys;
        }
    }
}