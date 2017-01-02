# Phaka

## Synopsis

Phaka is a suite of libraries to facilitate the optimally schedule of activities during a deployment.

## Code Examples

### Scheduling Entire Deployment 

Lets assume that a deployment consists of five activities, t1, t2, t3, t4 and t5. These activities could be anything, from creating Azure, Amazon AWS or VMware resources to running scripts. 

|Activity|Time|Antecedent|
|:---|:---|:---|
| t1 | 10ms..100ms | |
| t2 | 10ms..100ms |t1 |
| t3 | 10ms..100ms | t2 |
| t4 | 100ms..200ms | t1 |
| t5 | 10ms..100ms | t4, t3 |

The following snippet shows how to schedule the activities.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Phaka.Collections;
using Phaka.Scheduling;
using Phaka.Scheduling.Abstractions;

namespace MyDeployment
{
    internal class Program
    {
        private static readonly Random Random = new Random();

        private static void Main(string[] args)
        {
            try
            {
                var program = new Program();
                program.Run().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        private async Task Run()
        {
            IActivity t1 = new Activity("t1", T1);
            IActivity t2 = new Activity("t2", T2);
            IActivity t3 = new Activity("t3", T3);
            IActivity t4 = new Activity("t4", T4);
            IActivity t5 = new Activity("t5", T5);

            IDependencyGraph<IActivity> graph = new DependencyGraph<IActivity>();
            graph.AddDependency(t1, t2);
            graph.AddDependency(t2, t3);
            graph.AddDependency(t3, t5);
            graph.AddDependency(t1, t4);
            graph.AddDependency(t4, t5);

            IActivityScheduler<IActivity> scheduler = new ActivityScheduler<IActivity>();
            await scheduler.ScheduleAsync(graph);
        }

        private static async Task T1(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("+t1");
            var delay = Random.Next(10, 100);
            await Task.Delay(delay, cancellationToken);
            await Console.Out.WriteLineAsync("-t1");
        }

        private static async Task T2(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("+t2");
            var delay = Random.Next(10, 100);
            await Task.Delay(delay, cancellationToken);
            await Console.Out.WriteLineAsync("-t2");
        }

        private static async Task T3(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("+t3");
            var delay = Random.Next(10, 100);
            await Task.Delay(delay, cancellationToken);
            await Console.Out.WriteLineAsync("-t3");
        }

        private static async Task T4(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("+t4");
            var delay = Random.Next(100, 300);
            await Task.Delay(delay, cancellationToken);
            await Console.Out.WriteLineAsync("-t4");
        }

        private static async Task T5(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("+t5");
            var delay = Random.Next(10, 100);
            await Task.Delay(delay, cancellationToken);
            await Console.Out.WriteLineAsync("-t5");
        }
    }
}
```

The output looks as follows. 

```
+t1
-t1
+t4
+t2
-t2
+t3
-t4
-t3
+t5
-t5
```

## Motivation

The project was created to facilitate complex deployments that are autonomous. 

## Installation

TODO

## API Reference

TODO

## Tests

Tests are implemented in NUnit

## Contribution

TODO

## Contributors

- Werner Strydom

    - Twitter: @bloudraak
    - Email: hello at wernerstrydom.com

## License

The MIT License(MIT)

Copyright(c) 2016 Werner Strydom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.