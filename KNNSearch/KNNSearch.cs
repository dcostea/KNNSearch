using Accord.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static KNNSearch.ConsoleHelper;

namespace KNNSearch
{
    class KNN
    {
        const string FilePath = "faces.csv";

        private readonly Stopwatch sw = new Stopwatch();

        private int _dimension { get; set; }
        private int _range { get; set; }
        private int _neighbors { get; set; }
        private Random _random { get; set; }

        public KDTree<Face> Tree { get; set; }
        public List<Face> Faces { get; set; }

        public KNN(int range, int dimension, int neighbors)
        {
            _range = range;
            _dimension = dimension;
            _neighbors = neighbors;
            _random = new Random();

            // uncomment next lines to generate new csv file, and comment the ReadCsv line
            GenerateFaces();
            //WriteCsv().Wait();
            //ReadCsv().Wait();

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

        public void GenerateFaces()
        {
            sw.Restart();
            Faces = new List<Face>(_range);
            for (int i = 0; i < _range; i++)
            {
                Faces.Add(new Face { Guid = Guid.NewGuid(), Url = $"img{i}.png", Metrics = NewRandomPoint() });
            }
            sw.Stop();
            WriteLineColored($"{_range} faces generated [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
        }

        private void BuildIndex()
        {
            sw.Restart();
            Tree = KDTree.FromData(Faces.Select(f => f.Metrics).ToArray(), Faces.ToArray(), false);
            sw.Stop();
            WriteLineColored($"INDEX built for {Faces.Count} faces [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
        }

        public void AddPointsToIndex(int range)
        {
            sw.Restart();
            for (int i = 0; i < range; i++)
            {
                Faces.Add(new Face { Guid = Guid.NewGuid(), Url = $"img{_range + i}.png", Metrics = NewRandomPoint() });
            }
            sw.Stop();
            WriteLineColored($"\n{range} more faces generated [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);

            sw.Restart();
            for (int i = 0; i < range; i++)
            {
                Tree.Add(Faces[_range + i].Metrics, Faces[_range + i]);
            }
            sw.Stop();
            WriteLineColored($"INDEX rebuilt (added with {range} more faces) [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
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

        public async Task ReadCsv()
        {
            sw.Restart();
            var lines = await File.ReadAllLinesAsync(FilePath);
            Faces = new List<Face>();
            foreach (var line in lines)
            {
                var fields = line.Split(',');
                Faces.Add(
                    new Face
                    {
                        Url = fields[0],
                        Guid = new Guid(fields[1]),
                        Metrics = fields[2].Split(';').Select(f => double.Parse(f)).ToArray()
                    });
            }
            sw.Stop();
            WriteLineColored($"CSV file read [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
        }

        public async Task WriteCsv()
        {
            sw.Restart();
            await File.WriteAllLinesAsync(FilePath, Faces.Select(f => $"{f.Url},{f.Guid},{string.Join(';', f.Metrics)}"));
            sw.Stop();
            WriteLineColored($"CSV file write [{sw.ElapsedMilliseconds} ms]", ConsoleColor.Yellow);
        }
    }
}
