using System;
using static System.Console;

namespace KNNSearch
{
    public class ConsoleHelper
    {
        public static void WriteLineColored(string message, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine(message);
            ResetColor();
        }
    }
}
