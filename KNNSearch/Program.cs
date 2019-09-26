using System;

namespace KNNSearch
{
    class Program
    {
        const int DIMENSION = 128;
        const int RANGE = 10_000_000;
        
        static void Main()
        {
            Console.WriteLine("Generating points...");
            var points = KNN.GeneratePoints(DIMENSION, RANGE);
            var query = KNN.GeneratePoint(DIMENSION);

            foreach (var item in KNN.GetNeighbors(points, query, DIMENSION, RANGE))
            {
                //Console.WriteLine($"{item.Distance}");
            }
        }
    }
}
