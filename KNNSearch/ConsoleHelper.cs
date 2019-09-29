using System;
using static System.Console;

namespace KNNSearch
{
    public static class ConsoleHelper
    {
        public static void WriteLineColored(string message, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine(message);
            ResetColor();
        }
    }
}
