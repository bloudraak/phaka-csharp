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
using System.Linq;
using System.Threading.Tasks;
using Phaka.Collections;
using Phaka.Scheduling.Abstractions;

namespace Phaka.Scheduling
{
    public class Scheduler : IScheduler
    {
        public async Task ScheduleAsync<T>(IDependencyGraph<T> graph, Func<T, Task> activity)
        {
            var mapping = new ConcurrentDictionary<T, ActivityStatus>();
            var running = new SynchronizedSet<Task>();

            do
            {
                var pendingNodes = graph.GetNodes().Where(arg => IsPending(arg, mapping)).ToArray();
                foreach (var node in pendingNodes)
                {
                    var canSchedule = true;
                    var antecedents = graph.GetAntecedents(node).ToArray();
                    foreach (var antecedent in antecedents)
                        if (!IsComplete(antecedent, mapping))
                        {
                            canSchedule = false;
                            break;
                        }

                    if (canSchedule)
                    {
                        mapping.AddOrUpdate(node, ActivityStatus.Running, (arg1, status) => ActivityStatus.Running);
                        var task = ExecuteAsync(node, mapping, activity);
                        running.Add(task);
                    }
                }

                await Task.WhenAny(running);
            } while (graph.GetNodes().Any(arg => IsPending(arg, mapping)));

            await Task.WhenAll(running);
        }

        private async Task ExecuteAsync<T>(T item, ConcurrentDictionary<T, ActivityStatus> mapping,
            Func<T, Task> activity)
        {
            await activity(item);
            mapping.AddOrUpdate(item, ActivityStatus.Completed, (arg1, status) => ActivityStatus.Completed);
        }

        private bool IsComplete<T>(T item, ConcurrentDictionary<T, ActivityStatus> mapping)
        {
            var status = mapping.GetOrAdd(item, ActivityStatus.Pending);
            return status == ActivityStatus.Completed;
        }

        private bool IsPending<T>(T item, ConcurrentDictionary<T, ActivityStatus> mapping)
        {
            var status = mapping.GetOrAdd(item, ActivityStatus.Pending);
            return status == ActivityStatus.Pending;
        }
        
        private enum ActivityStatus
        {
            Pending,
            Running,
            Completed
        }
    }
}