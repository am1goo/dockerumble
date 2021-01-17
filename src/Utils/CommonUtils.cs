using System;
using System.Collections.Generic;
using System.Reflection;

namespace dockerumble
{
    public static class CommonUtils
    {
        public enum InheritFlags : byte
        {
            Interface = 1 << 0,
            Abstract = 1 << 1,            
        }

        public static void GetInheritTypes(Type expectType, InheritFlags flags, List<Type> result)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (!flags.HasFlag(InheritFlags.Abstract))
                    {
                        if (type.IsAbstract)
                            continue;
                    }

                    if (!flags.HasFlag(InheritFlags.Interface))
                    {
                        if (type.IsInterface)
                            continue;
                    }

                    if (expectType.IsAssignableFrom(type))
                    {
                        result.Add(type);
                    }
                }
            }
        }

        public static void PrintKeys<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            int counter = 1;
            foreach (var kv in dict)
            {
                Console.WriteLine($"{counter}) {kv.Key}");
                counter++;
            }
        }

        public static void PrintKeyValue<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            int counter = 1;
            foreach (var kv in dict)
            {
                Console.WriteLine($"{counter}) {kv.Key}={kv.Value}");
                counter++;
            }
        }
    }
}
