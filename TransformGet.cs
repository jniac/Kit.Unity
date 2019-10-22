using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Kit
{
    public static class TransformGet
    {
        // Get() replace Collect()
        // Get() is based on IEnumerable, so it's better, since IEnumerable<T> are lazy.
        public static IEnumerable<T> Get<T>(this Transform transform,
            Func<Transform, bool> test = null, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component
        {
            if (includeSelf)
                if (test == null || test(transform))
                    foreach (T c in transform.GetComponents<T>())
                        yield return c;

            if (recursiveLimit != 0)
            {
                foreach (Transform child in transform)
                    if (test == null || test(child))
                        foreach (T c in child.GetComponents<T>())
                            yield return c;

                foreach (Transform child in transform)
                    foreach (T c in Get<T>(child, test, false, recursiveLimit - 1))
                        yield return c;
            }
        }

        // NOTE: standby
        public static Func<string, bool> Match(string mask) =>
            name => mask[0] == '!' ? name != mask.Substring(1) : name == mask;

        public static IEnumerable<T> Get<T>(this Transform transform,
            string str, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component
        {
            if (str.Contains(','))
            {
                var parts = new Regex(@"\s*,\s*").Split(str);

                return Get<T>(transform, t => parts.Contains(t.gameObject.name), includeSelf, recursiveLimit);
            }

            if (str[0] == '!')
            {
                str = str.Substring(1);
                return Get<T>(transform, t => t.gameObject.name != str, includeSelf, recursiveLimit);
            }

            return Get<T>(transform, t => t.gameObject.name == str, includeSelf, recursiveLimit);
        }

        public static T First<T>(this Transform transform,
            Func<Transform, bool> test = null, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component =>
            Get<T>(transform, test, includeSelf, recursiveLimit).First();

        public static T First<T>(this Transform transform,
            string gameObjectName, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component =>
            Get<T>(transform, gameObjectName, includeSelf, recursiveLimit).First();

        public static Transform First(this Transform transform,
            string gameObjectName, bool includeSelf = false, int recursiveLimit = -1) =>
            Get<Transform>(transform, gameObjectName, includeSelf, recursiveLimit).First();
    }
}
