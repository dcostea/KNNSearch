using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KNNSearch
{
    class Euclidian
    {
        static IEnumerable<(Face, double)> LocateNeighbours(SortedSet<Face> sets, Face test)
        {
            foreach (var set in sets)
            {
                var distance = Distance.Euclidean(set.Metrics, test.Metrics);
                if (distance < 15)
                {
                    Console.WriteLine($"distance: {distance} metrics: {set.Metrics}");
                    yield return (set, distance);
                }
            }
        }

        static Face GetNeighbour(SortedSet<Face> sets, Face test)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            test.Metrics[0] += 0.1;

            if (sets.TryGetValue(test, out Face actual))
            {
                stopwatch.Stop();
                Console.WriteLine($"{actual.Metrics} ticks elapsed: {stopwatch.ElapsedTicks} ms elapsed: {stopwatch.ElapsedMilliseconds}");
                return actual;
            }
            else
            {
                stopwatch.Stop();
                return new Face();
            }
        }
    }
}
