using System.Collections;
using System.Linq;
using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static void Print(params object[] args)
        {
            Debug.Log(string.Join(" ", args));
        }
    }

    static class Extensions
    {
        public static T Print<T>(this T t, string pattern = null, bool type = false)
        {
            var str = t?.ToString() ?? "null";

            if (pattern != null)
                str = pattern.Contains("$")
                    ? pattern.Replace("$", str)
                    : $"{pattern} {str}";

            if (type)
                str = $"({t.GetType().Name}) {str}";

            Debug.Log(str);

            return t;
        }

        public static T PrintList<T>(this T t, string pattern = null, int max = 10)
            where T : IEnumerable
        {
            var list = (t as IEnumerable).Cast<object>();

            string str = $"({t.GetType().Name}, {list.Count()})";

            if (pattern != null)
                str = pattern.Contains("$")
                    ? pattern.Replace("$", str)
                    : $"{pattern} {str}";

            var preview = string.Join(", ", list.Take(max));

            Debug.Log($"{str} {preview}");

            return t;
        }
    }
}
