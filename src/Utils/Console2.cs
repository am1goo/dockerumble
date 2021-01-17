using System;

namespace dockerumble
{
    public static class Console2
    {
        public static void WriteLine(string value, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = prevColor;
        }
        public static void WriteLine(string format, object arg0, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, arg0);
            Console.ForegroundColor = prevColor;
        }

        public static void WriteLine(string format, object arg0, object arg1, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, arg0, arg1);
            Console.ForegroundColor = prevColor;
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2, ConsoleColor color)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, arg0, arg1, arg2);
            Console.ForegroundColor = prevColor;
        }
    }
}
