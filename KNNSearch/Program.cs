using System;
using static KNNSearch.ConsoleHelper;

namespace KNNSearch
{
    class Program
    {
        static void Main()
        {
            KNN knn = new KNN(65535, 128, 10);

            var query = knn.NewRandomPoint();

            foreach (var item in knn.GetNeighbors(query, DistanceFormula.Euclidean))
            {
                WriteLineColored($"{item.Node.Value.Url} [{item.Distance:0.###}]", ConsoleColor.Gray);
            }

            //////WriteLineColored($"\nGet all {knn.Tree.Count} nodes:", ConsoleColor.Cyan);
            //////foreach (var item in knn.Tree)
            //////{
            //////    WriteLineColored($"{item.Value.Url}", ConsoleColor.Cyan);
            //////}

            knn.AddPointsToIndex(1);

            //////WriteLineColored($"\nGet all {knn.Tree.Count} nodes:", ConsoleColor.Cyan);
            //////foreach (var item in knn.Tree)
            //////{
            //////    WriteLineColored($"{item.Value.Url}", ConsoleColor.Cyan);
            //////}

            foreach (var item in knn.GetNeighbors(query, DistanceFormula.Euclidean))
            {
                WriteLineColored($"{item.Node.Value.Url} [{item.Distance:0.###}]", ConsoleColor.Gray);
            }

            //task.Wait();
        }
    }
}
