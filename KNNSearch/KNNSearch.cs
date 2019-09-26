using Accord.Collections;
using System;
using System.Diagnostics;
using System.Linq;
using static KNNSearch.ConsoleHelper;

namespace KNNSearch
{
    class KNN
    {
        public static double[] GeneratePoint(int dimension)
        {
            return GeneratePoints(dimension, 1).First();
        }

        public static KDTree<double> AddPoint(KDTree<double> tree, double[] point)
        {
            tree.Add(point, 10);
            return tree;
        }

        public static double[][] GeneratePoints(int dimension, int range)
        {
            double[][] points = new double[range][];
            var random = new Random();
            for (int i = 0; i < range; i++)
            {
                var row = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    row[j] = random.NextDouble();
                }
                points[i] = row;
            }

            return points;
        }

        public static IOrderedEnumerable<NodeDistance<KDTreeNode<double>>> GetNeighbors(double[][] points, double[] query, int dimension, int range)
        {
            var sw = new Stopwatch();

            var values = new double[range];
            for (int i = 0; i < range; i++)
            {
                values[i] = i;
            }

            sw.Start();
            var tree = KDTree.FromData<double>(points, values, false);
            sw.Stop();
            WriteLineColored($"Time to build the index: {sw.ElapsedMilliseconds} ms, {sw.ElapsedTicks} ticks", ConsoleColor.Green);

            //var tree = KDTree.FromData<double>(points, new Accord.Math.Distances.Manhattan(), true);
            //var tree = KDTree.FromData<double>(points, new Accord.Math.Distances.Euclidean(), true);

            sw.Restart();
            var n1 = tree.Nearest(query, neighbors: 10);
            sw.Stop();
            WriteLineColored($"Time to find the neighbours: {sw.ElapsedMilliseconds} ms, {sw.ElapsedTicks} ticks", ConsoleColor.Green);

            Console.WriteLine("---------------------------");
            foreach (var nn1 in n1.OrderBy(n => n.Distance))
            {
                Console.WriteLine($"{nn1.Distance,4} {nn1.Node.Value}");
            }

            var newPoint = GeneratePoint(dimension);

            sw.Restart();
            AddPoint(tree, newPoint);
            sw.Stop();
            WriteLineColored($"Time to add a point to the index: {sw.ElapsedMilliseconds} ms, {sw.ElapsedTicks} ticks", ConsoleColor.Green);

            ////var ts = tree.Traverse(TreeTraversal.BreadthFirst<KDTreeNode<double>>);

            sw.Restart();
            var n2 = tree.Nearest(query, neighbors: 11);
            sw.Stop();
            WriteLineColored($"Time to find the neighbours: {sw.ElapsedMilliseconds} ms, {sw.ElapsedTicks} ticks", ConsoleColor.Green);

            Console.WriteLine("---------------------------");
            foreach (var nn2 in n2.OrderBy(n => n.Distance))
            {
                Console.WriteLine($"{nn2.Distance, 4} {nn2.Node.Value}");
            }

            Console.WriteLine("===============================");

            return n2.OrderBy(n => n.Distance);
        }
    }
}
