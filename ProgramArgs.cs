using System;
using System.Collections.Generic;
using System.Reflection;

namespace dockerumble
{
    public static class ProgramArgs
    {
        public static readonly string ApplicationName = AppDomain.CurrentDomain.FriendlyName;
        public static readonly string ApplicationEmail = "forgesoft@yandex.ru";
        public static readonly string ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private static Dictionary<string, string> dict = new Dictionary<string, string>();
        public static void Parse(string[] args)
        {
            dict.Clear();

            for (int i = 0; i < args.Length; ++i)
            {
                string arg = args[i];
                string[] splitted = arg.Split('=');

                if (splitted.Length == 1)
                {
                    dict[splitted[0]] = string.Empty;
                }
                else if (splitted.Length == 2)
                {
                    dict[splitted[0]] = splitted[1];
                }
                else
                {
                    //do nothing
                }
            }
        }

        public static bool GetArg(string shortKey, string longKey, out string value)
        {
            if (dict.TryGetValue(shortKey, out value))
            {
                return true;
            }

            if (dict.TryGetValue(longKey, out value))
            {
                return true;
            }

            value = string.Empty;
            return false;
        }

        public static void Print()
        {
            CommonUtils.PrintKeyValue(dict);
        }
    }
}
