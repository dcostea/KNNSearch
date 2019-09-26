using Accord.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static KNNSearch.ConsoleHelper;

namespace KNNSearch
{
    class KNN
    {
        const int DIMENSION = 128;
        const int RANGE = 1_000_000;

        private static Stopwatch sw = new Stopwatch();

        public static double[][] Points { get; set; } = new double[RANGE][];
        public static KDTree<Face> Tree { get; set; }
        public static List<Face> Faces { get; set; }

        public static double[] NewRandomPoint()
        {
            var random = new Random();
            var point = new double[DIMENSION];
            for (int j = 0; j < DIMENSION; j++)
            {
                point[j] = random.NextDouble();
            }

            return point;
        }

        public static Face AddPointToIndex(double[] point)
        {
            WriteLineColored($"Adding new point to the index...", ConsoleColor.White);

            sw.Start();
            var face = new Face { Guid = Guid.NewGuid(), Url = $"img{Faces.Count}.png" };
            Tree.Add(point, face);
            Faces.Add(face);
            sw.Stop();
            WriteLineColored($"Point {face.Url} {face.Guid} added to the index: {sw.ElapsedMilliseconds} ms", ConsoleColor.Yellow);

            return face;
        }

        public static void GenerateRandomPoints()
        {
            WriteLineColored($"Generating {RANGE} points with {DIMENSION} dimensions...", ConsoleColor.White);

            var random = new Random();
            for (int i = 0; i < RANGE; i++)
            {
                var row = new double[DIMENSION];
                for (int j = 0; j < DIMENSION; j++)
                {
                    row[j] = random.NextDouble();
                }
                Points[i] = row;
            }
        }

        public static void BuildIndex()
        {
            Faces = new List<Face>(RANGE);
            for (int i = 0; i < RANGE; i++)
            {
                Faces.Add(new Face { Guid = Guid.NewGuid(), Url = $"img{i}.png" });
            }

            sw.Start();
            Tree = KDTree.FromData(Points, Faces.ToArray(), false);
            sw.Stop();
            WriteLineColored($"Time to build the index: {sw.ElapsedMilliseconds} ms", ConsoleColor.Yellow);
        }

        public static IOrderedEnumerable<NodeDistance<KDTreeNode<Face>>> GetNeighbors(double[] query)
        {
            sw.Restart();
            var neighbours = Tree.Nearest(query, neighbors: 10);
            sw.Stop();
            WriteLineColored($"Time to find the first 10 neighbours: {sw.ElapsedMilliseconds} ms", ConsoleColor.Yellow);

            return neighbours.OrderBy(n => n.Distance);
        }
    }
}
