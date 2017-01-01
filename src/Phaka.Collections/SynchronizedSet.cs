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
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Phaka.Collections
{
    public class SynchronizedSet<T> : ISet<T>
    {
        private readonly ReaderWriterLock _lock;
        private readonly ISet<T> _set;
        private readonly TimeSpan _timeout;

        public SynchronizedSet()
        {
            _set = new HashSet<T>();
            _lock = new ReaderWriterLock();
            _timeout = TimeSpan.FromSeconds(1);
        }

        public SynchronizedSet(IEqualityComparer<T> comparer)
        {
            _set = new HashSet<T>(comparer);
            _lock = new ReaderWriterLock();
            _timeout = TimeSpan.FromSeconds(1);
        }

        public SynchronizedSet(IEnumerable<T> collection)
        {
            _set = new HashSet<T>(collection);
            _lock = new ReaderWriterLock();
            _timeout = TimeSpan.FromSeconds(1);
        }

        public SynchronizedSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _set = new HashSet<T>(collection, comparer);
            _lock = new ReaderWriterLock();
            _timeout = TimeSpan.FromSeconds(1);
        }

        public IEnumerator<T> GetEnumerator()
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                var array = new T[_set.Count];
                if (array.Length > 0)
                    _set.CopyTo(array, 0);
                return ((IEnumerable<T>) array).GetEnumerator();
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                _set.Add(item);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                _set.UnionWith(other);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                _set.IntersectWith(other);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                _set.ExceptWith(other);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                _set.SymmetricExceptWith(other);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.IsSubsetOf(other);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.IsSupersetOf(other);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.IsProperSupersetOf(other);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.IsProperSubsetOf(other);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.Overlaps(other);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.SetEquals(other);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        bool ISet<T>.Add(T item)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                return _set.Add(item);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public void Clear()
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                _set.Clear();
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public bool Contains(T item)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.Contains(item);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                _set.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        public bool Remove(T item)
        {
            _lock.AcquireWriterLock(_timeout);
            try
            {
                return _set.Remove(item);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        public int Count
        {
            get
            {
                _lock.AcquireReaderLock(_timeout);
                try
                {
                    return _set.Count;
                }
                finally
                {
                    _lock.ReleaseReaderLock();
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                _lock.AcquireReaderLock(_timeout);
                try
                {
                    return _set.IsReadOnly;
                }
                finally
                {
                    _lock.ReleaseReaderLock();
                }
            }
        }

        public T Find(Func<T, bool> item)
        {
            _lock.AcquireReaderLock(_timeout);
            try
            {
                return _set.FirstOrDefault(item);
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }
    }
}