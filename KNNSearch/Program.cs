using System;
using static System.Console;
using static KNNSearch.ConsoleHelper;

namespace KNNSearch
{
    class Program
    {
        static void Main()
        {
            KNN.GenerateRandomPoints();

            KNN.BuildIndex();

            var query = KNN.NewRandomPoint();

            foreach (var item in KNN.GetNeighbors(query))
            {
                WriteLineColored($"{item.Node.Value.Url} {item.Node.Value.Guid} {item.Distance}", ConsoleColor.White);
            }

            var newPoint1 = KNN.NewRandomPoint();
            var val1 = KNN.AddPointToIndex(newPoint1);

            var newPoint2 = KNN.NewRandomPoint();
            var val2 = KNN.AddPointToIndex(newPoint2);

            foreach (var item in KNN.GetNeighbors(query))
            {
                if (val1 == item.Node.Value || val2 == item.Node.Value)
                {
                    WriteLineColored($"{item.Node.Value.Url} {item.Node.Value.Guid} {item.Distance}", ConsoleColor.Yellow);
                }
                else
                {
                    WriteLineColored($"{item.Node.Value.Url} {item.Node.Value.Guid} {item.Distance}", ConsoleColor.White);
                }
            }
        }
    }
}
