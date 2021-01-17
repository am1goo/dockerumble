using System;

namespace dockerumble
{
    public static class Errors
    {
        public static void Arguments(string shortKey, string normalKey)
        {
            Error($"{shortKey} or {normalKey} are required and not assigned");
        }

        public static void Error(string text)
        {
            Console.WriteLine($"[ERR] {text}");
        }
    }
}
