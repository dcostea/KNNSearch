using System;
using static KNNSearch.ConsoleHelper;

namespace KNNSearch
{
    class Program
    {
        static void Main()
        {
            KNN knn = new KNN(1000000, 128, 10);

            var query = knn.NewRandomPoint();

            foreach (var item in knn.GetNeighbors(query, DistanceFormula.Chebyshev))
            {
                WriteLineColored($"{item.Node.Value.Url} [{item.Distance:0.###}]", ConsoleColor.Gray);
            }

            //////WriteLineColored($"\nGet all {knn.Tree.Count} nodes:", ConsoleColor.Cyan);
            //////foreach (var item in knn.Tree)
            //////{
            //////    WriteLineColored($"{item.Value.Url}", ConsoleColor.Cyan);
            //////}

            var newPoints = knn.NewRandomPoints(1000000);
            knn.AddPointsToIndex(newPoints);

            //////WriteLineColored($"\nGet all {knn.Tree.Count} nodes:", ConsoleColor.Cyan);
            //////foreach (var item in knn.Tree)
            //////{
            //////    WriteLineColored($"{item.Value.Url}", ConsoleColor.Cyan);
            //////}

            foreach (var item in knn.GetNeighbors(query, DistanceFormula.Chebyshev))
            {
                WriteLineColored($"{item.Node.Value.Url} [{item.Distance:0.###}]", ConsoleColor.Gray);
            }
        }
    }
}
