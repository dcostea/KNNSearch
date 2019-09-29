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
        private readonly Stopwatch sw = new Stopwatch();

        private int _dimension { get; set; }
        private int _range { get; set; }
        private int _neighbors { get; set; }
        private Random _random { get; set; }

        public double[][] Points { get; set; }
        public KDTree<Face> Tree { get; set; }
        public List<Face> Faces { get; set; }

        public KNN(int range, int dimension, int neighbors)
        {
            _range = range;
            _dimension = dimension;
            _neighbors = neighbors;
            Points = new double[_range][];
            _random = new Random();

            Points = NewRandomPoints(_range);
            BuildIndex();
        }

        public double[] ZeroPoint()
        {
            var point = new double[_dimension];
            for (int j = 0; j < _dimension; j++)
            {
                point[j] = 0;
            }

            return point;
        }

        public double[] NewRandomPoint()
        {
            var point = new double[_dimension];
            for (int j = 0; j < _dimension; j++)
            {
                point[j] = _random.NextDouble();
            }

            return point;
        }

        public double[][] NewRandomPoints(int range)
        {
            sw.Restart();
            var points = new double[range][];
            for (int i = 0; i < range; i++)
            {
                points[i] = NewRandomPoint();
            }
            sw.Stop();
            WriteLineColored($"\n{range} random points generated [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);

            return points;
        }

        private void BuildIndex()
        {
            sw.Restart();
            Faces = new List<Face>(_range);
            for (int i = 0; i < _range; i++)
            {
                Faces.Add(new Face { Guid = Guid.NewGuid(), Url = $"img{i}.png" });
            }
            sw.Stop();
            WriteLineColored($"{_range} faces generated [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);

            sw.Restart();
            Tree = KDTree.FromData(Points, Faces.ToArray(), false);
            sw.Stop();
            WriteLineColored($"Index built [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
        }

        public void AddPointsToIndex(double[][] points)
        {
            sw.Restart();
            for (int i = 0; i < points.Length; i++)
            {
                Faces.Add(new Face { Guid = Guid.NewGuid(), Url = $"img{Faces.Count}.png" });
            }
            sw.Stop();
            WriteLineColored($"{points.Length} faces generated [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);

            sw.Restart();
            for (int i = 0; i < points.Length; i++)
            {
                Tree.Add(points[i], Faces[_range + i]);
            }
            sw.Stop();
            WriteLineColored($"Index added with {points.Length} points [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
        }

        public IOrderedEnumerable<NodeDistance<KDTreeNode<Face>>> GetNeighbors(double[] query, DistanceFormula distanceFormula)
        {
            /* https://scikit-learn.org/stable/modules/generated/sklearn.neighbors.DistanceMetric.html */

            sw.Restart();
            switch (distanceFormula)
            {
                case DistanceFormula.Manhattan:
                    Tree.Distance = new Accord.Math.Distances.Manhattan();
                    break;

                case DistanceFormula.Euclidean:
                    Tree.Distance = new Accord.Math.Distances.Euclidean();
                    break;

                case DistanceFormula.Chebyshev:
                    Tree.Distance = new Accord.Math.Distances.Chebyshev();
                    break;
            }
            var neighbours = Tree.Nearest(query, neighbors: _neighbors);
            sw.Stop();
            WriteLineColored($"\nThe first {_neighbors} neighbours for {Tree.Count} nodes [{sw.ElapsedMilliseconds} ms]:", ConsoleColor.Cyan);

            return neighbours.OrderBy(n => n.Distance);
        }
    }
}
