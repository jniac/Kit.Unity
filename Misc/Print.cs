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
            var str = t.ToString();

            if (pattern != null)
                str = pattern.Replace("$", str);

            if (type)
                str = $"({t.GetType().Name}) {str}";

            Debug.Log(str);

            return t;
        }
    }
}
