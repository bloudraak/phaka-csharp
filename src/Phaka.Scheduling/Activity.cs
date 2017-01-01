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
using System.Threading;
using System.Threading.Tasks;

namespace Phaka.Scheduling
{
    public class Activity<T1, T2, T3> : IActivity
    {
        public Activity(Func<T1, T2, T3, CancellationToken, Task> func, string key, T1 item1, T2 item2, T3 item3)
        {
            Func = func;
            Key = key;
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public Func<T1, T2, T3, CancellationToken, Task> Func { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public string Key { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Func(Item1, Item2, Item3, cancellationToken);
        }

        public T1 Item1 { get; }
    }

    public class Activity<T1, T2, T3, T4> : IActivity
    {
        public Activity(Func<T1, T2, T3, T4, CancellationToken, Task> func, string key, T1 item1, T2 item2, T3 item3,
            T4 item4)
        {
            Func = func;
            Key = key;
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public Func<T1, T2, T3, T4, CancellationToken, Task> Func { get; }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public string Key { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Func(Item1, Item2, Item3, Item4, cancellationToken);
        }
    }

    public class Activity<T1, T2> : IActivity
    {
        public Activity(Func<T1, T2, CancellationToken, Task> func, string key, T1 item1, T2 item2)
        {
            Func = func;
            Key = key;
            Item1 = item1;
            Item2 = item2;
        }

        public Func<T1, T2, CancellationToken, Task> Func { get; }

        public T2 Item2 { get; }

        public string Key { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Func(Item1, Item2, cancellationToken);
        }

        public T1 Item1 { get; }
    }


    public class Activity<T1> : IActivity
    {
        public Activity(Func<T1, CancellationToken, Task> func, string key, T1 item1)
        {
            Func = func;
            Key = key;
            Item1 = item1;
        }

        public Func<T1, CancellationToken, Task> Func { get; }

        public string Key { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Func(Item1, cancellationToken);
        }

        public T1 Item1 { get; }
    }

    public class Activity : IActivity
    {
        public Activity(string key, Func<CancellationToken, Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            Key = key;
            Func = func;
        }

        public Func<CancellationToken, Task> Func { get; }

        public string Key { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Func(cancellationToken);
        }
    }
}